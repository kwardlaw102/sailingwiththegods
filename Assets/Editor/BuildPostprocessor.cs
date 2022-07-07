using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class BuildPostprocessor
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		var hash = GitInfo.FindGitShortHash();
		File.WriteAllText(Path.Combine(pathToBuiltProject.Replace(".exe", "_Data"), "gitversion.txt"), hash);
	}
}
