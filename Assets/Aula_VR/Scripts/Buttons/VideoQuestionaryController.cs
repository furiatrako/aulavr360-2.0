using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class VideoQuestionaryController : MonoBehaviour
{

	// This will be the name of all canvases, followed by "-" and the identifier (int+char)
	protected string ObjectName = "Questionary_Canvas";

    //Automatic selected panel to open or close.
	private GameObject QuestionsToShow;
    
    //Array where save all questions
	private GameObject[] Questionaries;

    //VideoPlayer to play videos
	public VideoPlayer vPlayer;

    //Parent panel to search all question
	public GameObject questions;

	// Result of getting Level + ButtonName. Used to load next question
	public string identifier;

	// Bool to control video playing and execution
	private bool control = false;
    private bool paused=false;

    //Audio Source to play sounds
    public AudioSource aSource;

    //Panels to control pause and play
    public GameObject playPanel;
    public GameObject pausePanel;

    //Initial video to play
    public AudioSource mainAudioSource;
    public Renderer reticleRenderer;
    Color reticleColor = Color.red;

    // Use this for initialization
    void Start()
	{
        playPanel.SetActive(false);
        identifier = "0x";
		getChildInQuestions();
        HideReticle();
    }


    //Use to set all questions in the scene
	public void getChildInQuestions()
	{
		Questionaries = new GameObject[questions.transform.childCount];

		for (int i = 0; i < questions.transform.childCount; i++)
		{
			Questionaries[i] = questions.transform.GetChild(i).gameObject;
		}
	}


	// Update is called once per frame
	void Update()
	{
		//When the video ends, call the function endReached()
		if (control == false && !vPlayer.isPlaying && paused==false)
		{
			endReached();
			control = true;
            stopAudio();
        }
	}


	//Set a Video to play in the button.
	public void loadVideo(VideoClip video)
	{

        //Find the button pressed and set  the identifier
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			identifier = hit.collider.name;
		}

        //Stop the video before change
        vPlayer.Stop();
        mainAudioSource.Stop();

        //Change and play the video
        StartCoroutine(testFunction(video, 0.1f));
	}


    //Change and play the video with delay to solve problems with Stop and Play        
    IEnumerator testFunction(VideoClip video, float delay)
    {
        yield return new WaitForSeconds(delay);

        //vPlayer.url= Path.Combine(Application.persistentDataPath + "/Download/" + Application.loadedLevelName, video + ".mp4");
        vPlayer.clip = video;
        vPlayer.Play();
        control = false;
    }


	//Add To Close Questionary set in Inspector.
	public void closeQuestionary()
	{
		// Hides actual set of questions
		QuestionsToShow.SetActive(false);
        HideReticle();
	}


	public void openQuestionary()
	{
		// Shows actual set of questions
		QuestionsToShow.SetActive(true);
        ShowReticle();
    }


	//Call it when the video end, this function change the video and open the questionary panel
	public void endReached()
	{        
        foreach (GameObject Questionary in Questionaries)
		{
			// Sets the identifier as the value to search
			// Out of every canvas name, searches for the identifier
            String q = string.Format("Questionary_Canvas-{0}", identifier);
            if (q == Questionary.name) // If the generated identifier matches a Questionary_Canvas
			{
				// Show the child of the matching canvas
				QuestionsToShow = Questionary.transform.GetChild(0).gameObject;
				openQuestionary();
			}
		} // Close foreach
	}


    //Load a scene set in the button
	public void loadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}


    //Pause the video and display the play panel
    public void pauseVideo()
    {
        playPanel.SetActive(true);
        pausePanel.SetActive(false);
        paused = true;
        vPlayer.Pause();
		mainAudioSource.Pause();
    }


    //Play the video and display the pause panel
    public void continueVideo()
    {
        pausePanel.SetActive(true);
        playPanel.SetActive(false);
        paused = false;
        vPlayer.Play();
		mainAudioSource.Play();
	}


    //Play a custom sound set in the button
    public void playAudio(AudioClip aClip)
    {       
        aSource.clip = aClip;
        aSource.Play();
    }

    public void stopAudio()
    {
        mainAudioSource.Stop();
    }

    public void playMainAudio(AudioClip aClip)
    {
        mainAudioSource.clip = aClip;
        mainAudioSource.Play();
    }

    private void ShowReticle()
    {
        reticleRenderer.material.color = reticleColor;
    }


    private void HideReticle()
    {
        reticleColor = reticleRenderer.material.color;
        reticleRenderer.material.color = new Color(0, 0, 0, 0);
    }
}
