using UnityEngine;
using System.Collections;
using System.Text;

public class bl_SaveInfo : MonoBehaviour {

    public const string SaveInfoName = "PlayerInfo";
    [Header("Information")]
    public string m_UserName = "";
    public int m_Kills = 0;
    public int m_Deaths = 0;
    public int Score = 0;
    public string m_IP = string.Empty;
    public string DbIP = string.Empty;


    public static bool isAdmin = false;
    public static bool isModerator = false;
    
    /// <summary>
    /// You can use the reference eg:
    /// m_Status = 0; = normal player
    /// m_Status = 1; = moderator;
    /// m_Status = 2; = admin;,etc...
    /// </summary>
    public int m_Status = 0;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
       
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    void OnEnable()
    {
        bl_LoginManager.OnLogin += this.GetInfo;
    }
    /// <summary>
    /// 
    /// </summary>
    void OnDisable()
    {
        bl_LoginManager.OnLogin -= this.GetInfo;
    }
    /// <summary>
    /// Cache info
    /// </summary>
    /// <param name="n"></param>
    /// <param name="k"></param>
    /// <param name="d"></param>
    /// <param name="s"></param>
    void GetInfo(string n)
    {
        m_UserName = n;
        if (bl_LoginDataBase.Instance.UpdateIP)
        {
            CheckIP();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CheckIP()
    {
      if(DbIP != m_IP)
        {
            Debug.Log("Is not the same: DbIP " + DbIP + " - LocalIP " + m_IP + ", then now will update automatically.");
            StartCoroutine(SendUpdateIP());
        }
        else
        {
            Debug.Log("is the same IP");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator SendUpdateIP()
    {
        string hash = Md5Sum(m_UserName + bl_LoginDataBase.Instance.SecretKey).ToLower();
        WWWForm wf = new WWWForm();
        wf.AddField("name", m_UserName);
        wf.AddField("nIP", m_IP);
        wf.AddField("typ", "2");
        wf.AddField("hash", hash);

        WWW w = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.SaveInfo), wf);

        yield return w;

        if(!string.IsNullOrEmpty(w.error))
        {
            Debug.LogWarning(w.error);
        }
        else
        {
            if(w.text.Contains("successip"))
            {
                Debug.Log(string.Format("Save new IP{0} successful!",m_IP));
            }
            else
            {
                Debug.Log(w.text);
            }
        }
    }

    /// <summary>
    /// If we connect then is available for update
    /// get current player propertied info for send and convert in int
    /// </summary>
    public void SaveInfo(string k, string d, string s)
    {

        int kp = int.Parse(k);
        int dp = int.Parse(d);
        int sp = int.Parse(s);
        SaveInfo(kp, dp, sp);

    }

    /// <summary>
    /// If we connect then is available for update
    /// get current player propertied info for send
    /// </summary>
    public void SaveInfo(int k, int d, int s)
    {
        StartCoroutine(Save(k, d, s));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="k"></param>
    /// <param name="d"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    IEnumerator Save(int k,int d, int s)
    {
        //Used for security check for authorization to modify database
        string hash = Md5Sum(m_UserName + bl_LoginDataBase.Instance.SecretKey).ToLower();

        //Get old Player Info and Update with new info
        //for update DB info
        int t_kills = m_Kills + k;
        int t_deaths = m_Deaths + d;
        int t_score = Score + s;
        //Assigns the data we want to save
        //Where -> Form.AddField("name" = matching name of value in SQL database
        WWWForm mForm = new WWWForm();
        mForm.AddField("name", m_UserName); // adds the player name to the form
        mForm.AddField("kills", t_kills); // adds the kill total to the form
        mForm.AddField("deaths", t_deaths); // adds the death Total to the form
        mForm.AddField("score",t_score); // adds the score Total to the form
        mForm.AddField("hash", hash); // adds the security hash for Authorization
        mForm.AddField("typ", "1");

        //Creates instance of WWW to runs the PHP script to save data to mySQL database
        WWW www = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.SaveInfo), mForm);
        Debug.Log("Processing...");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Saved Info Successfully.");
            //Update Vals Locals
            m_Kills = t_kills;
            m_Deaths = t_deaths;
            Score = t_score;
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    /// <summary>
    /// Md5s Security Features
    /// </summary>
    public string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++) { sb.Append(hash[i].ToString("X2")); }
        return sb.ToString();
    }
}
