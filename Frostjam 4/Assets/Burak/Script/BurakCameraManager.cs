using UnityEngine;

public class BurakCameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float zOffset = -10f;
    [SerializeField] private float smoothStep = 3f;

    private Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, zOffset);

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, smoothStep * Time.deltaTime);

        transform.position = currentPosition;
    }
}
