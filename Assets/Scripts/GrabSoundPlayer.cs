using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabSoundPlayer : MonoBehaviour
{
    public AudioClip grabSound;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        // Cari XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Cari atau tambahkan AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hubungkan event
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDestroy()
    {
        // Lepas listener agar aman
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (grabSound != null)
        {
            audioSource.PlayOneShot(grabSound);
            Debug.Log($"[GrabSoundPlayer] Played grab sound on {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"[GrabSoundPlayer] No grabSound assigned on {gameObject.name}");
        }
    }
}
