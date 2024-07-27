using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawn : MonoBehaviourPunCallbacks
{
    public GameObject item1;
    public int item1Count = 1;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        List<int> spawnIndexes = GetUniqueSpawnIndexes(item1Count);

        for (int i = 0; i < item1Count; i++)
        {
            Transform spawnPoint = transform.GetChild(spawnIndexes[i]);
            PhotonNetwork.Instantiate(item1.name, spawnPoint.position, Quaternion.identity);
        }
    }

    private List<int> GetUniqueSpawnIndexes(int count)
    {
        List<int> spawnIndexes = new List<int>();

        while (spawnIndexes.Count < count)
        {
            int spawnNum = Random.Range(0, transform.childCount);
            if (!spawnIndexes.Contains(spawnNum))
            {
                spawnIndexes.Add(spawnNum);
            }
        }

        return spawnIndexes;
    }
}
