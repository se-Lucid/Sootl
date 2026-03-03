using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class SpotlightAnimator : MonoBehaviour
{
    //NOTE: This class is not for animating characters.

    public GameObject cam;          //We must access the player's camera for billboarding.
    public Light lightSource;       //Are we using a built-in light source?
    public GameObject obj;          //If we are using our own objects as a base.
    public Texture2D[] lightFrame;  //An array containing all frames
    public float frameTime;         //How fast should frames alternate?
    public bool billboardAll;       //Will the sprite always face the camera?
    public bool billboardLong;      //Will the sprite face the camera only across the X and Z axis?
    
    //NOTE: billboardAll and billboardLong should never BOTH be set to true.

    void Start()
    {
        if (lightSource != null)
        {
            lightSource.cookie = lightFrame[0];
            StartCoroutine(FlickerCoroutine());
        }
        if (obj != null)
        {
            StartCoroutine(objAnimCoroutine());
        }
    }

    private IEnumerator FlickerCoroutine()
    {
        for (int i = 0; i < lightFrame.Length; i++)
        {
            lightSource.cookie = lightFrame[i];
            yield return new WaitForSeconds(frameTime);
        }

        yield return null;
        StartCoroutine(FlickerCoroutine());
    }
    private IEnumerator objAnimCoroutine()
    {
        //Most objects will be animated using a spritesheet
        //This coroutine exists to specify other features.
        if(billboardAll)
        {
            obj.transform.rotation = cam.transform.rotation;
        }
        else if(billboardLong)
        {
            obj.transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);
        }
        yield return null;
        StartCoroutine(objAnimCoroutine());
    }
}
