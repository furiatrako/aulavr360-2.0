using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour {

    public GameObject cameraObj;
    public GameObject camParent;
    public Text text;
    public Text seconds;


    [HideInInspector]
    private float initialRotation;
    private float finalRotation;
    public float rotationOverTime;
    private float time=10;
    private float currentTime;

    private bool calibrating;

    public RunCalibration cal;

	// Use this for initialization
	void Start () {
            initialRotation = cameraObj.transform.rotation.eulerAngles.y;
            StartCoroutine(FindRotation(time));
            cal.calibrating = true;
    }

    private void Update()
    {
        if ((time - (Time.time - currentTime)) > 0)
        {
            seconds.text = Convert.ToString(Mathf.RoundToInt(time - (Time.time - currentTime)));
            text.text = "Calibració en curs";
        }
        else
        {
            seconds.text = "0";
            text.text = "Calibració finalitzada";
        }
    }

    IEnumerator FindRotation(float waitingTime)
    {
        currentTime = Time.time;
        calibrating = true;

        yield return new WaitForSeconds(waitingTime);

        finalRotation = cameraObj.transform.rotation.eulerAngles.y;
        CalculateRotationOverTime();
        saveRotation();
    }

    void CalculateRotationOverTime()
    {
        initialRotation = (initialRotation > 180) ? (initialRotation - 360) : initialRotation;
        finalRotation = (finalRotation > 180) ? (finalRotation - 360) : finalRotation;
        rotationOverTime = (finalRotation - initialRotation) / time;
        calibrating = false;
    }

    public void recalibrate()
    {
        if (!calibrating)
        {
            initialRotation = cameraObj.transform.rotation.eulerAngles.y;
            StartCoroutine(FindRotation(time));
            rotationOverTime = 0;
            cal.calibrating = true;
        }
    }

    void saveRotation()
    {
        PlayerPrefs.SetFloat("_rotationCam", (float)rotationOverTime);
    }
}
