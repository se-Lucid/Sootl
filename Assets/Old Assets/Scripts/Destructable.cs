using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject stuckMonster;
    public GameObject nextMonster;
    public GameObject[] toDestroy, toUnhide;
    private void Start()
    {
        if (nextMonster != null)
        {
            stuckMonster.SetActive(false);
            nextMonster.SetActive(false);
        }
    }
    public void BlowUp(GameObject go)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        //insert other effects for when destroyed
        //sfx
        //prob a for each to activate particle effect array
        
    }
    public void BlowUp()
    {
        foreach (GameObject go in toUnhide)
        {
            go.SetActive(true);
        }
        foreach(GameObject go in toDestroy)
        {
            Destroy(go);
        }
        
    }
    public void GetStuck()
    {
        Debug.Log("stuck");
        //play car crash sfx
        stuckMonster.SetActive(true);
        stuckMonster.GetComponent<Animator>().SetBool("Lost", true);
    }
    public void GetUnstuck()
    {
        nextMonster.SetActive(true);
        stuckMonster.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetUnstuck();
        }
    }
}
