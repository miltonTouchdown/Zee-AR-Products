/*
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Zip; // this uses the Unity port of DotNetZip https://github.com/r2d2rigo/dotnetzip-for-unity

// place in an "Editor" folder in your Assets folder
public class ProjectBuilder
{
    //Up version counter vx.10 -> vx.11
    //Build Android ARM  ProjectName_vx.x_ANDROID_ARM
    //Build Android X86 ProjectName_vx.x_ANDROID_x86
    //ZipFile ProjectName_vx.x_SOURCECODE.zip

    [MenuItem("AbstractDigital/Build Project Production")]
    public static void BuildProjectProduction()
    {
        string path = EditorUtility.SaveFolderPanel("Build out WINDOWS to...",
                                                    GetProjectFolderPath() + "/Builds/",
                                                    "");
    }

    public static void StartWindows()
    {
        // Get filename.
        
        string path = EditorUtility.SaveFolderPanel("Build out WINDOWS to...",
                                                    GetProjectFolderPath() + "/Builds/",
                                                    "");
        var filename = path.Split('/'); // do this so I can grab the project folder name
        BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
    }


    [MenuItem("BuildRadiator/Build Windows + Mac OSX + Linux")]
    public static void StartAll()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Build out ALL STANDALONES to...",
                                                    GetProjectFolderPath() + "/Builds/",
                                                    "");
        var filename = path.Split('/'); // do this so I can grab the project folder name
        BuildPlayer(BuildTarget.StandaloneOSXUniversal, filename[filename.Length - 1], path + "/");
        BuildPlayer(BuildTarget.StandaloneLinuxUniversal, filename[filename.Length - 1], path + "/");
        BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");

    }

    // this is the main player builder function
    static void BuildPlayer(BuildTarget buildTarget, string filename, string path)
    {
        string fileExtension = "";
        string dataPath = "";
        string modifier = "";

        // configure path variables based on the platform we're targeting
        switch (buildTarget)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                modifier = "_windows";
                fileExtension = ".exe";
                dataPath = "_Data/";
                break;
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                modifier = "_mac-osx";
                fileExtension = ".app";
                dataPath = fileExtension + "/Contents/";
                break;
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
                modifier = "_linux";
                dataPath = "_Data/";
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneLinux: fileExtension = ".x86"; break;
                    case BuildTarget.StandaloneLinux64: fileExtension = ".x64"; break;
                    case BuildTarget.StandaloneLinuxUniversal: fileExtension = ".x86_64"; break;
                }
                break;
        }

        Debug.Log("====== BuildPlayer: " + buildTarget.ToString() + " at " + path + filename);
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

        // build out the player
        string buildPath = path + filename + modifier + "/";
        Debug.Log("buildpath: " + buildPath);
        string playerPath = buildPath + filename + modifier + fileExtension;
        Debug.Log("playerpath: " + playerPath);
        BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, buildTarget, buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None);

        // Copy files over into builds
        string fullDataPath = buildPath + filename + modifier + dataPath;
        Debug.Log("fullDataPath: " + fullDataPath);
        CopyFromProjectAssets(fullDataPath, "languages"); // language text files that Radiator uses
                                                          // TODO: copy over readme

        // ZIP everything
        CompressDirectory(buildPath, path + "/" + filename + modifier + ".zip");
    }

    // from http://wiki.unity3d.com/index.php?title=AutoBuilder
    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string GetProjectFolderPath()
    {
        var s = Application.dataPath;
        s = s.Substring(s.Length - 7, 7); // remove "Assets/"
        return s;
    }

    // copies over files from somewhere in my project folder to my standalone build's path
    // do not put a "/" at beginning of assetsFolderName
    static void CopyFromProjectAssets(string fullDataPath, string assetsFolderPath, bool deleteMetaFiles = true)
    {
        Debug.Log("CopyFromProjectAssets: copying over " + assetsFolderPath);
        FileUtil.ReplaceDirectory(Application.dataPath + "/" + assetsFolderPath, fullDataPath + assetsFolderPath); // copy over languages

        // delete all meta files
        if (deleteMetaFiles)
        {
            var metaFiles = Directory.GetFiles(fullDataPath + assetsFolderPath, "*.meta", SearchOption.AllDirectories);
            foreach (var meta in metaFiles)
            {
                FileUtil.DeleteFileOrDirectory(meta);
            }
        }
    }

    // compress the folder into a ZIP file, uses https://github.com/r2d2rigo/dotnetzip-for-unity
    static void CompressDirectory(string directory, string zipFileOutputPath)
    {
        Debug.Log("attempting to compress " + directory + " into " + zipFileOutputPath);
        // display fake percentage, I can't get zip.SaveProgress event handler to work for some reason, whatever
        EditorUtility.DisplayProgressBar("COMPRESSING... please wait", zipFileOutputPath, 0.38f);
        using (ZipFile zip = new ZipFile())
        {
            zip.ParallelDeflateThreshold = -1; // DotNetZip bugfix that corrupts DLLs / binaries http://stackoverflow.com/questions/15337186/dotnetzip-badreadexception-on-extract
            zip.AddDirectory(directory);
            zip.Save(zipFileOutputPath);
        }
        EditorUtility.ClearProgressBar();
    }
    

}
*/
