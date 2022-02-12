using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField]
    private float distanceBetweenWalls = 0.5f;

    [SerializeField] private float movementMultiplier = 2f;
    
    public Transform parent;
    public GameObject wall;

    private Vector3 lastWallPos;
    private PlayerMovement _playerMovement;
    private bool creating = false;

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            creating = true;
            _playerMovement.multiplier = movementMultiplier;
            Instantiate(wall, transform.position, wall.transform.rotation, parent);
            
            lastWallPos = transform.position;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            creating = false;
            _playerMovement.multiplier = 1;
        }
        else
        {
            if (creating)
            {
                adjust();
            }
        }
    }
    
    /*
    void startBuilding()
    {
        creating = true;
        _playerMovement.multiplier = 2;
        start.transform.position = transform.position;
        
        start.SetActive(true);
    }

    void closeBuilding()
    {
        creating = false;
        _playerMovement.multiplier = 1;
        
        start.SetActive(false);
        wall.SetActive(false);
    }
    
    void adjust()
    {
        float distance = Vector3.Distance(start.transform.position, transform.position);
        
        Vector3 difference = transform.position - start.transform.position;
        
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        start.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90f);
        wall.transform.rotation = start.transform.rotation;
        
        wall.transform.position = start.transform.position + (transform.position - start.transform.position) / 2;
        wall.transform.localScale = new Vector3(wall.transform.localScale.x, distance);
        wall.SetActive(true);
    }
    */

    void adjust()
    {
        if (Vector3.Distance(lastWallPos, transform.position) > distanceBetweenWalls)
        {
            Instantiate(wall, transform.position, wall.transform.rotation, parent);

            lastWallPos = transform.position;
        }
    }

    /*
    void rotator(GameObject willBeRotated)
    {
        Vector3 difference = transform.position - willBeRotated.transform.position;
        
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        willBeRotated.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + 90f);
    }
    */
}
