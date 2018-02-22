using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadScenes : MonoBehaviour {

	public AudioSource aSource;

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public Image imageLoading;

    public void chargeScene(string sceneName)
    {
        StartCoroutine(LoadAsyncronously(sceneName));
    }


    IEnumerator LoadAsyncronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            imageLoading.fillAmount = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }


	public void playSound(AudioClip aClip)
	{
		aSource.clip = aClip;
		aSource.Play();
	}
}
