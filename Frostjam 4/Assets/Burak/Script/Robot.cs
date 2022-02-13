using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private int distance;
    [SerializeField] public Vector2Int direction;
    [SerializeField] private float scanProblemDistance;

    public Vector2Int gridPosition;
    public Vector2Int targetGridPosition;
    public Vector2Int mainTargetGridPosition;

    public ProgramState programState;

    private Vector2Int focusedProblem;
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GridManager.Instance;   
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        targetGridPosition = gridPosition;
        gridManager.robotTargetList[this] = targetGridPosition;
        mainTargetGridPosition = gridPosition;
        gridManager.robotMainTargetList[this] = mainTargetGridPosition;

        programState = ProgramState.Moving;
        speed = (1f / gridManager.loopTime) / gridManager.cellSize;
    }

    // Update is called once per frame
    void Update()
    {
        GoToTargetGridPosition();
    }

    public void GoToTargetGridPosition()
    {
        if (programState == ProgramState.Moving || programState == ProgramState.TargetLock)
        {
            Vector3 directionV3 = new Vector3(direction.x, -direction.y, 0f);
            transform.position += directionV3 * speed * Time.deltaTime;
        }
    }

    public void ActBasedOnState()
    {
        switch (programState)
        {
            case ProgramState.TargetLock:
                break;
            case ProgramState.WaitingAnotherRobot:
                Debug.Log("Waited");
                programState = ProgramState.Moving;
                break;
            case ProgramState.Knockback:
                break;
            case ProgramState.Moving:
                break;
            case ProgramState.Communicating:
                Debug.Log("Communicated");
                programState = ProgramState.Moving;
                //Debug.LogError(this.name + " state 2");
                break;
            case ProgramState.SolvingProblem:
                Debug.LogError(this.name + " state 2");
                break;
        }
    }

    public bool IsAvailable()
    {
        return programState == ProgramState.Moving;
    }

    public void CheckIfAnyAdjacentRobot()
    {
        if (!IsAvailable()) return;

        foreach (KeyValuePair<Robot, Vector2Int> item in gridManager.robotList)
        {
            if (item.Key.IsAvailable() != true)
                continue;

            var V2 = item.Value;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;

            if (item.Key != this && (isAdjacentX ^ isAdjacentY))
            {
                // TODO: There may be some conditions for communicating
                programState = ProgramState.Communicating;
                item.Key.programState = ProgramState.Communicating;
            }
        }
    }

    public void CheckIfAnyAdjacentProblem()
    {
        // logic
    }

    public void CheckIfAnyAdjacentRobotIncoming() 
    {
        if (!IsAvailable()) return;

        foreach (KeyValuePair<Robot, Vector2Int> item in gridManager.robotTargetList)
        {
            if (item.Key.IsAvailable() != true)     
                continue;

            var V2 = item.Value;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1;

            if (item.Key != this && (isAdjacentX ^ isAdjacentY))
            {
                // TODO: There may be some conditions for waiting
                programState = ProgramState.WaitingAnotherRobot;
                item.Key.programState = ProgramState.TargetLock;
            }
        }
    }

    public void SetMainTargetGridPosition()
    {
        // get random x or y direction (0 or 1)
        int _randomAxis = Random.Range(0, 2);
        // get random direction
        int _randomDirection = Random.Range(0, 2);

        direction = _randomAxis == 0 ?  new Vector2Int(_randomDirection == 0 ? 1 : -1, 0) :
                                        new Vector2Int(0, _randomDirection == 0 ? 1 : -1);
        // get random distance
        distance = Random.Range(minDistance, maxDistance + 1);

        if (focusedProblem != null)
        {

        }

        // set main target grid position
        mainTargetGridPosition = gridPosition + direction * distance;
        gridManager.robotMainTargetList[this] = mainTargetGridPosition;

        // if target is in bounding box set object is moving, otherwise set another target position
        if (gridManager.gridList.ContainsKey(mainTargetGridPosition) == true)
        {
            return;
        }
        else if (gridManager.gridList.ContainsKey(mainTargetGridPosition) == false)
        {
            SetMainTargetGridPosition();
        }
    }

    public void SetTargetGridPosition()
    {
        // TODO: Check if grid position is equal to main target
        if (gridPosition == mainTargetGridPosition)
        {
            Debug.Log("Set new target");
            SetMainTargetGridPosition();
            gridManager.robotMainTargetList[this] = mainTargetGridPosition;
        }
        
        if(gridManager.gridList.ContainsKey(gridPosition + direction)) {
            targetGridPosition = gridPosition + direction;
            gridManager.robotTargetList[this] = targetGridPosition;
        }
    }

    public void SearchProblem()
    {
        // TODO: update focused problem.

        Vector2 rayDirection = (Vector2)direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, scanProblemDistance);

        if(hit.transform.TryGetComponent(out Problem problem))
        {
            // mainTargetGridPosition = problem.gridPosition;
            // gridManager.robotMainTargetList[this] = mainTargetGridPosition;
        }
    }
}

public enum ProgramState
{
    None,
    Moving,
    SolvingProblem,
    WaitingAnotherRobot,
    Communicating,
    TargetLock,
    Knockback
}