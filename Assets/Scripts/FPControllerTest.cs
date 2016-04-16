using UnityEngine;

public class FPControllerTest : MonoBehaviour
{
    public float speed = 3.0F;
    public float mouseSensitivity = 5.0f;
    public bool floating = false;
    float verticalRotation = 0;

    private bool buttonPressed = false;

    private Vector3 moveDirection = Vector3.zero;
    
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
 
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        //verticalRotation = Mathf.Clamp(verticalRotation, -60.0f, 60.0f);
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        if (Input.GetButtonDown("Fire1") && !buttonPressed)
        {
            buttonPressed = true;

            // trigger teleport
            GetComponent<Teleport>().doTeleport();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            buttonPressed = false;
        }


        if (floating == true)
        {
            float angleRad = -(verticalRotation * Mathf.PI) / 180;
            float forward = Input.GetAxis("Vertical"); 
            float up = Mathf.Sin(angleRad) * forward;

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), up, forward * Mathf.Cos(angleRad));
        }
        else
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        moveDirection.y -= Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

    }  
}