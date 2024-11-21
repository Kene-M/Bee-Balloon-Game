using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBeeTwo : MonoBehaviour
{
    public Collider rColl;

    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        mousePos3D.y += .5f;
        mousePos3D.z += -1f;

        transform.position = mousePos3D;
    }
}
