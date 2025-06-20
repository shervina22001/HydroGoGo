using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog; // Reference to the Dialog scriptable object

    public void TriggerDialog()
    {
        var dialogManager = FindFirstObjectByType<DialogManager>();
        if (dialogManager != null && !dialogManager.IsOpen)
        {
            dialogManager.StartDialog(dialog);
        }
    }
    public void TriggerTutorial()
    {
        var dialogManager = FindFirstObjectByType<DialogManager>();
        if (dialogManager != null && !dialogManager.IsOpen)
        {
            dialogManager.StartDialog(dialog);
        }
    }
}
