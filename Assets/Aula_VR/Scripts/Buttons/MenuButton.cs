using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MenuButton : MonoBehaviour {

    //Panels
    public GameObject panelScenes;
    public GameObject panelDownload;
    public GameObject panelRemove;

    public AudioSource aSource;

    //Parent to get the buttons, and panels of every scene
    public GameObject scenes;
    private GameObject[] scenesContainers;
    private GameObject[] activeButton;
    private GameObject[] inactiveButton;
    private GameObject[] removeButton;

    private DownloadData data= new DownloadData();

    private bool[] downloads;

    //Index of buttons to know his position in the array
    public int buttonIndex;

    
    private void Start()
    {
        //Define the index in the arrays
        setPanels();

        //If any array have a data saved, load his data
        for (int i = 0; i < scenes.transform.childCount; i++)
        {
            Load(scenes, i);
        }

        activateApropiatePanel();
    }


    private void Update()
    {
    }


    //If the panels is already download, activate the active button.
    public void activateApropiatePanel()
    {
        for (int i = 0; i < scenes.transform.childCount; i++)
        {
            if (downloads[i] == true)
            {
                activeButton[i].SetActive(true);
                removeButton[i].SetActive(true);
                inactiveButton[i].SetActive(false);
            }
            else
            {
                inactiveButton[i].SetActive(true);
                activeButton[i].SetActive(false);
                removeButton[i].SetActive(false);
            }

        }
    }

    //Set the gameObjects in the arrays
    public void setPanels()
    {
        //Get the length of the buttons and define the bool to know when is downloaded.
        scenesContainers = new GameObject[scenes.transform.childCount];
        activeButton = new GameObject[scenes.transform.childCount];
        inactiveButton = new GameObject[scenes.transform.childCount];
        removeButton= new GameObject[scenes.transform.childCount];
        downloads = new bool[scenes.transform.childCount];
        data.downloads = new bool[scenes.transform.childCount];

        //Set the parent of the buttons
        for (int i=0; i < scenes.transform.childCount; i++)
        {
            scenesContainers [i] = scenes.transform.GetChild(i).gameObject;            
        }

        //Get the buttons
        int a = 0;
        foreach (GameObject scene in scenesContainers)
        {
            activeButton[a] = scene.transform.GetChild(1).gameObject;
            inactiveButton[a] = scene.transform.GetChild(2).gameObject;
            removeButton[a] = scene.transform.GetChild(0).gameObject;
            a++;
        }
    }


    //Get the button position on his array
    public void getButton()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            for (int i = 0; i < inactiveButton.Length; i++)
            {
                if (hit.collider.gameObject.name == inactiveButton[i].gameObject.name || hit.collider.gameObject.name == removeButton[i].gameObject.name)
                {
                    buttonIndex = i;
                }
            }//Close for

        }//Close if
    }


    //Activate or deactivate the button selected
    public void toggleActiveAndInactiveButton()
    {
        //Defines when the panels are enable or disable
        activeButton[buttonIndex].SetActive(!activeButton[buttonIndex].activeInHierarchy);
        inactiveButton[buttonIndex].SetActive(!inactiveButton[buttonIndex].activeInHierarchy);
        removeButton[buttonIndex].SetActive(!removeButton[buttonIndex].activeInHierarchy);
        setDownload();
        
    }


    //Set the bool of download for the scene selected.
    public void setDownload()
    {
        //Define when the download bool of this button is true or false
        downloads[buttonIndex] = !downloads[buttonIndex];

        //Save a bool data for a scene to know when this scene is already downloaded
        for (int i = 0; i < scenes.transform.childCount; i++)
        {
            if (i == buttonIndex)
            {
                Save(scenes, i);
            }
        }
    }


    //Activate or deactivate the download panel
    public void toggleDownloadPanel()
    {
        panelScenes.SetActive(!panelScenes.activeInHierarchy);
        panelDownload.SetActive(!panelDownload.activeInHierarchy);
    }


    //Activate or deactivate the remove panel
    public void toggleRemovePanel()
    {
        panelScenes.SetActive(!panelScenes.activeInHierarchy);
        panelRemove.SetActive(!panelRemove.activeInHierarchy);
    }


    //Load a custom scene set in the button
    public void chargeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    //Use it to play sounds
    public void playSound(AudioClip newClip)
    {
        aSource.clip = newClip;
        aSource.Play();
    }


    //Use it to load
    public void Save(GameObject scene, int boolToSave)
    {
        //Get the scene name and save a file with his name
        scene = scenes.transform.GetChild(boolToSave).gameObject;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/downloadsInfo_" + scene.name + ".txt");

        //Save the bool data in this file
        DownloadData data = new DownloadData();
        data.downloads = downloads;
        
        bf.Serialize(file, data);
        file.Close();
    }


    //Use it to load
    public void Load(GameObject scene, int boolToLoad)
    {
        //Get the scene name and load a file with his name
        scene = scenes.transform.GetChild(boolToLoad).gameObject;
        if (File.Exists(Application.persistentDataPath + "/downloadsInfo_" + scene.name + ".txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/downloadsInfo_" + scene.name + ".txt", FileMode.Open);
            DownloadData data = (DownloadData)bf.Deserialize(file);
            file.Close();

            //Load the bool data of this file
            downloads[boolToLoad] = data.downloads[boolToLoad];
            
        }
    }
}

[Serializable]
public class DownloadData
{
    //This is what we need to save and load the bool data
    public bool[] downloads;
}
