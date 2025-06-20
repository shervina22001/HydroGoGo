using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    public VideoPlayer introPlayer; // Video yang otomatis diputar saat start
    public GameObject introUI;      // UI untuk intro

    public VideoPlayer outroPlayer; // Outro video yang dipicu secara manual
    public GameObject outroUI;      // UI untuk outro

    void Start()
    {
        if (introPlayer != null)
        {
            introPlayer.loopPointReached += OnVideoFinished;
        }

        if (outroPlayer != null)
        {
            outroPlayer.loopPointReached += OnOutroFinished;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video utama selesai. Menutup videoUI...");
        if (introUI != null)
        {
            introUI.SetActive(false);
        }
    }

    void OnOutroFinished(VideoPlayer vp)
    {
        Debug.Log("Outro video selesai. Menutup outroUI...");
        if (outroUI != null)
        {
            outroUI.SetActive(false);
        }
        SceneManager.LoadScene(0);

    }
}
