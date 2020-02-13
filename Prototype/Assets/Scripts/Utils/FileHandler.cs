using UnityEngine;
using UnityEditor;
using System.IO;

public static class FileHandler
{
    [MenuItem("Tools/Write file")]
    public static void WriteString(string text, string resourceFile)
    {
        string path = "Assets/Resources/" + resourceFile + ".txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (UnityEngine.TextAsset)Resources.Load(resourceFile);

        //Print the text from the file
        Debug.Log(asset.text);
    }

    [MenuItem("Tools/Read file")]
    public static string ReadString(string resourceFile)
    {
        string path = "Assets/Resources/" + resourceFile + ".txt";
        Debug.Log("Reading from file " + path);

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();

        return text;
    }

}