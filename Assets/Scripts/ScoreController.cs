using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ScoreController : MonoBehaviour
{
    public GameObject outro;
    public VideoPlayer videoPlayer;
    public GameObject scoreUI;
    public VideoPlayer outroPlayer;

    void Start()
    {
        if (outro != null)
        {
            outro.SetActive(false);
            outroPlayer.Stop(); // Pastikan video tidak diputar saat awal
        }   
    }   
    public void OnRetryPressed()
    {
        SceneManager.LoadScene(3);
    }
    public void OnContinuePressed()
    {
        scoreUI.SetActive(false);
        if(outro != null)
        {
            Debug.Log("Menampilkan outro UI dan memutar video outro.");
            outro.SetActive(true); // Menampilkan UI outro
            if (outroPlayer != null)
            {
                outroPlayer.Play();
                Debug.Log("Memutar video outro.");
            }
            else
            {
                Debug.LogWarning("Video Player untuk outro tidak ditemukan!");
            }
        }
        else
        {
            Debug.LogWarning("Outro UI tidak ditemukan!");
        }
    }
}
