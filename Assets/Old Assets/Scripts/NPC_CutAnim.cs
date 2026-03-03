using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_CutAnim : MonoBehaviour
{
    public Animator animator;
    private Material drawings;
    public Texture[] textures = new Texture[6];
    private Transform frame;
    public Transform player;
    private int i = 0;

    public void Start()
    {
        frame = transform.GetChild(0).GetComponent<Transform>();
        drawings = frame.GetComponent<MeshRenderer>().material;
        drawings.mainTexture = textures[i];
    }

    public void Update()
    {
        frame.transform.LookAt(player, Vector3.forward);
        frame.transform.position.z.Equals(0);
    }

    public void play()
    {
        animator.SetBool("run", true);
    }
    public void change()
    {
        i++;
        drawings.mainTexture = textures[i];
        if (i > 4)
        {
            i = 0;
        }
    }
}
