using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions; // needed for Regex

public class bl_Register : MonoBehaviour {

    [Header("References")]
    public InputField UserInput = null;
	public InputField UserSurname1 = null;
	public InputField UserSurname2 = null;
	public InputField MailInput = null;
    public InputField PasswordInput = null;
    public InputField Re_PasswordInput = null;
    [Header("Settings")]
    public int MaxNameLength = 15;

    protected string UserName = "";
    protected string Surname1 = "";
    protected string Surname2 = "";
	protected string Mail = "";
    protected string Password = "";
    protected string Re_Password = "";
    private bool isRegister = false;

	// Use this for initialization
	void Start () {

        if (UserInput != null) {
            UserInput.characterLimit = MaxNameLength;
        }
	}
	
	/// <summary>
	/// 
	/// </summary>
    void Update() {
        if (UserInput != null) {
            UserInput.text = Regex.Replace(UserInput.text, @"[^a-zA-Z0-9 ]", "");//not permit simbols
            UserName = UserInput.text;
        }
        if (UserSurname1 != null) {
            UserSurname1.text = Regex.Replace(UserSurname1.text, @"[^a-zA-Z0-9 ]", "");//not permit simbols
            Surname1 = UserSurname1.text;
        }
        if (UserSurname2 != null) {
            UserSurname2.text = Regex.Replace(UserSurname2.text, @"[^a-zA-Z0-9 ]", "");//not permit simbols
            Surname2 = UserSurname2.text;
        }
        if (MailInput != null) {
            Mail = MailInput.text;
        }
        if (PasswordInput != null) {
            Password = PasswordInput.text;
        }
        if (Re_PasswordInput != null) {
            Re_Password = Re_PasswordInput.text;
        }
    }

    /// <summary>
    /// Register function to check.
    /// </summary>
    public void Register() {
        if (isRegister)//if alredy connect
            return;

        if (UserName != string.Empty && Surname1 != string.Empty && Mail != string.Empty && Re_Password != string.Empty && Password != string.Empty) {
            if (Password == Re_Password) {
                StartCoroutine(RegisterProcess());
                bl_LoginManager.UpdateDescription("");
            }
            else {
                bl_LoginManager.UpdateDescription("Password does not match");
                Debug.LogWarning("Password does not match");
            }
        }
        else {
            if (UserName == string.Empty) {
                Debug.LogWarning("User name is empty");
            }
            if (Surname1 == string.Empty) {
                Debug.LogWarning("Surname is empty");
            }
            if (Mail == string.Empty) {
                Debug.LogWarning("Mail is empty");
            }
            if (Password == string.Empty) {
                Debug.LogWarning("Password is empty");
            }
            if (Re_Password == string.Empty) {
                Debug.LogWarning("Re-Password is empty");
            }
            bl_LoginManager.UpdateDescription("Complete all fields");
        }
    }

    /// <summary>
    /// Connect with database
    /// </summary>
    /// <returns></returns>
    IEnumerator RegisterProcess()
    {
        if (isRegister)
            yield return null;

        isRegister = true;
        bl_LoginManager.UpdateDescription("Registering...");
        bl_LoginManager.LoadingCache.ChangeText("Request Register...", true);
        //Used for security check for authorization to modify database
        string hash = Md5Sum(UserName + Password + bl_LoginDataBase.Instance.SecretKey).ToLower();

        //Assigns data we want to save
        //Where -> Form.AddField("name" = matching name of column in SQL database
        WWWForm mForm = new WWWForm();
        mForm.AddField("name", UserName); // adds user's name to the form
        mForm.AddField("surname1", Surname1); // adds user's surname to the form
        mForm.AddField("surname2", Surname2); // adds user's second surname (not required)
		mForm.AddField("mail", Mail); // adds user's email
        mForm.AddField("pass", Password); // adds user's password to the form
        mForm.AddField("hash", hash); // adds security hash for Authorization

        // Creates instance of WWW to run the PHP script to store data to mySQL database
        WWW www = new WWW(bl_LoginDataBase.Instance.GetUrl(bl_LoginDataBase.URLType.Register), mForm);

        bl_LoginManager.UpdateDescription("Processing...");
        Debug.Log("Processing...");
        yield return www;
        Debug.Log(www.text);

        bl_LoginManager.LoadingCache.ChangeText("Get response...", true,2f);

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.text);
            if (www.text.Contains("Done"))
            {
                Debug.Log("Registered Successfully.");
                this.GetComponent<bl_LoginManager>().ShowLogin();
                bl_LoginManager.UpdateDescription("Registered Successfully, Login Now");

            }
            else
            {
                Debug.Log(www.text);
                bl_LoginManager.UpdateDescription(www.text);

            }
        }
        isRegister = false;
    }
    
    /// <summary>
    /// Md5s Security Features
    /// </summary>
    public string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++) { sb.Append(hash[i].ToString("X2")); }
        return sb.ToString();
    }
}