using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Exists only for file-reading
public class ReadProbabilityFile
{
    //Singleton code, since only one is ever needed
    static ReadProbabilityFile instance;
    public static ReadProbabilityFile Instance
    {
        get { if (instance == null) { instance = new ReadProbabilityFile(); } return instance; }
    }

    //returns an array of strings
    public string[] GetFileContents(string fileLocation)
    {
        //For debugging purposes.
        //A null array is assumed to be a file not found.
        if (!File.Exists(fileLocation))
        {
            Debug.Log("OOF");
            return null;
        }

        StreamReader sr = new StreamReader(fileLocation);

        List<string> strOut = new List<string>();

        while (!sr.EndOfStream)
        {
            strOut.Add(sr.ReadLine());
        }

        //Return array of strings, containing probability data.
        return strOut.ToArray();
    }
}
