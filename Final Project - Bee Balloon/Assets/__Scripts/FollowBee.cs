using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBee : MonoBehaviour
{
    public float speed = 5f;  // Speed of movement

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse position to world position at z = 0
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.transform.position.z));

        // Set the z position explicitly to 0
        mouseWorldPosition.z = 0f;

        // Move the bee toward the mouse position
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldPosition, speed * Time.deltaTime);
        Debug.Log("z: " +transform.position.z);
    }
}
