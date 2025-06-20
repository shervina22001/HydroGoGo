using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipHandler : MonoBehaviour
{
    public GameObject videoUI; // Canvas yang menampilkan video
    public GameObject outroUI;

    public void SkipVideo()
    {
        videoUI.SetActive(false); // Menyembunyikan video
        // Tambahkan logika lanjut ke scene berikut / lanjut simulasi
    }
    public void SkipOutro()
    {
        SceneManager.LoadScene(0); 
    }
}
