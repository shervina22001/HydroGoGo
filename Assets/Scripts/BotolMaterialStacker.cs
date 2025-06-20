using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BotolMaterialStacker : MonoBehaviour
{
    [System.Serializable]
    public class Lapisan
    {
        public string name;
        public Renderer renderer;
        public Material currentMaterial;
        [HideInInspector] public string originalObjectName;
    }

    [System.Serializable]
    public class PrefabMapping
    {
        public string objectName;
        public GameObject prefab;
    }

    [Header("Lapisan Settings")]
    public List<Lapisan> lapisan = new List<Lapisan>();

    [Header("Object Settings")]
    public string[] allowedNames;
    public float triggerDistance = 0.2f;

    [Header("Prefab Mapping (isi di Inspector)")]
    public List<PrefabMapping> prefabMappings = new List<PrefabMapping>();

    [Header("VR Input Settings")]
    public bool useSelectButton = true;

    [Header("Tutup Botol Settings")]
    public Collider tutupCollider;

    [Header("Debug Settings")]
    public bool enableDebugLogs = true;

    public XRInteractionManager interactionManager;

    private Dictionary<string, GameObject> prefabDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var mapping in prefabMappings)
        {
            string key = mapping.objectName.ToLower();
            if (!prefabDict.ContainsKey(key))
                prefabDict.Add(key, mapping.prefab);
        }

        if (interactionManager == null)
        {
            interactionManager = FindObjectOfType<XRInteractionManager>();
        }
    }

    private void Update()
    {
        // Hanya check material insertion, extract dilakukan via BotolInteractable
        CheckForMaterialInsertion();
    }

    private bool CheckExtractInput()
    {
        if (useSelectButton)
        {
            return Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.JoystickButton7);
        }
        return false;
    }

    private void CheckForMaterialInsertion()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            if (!IsAllowedObject(obj)) continue;
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance <= triggerDistance)
            {
                TryInsertMaterial(obj);
                break;
            }
        }
    }

    private bool IsAllowedObject(GameObject obj)
    {
        if (!obj.activeInHierarchy) return false;
        foreach (var name in allowedNames)
        {
            if (obj.name == name) return true;
        }
        return false;
    }

    private void TryInsertMaterial(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponentInChildren<Renderer>();
        if (objRenderer == null) return;

        Material matToInsert = objRenderer.material;

        for (int i = lapisan.Count - 1; i >= 0; i--)
        {
            if (lapisan[i].currentMaterial == null)
            {
                lapisan[i].currentMaterial = matToInsert;
                lapisan[i].renderer.material = matToInsert;
                lapisan[i].originalObjectName = obj.name;

                Destroy(obj);
                return;
            }
        }
    }

    public void ExtractMaterialFromTop(IXRSelectInteractor interactor = null)
    {
        for (int i = 0; i < lapisan.Count; i++)
        {
            if (lapisan[i].currentMaterial != null)
            {
                // Gunakan coroutine untuk spawn dengan auto grab seperti TanahSpawner
                if (interactor != null)
                {
                    StartCoroutine(SpawnAndGrabCoroutine(lapisan[i].originalObjectName, lapisan[i].currentMaterial, interactor, i));
                }
                else
                {
                    SpawnPrefabForMaterial(lapisan[i].originalObjectName, lapisan[i].currentMaterial, null);
                    ClearLapisan(i);
                }
                return;
            }
        }
    }

    // TAMBAHAN: Method untuk dipanggil dari XRBaseInteractable
    public void OnBotolInteracted(IXRSelectInteractor interactor)
    {
        if (enableDebugLogs)
            Debug.Log($"[BotolStacker] OnBotolInteracted called by: {interactor.transform.name}");

        ExtractMaterialFromTop(interactor);
    }


    private IEnumerator SpawnAndGrabCoroutine(string objectName, Material material, IXRSelectInteractor interactor, int lapisanIndex)
    {
        if (enableDebugLogs)
            Debug.Log("[BotolStacker] Starting spawn and grab coroutine...");

        // SOLUSI 1: Release botol dulu sebelum spawn objek baru (SAMA DENGAN TanahSpawner)
        XRBaseInteractable botolInteractable = GetComponent<XRBaseInteractable>();
        if (botolInteractable == null)
        {
            // Cari BotolGrabHandler jika ada
            botolInteractable = GetComponent<BotolGrabHandler>();
        }

        if (botolInteractable != null)
        {
            interactionManager.SelectExit(interactor, botolInteractable);
        }

        // SOLUSI 2: Tunggu beberapa frame untuk memastikan state clear (SAMA DENGAN TanahSpawner)
        yield return null;
        yield return null;

        // Tentukan posisi spawn (SAMA DENGAN TanahSpawner)
        Transform attachPoint = (interactor as XRBaseInteractor)?.attachTransform;
        Vector3 spawnPos = attachPoint != null ? attachPoint.position : interactor.transform.position;
        Quaternion spawnRot = attachPoint != null ? attachPoint.rotation : interactor.transform.rotation;

        if (enableDebugLogs)
            Debug.Log($"[BotolStacker] Attempting to spawn at position: {spawnPos}");

        // Spawn object
        string key = objectName.ToLower();
        GameObject spawned = null;

        if (prefabDict.TryGetValue(key, out GameObject prefabToSpawn))
        {
            if (enableDebugLogs)
                Debug.Log($"[BotolStacker] Found prefab for: {objectName}");

            spawned = Instantiate(prefabToSpawn, spawnPos, spawnRot);
            // TIDAK rename - biarkan nama asli prefab
            // spawned.name = objectName; // DIHAPUS

            // Apply material
            Renderer rend = spawned.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material = material;
            }
        }
        else
        {
            Debug.LogWarning($"[BotolStacker] No prefab found for: {objectName}, creating default");
            spawned = CreateDefaultObject(objectName);
            spawned.transform.position = spawnPos;
            spawned.transform.rotation = spawnRot;
        }

        // DEBUG: Cek apakah spawned berhasil (SAMA DENGAN TanahSpawner)
        if (spawned == null)
        {
            Debug.LogError("[BotolStacker] Failed to instantiate object!");
            yield break;
        }

        if (enableDebugLogs)
        {
            Debug.Log($"[BotolStacker] Successfully spawned object: {spawned.name} at {spawnPos}");
            Debug.Log($"[BotolStacker] Spawned object active: {spawned.activeInHierarchy}");
        }

        // DEBUG: Cek komponen pada spawned object (SAMA DENGAN TanahSpawner)
        XRGrabInteractable grab = spawned.GetComponent<XRGrabInteractable>();
        if (grab == null)
        {
            Debug.LogWarning("[BotolStacker] Prefab tidak memiliki XRGrabInteractable! Checking all components...");

            // List semua komponen untuk debug
            Component[] components = spawned.GetComponents<Component>();
            foreach (Component comp in components)
            {
                Debug.Log($"[BotolStacker] Found component: {comp.GetType().Name}");
            }

            ClearLapisan(lapisanIndex);
            yield break;
        }

        if (enableDebugLogs)
            Debug.Log($"[BotolStacker] Found XRGrabInteractable: {grab.name}, enabled: {grab.enabled}");

        // SOLUSI 3: Pastikan grab interactable sudah ready (SAMA DENGAN TanahSpawner)
        grab.enabled = true;

        // Tunggu satu frame lagi (SAMA DENGAN TanahSpawner)
        yield return null;

        // SOLUSI 4: Cek apakah interactor masih valid dan available (SAMA DENGAN TanahSpawner)
        if (interactor != null && interactor.transform != null)
        {
            if (enableDebugLogs)
                Debug.Log("[BotolStacker] Attempting to force grab...");

            // Force grab dengan manual selection (SAMA DENGAN TanahSpawner)
            interactionManager.SelectEnter(interactor, grab);

            if (enableDebugLogs)
                Debug.Log("[BotolStacker] Objek baru di-spawn dan langsung digrab");
        }
        else
        {
            Debug.LogWarning("[BotolStacker] Interactor became invalid during spawn process!");
        }

        // Clear lapisan after successful spawn
        ClearLapisan(lapisanIndex);
    }

    private void ClearLapisan(int index)
    {
        if (index >= 0 && index < lapisan.Count)
        {
            if (lapisan[index].renderer != null)
            {
                Material[] oldMats = lapisan[index].renderer.materials;
                List<Material> newMats = new List<Material>(oldMats);
                if (newMats.Count > 0)
                {
                    newMats.RemoveAt(newMats.Count - 1);
                }
                lapisan[index].renderer.materials = newMats.ToArray();
            }

            lapisan[index].currentMaterial = null;
            lapisan[index].originalObjectName = null;
        }
    }

    private void SpawnPrefabForMaterial(string objectName, Material material, IXRSelectInteractor interactor = null)
    {
        string key = objectName.ToLower();
        GameObject spawnedObj = null;

        if (prefabDict.TryGetValue(key, out GameObject prefabToSpawn))
        {
            Vector3 spawnPos = interactor != null ? interactor.transform.position : transform.position + Vector3.up;
            Quaternion spawnRot = interactor != null ? interactor.transform.rotation : Quaternion.identity;

            spawnedObj = Instantiate(prefabToSpawn, spawnPos, spawnRot);
            // TIDAK rename - biarkan nama asli prefab
            // spawnedObj.name = objectName; // DIHAPUS
            Debug.Log("[BotolStacker] Asli coy");
            Renderer rend = spawnedObj.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material = material;
            }
        }
        else
        {
            spawnedObj = CreateDefaultObject(objectName);
            spawnedObj.transform.position = transform.position + Vector3.up;
        }
    }

    private GameObject CreateDefaultObject(string objectName)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = objectName;
        obj.transform.localScale = Vector3.one * 0.1f;

      

        // Pastikan ada Rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false; // Penting untuk XR interaction

        // Add XRGrabInteractable component
        XRGrabInteractable grab = obj.GetComponent<XRGrabInteractable>();
        if (grab == null)
        {
            grab = obj.AddComponent<XRGrabInteractable>();
        }

        // Pastikan grab settings yang benar
        grab.enabled = true;

        if (enableDebugLogs)
            Debug.Log($"[BotolStacker] Created default object: {objectName} with XRGrabInteractable");

        return obj;
    }

    private Material GetDefaultMaterial()
    {
        return Resources.GetBuiltinResource<Material>("Default-Material.mat");
    }

    // DEBUG: Test method
    [ContextMenu("Test Extract")]
    public void TestExtract()
    {
        ExtractMaterialFromTop();
    }
}