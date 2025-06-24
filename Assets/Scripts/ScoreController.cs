using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ScoreController : MonoBehaviour
{
    public GameObject outro;
    public VideoPlayer videoPlayer;
    public GameObject scoreUI;
    public VideoPlayer outroPlayer;
    public WaterTap waterTap;

    void Start()
    {
        if (outro != null)
        {
            outro.SetActive(false);
            outroPlayer.Stop(); // Ensure video is not playing at the start  
        }
    }

    public void OnRetryPressed()
    {
        SceneManager.LoadScene(3);
    }

    public void OnContinuePressed()
    {
        AudioSource[] allAudio = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudio)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
                Debug.Log("Audio stopped: " + audio.gameObject.name);
            }
        }
        scoreUI.SetActive(false);
        if (outro != null)
        {
            Debug.Log("Displaying outro UI and playing outro video.");
            outro.SetActive(true); // Display outro UI  
            if (outroPlayer != null)
            {
                outroPlayer.Play();
                Debug.Log("Playing outro video.");
            }
            else
            {
                Debug.LogWarning("Video Player for outro not found!");
            }
        }
        else
        {
            Debug.LogWarning("Outro UI not found!");
        }
    }
}
