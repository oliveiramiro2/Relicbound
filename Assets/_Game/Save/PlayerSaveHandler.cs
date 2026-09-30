using UnityEngine;

public class PlayerSaveHandler : MonoBehaviour
{
  public PlayerSaveData Capture()
  {
    Vector3 position = transform.position;

    return new PlayerSaveData
    {
      positionX = position.x,
      positionY = position.y
    };
  }

  public void Restore(PlayerSaveData data)
  {
    if (data == null)
      return;

    transform.position = new Vector3(
        data.positionX,
        data.positionY,
        transform.position.z
    );
  }
}