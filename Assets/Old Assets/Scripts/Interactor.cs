using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public void Interact(GameObject gameObject);
}
public class Interactor : MonoBehaviour
{
    public GameObject InteractorSource;
    public float grabDistance = 5f;

    private void Start()
    {
        InteractorSource = this.gameObject;
        grabDistance = 5f;
    }


    void Update()
    {
        
    }
}
