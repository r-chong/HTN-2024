using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed, jumpForce;
    private Vector2 moveInput;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision occurred");
    }

    // private void InteractWithTypewriter(GameObject typewriter) {
    //     Debug.Log("Interacting with typewriter. Activate typing UI here");
    // }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);
    }
}
