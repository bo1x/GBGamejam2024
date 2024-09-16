using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movX = 0;
    private float movY = 0;
    public float speed = 1f;
    public float JumpSpeed = 3f;

    [SerializeField] private Transform groundTransform;
    private bool groundCheck;
    private float groundCheckRayLenght = 0.2f;
    //We do 3 cast to determine if characters touches ground, left= groundtransform-variation middle= groundtransform and right = groundtransform+variation
    [SerializeField] private float groundCheckVariations = 0.2f;
    [SerializeField] private LayerMask groundLayer;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        WallCheck();
        GetInputs();
        Walk(movX);
        Jump(movY);

        
    }

    private void WallCheck()
    {
        
    }

    private void GroundCheck()
    {
        //this does three small raycasts at the specified positions to see if the player is grounded.
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckRayLenght, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(-groundCheckVariations, 0), Vector2.down, groundCheckRayLenght, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(groundCheckVariations, 0), Vector2.down, groundCheckRayLenght, groundLayer))
        {
            groundCheck = true;
        }
        else
        {
            groundCheck = false;
        }
        Debug.Log(groundCheck);
    }

    private void Jump(float movY)
    {
        if (groundCheck == false) { return;}
        if (movY == 0) { return; }
        rb.velocity = new Vector2(rb.velocity.x, movY * JumpSpeed);
    }

    private void Walk(float movX)
    {
        rb.velocity = new Vector2(movX * speed, rb.velocity.y);
    }

    private void GetInputs()
    {
        movX = 0;
        movY = 0;
        if (Input.GetKey(KeyCode.A))
        {
            movX = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movX = 1;

        }
        if (Input.GetKey(KeyCode.Space))
        {
            movY = 1;

        }
        if (Input.GetKey(KeyCode.S))
        {
            movY = -1;

        }
    }
}
