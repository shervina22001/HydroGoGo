using UnityEngine;
using TMPro;

public class EvaluationUIController : MonoBehaviour
{
    public GameObject evaluationPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;

    /// <summary>
    /// Menampilkan pop-up evaluasi.
    /// </summary>
    /// <param name="score">Skor dari evaluasi</param>
    /// <param name="maxScore">Skor maksimal (biasanya 7)</param>
    public void ShowEvaluation(int score, int maxScore)
    {
        evaluationPanel.SetActive(true);

        scoreText.text = $"Skor Anda: {score} / {maxScore}";

        // Tampilkan pesan hasil sesuai skor
        if (score == maxScore)
        {
            resultText.text = "Selamat! Anda menyusun filter dengan benar.";
        }
        else if (score >= 4)
        {
            resultText.text = "Cukup baik, tapi masih bisa lebih sempurna.";
        }
        else
        {
            resultText.text = "Susunan filter kurang tepat. Coba lagi.";
        }
    }

    // Tambahkan fungsi untuk menutup UI jika perlu
    public void HideEvaluation()
    {
        evaluationPanel.SetActive(false);
    }
}
