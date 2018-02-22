using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour {

    public SobreImpresiones text1;
    public SobreImpresiones text2;
    public SobreImpresiones text3;
    public SobreImpresiones text4;
    public string sceneName;

    public float timeToStartText1;
    private float timeToStartText2;
    private float timeToStartText3;
    private float timeToStartText4;
    private float timeToChangeScene;

    private bool text1faded;
    private bool text2faded;
    private bool text3faded;
    private bool text4faded;



    private void Start()
    {
        timeToStartText2 = timeToStartText1+text1.timeToStay + 2;
        timeToStartText3 = timeToStartText2 + text2.timeToStay + 2;
        timeToStartText4 = timeToStartText3 + text3.timeToStay + 2;

        timeToChangeScene = 200;
    }

    public void Update()
    {

        if (Time.timeSinceLevelLoad > timeToStartText1 && text1faded == false)
        {
            timeToChangeScene = Time.timeSinceLevelLoad + text1.timeToStay + text2.timeToStay + text3.timeToStay + text4.timeToStay + 10;
            text1.alpha = true;
            text1faded = true;
        }

        if (Time.timeSinceLevelLoad > timeToStartText2 && text2faded == false)
        {
            text2.alpha = true;
            text2faded = true;
        }

        if (Time.timeSinceLevelLoad > timeToStartText3 && text3faded == false)
        {
            text3.alpha = true;
            text3faded = true;
        }

        if (Time.timeSinceLevelLoad > timeToStartText4 && text4faded == false)
        {
            text4.alpha = true;
            text4faded = true;
        }

        if (Time.timeSinceLevelLoad > timeToChangeScene)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
