using UnityEngine;

public class BurakCharacter : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    
    private float horizontalInput;
    private float verticalInput;
    private Vector3 inputDirection;



    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        GetInputs();

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
        {
            Move(inputDirection);
        }   
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        
        //if(Input.GetAxisRaw("Horizontal") > 0f)
        //{
        //    horizontalInput = 1f;
        //}
        //else if (Input.GetAxisRaw("Horizontal") > 0f)
        //{
        //    horizontalInput = -1f;
        //}
        //else if (Input.GetAxisRaw("Horizontal") > 0f)
        //{
        //    horizontalInput = 0f;
        //}
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
