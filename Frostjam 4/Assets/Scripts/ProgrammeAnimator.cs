using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammeAnimator : MonoBehaviour
{
    private Vector2Int direction;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        direction = GetComponent<Robot>().direction;
    }

    private void Update()
    {
        if (direction.x == 0 && direction.y == 0)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("HorizontalSpeed", 0f);
            _animator.SetFloat("VerticalSpeed", 0f);
        }
        else
        {
            _animator.SetBool("isMoving", true);
            if (direction.x > 0)
            {
                _animator.SetFloat("HorizontalSpeed", 1f);
                _animator.SetFloat("VerticalSpeed", 0f);
            }
            else if (direction.x < 0)
            {
                _animator.SetFloat("HorizontalSpeed", -1f);
                _animator.SetFloat("VerticalSpeed", 0f);
            }
            else if (direction.y > 0)
            {
                _animator.SetFloat("VerticalSpeed", 1f);
                _animator.SetFloat("HorizontalSpeed", 0f);
            }
            else if (direction.y < 0)
            {
                _animator.SetFloat("VerticalSpeed", -1f);
                _animator.SetFloat("HorizontalSpeed", 0f);
            }
        }
    }
}
