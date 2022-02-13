using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    /*
    [SerializeField] private float repeatRate = 0.01f;
    [SerializeField] private float knockbackDuration = 0.6f;
    */
    [SerializeField] private float length = 3f;
    [SerializeField] private float duration = 3f;
    [SerializeField]
    private AnimationCurve heightCurve;
    [SerializeField]
    private AnimationCurve speedCurve;


    private ProgrammeLineRenderer _programmeLineRendererScr;
    private LineRenderer _programmeLineRenderer;
    private Rigidbody2D programmeRb;
    private Robot _robot;
    private bool haveNoControll = false;
    
    private void Start()
    {
        _programmeLineRendererScr = GetComponent<ProgrammeLineRenderer>();
        _programmeLineRenderer = GetComponent<LineRenderer>();
        _robot = GetComponent<Robot>();
        programmeRb = GetComponent<Rigidbody2D>();
    }

    public void startKnockback(Vector3 direction)
    {
        deactivateClaimInfo();
        _robot.programState = ProgramState.Knockback;
        //programmeRb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(getKnocked(direction));
        haveNoControll = true;
    }

    private IEnumerator getKnocked(Vector3 direction)
    {
        Vector3 origin = transform.position;
        Vector2 targetPos = (Vector2)origin + (Vector2)direction.normalized * length;
        Debug.Log(origin + " -> " + targetPos);
        float time = 0f;
        Vector3 straightPosition = origin;
        while (time < duration)
        {
            time = Mathf.Min(time + Time.deltaTime, duration);
            float ratio = time / duration;
            float curve1 = speedCurve.Evaluate(ratio);
            straightPosition = Vector2.Lerp(origin, targetPos, curve1);
            float yCurve = heightCurve.Evaluate(ratio);
            
            Vector2 positionWithHeight = new Vector2(straightPosition.x, straightPosition.y + yCurve);
            Debug.Log(positionWithHeight);
            programmeRb.MovePosition(positionWithHeight);
            yield return null;
        }
        programmeRb.velocity = Vector2.zero;
        haveNoControll = false;
        _robot.programState = ProgramState.WaitingForNextTurn;
        findClosestGrid();
        activateClaimInfo();
        //programmeRb.bodyType = RigidbodyType2D.Static;
    }

    void activateClaimInfo()
    {
        _programmeLineRenderer.enabled = true;
        _programmeLineRendererScr.instance.SetActive(true);
    }
    void deactivateClaimInfo()
    {
        _programmeLineRenderer.enabled = false;
        _programmeLineRendererScr.instance.SetActive(false);
    }

    private void findClosestGrid()
    {
        List<float> distances = new List<float>();
        
        for (int i = 0; i < GridManager.Instance.gridList.Count; i++)
        {
            distances.Add(Vector3.Distance(transform.position, GridManager.Instance.gridList.ElementAt(i).Value));
        }

        int minIndex = distances.IndexOf(distances.Min());
        transform.position = GridManager.Instance.gridList.ElementAt(minIndex).Value;
        _robot.gridPosition = GridManager.Instance.gridList.ElementAt(minIndex).Key;
        _robot.targetGridPosition = _robot.gridPosition;
        _robot.SetMainTargetGridPosition();
    }

    /*
    public void startKnockback(Vector3 direction)
    {
        directionOfForce = direction;
        InvokeRepeating("getKnocked", 0f, repeatRate);
    }
    
    public void getKnocked()
    {
        transform.position += new Vector3(speedCurve.Evaluate(knockbackTime) * directionOfForce.normalized.x, heightCurve.Evaluate(knockbackTime) * directionOfForce.normalized.y, transform.position.z);
        knockbackTime += 0.05f;
        if (knockbackTime >= knockbackDuration)
        {
            knockbackTime = 0f;
            CancelInvoke("getKnocked");
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (haveNoControll)
        {
            if (other.gameObject.CompareTag("Border") || other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
