using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to instantiate the beans
public class SpawnTheBeanz : MonoBehaviour
{
    public ICanSpawn[] spawnLocations;
    public List<BeanScript> allBeanz;
    public BeanScript prefabBean;

    public static BeanChances worldChance = null;
    public static SpawnTheBeanz mainBeanSpawner = null;

    //Initialization
    private void Awake()
    {
        mainBeanSpawner = this;
        allBeanz = new List<BeanScript>();
        spawnLocations = GetComponentsInChildren<ICanSpawn>();
    }

    //Recreate is assigned its own function such that the player may reset the map on keystroke
    private void Start()
    {
        Recreate();
    }

    //Spawns beans at the given locations
    public void Recreate()
    {
        //Clear the list first
        while (allBeanz.Count > 0)
        {
            if (allBeanz[allBeanz.Count - 1] != null)
            {
                Destroy(allBeanz[allBeanz.Count - 1].gameObject);
            }

            allBeanz.RemoveAt(allBeanz.Count - 1);
        }

        //Spawn beans, one per location
        for (int i = 0; i < spawnLocations.Length; ++i)
        {
            Transform tform = spawnLocations[i].transform;

            float totChance = GrabFileContents.totalChance;
            float randomChance = Random.Range(0f, totChance);
            BeanChances bc = null;

            //Choose a random number between 0 and the sum of all chances.
            //Repeatedly decrement this number until it becomes below 0.
            //Whichever BeanChances was last chosen, assume its properties for HOT/NOISY/SAFE
            for (int j = 0; j < GrabFileContents.chances.Count; ++j)
            {
                BeanChances testBean = GrabFileContents.chances[j];

                randomChance -= testBean.CHANCE;

                if (randomChance < 0)
                {
                    bc = testBean;
                    break;
                }
            }

            //Edge case to resolve randomChance = totChance, such that it stops at exactly 0 instead of under 0
            //Go backwards through the list and assume the characteristics of the first non-zero probability
            if (bc == null)
            {
                for (int j = GrabFileContents.chances.Count - 1; j >= 0; --j)
                {
                    BeanChances testBean = GrabFileContents.chances[j];

                    if (testBean.CHANCE > 0f)
                    {
                        bc = testBean;
                        break;
                    }
                }
            }

            //Set a static variable, so that the bean being created may access it in its Awake call.
            worldChance = bc;

            //Instantiate a bean at the spawn location. Add it to the list of all beans.
            BeanScript newBean = Instantiate(prefabBean, tform.position, tform.rotation);
            allBeanz.Add(newBean);

            //Reset the static variable.
            worldChance = null;
        }
    }
}
