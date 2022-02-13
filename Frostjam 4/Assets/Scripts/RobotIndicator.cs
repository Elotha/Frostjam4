using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIndicator : MonoBehaviour
{
    public GameObject indicator;

    public void ActivateCommunicationIndicator()
    {
        indicator.SetActive(true);
    }
    
    public void DeactivateCommunicationIndicator()
    {
        indicator.SetActive(false);
    }
}
