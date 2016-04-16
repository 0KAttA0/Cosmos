using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public float teleportSpeed = 15.0F;
    public float minDistance = 1.5f;
    public float detectAngle = 15;

    GameObject[] glowObjects;

    GameObject lastGlowObject = null;
    bool inTeleport = false;
    float distance = 0;

    // Use this for initialization
    void Start () {
        glowObjects = GameObject.FindGameObjectsWithTag("space");
	}
	
	// Update is called once per frame
	void Update () {

        /* move to object - maybe GetComponent<CharacterController>().Move() is better because of collision */
        if (inTeleport && distance > minDistance)
        {
            // TODO add speed up - speed down
            //var newDistance = Vector3.Distance(transform.position, lastGlowObject.transform.position);

            float step = teleportSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastGlowObject.transform.position, step);

            if (Vector3.Distance(transform.position, lastGlowObject.transform.position) <= minDistance)
            {
                inTeleport = false;
            }
        }
        /* detect new possible object */
        else
        {
            float besthit = 190; // can never exceed 180
            GameObject toGlow = null;
            foreach (GameObject space in glowObjects)
            {
                float hit = Vector3.Angle(Camera.main.transform.forward, space.transform.position - transform.position);
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
    }

    public void doTeleport()
    {
        if (!inTeleport && lastGlowObject)
        {
            distance = Vector3.Distance(transform.position, lastGlowObject.transform.position);
            inTeleport = true;
            Debug.Log("teleport to " + lastGlowObject.ToString() + " distance " + distance);
        }
    }
}
