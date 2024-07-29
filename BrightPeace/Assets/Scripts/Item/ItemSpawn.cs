using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ItemSpawn : NetworkBehaviour
{
    public NetworkPrefabRef item1Prefab;
    public int item1Count = 1;

    private NetworkRunner _runner;

    void Start()
    {
        _runner = FindObjectOfType<NetworkRunner>();

        if (_runner.IsServer)
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
            _runner.Spawn(item1Prefab, spawnPoint.position, Quaternion.identity);
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
