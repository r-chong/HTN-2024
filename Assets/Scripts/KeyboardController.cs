using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class ResponseData
{
    public int score;
    public string[] feedback;
}

[System.Serializable]
public class FeedbackWrapper
{
    public string[] feedback;
}


public class KeyboardController : MonoBehaviour
{
    public TextMeshPro TextMesh, TextMeshPrompt;
    public PlayerController PlayerControllerReference;
    public MeshRenderer MeshRendererReference1, MeshRendererReference2;

    public GameObject Keyboard;
    public bool isAwaiting;
    public bool isDisplaying;
    public int score;
    public string feedback;

    private string apiUrl = "https://chatgpme-873976347647.us-east1.run.app/compare";

    void Start(){
        TextMesh.text = "";
        MeshRendererReference1.enabled = false;
        MeshRendererReference2.enabled = false;
        isAwaiting = false;
        isDisplaying = false;
        score = 0;
        feedback = "";

        if (Keyboard == null) Debug.Log("Keyboard object is null!");
    }

    void Update()
    {
        // Debug.Log(TextMesh.text);
        // whether this screen should be visible
        if (PlayerControllerReference != null){
            if (PlayerControllerReference.isTyping){
                MeshRendererReference1.enabled = true;
            }
            else {
                MeshRendererReference1.enabled = false;
            }

            if (isAwaiting){
                MeshRendererReference1.enabled = true;
                TextMesh.text = "Loading...";
            }
        }

        // if it's displaying right now
        if (isDisplaying){
            MeshRendererReference1.enabled = true;

            TextMesh.text = (score + " / 100 " + feedback);

            bool anyKeys = false;

            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode))) if (Input.GetKeyDown(key)) anyKeys = true;

            if (anyKeys){
                isDisplaying = false;
                score = 0;
                feedback = "";
                TextMesh.text = "";
                MeshRendererReference2.enabled = false;
                TextMeshPrompt.text = "";
            }
        }

        // keyboard input
        if (PlayerControllerReference.isTyping){
            bool isShiftPressed = false;

            // Check for shift key
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) isShiftPressed = true;
            else isShiftPressed = false;

            // Handle key input
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key >= KeyCode.A && key <= KeyCode.Z) // Check if the key is a letter
{
                        TextMesh.text += isShiftPressed ? key.ToString() : key.ToString().ToLower();
                    }
                    else if (KeyCode.Alpha0 <= key && key <= KeyCode.Alpha9) // Handle number keys
                    {
                        int number = (int)key - (int)KeyCode.Alpha0;
                        char character = (char)('0' + number);

                        // Handle Shift key for symbols
                        if (isShiftPressed)
                        {
                            // Mapping for shifted number keys
                            string[] shiftedSymbols = { ")", "!", "@", "#", "$", "%", "^", "&", "*", "(" };
                            TextMesh.text += shiftedSymbols[number];
                        }
                        else TextMesh.text += character;
                    }
                    else if (key == KeyCode.Space) // Handle space key
                    {
                        TextMesh.text += " ";
                    }
                    else if (key == KeyCode.Backspace) // Handle backspace
                    {
                        if (TextMesh.text.Length > 0)
                        {
                            TextMesh.text = TextMesh.text.Substring(0, TextMesh.text.Length - 1);
                        }
                    }
                    else if (key == KeyCode.Return) // Handle enter key (for example, add a new line)
                    {
                        isAwaiting = true;
                        StartCoroutine(SendPostRequest(TextMesh.text));

                        TextMesh.text = ""; // Clear the text after sending
                        PlayerControllerReference.isTyping = false;
                    }
                    // Additional symbols handling
                    else if (key == KeyCode.Comma)
                    {
                        TextMesh.text += isShiftPressed ? "<" : ",";
                    }
                    else if (key == KeyCode.Period)
                    {
                        TextMesh.text += isShiftPressed ? ">" : ".";
                    }
                    else if (key == KeyCode.Slash)
                    {
                        TextMesh.text += isShiftPressed ? "?" : "/";
                    }
                    else if (key == KeyCode.Semicolon)
                    {
                        TextMesh.text += isShiftPressed ? ":" : ";";
                    }
                    else if (key == KeyCode.Quote)
                    {
                        TextMesh.text += isShiftPressed ? "\"" : "'";
                    }
                    else if (key == KeyCode.LeftBracket)
                    {
                        TextMesh.text += isShiftPressed ? "{" : "[";
                    }
                    else if (key == KeyCode.RightBracket)
                    {
                        TextMesh.text += isShiftPressed ? "}" : "]";
                    }
                    else if (key == KeyCode.Minus)
                    {
                        TextMesh.text += isShiftPressed ? "_" : "-";
                    }
                    else if (key == KeyCode.Equals)
                    {
                        TextMesh.text += isShiftPressed ? "+" : "=";
                    }
                }
            }
        } 
    }

    IEnumerator SendPostRequest(string userResponseParam)
    {
        // Create the JSON body
        string jsonBody = $"{{\"respondedPrompt\": \"Should I get back with my ex?\",\"userResponse\": \"{userResponseParam}\"}}";

        // Create a new UnityWebRequest for a POST request
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

            Debug.Log("Response: " + request.downloadHandler.text);

            score = responseData.score;
            feedback = string.Join(" ", responseData.feedback);
            Debug.Log("feedback is " + feedback);
        }

        isAwaiting = false;
        isDisplaying = true;
    }
}
