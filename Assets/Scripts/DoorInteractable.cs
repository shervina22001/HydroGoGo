using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorInteractable : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    public XRBaseInteractable interactable;

    void Start()
    {
        animator = GetComponent<Animator>();
        var interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnDoorClicked);
    }

    void OnDoorClicked(SelectEnterEventArgs args)
    {
        if (!isOpen)
        {
            animator.SetTrigger("OpenDoor");
        }
        else
        {
            animator.SetTrigger("CloseDoor");
        }

        isOpen = !isOpen;
    }   
}
