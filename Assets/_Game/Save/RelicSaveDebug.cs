using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RelicManager))]
public class RelicSaveDebug : MonoBehaviour
{
  [SerializeField] private RelicDatabase database;

  private RelicManager relicManager;
  private RelicSaveService saveService;
  private SaveSystem saveSystem;
  private RelicSaveData cachedSave;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();

    saveService = new RelicSaveService(
        relicManager,
        database
    );

    saveSystem = new SaveSystem();
  }

  private void Update()
  {
    if (Keyboard.current.f5Key.wasPressedThisFrame)
    {
      Capture();
    }

    if (Keyboard.current.f9Key.wasPressedThisFrame)
    {
      Restore();
    }
  }

  private void Capture()
  {
    cachedSave = saveService.Capture();

    string json = JsonUtility.ToJson(
        cachedSave,
        true
    );

    Debug.Log(
        $"SAVE CAPTURED:\n{json}"
    );

    saveSystem.Save(cachedSave);
  }

  private void Restore()
  {
    RelicSaveData loadedData =
        saveSystem.Load();

    if (loadedData == null)
    {
      Debug.LogWarning(
          "Nenhum save encontrado."
      );

      return;
    }

    bool restored =
        saveService.Restore(loadedData);

    Debug.Log(
        $"SAVE RESTORED: {restored}"
    );
  }
}