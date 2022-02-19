using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammeAnimator : MonoBehaviour
{
    private Vector2Int direction;
    private Animator _animator;
    private Robot _robot;
    private ProgramState _state;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _robot = GetComponent<Robot>();
    }

    private void Update()
    {
        _state = _robot.programState;
        direction = _robot.direction;
        if (_state == ProgramState.Moving)
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
                    _animator.SetFloat("VerticalSpeed", -1f);
                    _animator.SetFloat("HorizontalSpeed", 0f);
                }
                else if (direction.y < 0)
                {
                    _animator.SetFloat("VerticalSpeed", 1f);
                    _animator.SetFloat("HorizontalSpeed", 0f);
                }
            }
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }
}
