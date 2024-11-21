using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
public class FollowBee : MonoBehaviour
{
    public float speed = 40f;  // Speed of movement
    private Rigidbody rb;
    bool isMoving = true;

    private Vector3 lastSafe;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //void Update()
    //{
    //    // Get the mouse position in screen space
    //    Vector3 mouseScreenPosition = Input.mousePosition;

    //    // Convert mouse position to world position at z = 0
    //    //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.z));
    //    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 8.85f));

    //    mouseWorldPosition.y += .5f;
    //    //mouseWorldPosition.z += -1.15f;

    //    // Set the z position explicitly to 0
    //    //mouseWorldPosition.z = -1.15f;

    //    // Move the bee toward the mouse position
    //    Vector3 nextPosition = Vector3.MoveTowards(transform.position, mouseWorldPosition, speed * Time.deltaTime);

    //    //move the bee using rigidbody
    //    rb.MovePosition(nextPosition);

    //    //Debug.Log("z: " + transform.position.z);
    //}

    private void Update()
    {
        //print(isMoving);

        if (isMoving)
        {


            Vector3 mousePos2D = Input.mousePosition;
            mousePos2D.z = -Camera.main.transform.position.z;
            Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

            mousePos3D.y += .5f;
            mousePos3D.z += -1f;

            transform.position = mousePos3D; 
            lastSafe = transform.position;
        }
    }

    public void OnTriggerStay(Collider other)
    {

        //if (other.gameObject.GetComponentInParent<BoxCollider>.tag == "wall")
        //    isMoving = false;
        //else 
        //    isMoving = true;
        print("working");

        if (other.CompareTag("RestrictedArea")) // Tag your box as "RestrictedArea"
        {
            print("inside the if");
            // Revert to the last safe position
            transform.position = lastSafe;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
}
*/


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
            Debug.Log("Mouse is inside the restricted area!");
            

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
}

