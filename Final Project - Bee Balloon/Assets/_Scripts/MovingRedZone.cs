using UnityEngine;

public class MovingRedZone : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Vector3 minBounds, maxBounds; // Define maze bounds in 3D

    private Vector3 direction; // Movement direction

    void Start()
    {
        // Random initial direction, normalized for consistent speed
        direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void Update()
    {
        // Move the red zone
        transform.position += direction * speed * Time.deltaTime;

        // Clamp position within maze bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
            Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z)
        );

        // Reverse direction if at bounds
        if (transform.position.x <= minBounds.x || transform.position.x >= maxBounds.x)
            direction.x = -direction.x;
        if (transform.position.y <= minBounds.y || transform.position.y >= maxBounds.y)
            direction.y = -direction.y;
        if (transform.position.z <= minBounds.z || transform.position.z >= maxBounds.z)
            direction.z = -direction.z;
    }
}

