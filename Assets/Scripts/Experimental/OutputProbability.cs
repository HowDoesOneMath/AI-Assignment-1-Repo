using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that was used to make sure the probability file was reading correctly
//Deprecated
public class OutputProbability : MonoBehaviour
{
    public string fileName;
    public string filePath;

    string[] fileCont;
    List<string> splitCont;
    

    private void Awake()
    {
        splitCont = new List<string>();

        fileCont = ReadProbabilityFile.Instance.GetFileContents(filePath + fileName);

        for (int i = 0; i < fileCont.Length; ++i)
        {
            string[] splitter = fileCont[i].Split('\t');

            for (int j = 0; j < splitter.Length; ++j)
            {
                splitCont.Add(splitter[j]);
            }

            //Debug.Log(fileCont[i]);
        }

        for (int i = 0; i < splitCont.Count; ++i)
        {
            Debug.Log(splitCont[i]);
        }
    }
}
