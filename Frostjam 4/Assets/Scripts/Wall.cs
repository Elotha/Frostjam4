using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float secondsAfterDestruction = 3f;
    [SerializeField] private float colliderEnableTime = 0.1f;

    private Animator _animator;
    private static readonly int TimeIsOut = Animator.StringToHash("TimeIsOut");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Invoke(nameof(ActivateCollider), colliderEnableTime);
        StartCoroutine(StartFalling());
    }

    private void ActivateCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(secondsAfterDestruction);
        _animator.SetBool(TimeIsOut, true);
    }

    // This is called from animation event
    public void DestroyWall()
    {
        Destroy(gameObject);
    }
}
