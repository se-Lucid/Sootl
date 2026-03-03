using UnityEngine;
using System.Collections.Generic;
using System.Security;

public class DynamicBillboarding : MonoBehaviour
{
    [Header("Insert in clockwise order (eg. front, right, back, left)")]
    public SpriteRenderer[] sides;
    private Vector3 awakeRot;
    private int numSides;
    private float sectionSize;
    private float startDegree;
    private float degree = 0f;
    [Header("Disable the")]
    public bool XRotation;
    public bool YRotation;
    public bool ZRotation;

    void Start()
    {
        awakeRot = transform.rotation.eulerAngles;
        numSides = sides.Length;
        sectionSize = 360 / numSides;
        startDegree = 360 - (sectionSize / 2);
        Debug.Log(numSides + " " + sectionSize + " " + startDegree);

    }

    void Update()
    {
        foreach (SpriteRenderer side in sides)
        {
            side.transform.LookAt(Camera.main.transform.position, Vector3.up);
            Vector3 rotation = side.transform.localRotation.eulerAngles;
            if (XRotation)
            {
            rotation.x = awakeRot.x;
            }
            if (ZRotation)
            {
                rotation.z = awakeRot.z;
            }
            side.transform.localRotation = Quaternion.Euler(rotation);
            if(side != sides[CheckDirection()])
            {
                side.gameObject.SetActive(false);
            }
            else
            {
                side.gameObject.SetActive(true);
            }
        }
        //sides[CheckDirection()].gameObject.SetActive(true);
    }

    private int CheckDirection()
    {
        
        //this makes the function work for an arbitrary number of sides ig
        degree = Mathf.Abs(sides[0].transform.localEulerAngles.y);
        Debug.Log(degree);
        return degree > sectionSize / 2 && degree < startDegree ? CheckDirection(1): 0;
    }
    private int CheckDirection(int times)
    {
        if (times > numSides - 1) { Debug.LogError(degree); return 0; }
        return degree > (sectionSize / 2) + (sectionSize * times) ? CheckDirection(times + 1) : times;
    }
}
