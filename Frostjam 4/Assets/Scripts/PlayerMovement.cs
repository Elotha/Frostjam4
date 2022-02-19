using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [NonSerialized]public Vector2 movementVec;
    [NonSerialized]public float multiplierModifier = 1f;
    [SerializeField]private float moveSpeed = 5f;
    //public float rayLength = 2.29f;

    private Rigidbody2D playerRb;
    private Animator _animator;
    
    private float diagonalMoveSpeed;
    private float perpendicularMoveSpeed;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        diagonalMoveSpeed = moveSpeed * 0.707f;
        perpendicularMoveSpeed = moveSpeed;
    }

    void Update()
    {
        //set horizontal and vertical speed on animator!!
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.y = Input.GetAxis("Vertical");
        if (movementVec.x * movementVec.y != 0)
        {
            _animator.SetBool("isMoving", true);
            moveSpeed = diagonalMoveSpeed;
            if (movementVec.x > 0)
            {
                _animator.SetFloat("horizontalSpeed", 1f);
                _animator.SetFloat("verticalSpeed", 0);
            }
            else
            {
                _animator.SetFloat("horizontalSpeed", -1f);
                _animator.SetFloat("verticalSpeed", 0);
            }
        }
        else if (movementVec.x == 0 && movementVec.y == 0)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("horizontalSpeed", 0);
            _animator.SetFloat("verticalSpeed", 0);
        }
        else
        {
            moveSpeed = perpendicularMoveSpeed;
            _animator.SetBool("isMoving", true);
            if (movementVec.x > 0)
            {
                _animator.SetFloat("horizontalSpeed", 1);
                _animator.SetFloat("verticalSpeed", 0);
            }
            else if (movementVec.x < 0)
            {
                _animator.SetFloat("horizontalSpeed", -1);
                _animator.SetFloat("verticalSpeed", 0);
            }
            else if (movementVec.y > 0)
            {
                _animator.SetFloat("verticalSpeed", 1);
                _animator.SetFloat("horizontalSpeed", 0);
            }
            else if (movementVec.y < 0)
            {
                _animator.SetFloat("verticalSpeed", -1);
                _animator.SetFloat("horizontalSpeed", 0);
            }
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
        playerRb.MovePosition(playerRb.position + movementVec * moveSpeed * multiplierModifier * Time.deltaTime);
    }
}
