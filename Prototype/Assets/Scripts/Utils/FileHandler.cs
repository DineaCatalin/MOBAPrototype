using UnityEngine;
using System.IO;

public static class FileHandler
{
    // Doesn't work at build runtime 
    public static void WriteString(string text, string resourceFile)
    {
        string path = "Assets/Resources/" + resourceFile + ".txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }

    public static string ReadString(string resourceFile)
    {
        string path = "Assets/Resources/" + resourceFile + ".txt";
        var textFile = Resources.Load<TextAsset>(resourceFile);

        return textFile.text;
    }

}