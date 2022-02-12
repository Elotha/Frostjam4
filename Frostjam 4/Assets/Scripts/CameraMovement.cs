using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float cameraDelayRate = 0.125f;
    private Vector3 cameraOffset = new Vector3(0f, 0f, -10f);

    void FixedUpdate()
    {
        Vector3 positionToBe = player.position + cameraOffset;
        Vector3 delayedMovement = Vector3.Lerp(transform.position, positionToBe, cameraDelayRate);
        transform.position = delayedMovement;
        
        //try it with sprites
        //transform.LookAt(player);
    }
}
