using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Robot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private Vector2 xBound;
    [SerializeField] private Vector2 yBound;
    [SerializeField] private float checkProblemRadius = 2f;
    [SerializeField] private Problem myProblem;

    [Header("Internal Parameters")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private int distance;
    [SerializeField] private bool isMoving;

    [Header("Prefab References")]
    [SerializeField] private GameObject claimSeal;
    
    private GameObject sealInstance;
    private LineRenderer _lineRenderer;
    private Animator _animator;
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
        sealInstance = Instantiate(claimSeal, transform.position, claimSeal.transform.rotation);
        sealInstance.SetActive(false);
        myProblem = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == false)
        {
            SetTargetPosition();
        }
        
        GoToTargetPosition();
        CheckProblemAround();
    }

    private void FixedUpdate()
    {
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void CheckProblemAround()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, checkProblemRadius);
        List<Transform> problemList = new List<Transform>();
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.TryGetComponent(out Problem problem))
            {
                problemList.Add(problem.transform);
                Debug.Log("Need to sleep, check this later");
                Debug.DrawLine(transform.position, problem.transform.position);
            }
        }

    }

    private void GoToTargetPosition()
    {
        if (isMoving == true)
        {
            transform.position += direction * speed * Time.deltaTime;
            _animator.SetFloat("HorizontalSpeed", direction.x);
            _animator.SetFloat("VerticalSpeed", direction.y);
        }

        if(Vector3.Distance(transform.position, targetPosition) <= direction.magnitude * speed * Time.deltaTime)
        {
            transform.position = targetPosition;
            isMoving = false;
            _animator.SetBool("isMoving", false);
        }
    }

    // BE CAREFUL, RECURSIVE FUNCTION
    private void SetTargetPosition()
    {
        sealInstance.SetActive(false);
        
        // return if object is moving
        if (isMoving == true)  return;
        // get random x or y direction (0 or 1)
        int _random = Random.Range(0, 2);
        // get random direction
        int xDirection = Random.Range(-1, 2);
        int yDirection = Random.Range(-1, 2);
        direction = _random == 0 ? new Vector3(xDirection, 0f, 0f) : new Vector3(0f, yDirection, 0f);
        // get random distance
        distance = Random.Range(minDistance, maxDistance + 1);
        // set target position
        targetPosition = transform.position + direction * distance;

        // if target is in bounding box set object is moving, otherwise set another target position
        if (isTargetInBoundingBox() == true)
        {
            _animator.SetBool("isMoving", true);
            isMoving = true;
        }
        else if (isTargetInBoundingBox() == false)
        {
            SetTargetPosition();
        }

        ClaimGrid(targetPosition);
    }

    private void ClaimGrid(Vector3 positionToBeClaimed)
    {
        _lineRenderer.SetPosition(1, positionToBeClaimed);
        sealInstance.transform.position = positionToBeClaimed;
        sealInstance.SetActive(true);
    }

    private bool isTargetInBoundingBox()
    {
        if (targetPosition.x >= xBound.x && targetPosition.x <= xBound.y && targetPosition.y >= yBound.x && targetPosition.y <= yBound.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Problem problem))
        {
            _gameManager.problemLeft--;
            _gameManager.UpdateProblemLeft();
            Debug.Log("Collided with problem and destroyed.");
            Destroy(problem.gameObject, 0.25f);
            myProblem = null;
        }
    }
}