using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody rb;
    public float moveSpeed, jumpForce;
    private Vector2 moveInput;
    private bool withinRangeOfTypewriter;
    private bool withinRangeOfWindow;
    public bool isTyping;
    
    public TextMeshPro TextMesh;

    public string[] prompts;

    public MeshRenderer promptMesh;
    public int prompts_ind;

    public SpriteRenderer staticSprite;
    public SpriteRenderer[] characterSprites; 
    public SpriteRenderer smileySprite;


    private void Start()
    {
        prompts_ind = 0;
        prompts = new string[] {
            "If you had arms, would you give me a virtual high-five or a virtual hug?",
            "Should I get back with my ex?",
            "My teacher gave me a bad grade. Should I slash his tires?",
            "Is it ethical to cannibalize, if absolutely necessary?",
            "Can you write a Shakespearean sonnet about my leftover pizza?",
            "Can you write a breakup letter for me… to my Netflix subscription?",
            "Can you create a business plan for a startup that only sells invisible products?",
            "Can you explain how cryptocurrency works, but in the style of a Dr. Seuss book?"
        };

        // Start the coroutine that handles the periodic arrival of characters
        StartCoroutine(ArrivalRoutine());
    }

    public IEnumerator ArrivalRoutine(){
        yield return StartCoroutine(ShowStatic());
        TextMesh.text = prompts[(++prompts_ind)%4];
    }

    public IEnumerator ShowStatic()
    {   for (int i = 0; i < 3; i++) characterSprites[i].enabled = false;
        smileySprite.enabled = false;
        staticSprite.enabled = true;
        // windowImage.sprite = staticSprite;
        yield return new WaitForSeconds(1.5f);
        ShowNewCharacter();
    }

    void ShowNewCharacter()
    {
        promptMesh.enabled = true;
        int randomIndex = Random.Range(0, characterSprites.Length);
        // windowImage.sprite = characterSprites[randomIndex];
        for (int i = 0; i < 3; i++){
            if (i != randomIndex) characterSprites[i].enabled = false;
            else characterSprites[i].enabled = true;
        }
        smileySprite.enabled = true;
        staticSprite.enabled = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Typewriter")) withinRangeOfTypewriter = true;
        if (other.CompareTag("Window")) withinRangeOfWindow = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Typewriter")) withinRangeOfTypewriter = false;
        if (other.CompareTag("Window")) withinRangeOfWindow = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isTyping is currently" + isTyping);
        if (Input.GetKeyDown(KeyCode.E) & withinRangeOfTypewriter == true)  {
            isTyping = true;
        }

        if (withinRangeOfTypewriter & Input.GetKeyDown(KeyCode.Escape) == true){
            isTyping = false;
        }

        if (withinRangeOfWindow & Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(ArrivalRoutine());
            Debug.Log("next person");
        }
        
        HandleAnimation();
        if (!isTyping) HandleMovement();
    }
    void HandleMovement() {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);
    }

    void HandleAnimation() {
        animator.SetFloat("Speed", moveInput.magnitude);
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
