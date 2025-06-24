using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class WaterTap : MonoBehaviour
{
    public Animator Tap;
    public Animator NPC;
    public Animator NPC1;
    public GameObject openText;
    public GameObject closeText;
    public ParticleSystem RunningWater;
    public AudioSource openSound;
    public AudioSource tapWater;
    public AudioSource attachBotol;

    [Header("Referensi Botol yang Tersambung")]
    public GameObject currentBottle;

    [Header("Evaluator Filter (bisa lebih dari satu)")]
    public FilterEvaluator[] evaluators;

    [Header("Material default untuk air (reset visual)")]
    public Material defaultWaterMaterial;

    [Header("Delay evaluasi setelah keran dibuka (detik)")]
    public float evaluationDelay = 3.0f;

    [Header("Teleportasi sebelum evaluasi")]
    public Transform teleportPoint;
    public Transform xrRigRoot;

    [Header("Sound efek")]
    public AudioSource successSound;
    public AudioSource failSound;

    [Header("UI Continue Button")]
    public GameObject continueButton;

    private bool isOpen = false;
    private bool isClosed = true;

    private Coroutine evaluationCoroutine;

    void Start()
    {
        isClosed = true;
        isOpen = false;

        if (RunningWater != null)
            RunningWater.Stop();

        if (continueButton != null)
            continueButton.SetActive(false);

        Debug.Log("WaterTap initialized.");
    }

    public bool IsBottleAttached()
    {
        return currentBottle != null;
    }

    public void ToggleWaterTap()
    {
        Debug.Log("ToggleWaterTap called.");

        if (isClosed)
        {
            OpenTap();
        }
        else if (isOpen)
        {
            CloseTap();
        }

        if (!IsBottleAttached())
        {
            Debug.Log("Botol belum terpasang. Mereset visual air ke default.");
            ResetWaterVisual();
        }
    }

    private void OpenTap()
    {
        tapWater.Play();
        Tap?.Play("tapOpen", 0, 0.0f);
        RunningWater?.Play();
        isOpen = true;
        isClosed = false;
        Debug.Log("Keran dibuka.");

        if (!IsBottleAttached())
        {
            Debug.Log("Botol belum terpasang saat keran dibuka. Evaluasi dibatalkan.");
            return;
        }

        // Ubah visual air langsung (warna air)
        if (evaluators != null)
        {
            foreach (var ev in evaluators)
            {
                ev?.EvaluateVisualOnly(currentBottle);
            }
        }

        // Mulai evaluasi setelah delay
        if (evaluationCoroutine != null)
        {
            StopCoroutine(evaluationCoroutine);
        }

        evaluationCoroutine = StartCoroutine(DelayedEvaluation());
    }

    private void CloseTap()
    {
        tapWater?.Stop();
        Tap?.Play("tapClosed", 0, 0.0f);
        RunningWater?.Stop();
        isOpen = false;
        isClosed = true;
        Debug.Log("Keran ditutup.");
    }

    public void ResetWaterVisual()
    {
        if (evaluators != null && defaultWaterMaterial != null)
        {
            foreach (var ev in evaluators)
            {
                if (ev != null && ev.waterRenderer != null)
                {
                    ev.waterRenderer.trailMaterial = defaultWaterMaterial;
                }
            }
            Debug.Log("Semua evaluator: material air direset ke default.");
        }
        else
        {
            Debug.LogWarning("Tidak bisa reset air: evaluators null atau ada yang tidak valid.");
        }
    }

    public void SetCurrentBottle(GameObject bottle)
    {
        currentBottle = bottle;

        if (currentBottle != null)
        {
            Debug.Log("Botol baru dipasang ke keran.");
            if (attachBotol != null && !attachBotol.isPlaying)
            {
                attachBotol.Play();
            }
        }
        else
        {
            Debug.Log($"Botol dilepas. Keran status: {(isOpen ? "NYALA" : "MATI")}. Reset visual air.");
            ResetWaterVisual();
        }
    }

    private IEnumerator DelayedEvaluation()
    {
        Debug.Log($"Menunggu {evaluationDelay} detik sebelum evaluasi...");
        yield return new WaitForSeconds(evaluationDelay);

        if (currentBottle == null || evaluators == null || evaluators.Length == 0)
        {
            Debug.LogWarning("Evaluasi dibatalkan: Botol belum terpasang atau evaluator null.");
            yield break;
        }

        // Teleportasi setelah delay
        if (teleportPoint != null && xrRigRoot != null)
        {
            Debug.Log($"Teleporting user ke titik evaluasi: {teleportPoint.position}");
            xrRigRoot.position = teleportPoint.position;
        }
        else
        {
            Debug.LogWarning("Teleport point atau XR Rig Root belum di-assign.");
        }

        // Evaluasi semua evaluator
        int maxScore = 0;
        foreach (var ev in evaluators)
        {
            if (ev != null)
            {
                int score = ev.EvaluateFilter(currentBottle);
                maxScore = Mathf.Max(maxScore, score);
                Debug.Log($"[Evaluator] Skor: {score}/7");
            }
        }

        Debug.Log($"[Evaluasi setelah delay] Skor kejernihan air terbaik: {maxScore}/7");

        // Mainkan suara dan kelola tombol
        if (maxScore == 7)
        {
            successSound?.Play();
            if (NPC != null)
            {
                NPC.SetBool("IsClap", true);
                Debug.Log("Animator NPC parameter 'IsClap' set to true.");
            }
            if (NPC1 != null)
            {
                NPC1.SetBool("IsClap", true);
                Debug.Log("Animator NPC parameter 'IsClap' set to true.");
            }
            if (continueButton != null)
            {
                continueButton.SetActive(true);
                Debug.Log("Tombol Continue ditampilkan.");
            }   
        }
        else
        {
            failSound?.Play();
            if (NPC != null)
            {
                NPC.SetBool("IsSad", true);
                Debug.Log("Animator NPC parameter 'IsSad' set to true.");
            }
            if (NPC1 != null)
            {
                NPC1.SetBool("IsSad", true);
                Debug.Log("Animator NPC parameter 'IsSad' set to true.");
            }
            if (continueButton != null)
            {
                continueButton.SetActive(false);
                Debug.Log("Tombol Continue disembunyikan.");
            }
        }

        evaluationCoroutine = null;
    }
}
