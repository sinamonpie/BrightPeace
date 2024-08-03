using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallHacker : MonoBehaviour
{
    Camera sensorCamera;
    void Start()
    {
        sensorCamera = FindObjectOfType<SensorCamera>().GetComponent<Camera>();
    }
    public void ApplyWallHack(float wallHackTime)
    {
        StartCoroutine(WallHack(wallHackTime));
    }
    IEnumerator WallHack(float wallHackTime)
    {
        sensorCamera.enabled = true;
        GameObject player = transform.parent.gameObject;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject otherPlayer in players)
        {
            if (otherPlayer != player)
            {
                PlayerState state = otherPlayer.GetComponent<PlayerState>();
                if(state != null && !state.IsInCabinet())
                {
                    otherPlayer.GetComponentInChildren<PlayerRenderer>().ApplyHighlight(wallHackTime);
                }
            }
        }

        yield return new WaitForSeconds(wallHackTime);

        sensorCamera.enabled = false;

    }
}
