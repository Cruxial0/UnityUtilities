#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildHelper {
    public class BuildMenu {
        #region MenuItems
        [MenuItem("Application/Build/Client/Current Platform", false, 1)]
        public static void BuildEnvironmentClient() => Build(BuildParameters.GetEnvironmentPlatform());
        
        [MenuItem("Application/Build/Client/Windows")]
        public static void BuildWindowsClient() => Build(BuildPlatform.Windows);
        
        [MenuItem("Application/Build/Client/Linux")]
        public static void BuildLinuxClient() => Build(BuildPlatform.Linux);
        
        [MenuItem("Application/Build/Client/MacOS")]
        public static void BuildOSXClient() => Build(BuildPlatform.MacOS);
        
        [MenuItem("Application/Build/Server/Current Platform", false, 1)]
        public static void BuildEnvironmentServer() => Build(BuildParameters.GetEnvironmentPlatform());
        
        [MenuItem("Application/Build/Server/Windows")]
        public static void BuildWindowsServer() => Build(BuildPlatform.Windows, true);
        
        [MenuItem("Application/Build/Server/Linux")]
        public static void BuildLinuxServer() => Build(BuildPlatform.Linux, true);
        
        [MenuItem("Application/Build/Server/MacOS")]
        public static void BuildOSXServer() => Build(BuildPlatform.MacOS, true);
        #endregion
        
        public static void Build(BuildPlatform platform, bool isServer = false) {
            Directory.CreateDirectory(BuildParameters.GetBuildFolder(platform, isServer));
            
            BuildPlayerOptions options = new BuildPlayerOptions();
            options.scenes = GetSceneOrderFromBuildSettings();
            options.target = BuildParameters.ClientPlatforms[platform];
            options.locationPathName = BuildParameters.GetFullBuildPath(platform, isServer);
            if (isServer) SetServerBuildOptions(options);
            
            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize/1000:n0}kB\nAvailable at: {BuildParameters.GetFullBuildPath(platform, isServer)}");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        private static void SetServerBuildOptions(BuildPlayerOptions options) {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, options.target);
            EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Server;
            options.subtarget = (int)StandaloneBuildSubtarget.Server;
        }

        private static string[] GetSceneOrderFromBuildSettings() {
            var scenes = new List<string>();
            foreach (var item in EditorBuildSettings.scenes) {
                if(item.enabled) scenes.Add(item.path);
            }

            return scenes.ToArray();
        }
    }

    public enum BuildPlatform {
        Windows,
        MacOS,
        Linux
    }
}
#endif