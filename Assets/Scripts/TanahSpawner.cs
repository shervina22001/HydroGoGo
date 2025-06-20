using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class TanahSpawner : XRBaseInteractable
{
    public GameObject objectPrefab;

    [Header("Debug Settings")]
    public bool enableDebugLogs = true;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        IXRSelectInteractor interactor = args.interactorObject;

        if (interactor == null)
        {
            Debug.LogWarning("Interactor null!");
            return;
        }

        // DEBUG: Cek prefab
        if (objectPrefab == null)
        {
            Debug.LogError("[TanahSpawner] objectPrefab is NULL! Assign prefab in inspector!");
            return;
        }

        if (enableDebugLogs)
            Debug.Log($"[TanahSpawner] OnSelectEntered oleh: {interactor.transform.name}");

        StartCoroutine(SpawnAndAttachCoroutine(interactor));
    }

    private IEnumerator SpawnAndAttachCoroutine(IXRSelectInteractor interactor)
    {
        if (enableDebugLogs)
            Debug.Log("[TanahSpawner] Starting spawn coroutine...");

        // SOLUSI 1: Release spawner dulu sebelum spawn objek baru
        interactionManager.SelectExit(interactor, this);

        // SOLUSI 2: Tunggu beberapa frame untuk memastikan state clear
        yield return null;
        yield return null;

        // Tentukan posisi spawn
        Transform attachPoint = (interactor as XRBaseInteractor)?.attachTransform;
        Vector3 spawnPos = attachPoint != null ? attachPoint.position : interactor.transform.position;
        Quaternion spawnRot = attachPoint != null ? attachPoint.rotation : interactor.transform.rotation;

        if (enableDebugLogs)
            Debug.Log($"[TanahSpawner] Attempting to spawn at position: {spawnPos}");

        // DEBUG: Cek prefab sebelum instantiate
        if (objectPrefab == null)
        {
            Debug.LogError("[TanahSpawner] objectPrefab became null during coroutine!");
            yield break;
        }

        GameObject spawned = Instantiate(objectPrefab, spawnPos, spawnRot);

        // DEBUG: Cek apakah spawned berhasil
        if (spawned == null)
        {
            Debug.LogError("[TanahSpawner] Failed to instantiate object!");
            yield break;
        }

        if (enableDebugLogs)
        {
            Debug.Log($"[TanahSpawner] Successfully spawned object: {spawned.name} at {spawnPos}");
            Debug.Log($"[TanahSpawner] Spawned object active: {spawned.activeInHierarchy}");
        }

        // DEBUG: Cek komponen pada spawned object
        XRGrabInteractable grab = spawned.GetComponent<XRGrabInteractable>();
        if (grab == null)
        {
            Debug.LogWarning("[TanahSpawner] Prefab tidak memiliki XRGrabInteractable! Checking all components...");

            // List semua komponen untuk debug
            Component[] components = spawned.GetComponents<Component>();
            foreach (Component comp in components)
            {
                Debug.Log($"[TanahSpawner] Found component: {comp.GetType().Name}");
            }

            yield break;
        }

        if (enableDebugLogs)
            Debug.Log($"[TanahSpawner] Found XRGrabInteractable: {grab.name}, enabled: {grab.enabled}");

        // SOLUSI 3: Pastikan grab interactable sudah ready
        grab.enabled = true;

        // Tunggu satu frame lagi
        yield return null;

        // SOLUSI 4: Cek apakah interactor masih valid dan available
        if (interactor != null && interactor.transform != null)
        {
            if (enableDebugLogs)
                Debug.Log("[TanahSpawner] Attempting to force grab...");

            // Force grab dengan manual selection
            interactionManager.SelectEnter(interactor, grab);

            if (enableDebugLogs)
                Debug.Log("[TanahSpawner] Objek baru di-spawn dan langsung digrab");
        }
        else
        {
            Debug.LogWarning("[TanahSpawner] Interactor became invalid during spawn process!");
        }
    }

    // DEBUG: Method untuk test spawn tanpa grab
    [ContextMenu("Test Spawn")]
    public void TestSpawn()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("No prefab assigned for test spawn!");
            return;
        }

        Vector3 testPos = transform.position + Vector3.up * 0.5f;
        GameObject testObj = Instantiate(objectPrefab, testPos, Quaternion.identity);
        Debug.Log($"Test spawn successful: {testObj.name} at {testPos}");
    }
}

// VERSI SEDERHANA UNTUK TESTING
public class TanahSpawnerSimple : XRBaseInteractable
{
    public GameObject objectPrefab;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        Debug.Log("[Simple] OnSelectEntered triggered!");

        if (objectPrefab == null)
        {
            Debug.LogError("[Simple] No prefab assigned!");
            return;
        }

        // Spawn sederhana di atas spawner
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject spawned = Instantiate(objectPrefab, spawnPos, Quaternion.identity);

        Debug.Log($"[Simple] Spawned: {spawned.name} at {spawnPos}");
    }
}