using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIndicator : MonoBehaviour
{
    public GameObject indicatorC;
    public GameObject indicatorM;

    public void ActivateCommunicationIndicator()
    {
        indicatorC.SetActive(true);
    }
    
    public void DeactivateCommunicationIndicator()
    {
        indicatorC.SetActive(false);
    }
    
    public void ActivateMiningIndicator()
    {
        indicatorM.SetActive(true);
    }
    
    public void DeactivateMiningIndicator()
    {
        indicatorM.SetActive(false);
    }
    
    
}
