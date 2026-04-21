using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class INT_Flashlight : MonoBehaviour, Interactable
{
    private float flashDist = 20f;
    private int spriteID = 2;
    private float storedRange = 6f;
    private float brightRange = 15f;
    private Color storedColor = Color.gray;
    private Color brightColor = Color.white;
    private bool on = false;
    private bool held = false;
    private GameObject lightSource;

    public GameObject lightTarget;
    public Camera playerCamera;
    public int GetSpriteID()
    {
        return spriteID;
    }
    public void PickUp()
    {
        Debug.Log("youve picked up the item!");
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Light>().enabled = false;
        held = true;
    }

    public void Drop(Transform obj)
    {
        held = false;
        if (on)
        {
            lightSource.GetComponent<Light>().range = storedRange;
            lightSource.GetComponent<Light>().color = storedColor;
            gameObject.GetComponent<Light>().enabled = true;
            on = false;
        }
        else
        {
            gameObject.GetComponent<Light>().enabled = false;
        }
        Debug.Log("youve dropped the item!");
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        transform.position = obj.position;
    }

    public void Use(GameObject obj)
    {
        SoundFX_Manager.Instance.Play(SoundFX_Manager.SoundType.Flashlight_Click); //Temp Sound
        if (on)
        {
            obj.GetComponent<Light>().range = storedRange;
            obj.GetComponent<Light>().color = storedColor;
            on = false;

        }
        else
        {
            lightSource = obj;
            storedRange = obj.GetComponent<Light>().range;
            obj.GetComponent<Light>().range = brightRange;
            storedColor = obj.GetComponent<Light>().color;
            obj.GetComponent<Light>().color = brightColor;
            on = true;

        }
    }

    public void Update()
    {
        if (on && held)
        {
            //For light against distant walls
            Ray r = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(r, out RaycastHit hit, flashDist))
            {
                Vector3 hitPoint = hit.point + hit.normal;
                Debug.Log(hitPoint);
                if (hitPoint != new Vector3(0,0,0))
                {
                    if (lightTarget == null)
                    {
                        lightTarget = Instantiate(lightSource, hitPoint, Quaternion.identity);
                        var lightTargetCol = lightTarget.AddComponent<SphereCollider>();
                        lightTargetCol.isTrigger = true;
                    }
                    else 
                    {
                        lightTarget.SetActive(true);
                        lightTarget.transform.position = hitPoint;
                    }
                }
            }
            else if (lightTarget != null)
            {
                lightTarget.SetActive(false);
            }

        }
        else
        {
            if (lightTarget != null)
            {
                lightTarget.SetActive(false);
            } 
        }
    }
}
