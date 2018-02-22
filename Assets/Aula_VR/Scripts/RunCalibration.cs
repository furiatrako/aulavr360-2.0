using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCalibration : MonoBehaviour {

    public GameObject camParent;
    public bool calibrating;
    public Calibration cal;

    [HideInInspector]
    public float rotationOverTime;


    // Use this for initialization
    void Start () {

        if (!calibrating)
        {
            LoadRotation();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (calibrating)
        {
            rotationOverTime = cal.rotationOverTime;
        }
        
        camParent.transform.Rotate(-(Vector3.up * rotationOverTime) * Time.deltaTime);
        Debug.Log(rotationOverTime);
        //camParent.transform.Rotate((Vector3.up * -11) * Time.deltaTime);
    }


    void LoadRotation()
    {
        rotationOverTime= PlayerPrefs.GetFloat("_rotationCam");
    }

}
