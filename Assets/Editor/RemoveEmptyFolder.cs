using Extensions;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RemoveEmptyFolder
{
    [MenuItem("Tools/删除空的文件夹")]
    public static void Execute()
    {
        string directory = Directory.GetCurrentDirectory();
        Debug.Log("检查： " + directory);
        CheckFolder(directory);
    }

    private static void CheckFolder(string parentDirectory)
    {
        var directories = Directory.GetDirectories(parentDirectory);
        foreach (var directory in directories)
        {
            CheckFolder(directory);
        }

        directories = Directory.GetDirectories(parentDirectory);
        var files = Directory.GetFiles(parentDirectory);
        if (directories.IsNullOrEmpty() && files.IsNullOrEmpty())
        {
            Debug.Log("删除了： " + parentDirectory);
            Directory.Delete(parentDirectory);

            string metaFile = parentDirectory + ".meta";
            if(File.Exists(metaFile))
            {
                Debug.Log("删除了： " + metaFile);
                File.Delete(metaFile);
            }
        }
    }
}
