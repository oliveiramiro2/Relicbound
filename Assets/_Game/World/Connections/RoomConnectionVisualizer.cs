using UnityEngine;

public class RoomConnectionVisualizer :
    MonoBehaviour
{
  [SerializeField]
  private Color connectionColor =
      Color.cyan;

  [SerializeField]
  private float connectionWidth =
      0.08f;

  public void Visualize(
      RoomConnectionResult result
  )
  {
    if (result == null)
      return;

    foreach (
        RoomConnection connection
        in result.Connections)
    {
      CreateLine(
          connection
      );
    }
  }

  private void CreateLine(
      RoomConnection connection
  )
  {
    GameObject lineObject =
        new GameObject(
            "RoomConnection"
        );

    lineObject.transform.SetParent(
        transform
    );

    LineRenderer line =
        lineObject.AddComponent<
            LineRenderer>();

    line.positionCount = 2;

    line.startWidth =
        connectionWidth;

    line.endWidth =
        connectionWidth;

    line.useWorldSpace = true;

    Shader shader =
        Shader.Find(
            "Sprites/Default"
        );

    if (shader != null)
    {
      line.material =
          new Material(shader);
    }

    line.startColor =
        connectionColor;

    line.endColor =
        connectionColor;

    line.SetPosition(
        0,
        connection.ExitPosition
    );

    line.SetPosition(
        1,
        connection.EntryPosition
    );
  }
}