using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammeLineRenderer : MonoBehaviour
{
    private Robot _robot;
    private GridManager _gridManager;
    private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _gameObject;
    public GameObject instance;
    private void Start()
    {
        instance = Instantiate(_gameObject);
        _gridManager = GridManager.Instance;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _robot = GetComponent<Robot>();
    }

    private void Update()
    {
        //Vector3 claimedPosition = _gridManager.gridList[_robot.targetGridPosition];
        Vector3 claimedPosition = _gridManager.gridList[_robot.mainTargetGridPosition];
        if (instance.transform.position != claimedPosition)
        {
            instance.transform.position = claimedPosition;
        }
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, claimedPosition);
    }
}
