using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurakGridManager : MonoBehaviour
{
    [SerializeField] public float cellSize;
    [SerializeField] public Vector3 startPoint;
    [SerializeField] public Vector3 endPoint;
    [SerializeField] private int width;
    [SerializeField] private int height;

    // Grid object prefab, it contains Rigidbody2D and collider
    [SerializeField] private BurakGridObject gridPrefab;
    // Holds all the grid objects with corresponding positions
    private Dictionary<Vector2Int, BurakGridObject> gridDictionaryList = new Dictionary<Vector2Int, BurakGridObject>();
    // This will be used to spawn programs at random location
    private Dictionary<Vector2Int, BurakGridObject> assignedGridList = new Dictionary<Vector2Int, BurakGridObject>();
    // Reference to GridManager
    public static BurakGridManager Instance;

    // Trial
    [SerializeField] private Robot robotPrefab;

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

        endPoint = startPoint + new Vector3((width-1) * cellSize, -(height-1) * cellSize, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Robot robot = (Robot) Instantiate(robotPrefab, transform.position, Quaternion.identity);

            Vector3 offset = new Vector3(0.3f, 0.4f, 0f);
            GetClosestGrid(transform.position + offset);
        }
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < width; row++)
        {
            for (int column = 0; column < height; column++)
            {
                BurakGridObject _gridObject = (BurakGridObject)Instantiate(gridPrefab, 
                                    startPoint + new Vector3(row * cellSize, -column * cellSize, 0), Quaternion.identity, transform);
                _gridObject.name = "GridObject_" + row + "_" + column;
                _gridObject.gridPosition = new Vector2Int(row, column);
                Vector2Int position = new Vector2Int(row, column);
                gridDictionaryList.Add(position, _gridObject);
            }
        }
    }

    public void GetClosestGrid(Vector3 _position)
    {
        Vector2Int closestPosition;
        float distance = Vector3.Distance(_position, gridDictionaryList.First().Value.transform.position);
        foreach (KeyValuePair<Vector2Int, BurakGridObject> grid in gridDictionaryList)
        {
            if(Vector3.Distance(_position, grid.Value.transform.position) < distance)
            {
                distance = Vector3.Distance(_position, grid.Value.transform.position);
                closestPosition = grid.Key;
            }
        }
        //return gridDictionaryList[closestPosition];
        Debug.Log("Closest distance: " + distance);
    }

    private BurakGridObject GetGridInfo(Vector2Int _position)
    {
        return gridDictionaryList[_position];
    }

    public Vector3 GetRandomPositionOnGrid()
    {
        if (assignedGridList.Count >= width * height)
        {
            // Find another solution
            return new Vector3(0f, 0f, 0f);
        }

        int randomX = Random.Range(0, width);
        int randomY = Random.Range(0, height);
        Vector2Int position = new Vector2Int(randomX, randomY);
        while (assignedGridList.ContainsKey(position) == true)
        {
            Debug.Log("Recalculate position");
            randomX = Random.Range(0, width);
            randomY = Random.Range(0, height);
            position = new Vector2Int(randomX, randomY);
        }
        
        assignedGridList.Add(position, GetGridInfo(position));

        Vector3 point = startPoint + new Vector3(position.x * cellSize, -position.y * cellSize, 0f);
        return point;
    }
}
