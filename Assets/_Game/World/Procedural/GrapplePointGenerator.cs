using System;
using UnityEngine;

public class GrapplePointGenerator
{
  private readonly GameObject grapplePointPrefab;

  public GrapplePointGenerator(
      GameObject grapplePointPrefab
  )
  {
    this.grapplePointPrefab =
        grapplePointPrefab;
  }

  public GameObject Generate(
      Vector2 position,
      Transform parent
  )
  {
    if (grapplePointPrefab == null)
      return null;

    if (parent == null)
      return null;

    return UnityEngine.Object.Instantiate(
        grapplePointPrefab,
        position,
        Quaternion.identity,
        parent
    );
  }
}