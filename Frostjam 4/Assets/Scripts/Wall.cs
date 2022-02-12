using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float secondsAfterDestruction = 3f;
    void Start()
    {
        StartCoroutine(DestroySelfAfter());
    }

    IEnumerator DestroySelfAfter()
    {
        yield return new WaitForSeconds(secondsAfterDestruction);
        Destroy(gameObject);
    }
}
