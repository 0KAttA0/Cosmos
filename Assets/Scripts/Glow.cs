using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour {

    private Behaviour m_halo;
    // Use this for initialization
    void Start () {
         m_halo = (Behaviour)GetComponent("Halo");
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void GlowOn()
    {
        m_halo.enabled = true;
    }

    public void GlowOff()
    {
        m_halo.enabled = false;
    }
}
