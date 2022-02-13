using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{
    [SerializeField] public Vector2Int gridPosition;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GridManager.Instance.objectPositions.Add(gridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
