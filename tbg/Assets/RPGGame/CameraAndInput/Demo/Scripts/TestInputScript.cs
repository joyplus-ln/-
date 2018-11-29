using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInputScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RPGInputManager.useMobileInputOnNonMobile = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (RPGInputManager.GetButtonDown("Jump"))
            Debug.Log("Press Jump");
        if (RPGInputManager.GetButtonUp("Jump"))
            Debug.Log("Release Jump");
        if (RPGInputManager.GetButton("Jump"))
            Debug.Log("Hold Jump");

        float hAxis = RPGInputManager.GetAxis("Horizontal", false);
        float vAxis = RPGInputManager.GetAxis("Vertical", false);
        if (hAxis != 0)
            Debug.Log("hAxis " + hAxis);
        if (vAxis != 0)
            Debug.Log("vAxis " + vAxis);
    }
}
