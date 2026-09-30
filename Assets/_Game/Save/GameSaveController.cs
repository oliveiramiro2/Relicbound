using UnityEngine;

[RequireComponent(typeof(RelicManager))]
public class GameSaveController : MonoBehaviour
{
  [SerializeField] private RelicDatabase relicDatabase;

  private RelicManager relicManager;

  private RelicSaveService relicSaveService;
  private SaveSystem saveSystem;

  private void Awake()
  {
    relicManager =
        GetComponent<RelicManager>();

    relicSaveService =
        new RelicSaveService(
            relicManager,
            relicDatabase
        );

    saveSystem =
        new SaveSystem();
  }

  public void SaveGame()
  {
    RelicSaveData data =
        relicSaveService.Capture();

    saveSystem.Save(data);
  }

  public bool LoadGame()
  {
    RelicSaveData data =
        saveSystem.Load();

    if (data == null)
      return false;

    return relicSaveService.Restore(data);
  }
}