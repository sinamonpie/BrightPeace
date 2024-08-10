using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EndingObject
{
    public UserRole role;
    public GameObject roleobject;
    public Ending[] endingobject;
}

[System.Serializable]
public class Ending
{
    public GameObject obj;
    public UserEnding ending;
}
public class EndingManager : MonoBehaviour
{
    public EndingObject[] endingObjects;
    private UserRole userRole;
    private UserEnding userEnding;

    // Start is called before the first frame update
    void Start()
    {
        userRole = GameManager.Instance.role;
        userEnding = GameManager.Instance.endding;
        SetEnding();

    }
    public void SetEnding()
    {
        for(int i = 0; i < endingObjects.Length; i++)
        {
            if (endingObjects[i].role == userRole)
            { 
                endingObjects[i].roleobject.SetActive(true);
                foreach(Ending obj in endingObjects[i].endingobject)
                {
                    if (obj.ending == userEnding)
                    {
                        obj.obj.SetActive(true);
                        return;
                    }
                }
            }

        }
    }
    public void SetEndingData()
    {

    }
}
