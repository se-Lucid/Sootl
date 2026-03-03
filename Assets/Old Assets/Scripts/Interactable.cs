using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    int GetSpriteID(); //gets sprite ID if applicable

    //the pickup action of putting the item into the hand is done in PlayerController in the Interact method
    void PickUp(); //add a definition to objects tagged "Pickup"
    void Drop(Transform hand); //same as above

    //to be added by objects extending this class
    void Use(GameObject obj = null); //add definition to stationary objects 
    
}
