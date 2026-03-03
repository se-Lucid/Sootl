using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public NPC_CutAnim playAnimation;
    public void OnTriggerEnter(Collider other)
    {
        playAnimation.play();
    }
}
