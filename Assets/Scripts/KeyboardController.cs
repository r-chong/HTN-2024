using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyboardController : MonoBehaviour
{
    public PlayerController PlayerControllerReference;
    public MeshRenderer MeshRendererReference;

    public GameObject Keyboard;

    public string KeyboardText;

    void Start(){
        MeshRendererReference  = GetComponent<MeshRenderer>();

        Color color = MeshRendererReference.material.color;
        color.a = 1.0f;
        MeshRendererReference.material.color = color;

        if (Keyboard == null) Debug.Log("Keyboard object is null!");
    }

    void Update()
    {
        // whether this screen should be visible
        if (PlayerControllerReference != null){
            Debug.Log("the value from PlayerController is " + PlayerControllerReference.isTyping);

            if (PlayerControllerReference.isTyping) MeshRendererReference.enabled = true;
            else MeshRendererReference.enabled = false;
        }

        // keyboard input
        if (PlayerControllerReference.isTyping){
            // Check for shift key
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                isShiftPressed = true;
            }
            else
            {
                isShiftPressed = false;
            }

            // Handle key input
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key >= KeyCode.A && key <= KeyCode.Z) // Check if the key is a letter
                    {
                        KeyboardText += isShiftPressed ? key.ToString() : key.ToString().ToLower();
                    }
                    else if (key == KeyCode.Space) // Handle space key
                    {
                        KeyboardText += " ";
                    }
                    else if (key == KeyCode.Backspace) // Handle backspace
                    {
                        if (KeyboardText.Length > 0)
                        {
                            KeyboardText = KeyboardText.Substring(0, KeyboardText.Length - 1);
                        }
                    }
                    else if (key == KeyCode.Enter) // Handle enter key (for example, add a new line)
                    {
                        KeyboardText += "\n";
                    }
                }
            }
        }
    }
}
