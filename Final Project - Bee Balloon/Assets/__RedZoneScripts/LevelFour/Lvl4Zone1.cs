using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4Zone1 : MonoBehaviour
{
    private float moveSpeed = 8f; // Speed of movement
    public Vector3[] moveOffsets; // Array of movement offsets
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Record the starting position
        moveOffsets = new Vector3[]
        {
            new Vector3(6.79f+.19f, 0, 0),  // Move right
            new Vector3(0, -4.01f + .71f, 0), // Move down
            new Vector3(-.19f - 8.61f, 0, 0), // Move left
            new Vector3(0, -.71f - 2.85f, 0), // Move down
            new Vector3(1.03f + 8.61f, 0, 0)   // Move right
        };

        StartCoroutine(MoveInPattern());
    }

    private IEnumerator MoveInPattern()
    {
        while (true)
        {
            foreach (Vector3 offset in moveOffsets)
            {
                Vector3 targetPosition = transform.position + offset;
                yield return MoveToPosition(targetPosition);
            }

            // Reset back to the starting position
            transform.position = startPosition;
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
