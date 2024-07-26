using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallHacker : MonoBehaviour
{
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
        foreach (GameObject otherPlayer in players)
        {
            if (otherPlayer != player)
            {
                otherPlayer.GetComponentInChildren<PlayerRenderer>().ApplyHighlight(wallHackTime);

            }
        }

        yield return new WaitForSeconds(wallHackTime);

        sensorCamera.transform.gameObject.SetActive(false);

    }
}
