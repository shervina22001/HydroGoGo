using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private Dialog dialog; // Reference to the Dialog data
    private DialogManager dialogManager;

    void Awake()
    {
        dialogManager = FindFirstObjectByType<DialogManager>();
    }

    public void TriggerDialog()
    {
        if (dialogManager != null && dialog != null && !dialogManager.IsOpen)
        {
            dialogManager.StartDialog(dialog);
        }
    }

    public void TriggerTutorial()
    {
        if (dialogManager != null && dialog != null && !dialogManager.IsOpen)
        {
            dialogManager.StartTutorial(dialog);
        }
    }
}
