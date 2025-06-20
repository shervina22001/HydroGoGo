using UnityEngine;
using UnityEngine.InputSystem;

public class DebugSelectSimulator : MonoBehaviour
{
    public InputActionProperty selectAction;  // Diassign dari XR Controller Select Action
    public InputActionProperty primaryAction;
    public InputActionProperty secondaryAction;

    private void Update()
    {
        if (selectAction.action.WasPressedThisFrame())
        {
            Debug.Log("[DEBUG] Select (trigger) ditekan.");
        }

        if (primaryAction.action.WasPressedThisFrame())
        {
            Debug.Log("[DEBUG] Primary button ditekan.");
        }

        if (secondaryAction.action.WasPressedThisFrame())
        {
            Debug.Log("[DEBUG] Secondary button ditekan.");
        }
    }
}
