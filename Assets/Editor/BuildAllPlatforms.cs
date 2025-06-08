using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class BuildAllPlatforms
{
    private static string[] scenes = { "Assets/Scenes/SampleScene.unity" };
    static string gameName => Application.productName;
    static string version = Application.version;

    [MenuItem("Build/Build All Targets")]
    public static void BuildAll()
    {
        BuildWindowsIntel64();
        BuildWindowsARM64();
        //BuildWebGLBrotli();
        //BuildWebGLNoCompression();
        BuildLinux();
        BuildMacOS();
    }

    [MenuItem("Build/Windows Intel64")]
    public static void BuildWindowsIntel64()
    {
        PlayerSettings.SetArchitecture(NamedBuildTarget.Standalone, 1); // x86_64
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-WindowsIntel64/{gameName}.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("Build/Windows ARM64")]
    public static void BuildWindowsARM64()
    {
        PlayerSettings.SetArchitecture(NamedBuildTarget.Standalone, 2); // ARM64
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-WindowsARM64/{gameName}.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        PlayerSettings.SetArchitecture(NamedBuildTarget.Standalone, 1); // zurück zu x86_64
    }

    [MenuItem("Build/WebGL Brotli")]
    public static void BuildWebGLBrotli()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-WebGL-Brotli", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("Build/WebGL No Compression")]
    public static void BuildWebGLNoCompression()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-WebGL-NoCompression", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("Build/Linux")]
    public static void BuildLinux()
    {
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-Linux/{gameName}.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
    }

    [MenuItem("Build/macOS")]
    public static void BuildMacOS()
    {
        BuildPipeline.BuildPlayer(scenes, $"Builds/{gameName}-{version}-macOS/{gameName}.app", BuildTarget.StandaloneOSX, BuildOptions.None);
    }
}
