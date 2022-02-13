using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class GridManager : MonoBehaviour
{
    [SerializeField] public float loopTime = 1f;
    [SerializeField] private int robotCount = 5;
    [SerializeField] public int width = 13;
    [SerializeField] public int height = 7;
    [SerializeField] public float cellSize = 1f;
    [SerializeField] public Vector3 startPoint;

    [SerializeField] private Robot robotPrefab;

    public Dictionary<Vector2Int, Vector3> gridList = new Dictionary<Vector2Int, Vector3>();
    public Dictionary<Robot, Vector2Int> robotList = new Dictionary<Robot, Vector2Int>();
    public Dictionary<Robot, Vector2Int> robotTargetList = new Dictionary<Robot, Vector2Int>();
    public Dictionary<Robot, Vector2Int> robotMainTargetList = new Dictionary<Robot, Vector2Int>();
    public List<Vector2Int> objectPositions = new List<Vector2Int>();
    public static GridManager Instance;

    private float timer = 0f;
    private int step = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        SpawnRobots();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= loopTime)
        {
            timer = 0f;
            step += 1;
            
            AllRobotsSetTarget();
            CheckAndSetRobotState();
            MakeThemActBasedOnState();
        }
    }
    
    private void AllRobotsSetTarget()
    {
        for (int i = 0; i < robotList.Count; i++)
        {
            // TODO: Search logic is not completed.
            //foreach (KeyValuePair<Robot, Vector2Int> item in robotList)
            //    item.Key.SearchProblem();
            Robot robot = robotList.ElementAt(i).Key;


            robot.UpdatePositionBefore();
            robot.EarlyUpdateState();
            if (robot.IsAvailableToMove == false) continue;

            
            // Set new target position
            // TODO: check state, if ok call the function
            robot.SetTargetGridPosition();
            robotTargetList[robot] = robot.targetGridPosition;
        }
    }
    
    private void CheckAndSetRobotState()
    {
        foreach (KeyValuePair<Robot, Vector2Int> item in robotList)
            item.Key.CheckIfAnyAdjacentProblem();

        foreach (KeyValuePair<Robot, Vector2Int> item in robotList)
            item.Key.CheckIfAnyAdjacentRobot();   
        
        foreach (KeyValuePair<Robot, Vector2Int> item in robotList)
            item.Key.CheckIfAnyAdjacentRobotIncoming();
    }    

    private void MakeThemActBasedOnState()
    {
        Debug.Log("start of ActBasedOnState");
        foreach (KeyValuePair<Robot, Vector2Int> item in robotList)
            item.Key.ActBasedOnState();
    }


    private void SpawnRobots()
    {
        // Spawn robot and log into robotList
        for (int i = 0; i < robotCount; i++)
        {
            // Select spawn position
            int randomX = Random.Range(0, width - 1);
            int randomY = Random.Range(0, height - 1);
            Vector2Int gridPosition = new Vector2Int(randomX, randomY);
            while (objectPositions.Contains(gridPosition) == true || robotList.ContainsValue(gridPosition) == true)
            {
                randomX = Random.Range(0, width - 1);
                randomY = Random.Range(0, height - 1);
                gridPosition = new Vector2Int(randomX, randomY);
            }

            Robot _robot = Instantiate(robotPrefab, gridList[gridPosition], Quaternion.identity);
            _robot.gridPosition = gridPosition;
            _robot.name = "Robot_" + i;

            robotList.Add(_robot, gridPosition);
            robotTargetList.Add(_robot, _robot.targetGridPosition);
            robotMainTargetList.Add(_robot, _robot.mainTargetGridPosition);
        }
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                Vector2Int gridPosition = new Vector2Int(row, col);
                Vector3 worldPosition = startPoint + new Vector3(row * cellSize, -col * cellSize, 0f);
                gridList.Add(gridPosition, worldPosition);
            }
        }
    }
}
