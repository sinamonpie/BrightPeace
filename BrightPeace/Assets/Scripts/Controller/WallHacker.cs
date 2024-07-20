using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallHacker : MonoBehaviour
{
    public GameObject markerPrefab;
    public Camera sensorCamera;
    public void ApplyWallHack(float wallHackTime)
    {
        StartCoroutine(WallHack(wallHackTime));
    }
    IEnumerator WallHack(float wallHackTime)
    {
        sensorCamera.transform.gameObject.SetActive(true);
        GameObject player = transform.parent.gameObject;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        /*List<GameObject> marker = new List<GameObject>();*/
        foreach (GameObject otherPlayer in players)
        {
            if (otherPlayer != player)
            {
                otherPlayer.GetComponentInChildren<PlayerRenderer>().ApplyHighlight(wallHackTime);

                /*GameObject indicator = Instantiate(markerPrefab, otherPlayer.transform.position, Quaternion.identity);
                indicator.transform.SetParent(otherPlayer.transform);
                marker.Add(indicator);*/
            }
        }

        yield return new WaitForSeconds(wallHackTime);

        sensorCamera.transform.gameObject.SetActive(false);

        /*foreach (GameObject indicator in marker)
        {
            Destroy(indicator);
        }*/

    }
}
