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
    [SerializeField] private float CommunicationDurationMax;
    private int CommunicatingCooldown;
    private float communicationDuration;
    private Robot communicationPartner = null;
    
    [SerializeField] private int problemCooldownMax;
    [SerializeField] private float problemDurationMax;
    private int problemCooldown;
    private float problemDuration;
    private Problem problemPartner = null;

    [Header("Grid related info")]
    public Vector2Int gridPosition;
    public Vector2Int targetGridPosition;
    public Vector2Int mainTargetGridPosition;

    [Header("Grid related info")]
    public ProgramState programState;
    private Vector2Int? focusedProblem = null;

    
    private Rigidbody2D rigid2D;
    private GridManager gridManager;


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
            var otherRobot = item.Key;
            
            if (!IsAvailable) continue;
            if (!otherRobot.IsAvailable) continue;
            
            var V2 = otherRobot.gridPosition;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;
            bool otherRobotSeeksPartner = (otherRobot.CommunicatingCooldown <= 0);
            bool thisRobotSeeksPartner = (CommunicatingCooldown <= 0);


            if (item.Key != this && (isAdjacentX ^ isAdjacentY) && otherRobotSeeksPartner && thisRobotSeeksPartner)
            {
                Debug.Log("Adjacent Robot");
                communicationPartner = otherRobot;
                otherRobot.communicationPartner = this;
                programState = ProgramState.Communicating;
                communicationDuration = CommunicationDurationMax;
                otherRobot.programState = ProgramState.Communicating;
                otherRobot.communicationDuration = CommunicationDurationMax;
            }
        }
    }

    public void CheckIfAnyAdjacentProblem()
    {
        if (!IsAvailable) return;

        foreach (Problem problem in gridManager.problemsList)
        {
                        
            if (!IsAvailable) continue;
            if (!problem.IsAvailable) continue;
            
            var V2 = problem.gridPosition;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;

            if ((isAdjacentX ^ isAdjacentY))
            {
                Debug.Log("Adjacent Problem");
                programState = ProgramState.SolvingProblem;
                problemPartner = problem;
                problem.IsAvailable = false;
                problemDuration = problemDurationMax;
            }
        }
    }

    public void CheckIfAnyAdjacentRobotIncoming() 
    {
        if (!IsAvailable) return;

        foreach (KeyValuePair<Robot, Vector2Int> item in gridManager.robotTargetList)
        {
            var otherRobot = item.Key; 
                        
            if (!IsAvailable) continue;
            if (!otherRobot.IsAvailable) continue;

            var V2 = otherRobot.targetGridPosition;
            bool isAdjacentX = Mathf.Abs(gridPosition.x - V2.x) == 1 && Mathf.Abs(gridPosition.y - V2.y) == 0;
            bool isAdjacentY = Mathf.Abs(gridPosition.y - V2.y) == 1 && Mathf.Abs(gridPosition.x - V2.x) == 0;
            bool otherRobotSeeksPartner = (otherRobot.CommunicatingCooldown <= 0);
            bool thisRobotSeeksPartner = (CommunicatingCooldown <= 0);

            if (otherRobot != this && (isAdjacentX ^ isAdjacentY) && otherRobotSeeksPartner && thisRobotSeeksPartner)
            {
                Debug.Log("Adjacent Robot inc");
                programState = ProgramState.WaitingAnotherRobot;
                otherRobot.programState = ProgramState.TargetLock;
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
        problemCooldown -= 1;
        
        if (programState == ProgramState.TargetLock || programState == ProgramState.WaitingAnotherRobot || programState == ProgramState.WaitingForNextTurn)
            programState = ProgramState.Moving;
    }
    
    
    public void ActBasedOnState()
    {
        Debug.Log(programState + ", " + this.name);

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
                // TODO; Slowly it fills up
                problemDuration -= Time.deltaTime;
                if (problemDuration < 0)
                {
                    programState = ProgramState.WaitingForNextTurn;
                    problemCooldown = problemCooldownMax;
                    problemPartner = null;
                }
                break;
            case ProgramState.Communicating:
                // TODO; Slowly it fills up
                communicationDuration -= Time.deltaTime;
                if (communicationDuration < 0)
                {
                    programState = ProgramState.WaitingForNextTurn;
                    CommunicatingCooldown = CommunicatingCooldownMax;
                    communicationPartner = null;
                }
                break;
            case ProgramState.Knockback:
                break;
        }
    }

    public void UpdatePositionBefore()
    {            
        // If we have moved so far, update position
        if (programState == ProgramState.Moving || programState == ProgramState.TargetLock)
        {
            gridPosition = targetGridPosition;
            transform.position = gridManager.gridList[gridPosition];
            gridManager.robotList[this] = gridPosition;
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