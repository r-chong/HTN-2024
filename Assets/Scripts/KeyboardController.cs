using System.Collections;
using System.Collections.Generic;
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

        Color color = MeshRendererReference.material.color;
        color.a = 1.0f;
        MeshRendererReference.material.color = color;

        if (Keyboard == null) Debug.Log("Keyboard object is null!");
    }

    void Update()
    {
        // Debug.Log(TextMesh.text);
        // whether this screen should be visible
        if (PlayerControllerReference != null){
            Debug.Log("the value from PlayerController is " + PlayerControllerReference.isTyping);

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
                        TextMesh.text += "\n";
                    }
                }
            }
        } 
        // resets the text mesh
        else TextMesh.text = "";
    }
}
