using UnityEngine;

public class FilterEvaluator : MonoBehaviour
{
    public EvaluationUIController evaluationUI;

    [Tooltip("Daftar nama material ideal, urut dari atas ke bawah")]
    public string[] idealOrder = new string[]
    {
        "KapasFilter", "Kerikil", "PasirSilica", "PasirMalang", "BatuBesar", "KerikilHias", "KapasFilter"
    };

    [Tooltip("Reference ke ParticleSystemRenderer (air)")]
    public ParticleSystemRenderer waterRenderer;

    [Tooltip("Material jernih (untuk trail)")]
    public Material clearWater;

    [Tooltip("Material setengah jernih (untuk trail)")]
    public Material mediumWater;

    [Tooltip("Material keruh (untuk trail)")]
    public Material dirtyWater;

    /// <summary>
    /// Menilai susunan filter pada botol dan mengubah trail material air.
    /// </summary>
    /// <param name="bottle">GameObject botol yang mengandung lap1 - lap7</param>
    /// <returns>Skor antara 0 - 7</returns>
    public int EvaluateFilter(GameObject bottle)
    {
        if (bottle == null)
        {
            Debug.LogError("Bottle tidak diberikan ke FilterEvaluator.");
            return 0;
        }

        Debug.Log($"Mulai menilai filter dari botol: {bottle.name}");
        int score = 0;

        for (int i = 0; i < idealOrder.Length; i++)
        {
            string lapName = "lap" + (i + 1);
            Transform lap = bottle.transform.Find(lapName);

            if (lap == null)
            {
                Debug.LogWarning($"Lapisan {lapName} tidak ditemukan di {bottle.name}");
                continue;
            }

            Renderer rend = lap.GetComponent<Renderer>();
            if (rend == null)
            {
                Debug.LogWarning($"Renderer tidak ditemukan di {lapName}");
                continue;
            }

            // Cek dengan sharedMaterial supaya tidak auto-assign material
            if (rend.sharedMaterial == null)
            {
                Debug.LogWarning($"Lapisan {lapName} belum punya material (sharedMaterial null)");
                continue;
            }

            string currentMat = rend.sharedMaterial.name.Replace(" (Instance)", "").Trim();
            string idealMat = idealOrder[i];

            Debug.Log($"[{lapName}] Material: {currentMat} | Ideal: {idealMat}");

            if (currentMat.Contains(idealMat))
            {
                score++;
                Debug.Log($"Cocok: {lapName} menggunakan material yang sesuai.");
            }
            else
            {
                Debug.Log($"Tidak cocok: {lapName} menggunakan material yang berbeda.");
            }
        }


        Debug.Log($"Skor akhir filter: {score}/{idealOrder.Length}");

        // Ubah trail material pada air
        if (waterRenderer != null)
        {
            if (score == idealOrder.Length)
            {
                waterRenderer.trailMaterial = clearWater;
                Debug.Log("Status air: Jernih (clearWater)");
            }
            else if (score >= 4)
            {
                waterRenderer.trailMaterial = mediumWater;
                Debug.Log("Status air: Setengah jernih (mediumWater)");
            }
            else
            {
                waterRenderer.trailMaterial = dirtyWater;
                Debug.Log("Status air: Keruh (dirtyWater)");
            }
        }
        else
        {
            Debug.LogWarning("WaterRenderer (ParticleSystemRenderer) belum di-assign di FilterEvaluator.");
        }

        if (evaluationUI != null)
        {
            evaluationUI.ShowEvaluation(score, idealOrder.Length);
        }
        else
        {
            Debug.LogWarning("evaluationUI belum di-assign.");
        }
        return score;
    }
    public void EvaluateVisualOnly(GameObject bottle)
    {
        if (bottle == null) return;

        int score = 0;

        for (int i = 0; i < idealOrder.Length; i++)
        {
            string lapName = "lap" + (i + 1);
            Transform lap = bottle.transform.Find(lapName);

            if (lap == null) continue;

            Renderer rend = lap.GetComponent<Renderer>();
            if (rend == null || rend.sharedMaterial == null) continue;

            string currentMat = rend.sharedMaterial.name.Replace(" (Instance)", "").Trim();
            if (currentMat.Contains(idealOrder[i])) score++;
        }

        if (waterRenderer != null)
        {
            if (score == idealOrder.Length)
                waterRenderer.trailMaterial = clearWater;
            else if (score >= 4)
                waterRenderer.trailMaterial = mediumWater;
            else
                waterRenderer.trailMaterial = dirtyWater;
        }
    }

}
