using UnityEngine;
using System;

public class bl_LoginDataBase : ScriptableObject
{
    [Header("Host Path")]
    public string PhpHostPath;
    public string SecretKey = "123456";
    public string Admin = "Admin";
    public string OnLoginLoadLevel = "NextLevelName";
    [Header("Script Names")]
    public string LoginPhp = "bl_Login";
    public string RegisterPhp = "bl_Register";
    public string SaveInfoPhp = "bl_SaveInfo";
    public string RankingPhp = "bl_GetTop";
    public string GetIpPhp = "bl_GetIP";
    public string BanListPhp = "bl_BanList";
    public string BanPhp = "bl_Ban";
    public string RequestUserPhp = "bl_RequestUser";
    public string FunctionsPhp = "Functions";

    [Header("Settings")]
    public bool UpdateIP = true;
    public bool GetIpOnAwake = true;
    public bool DetectBan = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public string GetUrl(URLType _type)
    {
        string scriptName = "None";
        switch(_type)
        {
            case URLType.Login:
                scriptName = LoginPhp;
                break;
            case URLType.Register:
                scriptName = RegisterPhp;
                break;
            case URLType.SaveInfo:
                scriptName = SaveInfoPhp;
                break;
            case URLType.GetIP:
                scriptName = GetIpPhp;
                break;
            case URLType.BanList:
                scriptName = BanListPhp;
                break;
            case URLType.Ranking:
                scriptName = RankingPhp;
                break;
            case URLType.Ban:
                scriptName = BanPhp;
                break;
            case URLType.RequestUser:
                scriptName = RequestUserPhp;
                break;
            case URLType.CallFunction:
                scriptName = FunctionsPhp;
                break;
        }
        string url = string.Format("{0}{1}.php", PhpHostPath, scriptName);
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) { Debug.Log("URL is not well formed, please check if your php script have the same name and have assign the host path."); }
        return url;
    }


    private static bl_LoginDataBase _dataBase;
    public static bl_LoginDataBase Instance
    {
        get
        {
            if(_dataBase == null) { _dataBase = Resources.Load("LoginDataBase", typeof(bl_LoginDataBase)) as bl_LoginDataBase; }
            return _dataBase;
        }
    }

    [Serializable]
    public enum URLType
    {
        Login,
        Register,
        SaveInfo,
        Ranking,
        GetIP,
        BanList,
        Ban,
        RequestUser,
        CallFunction,
    }
}