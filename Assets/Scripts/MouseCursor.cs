using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10.0f; // This was very important as the camera is 10f away we need to specify what relative position from the camera we in z we want to know about
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = mouseWorldPosition;
    }
}
