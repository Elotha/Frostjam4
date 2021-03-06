using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{
    [SerializeField] public Vector2Int gridPosition;
    [SerializeField] public float RemainingProblemInSeconds;
    [SerializeField] public Transform remainigPartIndicator;
    public bool IsAvailable = true;

    private float startRemainingtime;

    private GridManager gridManager;

    void Start()
    {
        gridManager = GridManager.Instance;
        startRemainingtime = RemainingProblemInSeconds;
    }

    public void ReduceProblem()
    {
        RemainingProblemInSeconds -= Time.deltaTime;
        remainigPartIndicator.localScale = new Vector3(RemainingProblemInSeconds / startRemainingtime, 
                            remainigPartIndicator.localScale.y, remainigPartIndicator.localScale.z);
        remainigPartIndicator.localPosition = new Vector3(-0.49f + RemainingProblemInSeconds / startRemainingtime / 2f,
                            remainigPartIndicator.localPosition.y, remainigPartIndicator.localPosition.z);


        if (RemainingProblemInSeconds < 0)
        {
            gridManager.problemsList.Remove(this);
            GameManager.Instance.UpdateProblemLeft();
            Destroy(gameObject);
        }
    }
}
