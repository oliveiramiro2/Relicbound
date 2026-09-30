using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
  [SerializeField] private GameObject inventoryPanel;

  private PlayerInputActions inputActions;

  private void Awake()
  {
    inputActions = new PlayerInputActions();
  }

  private void OnEnable()
  {
    inputActions.UI.Enable();
    inputActions.UI.Inventory.performed += OnInventoryPressed;
  }

  private void OnDisable()
  {
    inputActions.UI.Inventory.performed -= OnInventoryPressed;
    inputActions.UI.Disable();
  }

  private void OnInventoryPressed(InputAction.CallbackContext context)
  {
    inventoryPanel.SetActive(!inventoryPanel.activeSelf);
  }
}