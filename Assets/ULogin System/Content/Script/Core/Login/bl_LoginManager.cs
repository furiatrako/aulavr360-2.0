using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class bl_LoginManager : MonoBehaviour {

    //Call all script when Login
    public delegate void LoginEvent(string name);
    public static LoginEvent OnLogin;
    public static string m_IP = "None";
    [Header("References")]
    public GameObject PlayerInfo;
    public Animation LoginAnim;
    public Animation RegisterAnim;
    public Animation InfoAnim;
    [Header("UI References")]
    public Text Description = null;
    static Text mDescrip = null;
    public Image BlackScreen = null;
    [Header("Scrips References")]
    public bl_LoadingEffect Loading = null;
    public static bl_LoadingEffect LoadingCache = null;

    public const string SavedUser = "UserName";
    private Color alpha = new Color(0, 0, 0, 0);
    private bool InLogin = true;
    private  bool m_ShowInfo = false;
    private bl_SaveInfo m_SaveInfo = null;
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        m_IP = string.Empty;
        if (Loading != null) { LoadingCache = Loading; }
        if (bl_LoginDataBase.Instance.GetIpOnAwake) { StartCoroutine(GetIP()); }

        mDescrip = Description;
        OnLogin += onLogin;
        StartCoroutine(FadeOut());
        if (GameObject.Find(bl_SaveInfo.SaveInfoName) == null)
        {
            GameObject p = Instantiate(PlayerInfo, Vector3.zero, Quaternion.identity) as GameObject;
            p.name = p.name.Replace("(Clone)", "");
            m_SaveInfo = p.GetComponent<bl_SaveInfo>();
        }
        else
        {
            m_SaveInfo = GameObject.Find(bl_SaveInfo.SaveInfoName).GetComponent<bl_SaveInfo>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        OnLogin -= onLogin;
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowLogin()
    {
        if (!InLogin)
        {
            InLogin = true;
            LoginAnim.Play("Login_Show");
            RegisterAnim.Play("Register_Hide");
            UpdateDescription("");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowRegister()
    {
        if (InLogin)
        {
            InLogin = false;
            LoginAnim.Play("Login_Hide");
            RegisterAnim.Play("Register_Show");
            UpdateDescription("");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowInfo()
    {
        m_ShowInfo = !m_ShowInfo;
        if (m_ShowInfo)
        {
            InfoAnim["Info_Show"].time = 0;
            InfoAnim["Info_Show"].speed = 1.0f;
            InfoAnim.Play("Info_Show");
        }
        else
        {
            InfoAnim["Info_Show"].time = InfoAnim["Info_Show"].length;
            InfoAnim["Info_Show"].speed = -1.0f;
            InfoAnim.Play("Info_Show");
        }
    }
    /// <summary>
    /// Update Text description UI
    /// </summary>
    /// <param name="t"></param>
    public static void UpdateDescription(string t)
    {
        mDescrip.text = t;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public static void OnLoginEvent(string name)
    {
        if (OnLogin != null)
            OnLogin(name);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="n"></param>
    void onLogin(string n)
    {
        BlackScreen.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetIP()
    {
        Loading.ChangeText("Request public IP...", true);
        //Request public IP to the server
        WWW w = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.GetIP));
        //Wait for response
        if (w == null) yield return null;
        yield return w;
        Loading.ChangeText("Getting public IP...", true,2);
        //Get Ip
        string t;
        t = w.text;
        m_IP = t;

    }

    /// <summary>
    /// Effect of Fade In
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        alpha = BlackScreen.color;

        while (alpha.a < 1.0f)
        {
            alpha.a += Time.deltaTime ;
            BlackScreen.color = alpha;
            yield return null;
        }
    }

    internal static void OnLoginEvent()
    {
    }

    /// <summary>
    /// Effect of Effect Fade Out
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOut()
    {
        alpha.a = 1f;
        while (alpha.a > 0.0f)
        {
            alpha.a -= Time.deltaTime;
            BlackScreen.color = alpha;
            yield return null;
        }
        BlackScreen.gameObject.SetActive(false);
    }

    public static void LoadLevel(string scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
 SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }

    public static void LoadLevel(int scene)
    {
#if UNITY_5_3 || UNITY_5_4 || UNITY_5_3_OR_NEWER
 SceneManager.LoadScene(scene);
#else
        Application.LoadLevel(scene);
#endif
    }
}