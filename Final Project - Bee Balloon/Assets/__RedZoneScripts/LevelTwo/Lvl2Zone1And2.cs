using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl2Zone1And2 : MonoBehaviour
{
    Vector3 center; // Center of the circular motion
    float radius = 0.1f;            // Radius of the circular motion
    float angularSpeed = 100f;     // Speed of movement (degrees per second)

    private float currentAngle = 0f;     // Track the angle of motion

    private void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        // Increment the angle based on angular speed
        currentAngle += angularSpeed * Time.deltaTime;

        // Convert angle to radians for correct trigonometric calculations
        float radians = currentAngle * Mathf.Deg2Rad;

        // Calculate X and Z positions for circular motion
        float x = center.x + Mathf.Cos(radians) * radius;
        float y = center.y + Mathf.Sin(radians) * radius;
        //float z = center.z + Mathf.Sin(radians) * radius;

        // Update the object's position (keeping the Y-axis the same)
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
