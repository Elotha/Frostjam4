using System;
using System.Collections;
using System.Collections.Generic;
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

    private Rigidbody2D programmeRb;
    private bool haveNoControll = false;
    
    private void Start()
    {
        programmeRb = GetComponent<Rigidbody2D>();
    }

    public void startKnockback(Vector3 direction)
    {
        programmeRb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(getKnocked(direction));
        haveNoControll = true;
    }

    private IEnumerator getKnocked(Vector3 direction)
    {
        Vector3 origin = transform.position;
        Vector2 targetPos = (Vector2)origin + (Vector2)direction.normalized * length;
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
            programmeRb.MovePosition(positionWithHeight);
            yield return null;
        }
        programmeRb.velocity = Vector2.zero;
        haveNoControll = false;
        programmeRb.bodyType = RigidbodyType2D.Static;
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
