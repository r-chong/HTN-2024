using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WindowController : MonoBehaviour
{

    public TextMeshPro TextMesh;

    public string[] prompts;
    public int prompts_ind;

    public Sprite staticSprite;
    public Sprite[] characterSprites; 


    private void Start()
    {
        prompts_ind = 0;
        prompts = new string[] {
            "If you had arms, would you give me a virtual high-five or a virtual hug?",
            "Should I get back with my ex?",
            "My teacher gave me a bad grade. Should I slash his tires?",
            "Is it ethical to cannibalize, if absolutely necessary?"
        };

        // Start the coroutine that handles the periodic arrival of characters
        StartCoroutine(ArrivalRoutine());
    }

    IEnumerator ArrivalRoutine(){
        yield return StartCoroutine(ShowStatic());
        TextMesh.text = prompts[(++prompts_ind)%4];
        ShowNewCharacter();
    }

    IEnumerator ShowStatic()
    {        
        // windowImage.sprite = staticSprite;
        yield return new WaitForSeconds(1.5f);
    }

    void ShowNewCharacter()
    {
        int randomIndex = Random.Range(0, characterSprites.Length);
        // windowImage.sprite = characterSprites[randomIndex];
    }
}