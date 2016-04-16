using UnityEngine;
using System.Collections;

public class FPControllerCam : MonoBehaviour {

    public Camera mainCamera;

    bool teleportCamOn = false;
    public Camera teleportCamera;
    private bool inTeleport = false;
    private float teleportSpeed = 3.0f;

    public float speed = 3.0F;
    public float mouseSensitivity = 5.0f;
    public bool floating = false;
    float verticalRotation = 0;

    private bool buttonPressed = false;

    private Vector3 moveDirection = Vector3.zero;

    public float detectAngle = 15;

    private GameObject[] glowObjects;
    private GameObject lastGlowObject = null;

    void Start()
    {
        glowObjects = GameObject.FindGameObjectsWithTag("space");
        if (teleportCamera)
            teleportCamera.rect = new Rect(0.7f, 0.7f, 0.2f, 0.2f);
    }

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity; ;
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        teleportCamera.transform.rotation = transform.rotation * mainCamera.transform.localRotation;

        if (inTeleport)
        {
            /* first we scale the teleport cam */
            if (teleportCamera.rect != mainCamera.rect)
            {
                Vector2 size = Vector2.MoveTowards(teleportCamera.rect.size, mainCamera.rect.size, teleportSpeed*Time.deltaTime);
                Vector2 position = Vector2.MoveTowards(teleportCamera.rect.position, mainCamera.rect.position, teleportSpeed*Time.deltaTime);
 
                teleportCamera.rect = new Rect(position, size);
            }
            /* now we do the actual teleport */
            else
            {
                transform.position = teleportCamera.transform.position;
                inTeleport = false;
                teleportCamOn = false;
                showTeleportCam(teleportCamOn);
                teleportCamera.rect = new Rect(0.7f, 0.7f, 0.2f, 0.2f);
            }
        }
        else
        {

            if (!teleportCamOn)
                ToggleGlow();

            ProcessInput();

            float angleRad = -(verticalRotation * Mathf.PI) / 180;
            float forward = Input.GetAxis("Vertical");
            float up = Mathf.Sin(angleRad) * forward;

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), up, forward * Mathf.Cos(angleRad));

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            moveDirection.y -= Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    void ProcessInput()
    {
        /* we teleport to this direction */
        if (!buttonPressed)
        {
            if (Input.GetButtonDown("Fire2") && teleportCamOn)
            {
                // we don't save the button pressed state, since we so the teleport now
                inTeleport = true;
            }

            if (Input.GetButtonDown("Fire1") && lastGlowObject)
            {
                buttonPressed = true;

                // toggle teleport cam
                teleportCamOn = !teleportCamOn;
                showTeleportCam(teleportCamOn);
            }
        }
        else
        {
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
            {
                buttonPressed = false;
            }
        }
    }

    void ToggleGlow()
    {
        float besthit = 190; // can never exceed 180
        GameObject toGlow = null;
        foreach (GameObject space in glowObjects)
        {
            float hit = Vector3.Angle(mainCamera.transform.forward, space.transform.position - transform.position);
            if (hit < detectAngle && hit < besthit)
            {
                besthit = hit;
                toGlow = space;
            }

        }

        /* update the glow object - decativate the last one */
        if (toGlow)
        {
            if (toGlow != lastGlowObject)
            {
                if (lastGlowObject)
                {
                    lastGlowObject.GetComponent<Glow>().GlowOff();
                }
                lastGlowObject = toGlow;
                lastGlowObject.GetComponent<Glow>().GlowOn();
				Debug.Log ("glow obj " + toGlow.gameObject.name);
                /* set Camera in from of Object */
                Ray ray = new Ray(lastGlowObject.transform.position,  mainCamera.transform.position - lastGlowObject.transform.position);
                teleportCamera.transform.position = ray.GetPoint(15);
            }
        }
        else
        {
            if (lastGlowObject)
            {
                lastGlowObject.GetComponent<Glow>().GlowOff();
                lastGlowObject = null;
            }
        }
    }

    void showTeleportCam(bool on)
    {
        teleportCamera.enabled = on;
    }
}
