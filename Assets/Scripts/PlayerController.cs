using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody rb;
    public float moveSpeed, jumpForce;
    private Vector2 moveInput;
    private bool withinRangeOfInteractable;

    private void OnTriggerEnter(Collider other)
    {
        withinRangeOfInteractable = true;
    }

    private void OnTriggerExit(Collider other) {
        withinRangeOfInteractable = false;
    }

    private void InteractWithTypewriter() {
        Debug.Log("Interacting with typewriter. Activate typing UI here");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & withinRangeOfInteractable == true)  {
            InteractWithTypewriter();
        }
        
        HandleMovement();
        HandleAnimation();
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
