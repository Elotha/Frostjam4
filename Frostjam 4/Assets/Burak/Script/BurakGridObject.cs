using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurakGridObject : MonoBehaviour
{
    public Vector2Int gridPosition;
    private float gridSize;
    public Transform inside;

    private BurakGridManager gridManager;

    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = BurakGridManager.Instance;
        gridSize = gridManager.cellSize;

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        DetectObjects();
    }

    public Transform GetGridInfo()
    {


        return null;
    }

    private void DetectObjects()
    {
        float offset = 0.05f;
        sprite.color = Color.white;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(gridSize/2f - offset, gridSize/2f - offset), 0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer != gameObject.layer)
            {
                inside = hitCollider.transform;
                Debug.DrawLine(transform.position, hitCollider.transform.position,Color.red);
                sprite.color = Color.green;
            }
        }
    }
}
