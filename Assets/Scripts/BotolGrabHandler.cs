using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BotolGrabHandler : XRBaseInteractable
{
    public BotolMaterialStacker stacker;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (stacker != null)
        {
            stacker.ExtractMaterialFromTop(args.interactorObject);
        }
    }
}
