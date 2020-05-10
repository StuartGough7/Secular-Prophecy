using UnityEngine;

public class MouseController : MonoBehaviour
{
  public LayerMask LayerIDForHexTiles;

  int mouseDragThreshold = 1;                               // Threshold of mouse movement to start a drag
  Vector3 lastMousePosition;                                // From Input.mousePosition
  Vector3 lastMouseGroundPlanePosition;
  Vector3 cameraTargetOffset;
  delegate void UpdateFunc();
  UpdateFunc Update_CurrentFunc;


  void Start()
  {
    Update_CurrentFunc = Update_DetectModeStart;
  }

  void Update()
  {
    Update_CurrentFunc();
    Update_ScrollZoom();
    lastMousePosition = Input.mousePosition;
  }

  public void CancelUpdateFunc()
  {
    Update_CurrentFunc = Update_DetectModeStart;
  }

  void Update_DetectModeStart()
  {
    // Check here(?) to see if we are over a UI element,
    // if so -- ignore mouse clicks and such.
    if (Input.GetMouseButtonUp(0))
    {
      Debug.Log("MOUSE DOW");
      // MouseToHex();
    }

    if (Input.GetMouseButton(0) &&
        Vector3.Distance(Input.mousePosition, lastMousePosition) > mouseDragThreshold)
    {
      // Left button is being held down AND the mouse moved? That's a camera drag!
      Update_CurrentFunc = Update_CameraDrag;

      lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
      Update_CurrentFunc();
    }
  }

  void Update_ScrollZoom()
  {
    // Zoom to scrollwheel
    float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
    float minHeight = 10;
    float maxHeight = 35;
    // Move camera towards hitPos
    Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);
    Vector3 dir = hitPos - Camera.main.transform.position;

    Vector3 p = Camera.main.transform.position;

    // Stop zooming out at a certain distance.
    // TODO: Maybe you should still slide around at 20 zoom?
    if (scrollAmount > 0 || p.y < (maxHeight - 0.1f))
    {
      cameraTargetOffset += dir * scrollAmount;
    }
    Vector3 lastCameraPosition = Camera.main.transform.position;
    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position + cameraTargetOffset, Time.deltaTime * 5f);
    cameraTargetOffset -= Camera.main.transform.position - lastCameraPosition;


    p = Camera.main.transform.position;
    if (p.y < minHeight)
    {
      p.y = minHeight;
    }
    if (p.y > maxHeight)
    {
      p.y = maxHeight;
    }
    Camera.main.transform.position = p;

    // Change camera angle
    Camera.main.transform.rotation = Quaternion.Euler(
        Mathf.Lerp(20, 60, Camera.main.transform.position.y / maxHeight),
        Camera.main.transform.rotation.eulerAngles.y,
        Camera.main.transform.rotation.eulerAngles.z
    );
  }

  void Update_CameraDrag()
  {
    if (Input.GetMouseButtonUp(0))
    {
      CancelUpdateFunc();
      return;
    }

    Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);
    Vector3 diff = lastMouseGroundPlanePosition - hitPos;
    Camera.main.transform.Translate(diff, Space.World);

    lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);
  }

  // Hex MouseToHex() {
  //   Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
  //   RaycastHit hitInfo;

  //   int layerMask = LayerIDForHexTiles.value;

  //   if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, layerMask)) {
  //     // Something got hit
  //     Debug.Log(hitInfo.collider.name);

  //     // The collider is a child of the "correct" game object that we want.
  //     // because rigidbody is on the parent its smart enough to identify what is the closest rigidbody
  //     GameObject hexGO = hitInfo.rigidbody.gameObject;
  //     Debug.Log(hexGO);

  //     return // hexMap.GetHexFromGameObject(hexGO);
  //   }

  //   Debug.Log("Found nothing.");
  //   return null;
  // }

  Vector3 MouseToGroundPlane(Vector3 mousePos)
  {
    Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

    if (mouseRay.direction.y >= 0)                                          // What is the point at which the mouse ray intersects Y=0
    {
      return Vector3.zero;                                                  // Why is mouse pointing up?
    }

    float rayLength = (mouseRay.origin.y / mouseRay.direction.y);
    return mouseRay.origin - (mouseRay.direction * rayLength);
  }
}
