using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bl_Login : MonoBehaviour {


    [HideInInspector] public string m_User = "";
    [HideInInspector] public string m_Password = "";
    [HideInInspector] public bool KeepMe = false;
    [Header("References")]
    public Toggle mToggle = null;
    public InputField m_UserInput = null;
    public InputField m_PassInput = null;
    //Private
    private bool isLogin = false;
    private bool AlredyLogin = false;

    /// <summary>
    /// Get Name in Init
    /// </summary>
    void Awake()
    {
        if (mToggle != null)
        {
            if (PlayerPrefs.GetString(bl_LoginManager.SavedUser) != string.Empty)
            {
                mToggle.isOn = true;
                if (m_User != null)
                {
                    m_UserInput.text = PlayerPrefs.GetString(bl_LoginManager.SavedUser);
                }
            }
            else
            {
                mToggle.isOn = false;
            }
        }
    }

	/// <summary>
	/// 
	/// </summary>
    void FixedUpdate()
    {
        if (mToggle != null)
        {
            KeepMe = mToggle.isOn;
        }
        if (m_UserInput != null)
        {
            m_User = m_UserInput.text;
        }
        if (m_PassInput != null)
        {
            m_Password = m_PassInput.text;
        }
    }

    /// <summary>
    /// Login detects if user and password is not empty
    /// and start coroutine for the connection with the DB
    /// </summary>
    public void Login()
    {
        if (isLogin || AlredyLogin) {
            return;
        }

        if (m_User != string.Empty && m_Password != string.Empty) {
            StartCoroutine(LoginProcess());
        }
        else {
            if (m_User == string.Empty) {
                Debug.LogWarning("User Name is Empty");
            }
            if (m_Password == string.Empty) {
                Debug.LogWarning("Password is Empty");
            }
            bl_LoginManager.UpdateDescription("complete all fields");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoginProcess() {
        if (isLogin || AlredyLogin) {
            yield return null;
        }

        isLogin = true;
        bl_LoginManager.UpdateDescription("Login...");

        bl_LoginManager.LoadingCache.ChangeText("Request Login...", true);
        // Create instance of WWWForm
        WWWForm mForm = new WWWForm();
        //sets the mySQL query to the amount of rows to load
        mForm.AddField("name", m_User);
        mForm.AddField("password", m_Password);
        mForm.AddField("getIP", 1);
        //Creates instance to run the php script to access the mySQL database
        WWW www = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.Login), mForm);
        //Wait for server response...
        yield return www;
        bl_LoginManager.LoadingCache.ChangeText("Getting response from server...", true,1.2f);
        string result = www.text;

        //check if we have some error
        if (!string.IsNullOrEmpty(www.error)) {
            Debug.LogError(www.error);
            bl_LoginManager.LoadingCache.ChangeText(www.error, true, 2f);
        }
        else {
            //Separate information
            string[] mSplit = result.Split("|"[0]);

            if (mSplit[0].Contains("success"))
            {
                Debug.Log("Login " + mSplit[0]);
                bl_LoginManager.LoadingCache.ChangeText("Successfully logged.", true, 0.8f);
                AlredyLogin = true;
                
                if (KeepMe) {
                    PlayerPrefs.SetString(bl_LoginManager.SavedUser, m_User);
                }
                else {
                    PlayerPrefs.SetString(bl_LoginManager.SavedUser, string.Empty);
                }

                //Send information to SaveInfo.
                bl_LoginManager.OnLoginEvent();
                yield return new WaitForSeconds(1f);
				//Load next level
				bl_LoginManager.LoadLevel(bl_LoginDataBase.Instance.OnLoginLoadLevel);
			}
			else //Wait, have a error?, please contact me for help with the result of next debug log warning.
            {
                //Some error with the server.
                Debug.LogWarning(www.text);
                bl_LoginManager.UpdateDescription(www.text);
            }
        }
        isLogin = false;
    }
    
}
