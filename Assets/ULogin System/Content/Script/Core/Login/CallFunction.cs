using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class CallFunction : MonoBehaviour {
    
    public InputField inputFolder;
    
    protected string Folder = "";


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void CountFilesInFolder()
    {
        StartCoroutine(CountFiles());
    }
    
    IEnumerator CountFiles()
    {
        string function = "CountFolders";
        if (inputFolder != null)
        {
            Folder = inputFolder.text;
        }
        WWWForm sendInfo = new WWWForm();
        sendInfo.AddField("function", function);
        sendInfo.AddField("folder", Folder);

		WWW www = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.CallFunction), sendInfo);
		///WWW www = new WWW(FunctionLibrary.URLType.CallFunction.ToString(), sendInfo);
		yield return www;
        Debug.Log(www.text);
    }
}
