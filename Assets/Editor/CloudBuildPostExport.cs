using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using AssetBundles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class CloudBuildPostExport{
	
	public static void BuildAssetBundle(string pathToBuiltProject) {
		string assetbundleDir = Path.Combine(System.Environment.CurrentDirectory, Utility.AssetBundlesOutputPath);
		string platformDir = Path.Combine(assetbundleDir, Utility.GetPlatformName());
		
		Debug.Log("Start PostProessing");
		BuildScript.BuildAssetBundles();

		WWWForm form = new WWWForm();
		form.AddField("platform", Utility.GetPlatformName());
		
		TextAsset manifest = (TextAsset) Resources.Load("UnityCloudBuildManifest.json");
		JObject manifestDict = null;
		if (manifest != null)
        {
			manifestDict = JsonConvert.DeserializeObject<JObject>(manifest.text);
			form.AddField("projectId", manifestDict["projectId"].ToString());
			form.AddField("buildNumber", manifestDict["buildNumber"].ToString());
			form.AddField("targetName", manifestDict["cloudBuildTargetName"].ToString());
        }
		else
		{
			form.AddField("projectId", "social-saving");
			form.AddField("buildNumber", "0");
			form.AddField("targetName", "android");
			Debug.Log("No manifest found");
		}

		Debug.Log("Adding file to POST body");
		string [] fileEntries = Directory.GetFiles(platformDir);
		
        foreach(string file in fileEntries)
		{
			string fileName = Path.GetFileName(file);
			form.AddBinaryData(fileName, File.ReadAllBytes(file), fileName);
			Debug.Log("Added " + fileName + " into the request");
		}

		string localUrl = "http://localhost:3000/bundle/upload";
		string remoteUrl = "http://build.apptree.io/bundle/upload";

		Debug.Log("Sending request");
		WWW www = new WWW(remoteUrl, form);
		while(!www.isDone);

		if (!string.IsNullOrEmpty(www.error)) 
		{
			Debug.Log("WWW failed: " + www.error);
		}
		else
		{
			Debug.Log("WWW result : " + www.text);
		}
		return;
	}
}
