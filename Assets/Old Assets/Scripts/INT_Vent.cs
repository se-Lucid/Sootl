using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INT_Vent : MonoBehaviour, Interactable
{
    public int GetSpriteID(){
        return 0;
    }

    public void PickUp()
    {

    }
    public void Drop(Transform obj)
    {

    }
    public void Use(GameObject obj)
    {
        if (obj == null)
        {
            Debug.Log("You need an item to use this!");
        }
        else if(obj.GetComponent<INT_Wrench>())
        {
            gameObject.SetActive(false);
        }
        else
        {

            Debug.Log("You need a different item to use this!");
        }
    }
}
