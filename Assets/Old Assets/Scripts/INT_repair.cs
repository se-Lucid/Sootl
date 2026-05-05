using UnityEngine;

public class INT_repair : MonoBehaviour, Interactable
{
    public int batteryAmt = 0;
    public GameObject flashlight;
    public PlayerController player;
    public int GetSpriteID()
    {
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
        else if (obj.GetComponent<INT_Battery>())
        {
            if (player.heldItem1 != null 
                && player.heldItem1.GetComponent<INT_Battery>()
                )
            {
                player.heldItem1.GetComponent<Interactable>().Drop(player.hand1.transform);
                player.heldItem1 = null;
                player.hand1.GetComponent<SpriteRenderer>().sprite = player.textures[0];
            }
            else
            {
                player.heldItem2.GetComponent<Interactable>().Drop(player.hand2.transform);
                player.heldItem2 = null;
                player.hand2.GetComponent<SpriteRenderer>().sprite = player.textures[0];
            }

            obj.SetActive(false);
            batteryAmt++;
            //batytery load sfx
            if (batteryAmt > 1)
            {
                flashlight.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("You need a different item to use this!");
        }
    }
}
