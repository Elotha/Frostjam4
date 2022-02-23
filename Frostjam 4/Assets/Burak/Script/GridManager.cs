using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] public float loopTime = 1f;
    [SerializeField] private int robotCount = 5;
    [SerializeField] private int problemCount = 6;
    [SerializeField] public int width = 13;
    [SerializeField] public int height = 7;
    [SerializeField] public float cellSize = 1f;
    [SerializeField] public Vector3 startPoint;

    [SerializeField] private Robot robotPrefab;
    [SerializeField] private Problem problemPrefab;
    [SerializeField] private GameObject borderPrefab;

    public Dictionary<Vector2Int, Vector3> gridList = new Dictionary<Vector2Int, Vector3>();
    public Dictionary<Robot, Vector2Int> robotList = new Dictionary<Robot, Vector2Int>();
    public Dictionary<Robot, Vector2Int> robotTargetList = new Dictionary<Robot, Vector2Int>();
    public Dictionary<Robot, Vector2Int> robotMainTargetList = new Dictionary<Robot, Vector2Int>();
    public Dictionary<Problem, Vector2Int> problemsList = new Dictionary<Problem, Vector2Int>();
    public List<Vector2Int> objectPositions
    {
        // returns temporary
        get
        {
            var list = new List<Vector2Int>();
            list.AddRange(blockPositionList);
            foreach (var problem in problemsList)
                list.Add(problem.Value);
            return list;
        }
    }
    public List<BlockPositions> blockPositions = new List<BlockPositions>();
    
    public List<Vector2Int> blockPositionList = new List<Vector2Int>();

    [System.Serializable]
    public class BlockPositions
    {
        public Vector2Int startPosition;
        public Vector2Int endPosition;
    }
    
    public static GridManager Instance;

    private float timer = 0f;
    private int step = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            // Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        // GenerateGridBorders();

        SpawnDeadCells();
        SpawnProblems();
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
            if (robot.IsMoving == false) continue;

            
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
        // Debug.Log("start of ActBasedOnState");
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

    private void SpawnProblems()
    {
        // Spawn problems and log into problemList
        for (int i = 0; i < problemCount; i++)
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

            Problem _problem = Instantiate(problemPrefab, gridList[gridPosition], Quaternion.identity);
            _problem.gridPosition = gridPosition;
            _problem.name = "Problem_" + i;

            problemsList.Add(_problem, gridPosition);
        }
    }

    private void SpawnDeadCells()
    {
        foreach (var block in blockPositions)
        {
            for (int i = block.startPosition.x; i < block.endPosition.x; i++)
            {
                for (int j = block.startPosition.y; j < block.endPosition.y; j++)
                {
                    blockPositionList.Add(new Vector2Int(i, j));
                }
            }
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
    
    private void GenerateGridBorders()
    {
        // Top, Down
        for (int row = -1; row < width + 1; row++)
        {
            var borderTransformA = Instantiate(borderPrefab).transform;
            var borderTransformB = Instantiate(borderPrefab).transform;

            borderTransformA.position = startPoint + new Vector3(row * cellSize, 1 * cellSize, 0f);
            borderTransformB.position = startPoint + new Vector3(row * cellSize, -(height - 0) * cellSize, 0f);
        }
        
        // Left, Right
        for (int col = 0; col < height; col++)
        {
            var borderTransformA = Instantiate(borderPrefab).transform;
            var borderTransformB = Instantiate(borderPrefab).transform;

            borderTransformA.position = startPoint + new Vector3(-1 * cellSize, -col * cellSize, 0f);
            borderTransformB.position = startPoint + new Vector3((width) * cellSize, -col * cellSize, 0f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, 0.25f);
        Gizmos.DrawWireSphere(startPoint + width * Vector3Int.right
                              + height * Vector3Int.down, 0.25f);
        foreach (var gridItem in gridList)
        {
            Gizmos.DrawWireCube(gridItem.Value, new Vector3(cellSize, cellSize, 0f));
        }

        // This is for dead grid cells
        Gizmos.color = Color.red;
        for (int i = 0; i < blockPositionList.Count; i++)
        {
            Gizmos.DrawWireSphere(gridList[blockPositionList[i]], 0.4f);
        }
        
    }
}
