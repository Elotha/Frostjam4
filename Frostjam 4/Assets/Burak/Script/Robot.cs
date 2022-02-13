using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

public class Robot : MonoBehaviour
{
    public bool IsAvailableToMove => (programState == ProgramState.Moving || programState == ProgramState.TargetLock);
    public bool IsAvailable => (programState == ProgramState.Moving);
    
    [Header("Movement AI Settings")]
    private float speed;
    [SerializeField] private int minDistance;
    [SerializeField] private int maxDistance;
    [SerializeField] public int distance;
    [SerializeField] public Vector2Int direction;
    
    [Tooltip("How many turns inbetween two communications")]
    [SerializeField] private int CommunicatingCooldownMax;
    private int CommunicatingCooldown;
    private float communicationDuration;

    [Header("Grid related info")]
    public Vector2Int gridPosition;
    public Vector2Int targetGridPosition;
    public Vector2Int mainTargetGridPosition;

    [Header("Grid related info")]
    public ProgramState programState;
    private Vector2Int focusedProblem;

    
    private Rigidbody2D rigid2D;
    private GridManager gridManager;

    private int testCounter = 0;

    private void Awake()
    {
        gridManager = GridManager.Instance;
        rigid2D = GetComponent<Rigidbody2D>();
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
    void FixedUpdate()
    {
        GoToTargetGridPosition();

        ContinuousActBasedOnState();
    }


    public void GoToTargetGridPosition()
    {
        if (programState == ProgramState.Moving || programState == ProgramState.TargetLock)
        {
            Vector2 directionV2 = new Vector2(direction.x, -direction.y);
            Vector2 newPosition = rigid2D.position + directionV2 * speed * Time.fixedDeltaTime;
            rigid2D.MovePosition(newPosition);
        }
    }

    
    public void CheckIfAnyAdjacentRobot()
    {
        if (!IsAvailable) return;

        foreach (KeyValuePair<Robot, Vector2Int> item in gridManager.robotList)
        {
            if (item.Key.IsAvailable != true)
                continue;

            var V2 = item.Value;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;
            bool otherRobotSeeksPartner = (item.Key.CommunicatingCooldown <= 0);
            bool thisRobotSeeksPartner = (CommunicatingCooldown <= 0);

            if (item.Key != this && (isAdjacentX ^ isAdjacentY) && otherRobotSeeksPartner && thisRobotSeeksPartner)
            {
                Debug.Log("Adjacent Robot");
                testCounter += 1;
                item.Key.testCounter += 1;
                // TODO: There may be some conditions for communicating
                programState = ProgramState.Communicating;
                communicationDuration = 2.5f;
                item.Key.programState = ProgramState.Communicating;
                item.Key.communicationDuration = 2.5f;
            }
        }
    }

    public void CheckIfAnyAdjacentProblem()
    {
        // logic
    }

    public void CheckIfAnyAdjacentRobotIncoming() 
    {
        if (!IsAvailable) return;

        foreach (KeyValuePair<Robot, Vector2Int> item in gridManager.robotTargetList)
        {
            if (item.Key.IsAvailable != true)     
                continue;

            var V2 = item.Value;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;
            bool otherRobotSeeksPartner = (item.Key.CommunicatingCooldown <= 0);
            bool thisRobotSeeksPartner = (CommunicatingCooldown <= 0);

            if (item.Key != this && (isAdjacentX ^ isAdjacentY) && otherRobotSeeksPartner && thisRobotSeeksPartner)
            {
                Debug.Log("Adjacent Robot inc");
                testCounter += 1;
                item.Key.testCounter += 1;
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
        //for (int i = 0; i < gridManager.objectPositions.Count; i++)
        //{
            
        //}

        //Vector2 rayDirection = (Vector2)direction;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, scanProblemDistance);

        //if(hit.transform.TryGetComponent(out Problem problem))
        //{
        //    // mainTargetGridPosition = problem.gridPosition;
        //    // gridManager.robotMainTargetList[this] = mainTargetGridPosition;
        //}
    }

    public void EarlyUpdateState()
    {
        CommunicatingCooldown -= 1;
        
        if (programState == ProgramState.TargetLock || programState == ProgramState.WaitingAnotherRobot || programState == ProgramState.WaitingForNextTurn)
            programState = ProgramState.Moving;
    }
    
    
    public void ActBasedOnState()
    {
        Debug.Log(programState);

        switch (programState)
        {
            case ProgramState.TargetLock:
            case ProgramState.WaitingAnotherRobot:
                break;
            case ProgramState.Knockback:
                break;
            case ProgramState.Moving:
                break;
            case ProgramState.Communicating:
                break;
            case ProgramState.SolvingProblem:
                break;
        }
    }
    
    
    private void ContinuousActBasedOnState()
    {  
        switch (programState)
        {
            case ProgramState.WaitingForNextTurn:
            case ProgramState.Moving:
            case ProgramState.WaitingAnotherRobot:
            case ProgramState.TargetLock:
                break;
            
            // Special Cases
            case ProgramState.SolvingProblem:
                break;
            case ProgramState.Communicating:
                // Slowly it fills up
                communicationDuration -= Time.deltaTime;
                if (communicationDuration < 0)
                {
                    programState = ProgramState.WaitingForNextTurn;
                    CommunicatingCooldown = CommunicatingCooldownMax;
                }
                break;
            case ProgramState.Knockback:
                break;
        }
    }

}




public enum ProgramState
{
    WaitingForNextTurn,
    Moving,
    SolvingProblem,
    WaitingAnotherRobot,
    Communicating,
    TargetLock,
    Knockback
}