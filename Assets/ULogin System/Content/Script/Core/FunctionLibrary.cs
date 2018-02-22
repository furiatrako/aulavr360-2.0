using UnityEngine;
using System;

public class FunctionLibrary : ScriptableObject
{
	[Header("Host Path")]
	public string PhpHostPath = "http://192.168.0.110/unity/";
	[Header("Script Names")]
	public string FunctionsPhp = "Functions";
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="_type"></param>
	/// <returns></returns>
	public string GetUrl(URLType _type)
	{
		string scriptName = "None";
		switch (_type)
		{
			case URLType.CallFunction:
				scriptName = FunctionsPhp;
			break;

		}
		string url = string.Format("{0}{1}.php", PhpHostPath, scriptName);
		if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) { Debug.Log("URL is not well formed, please check if your php script have the same name and have assign the host path."); }
		return url;
	}
    
	[Serializable]
	public enum URLType
	{
		CallFunction,
	}
}