using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl1Zone5 : MonoBehaviour
{
    // Vector3 moveDirection = new Vector3(1f, 0f, 0f); // Direction of movement

    float moveSpeed = 2f;                            // Movement speed
    Vector3 pointA;  // Minimum boundary
    Vector3 pointB;   // Maximum boundary

    //private bool movingForward = true; // Toggle for direction
    private Vector3 targetPoint; // Current target point

    private void Start()
    {
        pointA = new Vector3(transform.position.x, 5.5f, transform.position.z);
        pointB = new Vector3(transform.position.x, 2.9f, transform.position.z);
        targetPoint = pointB;
    }

    void Update()
    {
        // Move the object towards the target point
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint,
            moveSpeed * Time.deltaTime
        );

        // If the object reaches the target point, switch the target
        if (transform.position == targetPoint)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }
}