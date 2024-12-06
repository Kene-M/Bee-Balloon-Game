using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Zone1 : MonoBehaviour
{
    //public Vector3 rotationCenter = new Vector3(0.31f, 2.07f, -1.15f); // The point around which to rotate
    public Vector3 rotationCenter; // The point around which to rotate
    private Vector3 rotationAxis = Vector3.forward; // Axis of rotation (e.g., Vector3.up for Y-axis)
    private float rotationSpeed = 20f; // Speed of rotation in degrees per second

    public void SetCenter (Vector3 center) {
        rotationCenter = center;
    }

    void Update()
    {
        // Rotate around the specified point
        transform.RotateAround(rotationCenter, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
