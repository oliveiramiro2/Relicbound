using UnityEngine;

[RequireComponent(typeof(RelicManager))]
[RequireComponent(typeof(PlayerSaveHandler))]
public class GameSaveController : MonoBehaviour
{
  [SerializeField] private RelicDatabase relicDatabase;

  private RelicManager relicManager;
  private PlayerSaveHandler playerSaveHandler;
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

    playerSaveHandler =
        GetComponent<PlayerSaveHandler>();
  }

  public void SaveGame()
  {
    GameSaveData gameSaveData =
        new GameSaveData();

    gameSaveData.relics =
        relicSaveService.Capture();

    gameSaveData.player =
        playerSaveHandler.Capture();

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

    bool relicsRestored =
        relicSaveService.Restore(
            gameSaveData.relics
        );

    if (!relicsRestored)
      return false;

    playerSaveHandler.Restore(
        gameSaveData.player
    );

    return true;
  }
}