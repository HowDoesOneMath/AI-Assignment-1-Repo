using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class object to hold important data regarding probability
public class BeanChances
{
    public BeanChances(char hot, char noisy, char safe, float chance)
    {
        HOT = (hot == 'Y');
        NOISY = (noisy == 'Y');
        SAFE = (safe == 'Y');
        CHANCE = chance;
    }

    public bool HOT, NOISY, SAFE;
    public float CHANCE;
}

//Used to read in the chance for the beans on startup
public class GrabFileContents : MonoBehaviour
{
    public UnityEngine.UI.InputField textfield;
    public UnityEngine.UI.Text errortext;

    public static List<BeanChances> chances;
    public static float totalChance;

    string[] cont;

    //Called once the 'EXPLODE SOME BEANZ' button is pressed in the start scene
    //Reads in the list of probabilities into a static list, accessible from anywhere.
    //Will refuse to start the scene if the total chance is 0, or if it cant find the file
    public void GetFile()
    {
        //Refresh key variables
        totalChance = 0;
        chances = new List<BeanChances>();

        //This function is used to parse the file
        cont = ReadProbabilityFile.Instance.GetFileContents(textfield.text);

        if (cont == null)
        {
            errortext.text = "CAN'T FIND THAT FILE!";
            return;
        }

        //Repeatedly add data to the list.
        //It is assumed tabs will always be the separator.
        for (int i = 0; i < cont.Length; ++i)
        {
            string[] splitter = cont[i].Split('\t');

            float chance;
            char HOT, NOISY, SAFE;

            if (float.TryParse(splitter[3], out chance) &&
                char.TryParse(splitter[0], out HOT) &&
                char.TryParse(splitter[1], out NOISY) &&
                char.TryParse(splitter[2], out SAFE)
                )
            {
                chances.Add(new BeanChances(HOT, NOISY, SAFE, chance));
                Debug.Log(HOT + ", " + NOISY + ", " + SAFE + ", " + chance);
                totalChance += chance;
            }
        }

        if (totalChance == 0)
        {
            errortext.text = "SUM OF PROBABILITIES IS 0!";
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
