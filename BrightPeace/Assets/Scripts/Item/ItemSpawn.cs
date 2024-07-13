using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject item1;
    public int item1Count = 1;
    public GameObject item2;
    public int item2Count = 1;

    void Start()
    {
        SpawnItems();
    }

    private void SpawnItems()
    {
        List<int> spawnIndexes = GetUniqueSpawnIndexes(item1Count + item2Count);

        for (int i = 0; i < item1Count; i++)
        {
            Transform spawnPoint = transform.GetChild(spawnIndexes[i]);
            GameObject newItem = Instantiate(item1, spawnPoint.position, Quaternion.identity);
        }

        for (int i = item1Count; i < item1Count + item2Count; i++)
        {
            Transform spawnPoint = transform.GetChild(spawnIndexes[i]);
            GameObject newItem = Instantiate(item2, spawnPoint.position, Quaternion.identity);
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
