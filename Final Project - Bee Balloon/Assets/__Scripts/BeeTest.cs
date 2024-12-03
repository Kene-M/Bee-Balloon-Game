using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BeeTest : MonoBehaviour
//{
//    //    private void Update()
//    //    {
//    //        Vector3 mousePos2D = Input.mousePosition;
//    //        mousePos2D.z = -Camera.main.transform.position.z;
//    //        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);


//    //        transform.position = mousePos3D;


//    //    }
//    //}


//    public float speed;           // Speed at which the sphere moves
//    public float stopDistance;  // Distance threshold to stop the sphere

//    private Rigidbody rb;

//    void Start()
//    {
//        speed = 10f;
//        stopDistance = .1f;
//        // Get the Rigidbody component
//        rb = GetComponent<Rigidbody>();
//    }

//    void FixedUpdate()
//    {
//        // Get the mouse position in screen space
//        Vector3 mouseScreenPosition = Input.mousePosition;

//        // Convert mouse position to world space
//        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Preserve z depth
//        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

//        // Calculate direction from the sphere to the target position
//        Vector3 direction = (targetPosition - transform.position).normalized;

//        // Calculate distance to the target
//        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

//        // Stop moving if the sphere is close to the target position
//        if (distanceToTarget > stopDistance)
//        {
//            // Apply velocity to move towards the target
//            rb.velocity = direction * speed;
//        }
//        else
//        {
//            // Stop the sphere by setting velocity to zero
//            rb.velocity = Vector3.zero;
//        }
//    }
//}


public class BeeTest : MonoBehaviour
{
    public float maxSpeed = 15f;       // Maximum speed at which the bee moves
    public float smoothFactor = 15f; // How quickly the bee adjusts its velocity
    public float stopDistance;   // Distance threshold to stop the bee

    private Rigidbody rb;

    void Start()
    {
        //maxSpeed = 10f;
        stopDistance = 0.1f;

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

        // Calculate direction from the bee to the target position
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // Stop moving if the bee is close to the target position
        if (distanceToTarget > stopDistance)
        {
            // Scale speed by distance, capping it at maxSpeed
            //float scaledSpeed = Mathf.Clamp(distanceToTarget, 0, maxSpeed);

            // Apply velocity to move towards the target
            //rb.velocity = direction * scaledSpeed;


            // Smoothly interpolate the current velocity towards the desired velocity
            Vector3 desiredVelocity = direction * maxSpeed;
            rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, smoothFactor * Time.fixedDeltaTime);
        }
        else
        {
            // Stop the bee by setting velocity to zero
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has the tag "redzone"
        if (other.CompareTag("redzone"))
        {
            //Debug.Log("Bee collided with a red zone!");
            Destroy(gameObject);

            // Life counter decrement...

        }

        // Check if balloon.
        else if (other.CompareTag("Balloon"))
        {
            // Destroy balloon
            Destroy(other.gameObject);

            // Award points...

        }

        // Check if bomb 
        else if (other.CompareTag("Bomb"))
        {
            Destroy(other.transform.parent.gameObject);
            
            //Destroy(gameObject);
            // Lose points instead

        }
    }
}
