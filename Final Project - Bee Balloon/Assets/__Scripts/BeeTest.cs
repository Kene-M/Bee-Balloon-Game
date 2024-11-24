using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTest : MonoBehaviour
{
    //    private void Update()
    //    {
    //        Vector3 mousePos2D = Input.mousePosition;
    //        mousePos2D.z = -Camera.main.transform.position.z;
    //        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);


    //        transform.position = mousePos3D;


    //    }
    //}

    
    public float speed = 10f;           // Speed at which the sphere moves
    public float stopDistance = 0.1f;  // Distance threshold to stop the sphere

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse position to world space
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Preserve z depth
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Calculate direction from the sphere to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Stop moving if the sphere is close to the target position
        if (distanceToTarget > stopDistance)
        {
            // Apply velocity to move towards the target
            rb.velocity = direction * speed;
        }
        else
        {
            // Stop the sphere by setting velocity to zero
            rb.velocity = Vector3.zero;
        }
    }
}


