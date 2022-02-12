using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] private int distance;
    [SerializeField] public Vector2Int direction;
    [SerializeField] private float scanProblemDistance;

    public Vector2Int gridPosition;
    public Vector2Int targetGridPosition;
    public Vector2Int mainTargetGridPosition;

    public ProgramState programState;

    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GridManager.Instance;
        targetGridPosition = gridPosition;
        mainTargetGridPosition = gridPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToTargetGridPosition()
    {

    }

    public void DoSmtBasedOnStatus()
    {
        switch (programState)
        {
            case ProgramState.TargetLock:
                break;
            case ProgramState.WaitingAnotherRobot:
                break;
            case ProgramState.Knockback:
                break;
            case ProgramState.Moving:
                break;
            case ProgramState.Communicating:
                Debug.LogError(this.name + " state 2");
                break;
            case ProgramState.SolvingProblem:
                Debug.LogError(this.name + " state 2");
                break;
        }
    }

    public bool IsAvailable()
    {
        // check if it is moving, return boolean
        if(programState == ProgramState.Moving)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetStatus()
    {

    }

    public void MoveToTargetPosition()
    {

    }

    public void CheckIfAnyAdjacentRobot()
    {

    }

    public void CheckIfAnyAdjacentRobotIncoming() 
    {
        
    }

    public void SetMainTargetGridPosition()
    {
        // get random x or y direction (0 or 1)
        int _randomAxis = Random.Range(0, 2);
        // get random direction
        int _randomDirection = Random.Range(0, 2);

        direction = _randomAxis == 0 ? new Vector2Int(_randomDirection == 0 ? 1 : -1, 0) :
                                        new Vector2Int(0, _randomDirection == 0 ? 1 : -1);
        // get random distance
        distance = Random.Range(minDistance, maxDistance + 1);
        // set main target grid position
        mainTargetGridPosition = gridPosition + direction * distance;

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
        targetGridPosition = gridPosition + direction;
        gridManager.robotTargetList[this] = targetGridPosition;

        // TODO: Check if grid position is equal to main target
        if (gridPosition == mainTargetGridPosition)
        {
            SetMainTargetGridPosition();
            gridManager.robotMainTargetList[this] = mainTargetGridPosition;
        }
    }

    public void SearchProblem ()
    {
        Vector2 rayDirection = (Vector2)direction;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, scanProblemDistance);

        if(hit.transform.TryGetComponent(out Problem problem))
        {
            mainTargetGridPosition = problem.gridPosition;
            gridManager.robotMainTargetList[this] = mainTargetGridPosition;
        }
    }
}

public enum ProgramState
{
    Moving,
    SolvingProblem,
    WaitingAnotherRobot,
    Communicating,
    TargetLock,
    Knockback
}