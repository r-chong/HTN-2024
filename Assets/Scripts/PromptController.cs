using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PromptController : MonoBehaviour
{
    public TextMeshPro TextMesh;

    public void SetText(string newText){
        TextMesh.text = newText;
    }
}
