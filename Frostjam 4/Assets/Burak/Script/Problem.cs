using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{
    [SerializeField] public Vector2Int gridPosition;
    [SerializeField] public float RemainingProblemInSeconds;
    public bool IsAvailable = true;

    void Start()
    {
        GridManager.Instance.problemsList.Add(this);
    }

    public void ReduceProblem()
    {
        RemainingProblemInSeconds -= Time.deltaTime;
        if (RemainingProblemInSeconds < 0)
        {
            GridManager.Instance.problemsList.Remove(this);
            GameManager.Instance.UpdateProblemLeft();
            Destroy(gameObject);
        }
    }
}
