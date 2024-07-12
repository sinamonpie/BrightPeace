using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject spawnPointSet;
    Transform spawnPoint;

    private void Start()
    {
        int spawnNum = Random.Range(0, 10);

        spawnPoint = spawnPointSet.transform.GetChild(spawnNum);

        this.transform.position = spawnPoint.position;
    }
}
