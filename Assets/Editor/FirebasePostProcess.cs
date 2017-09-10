#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class FirebasePostProcess {
	[PostProcessBuild]
	private static void ProcessPostBuild (BuildTarget buildTarget, string path)
	{
		// Only perform these steps for iOS builds
		#if UNITY_IOS
		string projPath = PBXProject.GetPBXProjectPath(path);
		PBXProject project = new PBXProject();
		project.ReadFromString(File.ReadAllText(projPath));
		string target = project.TargetGuidByName(PBXProject.GetUnityTargetName());
		
		project.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
		
		File.WriteAllText(projPath, project.WriteToString());
		#endif
	}
}
#endif