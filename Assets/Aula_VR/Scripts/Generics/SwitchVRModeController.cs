using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SwitchVRModeController : MonoBehaviour {

    public bool vrMode;

	// Use this for initialization
	void Start () {
        switchModeVR();
    }
	
	// Update is called once per frame
	public void switchModeVR () {

        if (vrMode==true)
        {
            XRSettings.enabled = true;
        } else if (vrMode == false)
        {
            XRSettings.enabled = false;
        }

    }
}
