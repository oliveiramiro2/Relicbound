using UnityEngine;

public class PlatformGraphDebug : MonoBehaviour
{
  [SerializeField]
  private Color connectionColor = Color.yellow;

  [SerializeField]
  private float connectionHeight = 0.1f;

  private PlatformGraph graph;

  public void SetGraph(
      PlatformGraph graph
  )
  {
    this.graph =
        graph;
  }

  private void OnDrawGizmos()
  {
    if (graph == null)
      return;

    foreach (
        GeneratedPlatform platform
        in graph.Platforms)
    {
      //IReadOnlyListWrapper connections =
          //null;
    }
  }
}