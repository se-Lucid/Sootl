using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Fix: MonoBehaviour
{
    public float z_scale = 1;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject wall = transform.GetChild(i).gameObject;
            Renderer wall_renderer = wall.GetComponent<Renderer>();

            wall_renderer.material = new Material(wall_renderer.material);

            Vector3 scale = wall.transform.localScale;
            wall_renderer.material.mainTextureScale = new Vector2(scale.z * z_scale, wall_renderer.material.mainTextureScale.y);
        }
    }
} 
