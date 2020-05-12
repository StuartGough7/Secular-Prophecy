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

  public override string ToString() {
    return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
  }

  public string ToStringOnSeparateLines() {
    return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
  }
}