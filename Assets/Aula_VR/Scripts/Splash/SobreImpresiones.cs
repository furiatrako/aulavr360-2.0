using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SobreImpresiones : MonoBehaviour {

	private float t;
	public bool alpha;
	public float timeToStay;
	public float fadeSpeed;
	public Vector4 textColor;
	public Text textObject;

    private void Start()
    {
        textObject.material.color = textColor;
    }

    void Update () {
        if (alpha == true)
        {
            StartCoroutine("fadeText");
            StartCoroutine("delayedExecution", (timeToStay));
            alpha = false;
        }
	}

    public void inFade()
    {
        StartCoroutine("fadeText");
    }

    public void outfade()
    {
        StartCoroutine("fadeOut");
    }

	public IEnumerator delayedExecution (float delay){
		yield return new WaitForSeconds (delay);
		StartCoroutine("fadeOut");
	}

	public IEnumerator fadeText() {
		for (float fo = 0.0f; fo <= 1.0f; fo += fadeSpeed) {
			Color c = textObject.material.color;
			c.a = fo;
			textObject.material.color = c;
			yield return null;
		}
	}

	IEnumerator fadeOut(){
		for (float fi = 1.0f; fi >= 0f; fi -= fadeSpeed) {
			Color c = textObject.material.color;
			c.a = fi;
			textObject.material.color = c;
			yield return null;
		}
	}
}