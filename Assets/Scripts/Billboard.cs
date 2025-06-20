using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform targetCamera;
    [SerializeField] private bool useMainCamera = true;

    [Header("Billboard Settings")]
    [SerializeField] private bool lockYAxis = false;
    [SerializeField] private bool reverseDirection = false;

    private Transform cameraTransform;

    private void Start()
    {
        SetupCamera();
    }

    private void SetupCamera()
    {
        if (!useMainCamera && targetCamera != null)
        {
            cameraTransform = targetCamera;
        }
        else if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            // Fallback: cari kamera pertama yang aktif
            Camera fallbackCamera = FindObjectOfType<Camera>();
            if (fallbackCamera != null)
            {
                cameraTransform = fallbackCamera.transform;
                Debug.Log($"[Billboard] Menggunakan fallback camera: {fallbackCamera.name}");
            }
            else
            {
                Debug.LogWarning("[Billboard] Tidak ada kamera yang ditemukan!");
            }
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 directionToCamera = cameraTransform.position - transform.position;

        if (reverseDirection)
        {
            directionToCamera = -directionToCamera;
        }

        if (lockYAxis)
        {
            directionToCamera.y = 0;
        }

        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            transform.rotation = targetRotation * Quaternion.Euler(0, 180f, 0);  // Tambah 180 Y agar teks selalu benar
        }
    }


    // Method untuk mengganti target camera secara runtime
    public void SetTargetCamera(Transform newCamera)
    {
        targetCamera = newCamera;
        cameraTransform = newCamera;
        useMainCamera = false;
    }

    // Method untuk kembali ke main camera
    public void UseMainCamera()
    {
        useMainCamera = true;
        SetupCamera();
    }
}