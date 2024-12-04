using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeTest : MonoBehaviour
{
    public float maxSpeed = 15f;       // Maximum speed at which the bee moves
    public float smoothFactor = 15f; // How quickly the bee adjusts its velocity
    public float stopDistance;   // Distance threshold to stop the bee

    private Rigidbody rb;

    // public Button respawnButton;

    public Vector3 spawnPosition;

    public Quaternion spawnRotation;

        
   
    void Start()
    {
        // Main.S.spawnButton.gameObject.SetActive(false);

        //maxSpeed = 10f;
        stopDistance = 0.1f;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse position to world space
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Preserve z depth
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Calculate direction from the bee to the target position
        //Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 direction = (targetPosition - new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z)).normalized; // Make the bee closer to the mouse.

        // Calculate distance to the target
        //float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z), targetPosition); // Make the bee closer to the mouse.

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
            //Destroy(gameObject);

            // Life counter decrement
            Main.S.numBees--;

            // Reset position of bee and prompt a restart button.
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
            this.gameObject.SetActive(false);
            Main.S.spawnButton.gameObject.SetActive(true);
        }

        // Check if balloon.
        else if (other.CompareTag("Balloon"))
        {
            // Destroy balloon
            Destroy(other.gameObject);
            Main.S.numDestroyedBalloons++;

            // Award points
            Main.S.AwardPoints();
        }

        // Check if bomb 
        else if (other.CompareTag("Bomb"))
        {
            Destroy(other.transform.parent.gameObject);
            //Destroy(gameObject);
            Debug.Log("Bee collided with a bomb!");

            // Life counter decrement
            Main.S.numBees--;

            // Reset position of bee and prompt a restart button.
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
            Main.S.spawnButton.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
