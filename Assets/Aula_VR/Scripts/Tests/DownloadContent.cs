using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class DownloadContent : MonoBehaviour {

    private MenuButton button;

    // Use this for initialization
    void Start () {
        button = GetComponent<MenuButton>();
        downloadVideo();
    }


    public void downloadVideo()
    {
        WebClient client = new WebClient();

       

        //int sceneIdentifier = button.buttonIndex + 1;

        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

        //To know how much items have to download
        int downloadLength = Directory.GetFiles("http://www.twinforce.es/aulavr").Length;
        //Debug.Log(Directory.GetFiles("http://www.twinforce.es/aulavr/"));

        if (!Directory.Exists(Application.persistentDataPath + "/Download/image"))
        {
            var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Download/scene_1");
        }

        //client.DownloadFile("http://www.twinforce.es/aulavr/video_1.mp4", Application.persistentDataPath + "/Download/scene_1/video_1.mp4");

        ////Download the content into the url to the folder with this name.
        //for (int i = 0; i < downloadLength; i++)
        //{
        //    //If the folder is already created, download the content
        //    if (!Directory.Exists(Application.persistentDataPath + "/Download/scene_" + sceneIdentifier))
        //    {
        //        //if lenght=maxLength download the video "plano_recurso", if not download the scene videos ("video_" + int)
        //        if (i == downloadLength)
        //        {
        //            client.DownloadFile("https://www.twinforce.es/en/aularv" + "/scene_" + sceneIdentifier + "/plano_recurso" + ".mp4", Application.persistentDataPath + "/Download/scene_" + sceneIdentifier + "/plano_recurso" + ".mp4");
        //        }
        //        else
        //        {
        //            client.DownloadFile("https://www.twinforce.es/en/aularv" + "/scene_" + sceneIdentifier + "/video_" + (i + 1) + ".mp4", Application.persistentDataPath + "/Download/scene_" + sceneIdentifier + "/video_" + (i + 1) + ".mp4");
        //        }
        //    }
        //    else //if the folder doesn't exist, create the folder
        //    {
        //        var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Download/scene_" + sceneIdentifier);
        //    }
        //}//close for

    }

    public void deleteVideos()
    {
        int sceneIdentifier = button.buttonIndex + 1;

        //Remove the folder selected
        Directory.Delete(Application.persistentDataPath + "/Download/scene_" + sceneIdentifier);
    }


    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

}
