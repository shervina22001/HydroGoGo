using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicPlayer : MonoBehaviour
{
    public AudioSource musicSource;
    public string[] allowedScenes; // Nama scene yang boleh memutar musik

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool playMusic = false;
        foreach (string scene in allowedScenes)
        {
            if (scene == currentScene)
            {
                playMusic = true;
                break;
            }
        }

        if (musicSource != null)
        {
            if (playMusic)
            {
                if (!musicSource.isPlaying)
                    musicSource.Play();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }
}