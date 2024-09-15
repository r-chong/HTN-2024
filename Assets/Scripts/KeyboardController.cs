using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;


public class KeyboardController : MonoBehaviour
{
    public TextMeshPro TextMesh;
    public PlayerController PlayerControllerReference;
    public MeshRenderer MeshRendererReference;

    public GameObject Keyboard;

    void Start(){
        TextMesh.text = "";
        MeshRendererReference = GetComponent<MeshRenderer>();

        if (Keyboard == null) Debug.Log("Keyboard object is null!");
    }

    void Update()
    {
        // Debug.Log(TextMesh.text);
        // whether this screen should be visible
        if (PlayerControllerReference != null){
            if (PlayerControllerReference.isTyping) MeshRendererReference.enabled = true;
            else MeshRendererReference.enabled = false;
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
                        // Send the POST request with the current text when Enter is pressed
                        SendPostRequest(TextMesh.text).ContinueWith(task =>
                        {
                            if (task.Exception != null)
                                {
                                    Debug.LogError("Failed to send POST request: " + task.Exception.Message);
                                }
                        });

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
        // resets the text mesh
        else TextMesh.text = "";
    }

    public async Task SendPostRequest(string userResponseParam)
    {

        Debug.Log("passed value is " + userResponseParam);
        // Define the URL
        string url = "https://chatgpme-873976347647.us-east1.run.app/compare";

        // Create the data object
        var data = new
        {
            respondedPrompt = "Should I get back with my ex?", 
            userResponse = userResponseParam
        };

        // Serialize the data to JSON
        var jsonData = JsonUtility.ToJson(data);

        // Create an HttpClient instance
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Prepare the content to send (application/json)
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Read the response content
                string responseString = await response.Content.ReadAsStringAsync();

                // Output the response (you can modify this to show in the UI or handle it as needed)
                Debug.Log("Response from server: " + responseString);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error sending POST request: " + ex.Message);
            }
        }
    }
}
