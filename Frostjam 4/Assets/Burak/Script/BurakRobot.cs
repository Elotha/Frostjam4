using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BurakRobot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float timeBetweenMoves = 1f;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private Vector2 xBound;
    [SerializeField] private Vector2 yBound;
    [SerializeField] private float checkProblemRadius = 2f;
    [SerializeField] private BurakGridObject myGrid;
    //[SerializeField] private Problem myProblem;

    [Header("Internal Parameters")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 direction;
    [SerializeField] private int distance;
    [SerializeField] private bool isMoving;

    [SerializeField] private float timeSpent = 0f;

    private BurakGridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = BurakGridManager.Instance;

        speed = (1f / timeBetweenMoves) / gridManager.cellSize;

        xBound = new Vector2(gridManager.startPoint.x, gridManager.endPoint.x);
        yBound = new Vector2(gridManager.endPoint.y, gridManager.startPoint.y);
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

    private void CheckNextGrid()
    {

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
            timeSpent += Time.deltaTime;
            transform.position += direction * speed * Time.deltaTime;
        }

        if(Vector3.Distance(transform.position, targetPosition) <= direction.magnitude * speed * Time.deltaTime)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    // BE CAREFUL, RECURSIVE FUNCTION
    private void SetTargetPosition()
    {
        // return if object is moving
        if (isMoving == true)  return;
        // get random x or y direction (0 or 1)
        int _randomAxis = Random.Range(0, 2);
        // get random direction
        int _randomDirection = Random.Range(0, 2);

        direction = _randomAxis == 0 ?  new Vector3(_randomDirection == 0 ? 1f : -1f, 0f, 0f) : 
                                        new Vector3(0f, _randomDirection == 0 ? 1f : -1f, 0f);
        // get random distance
        distance = Random.Range(minDistance, maxDistance + 1);
        // set target position
        targetPosition = transform.position + direction * distance;

        // if target is in bounding box set object is moving, otherwise set another target position
        if (isTargetInBoundingBox() == true)
        {
            isMoving = true;
        }
        else if (isTargetInBoundingBox() == false)
        {
            SetTargetPosition();
        }
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
            Debug.Log("Collided with problem and destroyed.");
            Destroy(problem.gameObject, 0.25f);
        }

        if(collision.gameObject.TryGetComponent(out BurakGridObject grid))
        {
            myGrid = grid;
        }
    }
}