using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    private void Start()
    {
        SpawnCharacters();
    }

    private void SpawnCharacters()
    {
        List<int> spawnIndexes = GetUniqueSpawnIndexes(3); 

        Transform spawnPoint1 = transform.GetChild(spawnIndexes[0]);
        player1.transform.position = spawnPoint1.position;

        Transform spawnPoint2 = transform.GetChild(spawnIndexes[1]);
        player2.transform.position = spawnPoint2.position;

        Transform spawnPoint3 = transform.GetChild(spawnIndexes[2]);
        player3.transform.position = spawnPoint3.position;
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
