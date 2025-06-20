using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeranSnapZone : MonoBehaviour
{
    [Tooltip("Posisi target saat botol ditempel")]
    public Transform snapPoint;

    [Tooltip("Tag dari objek botol, misal 'Botol'")]
    public string targetTag = "Botol";

    [Tooltip("Referensi ke skrip WaterTap")]
    public WaterTap waterTap;

    private GameObject snappedBottle = null;          // Child botol (untuk evaluator)
    private GameObject parentObject = null;           // Parent XRGrabInteractable (misalnya Blaser Long)
    private XRGrabInteractable parentGrabInteractable = null;

    private bool isSnapped = false;

    private void Update()
    {
        GameObject[] botols = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject botol in botols)
        {
            float dist = Vector3.Distance(botol.transform.position, snapPoint.position);
            if (dist <= 0.5f)
            {
                GameObject parent = botol.transform.parent?.gameObject;
                XRGrabInteractable grab = parent?.GetComponent<XRGrabInteractable>();

                // Hanya snap jika tidak sedang dipegang
                if (grab != null && !grab.isSelected)
                {
                    TrySnap(botol);
                    break; // hentikan setelah satu botol tersnap
                }
            }
        }
    }

    private void TrySnap(GameObject botol)
    {
        parentObject = botol.transform.parent?.gameObject;
        if (parentObject == null)
        {
            Debug.LogWarning("Botol tidak memiliki parent!");
            return;
        }

        parentGrabInteractable = parentObject.GetComponent<XRGrabInteractable>();
        if (parentGrabInteractable == null)
        {
            Debug.LogWarning("Parent tidak memiliki XRGrabInteractable!");
            return;
        }

        // Tempelkan parent ke snap point
        parentObject.transform.SetPositionAndRotation(snapPoint.position, snapPoint.rotation);
        parentObject.transform.SetParent(snapPoint);

        // Nonaktifkan fisika
        Rigidbody rb = parentObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Simpan referensi botol child
        snappedBottle = botol;

        // Panggil ke WaterTap
        if (waterTap != null)
        {
            waterTap.SetCurrentBottle(snappedBottle);
        }

        // Dengarkan event pelepasan
        parentGrabInteractable.selectExited.AddListener(OnBottleReleased);

        isSnapped = true;
        Debug.Log("Botol ditempel ke keran.");
    }

    private void OnBottleReleased(SelectExitEventArgs args)
    {
        ReleaseSnap();
    }

    private void ReleaseSnap()
    {
        if (parentGrabInteractable != null)
        {
            parentGrabInteractable.selectExited.RemoveListener(OnBottleReleased);
        }

        // Kembalikan parent ke world
        parentObject.transform.SetParent(null);

        // Aktifkan kembali fisika
        Rigidbody rb = parentObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (waterTap != null)
        {
            waterTap.SetCurrentBottle(null);
        }

        Debug.Log("Botol dilepas dari keran.");

        // Bersihkan
        snappedBottle = null;
        parentObject = null;
        parentGrabInteractable = null;
        isSnapped = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (snapPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(snapPoint.position, 0.5f);
        }
    }
}
