using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{
    [SerializeField] public Vector2Int gridPosition;
    public bool IsAvailable = true;

    void Start()
    {
        GridManager.Instance.problemsList.Add(this);
    }

}
