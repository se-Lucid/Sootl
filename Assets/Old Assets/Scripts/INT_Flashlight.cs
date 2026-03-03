using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INT_Flashlight : MonoBehaviour, Interactable
{
    private int spriteID = 2;
    private float storedRange = 6f;
    private float brightRange = 15f;
    private Color storedColor = Color.gray;
    private Color brightColor = Color.white;
    private bool on = false;
    private GameObject lightSource;
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
        if (on) {
            lightSource.GetComponent<Light>().range = storedRange;
            lightSource.GetComponent<Light>().color = storedColor;
            on = false;
            gameObject.GetComponent<Light>().enabled = true;
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
}
