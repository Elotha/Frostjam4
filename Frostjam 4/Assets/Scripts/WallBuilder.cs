using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    [SerializeField] private float distanceBetweenWalls = 4f;

    [SerializeField] private float movementMultiplier = 2f;

    [SerializeField] private float wallBuildingDuration = 3f;
    [SerializeField] private Collider2D myCollider2D;
    
    public Transform parent;
    public GameObject wall;

    private Vector3 lastWallPos;
    private PlayerMovement _playerMovement;
    private bool creating = false;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            //_animator.speed = movementMultiplier;
            StartCoroutine("BuildWallsFor");
            //Physics.IgnoreLayerCollision(6, 7, true);
            creating = true;
            _animator.speed = 2.0f;
            _playerMovement.multiplierModifier = movementMultiplier;
            gameObject.layer = LayerMask.NameToLayer("PlayerElevated");
            lastWallPos = transform.position;
        }
        /*
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            //Physics.IgnoreLayerCollision(6, 7, false);
            creating = false;
            _playerMovement.multiplier = 1;
        }
        */
        if (creating)
        {
            adjust();
        }
    }

    IEnumerator BuildWallsFor()
    {
        yield return new WaitForSeconds(wallBuildingDuration);
        creating = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
        _animator.speed = 1;
        _playerMovement.multiplierModifier = 1;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (creating)
        {
            if (other.gameObject.CompareTag("Programme"))
            {
                //Vector3 directionOfForce = other.gameObject.transform.position - transform.position;
                other.gameObject.GetComponent<Knockback>().startKnockback(_playerMovement.movementVec);
                /*
                Rigidbody2D programmeRb = other.gameObject.GetComponent<Rigidbody2D>();
                programmeRb.AddForce(directionOfForce * ForceMultiplier, ForceMode2D.Impulse);
                */
            }
        }
    }
}


