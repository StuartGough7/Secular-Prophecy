using UnityEngine;

[System.Serializable]
public struct HexCoordinates {

  [SerializeField]
  private int x, z;

  public int X {
    get {
      return x;
    }
  }

  public int Z {
    get {
      return z;
    }
  }

  //Y is the horizontal mirror of X and gives us the 3rd axis of movement diagonjally the other way. All degrees added together equate to 0 so we can calulate Y from the other 2
  public int Y {
    get {
      return -X - Z;
    }
  }

  public HexCoordinates(int x, int z) {
    this.x = x;
    this.z = z;
  }

  public static HexCoordinates FromOffsetCoordinates(int x, int z) {
    return new HexCoordinates(x - z / 2, z); // converts the zig zag to axial
  }

  public static HexCoordinates FromPosition(Vector3 position) {
    float x = position.x / (HexMetrics.innerRadius * 2f);
    float y = -x;
    float offset = position.z / (HexMetrics.outerRadius * 3f);
    x -= offset;
    y -= offset;
    int iX = Mathf.RoundToInt(x);
    int iY = Mathf.RoundToInt(y);
    int iZ = Mathf.RoundToInt(-x - y);
    if (iX + iY + iZ != 0) {
      // this is due to errors in the rounding at teh edges. The value with the biggest rounding delta is clearly the furthest away so we should ignore
      float dX = Mathf.Abs(x - iX);
      float dY = Mathf.Abs(y - iY);
      float dZ = Mathf.Abs(-x - y - iZ);

      if (dX > dY && dX > dZ) {
        iX = -iY - iZ;
      } else if (dZ > dY) {
        iZ = -iX - iY;
      }
    }
    return new HexCoordinates(iX, iZ);
  }

  public override string ToString() {
    return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
  }

  public string ToStringOnSeparateLines() {
    return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
  }
}