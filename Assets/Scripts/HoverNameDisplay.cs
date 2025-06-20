using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverNameDisplay : XRBaseInteractable
{
    [Header("Nama dan Objek Teks")]
    public string objectName = "Nama Objek";
    public GameObject textObject;  // Objek teks yang sudah ada di scene (3D Text atau TMP)

    protected override void Awake()
    {
        base.Awake();

        // Pastikan textObject ada dan disable di awal
        if (textObject != null)
        {
            textObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[HoverNameDisplay] textObject belum di-assign!");
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        Debug.Log($"[HoverNameDisplay] Hover entered: {objectName} oleh {args.interactorObject.transform.name}");

        if (textObject != null)
        {
            textObject.SetActive(true);

            // Update teks jika ada komponen TextMesh
            TextMesh textMesh = textObject.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = objectName;
            }

            // Kalau pakai TMP (opsional, kalau kamu pakai)
            // TMPro.TextMeshPro tmp = textObject.GetComponent<TMPro.TextMeshPro>();
            // if (tmp != null)
            // {
            //     tmp.text = objectName;
            // }
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        Debug.Log($"[HoverNameDisplay] Hover exited: {objectName}");

        if (textObject != null)
        {
            textObject.SetActive(false);
        }
    }
}
