using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to test bean spawning
//Deprecated
public class MakeBeans : MonoBehaviour
{
    public GameObject bean;

    public int beanAmnt = 11;

    private void Awake()
    {
        Vector3 pos = Vector3.right * (beanAmnt - 1);

        for(int i = 0; i < beanAmnt; ++i)
        {
            GameObject newBean = Instantiate(bean, pos, transform.rotation);

            MeshRenderer mr = newBean.GetComponent<MeshRenderer>();

            Material mat = Instantiate(mr.material);
            mr.material = mat;

            mat.SetFloat("RandomOffset", Random.Range(0.0f, 10.0f));

            pos += Vector3.left * 2;
        }
    }
}
