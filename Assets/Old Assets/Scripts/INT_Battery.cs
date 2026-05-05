using UnityEngine;

public class INT_Battery : MonoBehaviour, Interactable
{
    private int spriteID = 3;
    public int GetSpriteID()
    {
        return spriteID;
    }
    public void PickUp()
    {
        Debug.Log("you picked up the item!");
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Light>().enabled = false;
        //held = true;
    }
    public void Drop(Transform obj)
    {
        Debug.Log("youve dropped the item!");
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Light>().enabled = true;
        transform.position = obj.position;
    }
    public void Use(GameObject obj)
    {
        Debug.Log("i feel used"); //ok
        //Play Voiceline

    }
}
