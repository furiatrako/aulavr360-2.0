using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VR_ButtonController : MonoBehaviour {

	// Uses cardboard's raycast instead of unity's

    private bool gazeAt;
    public float gazeTime = 2f;


    private float timer;
    private Image imageButton;
    private void Start()
    {

	}

	
    void Update () {

        //Raycast to get the button that have to be clicked
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Question")
            {
                //Fill button images to get feedback.
                imageButton = hit.transform.GetChild(0).gameObject.GetComponent<Image>();
                imageButton.fillAmount = timer / gazeTime;
            }

            
            if (gazeAt == true)
            {
                timer += Time.deltaTime;

                if (timer >= gazeTime)
                {
                    //Make a click and reset time
                    ExecuteEvents.Execute(hit.collider.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
                    timer = 0f;
                }
            }

        }
	}
	

	//Put on the event trigger of a Button to know when the gaze start doing raycast
	public void PointerEnter()
    {
        gazeAt = true;
		imageButton.fillAmount = 0;
    }


    //Put on the event trigger of a Button to know when the gaze stop doing raycast
    public void PointerExit()
    {
        gazeAt = false;
        timer = 0;
        imageButton.fillAmount = 0;
    }
}
