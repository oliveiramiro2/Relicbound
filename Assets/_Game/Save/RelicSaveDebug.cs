using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RelicManager))]
public class RelicSaveDebug : MonoBehaviour
{
  [SerializeField] private RelicDatabase database;

  private RelicManager relicManager;
  private RelicSaveService saveService;

  private RelicSaveData cachedSave;

  private void Awake()
  {
    relicManager = GetComponent<RelicManager>();

    saveService = new RelicSaveService(
        relicManager,
        database
    );
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
  }

  private void Restore()
  {
    if (cachedSave == null)
    {
      Debug.LogWarning(
          "Nenhum save foi capturado."
      );

      return;
    }

    bool restored =
        saveService.Restore(cachedSave);

    Debug.Log(
        $"SAVE RESTORED: {restored}"
    );
  }
}