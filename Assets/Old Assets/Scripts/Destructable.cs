using UnityEngine;

public class Destructable : MonoBehaviour
{
    public void BlowUp()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        //insert other effects for when destroyed
        //prob a for each to activate particle effect array
    }
}
