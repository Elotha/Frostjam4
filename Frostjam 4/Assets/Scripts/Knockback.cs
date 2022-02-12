using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve heightCurve;
    [SerializeField]
    private AnimationCurve speedCurve;

    private float knockbackTime;


    public void getKnocked(Vector3 direction)
    {
        transform.position += new Vector3(speedCurve.Evaluate(knockbackTime) * direction.normalized.x, heightCurve.Evaluate(knockbackTime) * direction.normalized.y, transform.position.z);
    }

    private void Update()
    {
        /*
        if (knockbackTime < 1f)
        {
            getKnocked();
        }
        */
    }
}
