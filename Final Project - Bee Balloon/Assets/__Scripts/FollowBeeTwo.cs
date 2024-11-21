using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBeeTwo : MonoBehaviour
{
    public float speed = 40f;  // Speed of movement
    private Rigidbody rb;
    //bool isMoving = true;

    private Vector3 lastSafe;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse position to world position at z = 0
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.z));
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 8.85f));

        //mouseWorldPosition.y += .5f;
        //mouseWorldPosition.z += -1.15f;

        // Set the z position explicitly to 0
        //mouseWorldPosition.z = -1.15f;

        // Move the bee toward the mouse position
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, mouseWorldPosition, speed * Time.deltaTime);

        //move the bee using rigidbody
        rb.MovePosition(nextPosition);

        //Debug.Log("z: " + transform.position.z);
    }
}
