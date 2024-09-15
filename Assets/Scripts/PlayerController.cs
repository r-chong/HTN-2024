using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed, jumpForce;
    private Vector2 moveInput;
    private bool withinRangeOfInteractable;
    public bool isTyping;

    private void OnTriggerEnter(Collider other)
    {
        withinRangeOfInteractable = true;
    }

    private void OnTriggerExit(Collider other) {
        withinRangeOfInteractable = false;
        isTyping = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("isTyping is currently" + isTyping);
        if (Input.GetKeyDown(KeyCode.E) & withinRangeOfInteractable == true)  {
            isTyping = true;
        }

        if (withinRangeOfInteractable & Input.GetKeyDown(KeyCode.Escape) == true){
            isTyping = false;
        }
        
        HandleMovement();
    }
    void HandleMovement() {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);
    }
}
