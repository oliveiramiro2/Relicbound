using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameSaveController))]
public class GameSaveInput : MonoBehaviour
{
  private GameSaveController saveController;
  private PlayerInputActions inputActions;

  private void Awake()
  {
    saveController =
        GetComponent<GameSaveController>();

    inputActions =
        new PlayerInputActions();
  }

  private void OnEnable()
  {
    inputActions.UI.Enable();

    inputActions.UI.Save.performed += OnSave;
    inputActions.UI.Load.performed += OnLoad;
  }

  private void OnDisable()
  {
    inputActions.UI.Save.performed -= OnSave;
    inputActions.UI.Load.performed -= OnLoad;

    inputActions.UI.Disable();
  }

  private void OnSave(InputAction.CallbackContext context)
  {
    saveController.SaveGame();
  }

  private void OnLoad(InputAction.CallbackContext context)
  {
    bool loaded =
        saveController.LoadGame();

    Debug.Log(
        $"LOAD RESULT: {loaded}"
    );
  }
}