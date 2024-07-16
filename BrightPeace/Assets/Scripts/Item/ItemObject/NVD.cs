using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVD : Item
{
    public GameObject playerIndicatorPrefab;
    public float wallHackTime = 2f;

    public override bool UseItem()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrayScreen>().ApplyGrayScreen(wallHackTime);
        return true;
    }

/*    IEnumerator WallHack(GameObject player)
    {
        overlay.SetActive(true);

        List<GameObject> indicators = new List<GameObject>();
        foreach (Transform player in allPlayers)
        {
            if (player != this.transform)
            {
                Debug.Log(player.transform.position);
                GameObject indicator = Instantiate(playerIndicatorPrefab, player.position, Quaternion.identity);
                indicator.transform.SetParent(player);
                indicators.Add(indicator);
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject otherPlayer in players)
        {
            if (otherPlayer != player)
            {
                Debug.Log(otherPlayer.transform.position);
            }
        }
        Debug.Log("투시경 사용중");

        yield return new WaitForSeconds(wallHackTime);

        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }

        overlay.SetActive(false);
    }*/

}
