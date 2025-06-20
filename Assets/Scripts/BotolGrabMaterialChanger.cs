using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BotolGrabMaterialChanger : MonoBehaviour
{
    [Tooltip("Urutan material baru: lap1 - lap7")]
    public Material[] newMaterials = new Material[7];

    private Material[] originalMaterials = new Material[7];
    private Renderer[] lapRenderers = new Renderer[7];

    private XRGrabInteractable parentInteractable;

    private void Awake()
    {
        // Cari renderer lap1 - lap7
        for (int i = 0; i < 7; i++)
        {
            string lapName = "lap" + (i + 1);
            Transform lap = transform.Find(lapName);
            if (lap != null)
            {
                Renderer rend = lap.GetComponent<Renderer>();
                if (rend != null)
                {
                    lapRenderers[i] = rend;
                    originalMaterials[i] = rend.material;
                }
                else
                {
                    Debug.LogWarning($"{lapName} tidak punya Renderer.");
                }
            }
            else
            {
                Debug.LogWarning($"Tidak ditemukan child: {lapName}");
            }
        }

        // Cari XRGrabInteractable di parent (BlaserLong)
        parentInteractable = GetComponentInParent<XRGrabInteractable>();
        if (parentInteractable != null)
        {
            parentInteractable.selectEntered.AddListener(OnGrab);
            parentInteractable.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogWarning("Tidak ditemukan XRGrabInteractable di parent.");
        }
    }

    private void OnDestroy()
    {
        // Bersihkan listener biar aman
        if (parentInteractable != null)
        {
            parentInteractable.selectEntered.RemoveListener(OnGrab);
            parentInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        ChangeMaterialsOnGrab();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        //RestoreOriginalMaterials();
    }

    public void ChangeMaterialsOnGrab()
    {
        for (int i = 0; i < lapRenderers.Length; i++)
        {
            if (lapRenderers[i] != null && i < newMaterials.Length && newMaterials[i] != null)
            {
                lapRenderers[i].material = newMaterials[i];
            }
        }
        Debug.Log(" Material tiap lapisan diganti.");
    }

    public void RestoreOriginalMaterials()
    {
        for (int i = 0; i < lapRenderers.Length; i++)
        {
            if (lapRenderers[i] != null && originalMaterials[i] != null)
            {
                lapRenderers[i].material = originalMaterials[i];
            }
        }
        Debug.Log(" Material lapisan dikembalikan.");
    }
}
