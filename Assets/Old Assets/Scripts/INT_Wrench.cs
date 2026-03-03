using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class INT_Wrench : MonoBehaviour, Interactable
{
    private int spriteID = 1;
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
    }

    public void Drop(Transform obj)
    {
        Debug.Log("youve dropped the item!");
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        transform.position = obj.position;
        gameObject.GetComponent<Light>().enabled = true;
    }

    public void Use(GameObject obj)
    {
        Debug.Log("i feel used"); //ok
    }
    
}
