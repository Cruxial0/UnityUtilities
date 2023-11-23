#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace BuildHelper {
    public static class BuildParameters {
        public static Dictionary<BuildPlatform, string> BuildFolder = new() {
            { BuildPlatform.Windows, "Windows" },
            { BuildPlatform.Linux, "Linux" },
            { BuildPlatform.MacOS, "MacOS" }
        };

        public static Dictionary<BuildPlatform, BuildTarget> ClientPlatforms = new() {
            { BuildPlatform.Windows, BuildTarget.StandaloneWindows64 },
            { BuildPlatform.Linux, BuildTarget.StandaloneLinux64 },
            { BuildPlatform.MacOS, BuildTarget.StandaloneOSX }
        };

        public static Dictionary<BuildPlatform, string> PlatformExtensions = new() {
            { BuildPlatform.Windows, ".exe" },
            { BuildPlatform.Linux, string.Empty },
            { BuildPlatform.MacOS, ".app" }
        };

        public static string BaseFolder = "Build";
        public static string ExecutableName = "Application";
        private static readonly char Seperator = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? '\\' : '/';

        public static BuildPlatform GetEnvironmentPlatform() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return BuildPlatform.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return BuildPlatform.Linux;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return BuildPlatform.MacOS;
            throw new ArgumentOutOfRangeException();
        }
        
        public static string GetBuildFolder(BuildPlatform platform, bool isServer = false) {
            List<string> path = new List<string>();
            
            path.Add(Application.dataPath.Replace("/Assets", string.Empty));
            path.Add(BaseFolder);
            path.Add(isServer ? "Server_"+BuildFolder[platform] : BuildFolder[platform]);
            return String.Join(Seperator, path);
        }

        public static string GetFullBuildPath(BuildPlatform platform, bool isServer = false) {
            return string.Join(Seperator, GetBuildFolder(platform, isServer), ExecutableName + PlatformExtensions[platform]);
        }
    }
}
#endif