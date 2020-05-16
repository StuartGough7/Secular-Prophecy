using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

  public int width = 6;
  public int height = 6;
  public Text cellLabelPrefab;
  public Color defaultColor = Color.white;
  public Color touchedColor = Color.magenta;
  public Texture2D noiseSource;


  Canvas gridCanvas;
  HexMesh hexMesh;

  public HexCell cellPrefab;

  HexCell[] cells;

  void OnEnable() {
    HexMetrics.noiseSource = noiseSource; // so the noiseTexture survives recompiles
  }

  void Awake() {
    HexMetrics.noiseSource = noiseSource; // because noiseSource isnt a component we just pass this here as an intermediate
    gridCanvas = GetComponentInChildren<Canvas>();
    hexMesh = GetComponentInChildren<HexMesh>();

    cells = new HexCell[height * width];

    for (int z = 0, i = 0; z < height; z++) {
      for (int x = 0; x < width; x++) {
        CreateCell(x, z, i++);
      }
    }
  }

  void Start() {
    hexMesh.Triangulate(cells); // after hex mesh has awoken triangulate
  }


  void CreateCell(int x, int z, int i) {
    Vector3 position;
    position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f); // The - z / 2 undoes the rhomus offset created from z* 0.5
    position.y = 0f;
    position.z = z * (HexMetrics.outerRadius * 1.5f);

    HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
    cell.transform.SetParent(transform, false);
    cell.transform.localPosition = position;
    cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
    cell.color = defaultColor;

    // Adding East -> West neighbours if the cell is not the first in the column (ie nothing west of it)
    if (x > 0) {
      cell.SetNeighbor(HexDirection.W, cells[i - 1]);
    }
    // Adding SE neighbours, skip first row entirely and handle even and odd rows differently due to zig zag
    if (z > 0) {
      if ((z & 1) == 0) { // the & operator is the bitwise & and is used to check even and odd quickly (ie even always have a 0 at the least significant digit in binary)
        cell.SetNeighbor(HexDirection.SE, cells[i - width]);
        if (x > 0) {
          cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]); // Sw neighbour on thej same row but not the first column
        }
      } else { // odd rows mirror the even logic above
        cell.SetNeighbor(HexDirection.SW, cells[i - width]);
        if (x < width - 1) {
          cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
        }
      }
    }
    // labelling
    Text label = Instantiate<Text>(cellLabelPrefab);
    label.rectTransform.SetParent(gridCanvas.transform, false);
    label.rectTransform.anchoredPosition =
      new Vector2(position.x, position.z);
    label.text = cell.coordinates.ToStringOnSeparateLines();
    cell.uiRect = label.rectTransform;

    cell.Elevation = 0;
  }

  public HexCell GetCell(Vector3 position) {
    position = transform.InverseTransformPoint(position);
    HexCoordinates coordinates = HexCoordinates.FromPosition(position);
    int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
    return cells[index];
  }

  public void Refresh() {
    hexMesh.Triangulate(cells);
  }
}