using UnityEngine;
using System.IO;
using System;

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
        string resultText = "EMPTY";
        string persistentDataPathFile = Application.persistentDataPath + "/" + resourceFile + ".txt";
        string streamingAssetsFilePath = Application.streamingAssetsPath + "/" + resourceFile + ".txt";

        // Try to read from persistenPath
        try
        {
            resultText = System.IO.File.ReadAllText(persistentDataPathFile);
        }
        catch(FileNotFoundException e)
        {
            Debug.Log("FileHandler ReadString there is no file " + persistentDataPathFile + " ERROR: " + e);
        }
        
        // File is created so return the contents
        if (!resultText.Equals("EMPTY"))
        {
            Debug.Log("FileHandler ReadString Read from persistentDataPath: " + resultText);
            return resultText;
        }

        // Copy file from resources to persistenDataPath location
        try
        {
            File.Copy(streamingAssetsFilePath, persistentDataPathFile);
            Debug.Log("FileHandler ReadString Copying from " + streamingAssetsFilePath + " to " + persistentDataPathFile);
        }
        catch (Exception e)
        {
            Debug.LogError("FileHandler ReadString couldnt copy file ERROR is: " + e.ToString());
        }

        resultText = System.IO.File.ReadAllText(persistentDataPathFile);
        return resultText;
    }
}