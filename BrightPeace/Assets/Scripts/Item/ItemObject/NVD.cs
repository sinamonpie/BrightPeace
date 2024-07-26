using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Older;
public class NVD : Item
{
    public float wallHackTime = 2f;

    public override bool UseItem()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrayScreen>().ApplyGrayScreen(wallHackTime);
        GameObject.Find("WallHacker").GetComponent<WallHacker>().ApplyWallHack(wallHackTime);
        return true;
    }

}
