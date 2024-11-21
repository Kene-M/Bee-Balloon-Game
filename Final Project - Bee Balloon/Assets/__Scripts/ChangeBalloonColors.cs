using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBalloonColors : MonoBehaviour
{
    // You can define specific colors or generate random ones
    private Color[] colors = { Color.blue, Color.green, Color.yellow, Color.red };


    void Start()
    {
        // Get all the Renderers attached to the parent and its children
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        Color randomColor = colors[Random.Range(0, colors.Length)];
        // Iterate over each renderer and change the material color randomly
        foreach (Renderer renderer in childRenderers)
        {
            // Ensure the renderer has a material
            if (renderer.material != null)
            {
                // Set the material color to the randomly chosen color
                renderer.material.color = randomColor;
            }
        }
    }
}


