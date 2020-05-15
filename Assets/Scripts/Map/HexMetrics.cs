using UnityEngine;

public static class HexMetrics {

  public const float outerRadius = 10f;
  public const float innerRadius = outerRadius * 0.866025404f;
  public const float solidFactor = 0.75f; // region qith full colour
  public const float blendFactor = 1f - solidFactor; // blend region of Hex

  static Vector3[] corners = {
    new Vector3(0f, 0f, outerRadius),
    new Vector3(innerRadius, 0f, 0.5f * outerRadius),
    new Vector3(innerRadius, 0f, -0.5f * outerRadius),
    new Vector3(0f, 0f, -outerRadius),
    new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
    new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
    new Vector3(0f, 0f, outerRadius) // this is the first corner again made for when we triangulate (ie so the last triangle can be made easily)
  };

  public static Vector3 GetFirstCorner(HexDirection direction) {
    return corners[(int)direction];
  }

  public static Vector3 GetSecondCorner(HexDirection direction) {
    return corners[(int)direction + 1];
  }

  public static Vector3 GetFirstSolidCorner(HexDirection direction) {
    return corners[(int)direction] * solidFactor;
  }

  public static Vector3 GetSecondSolidCorner(HexDirection direction) {
    return corners[(int)direction + 1] * solidFactor;
  }

  public static Vector3 GetBridge(HexDirection direction) {
    return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
  }
}