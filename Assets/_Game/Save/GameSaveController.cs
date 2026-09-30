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
    GameSaveData gameSaveData =
        new GameSaveData();

    gameSaveData.relics =
        relicSaveService.Capture();

    saveSystem.Save(gameSaveData);
  }

  public bool LoadGame()
  {
    GameSaveData gameSaveData =
        saveSystem.Load();

    if (gameSaveData == null)
      return false;

    if (gameSaveData.relics == null)
      return false;

    return relicSaveService.Restore(
        gameSaveData.relics
    );
  }
}