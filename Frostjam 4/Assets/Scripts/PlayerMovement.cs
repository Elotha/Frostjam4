using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float multiplier = 1f;
    public Vector2 movementVec;
    private float moveSpeed = 5f;
    //public float rayLength = 2.29f;

    private Rigidbody2D playerRb;
    private Animator _animator;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        //set horizontal and vertical speed on animator!!
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.y = Input.GetAxis("Vertical");
        if (movementVec.x * movementVec.y != 0)
        {
            _animator.SetBool("isMoving", true);
            moveSpeed = 3.55f;
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
        
        /*
        _animator.SetFloat("horSpeed", movementVec.x);
        _animator.SetFloat("verSpeed", movementVec.y);
        */
    }

    private void FixedUpdate()
    {
        /*
        Debug.DrawRay(transform.position, movementVec.normalized * (rayLength + 0.71f), Color.green);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)transform.position + movementVec.normalized * 0.71f, movementVec.normalized, rayLength);
        if (hit2d)
        {
            if (hit2d.collider.CompareTag("Player"))
            {
                playerRb.MovePosition(playerRb.position + movementVec * moveSpeed * Time.deltaTime);
            }
            else
            {
                Debug.Log("hit: " + hit2d.collider.name);
            }
        }
        
        else
        {
            playerRb.MovePosition(playerRb.position + movementVec * moveSpeed * Time.deltaTime);
        }
        */
        playerRb.MovePosition(playerRb.position + movementVec * moveSpeed * multiplier * Time.deltaTime);
    }
}
