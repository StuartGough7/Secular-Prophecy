using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

  public Color[] colors;

  public HexGrid hexGrid;

  private Color activeColor;

  void Awake() {
    SelectColor(0);
  }

  void Update() {
    // the second part is to ignore the clicking of game objects if on the Overlaid UI
    if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
      HandleInput();
    }
  }

  void HandleInput() {
    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(inputRay, out hit)) {
      hexGrid.ColorCell(hit.point, activeColor);
    }
  }

  public void SelectColor(int index) {
    activeColor = colors[index];
  }
}