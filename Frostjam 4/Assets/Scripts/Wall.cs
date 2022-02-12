using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float secondsAfterDestruction = 3f;
    void Start()
    {
        Invoke("ActivateCollider", 0.1f);
        StartCoroutine(DestroySelfAfter());
    }

    void ActivateCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator DestroySelfAfter()
    {
        yield return new WaitForSeconds(secondsAfterDestruction);
        Destroy(gameObject);
    }
}
