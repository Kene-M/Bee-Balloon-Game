using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowBee : MonoBehaviour
{

    public Collider rColl;
    private Vector3 lastTransform;

    private void Start()
    {
        lastTransform = transform.position;
    }

    void Update()
    {
        //// Get the mouse position in world space
        //Vector3 mousePos2D = Input.mousePosition;
        //mousePos2D.z = -Camera.main.transform.position.z; // Set z to match the world plane
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector2 mousePos3DBeforeUpdate = mousePos3D;

        mousePos3D.y += .5f;
        mousePos3D.z += -1f;

        Vector3 mouseDelta = mousePos3D - lastTransform;

        mouseDelta = new Vector3(Mathf.Abs(mouseDelta.x), Mathf.Abs(mouseDelta.y), Mathf.Abs(mouseDelta.z));

        // Check if the mouse is inside the restricted area
        if (IsMouseInsideArea(mousePos3DBeforeUpdate, rColl) && mouseDelta.x <= .5f && mouseDelta.y <= .5f && mouseDelta.z <= .5f )
        {
            //Debug.Log("Mouse is inside the restricted area!");
            

            transform.position = mousePos3D;
            lastTransform = transform.position;
        }
        else
        {
            Debug.Log("Mouse is outside the restricted area!");
        }
    }

    bool IsMouseInsideArea(Vector3 position, Collider area)
    {
        // Get all colliders in the parent and its children
        Collider[] colliders = area.GetComponentsInChildren<Collider>();

        // Check if the position is inside any of the colliders
        foreach (Collider collider in colliders)
        {
            if (collider.bounds.Contains(position))
            {
                return true;
            }
        }

        return false; // Not inside any collider
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
            Destroy(gameObject);

            // Life counter decrement...

        }
    }
}

