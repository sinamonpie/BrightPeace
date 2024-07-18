using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    
    public static PoolController instance;

    [SerializeField]
    GameObject[] prefabs;
    List<GameObject>[] objPools;
    int poolSize = 1;

    void Start()
    {
        if (instance != this)
        instance = this;

        InitObjPool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitObjPool()
    {
        objPools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < prefabs.Length; i++)
        {
            objPools[i] = new List<GameObject>();

            for(int j = 0; j <poolSize; j++)
            {
                GameObject obj = Instantiate(prefabs[i]);
                obj.SetActive(false);
                objPools[i].Add(obj);
            }
        }
    }

    public GameObject ActivateObj(int index)
    {
        GameObject obj = null;

        for(int i = 0; i < objPools[index].Count; i++)
        {
            if (!objPools[index][i].activeInHierarchy)
            {
                obj = objPools[index][i];
                obj.SetActive(true);
                return obj;
            }
        }

        obj = Instantiate(prefabs[index]);
        objPools[index].Add(obj);
        obj.SetActive(true);

        return obj;
    }
}
