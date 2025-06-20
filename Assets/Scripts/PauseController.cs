using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Panel utama pause menu
    public GameObject menuPanel;    // Panel untuk tombol "Continue", "Options", "Main Menu"
    public GameObject optionsPanel; // Panel untuk menu opsi
    public InputActionReference menuButtonAction;
    private bool isPaused = false;



    void Start()
    {
        pauseMenuUI.SetActive(false);
        menuButtonAction.action.Enable();
    }

    void Update()
    {
        if (menuButtonAction.action == null)
        {
            Debug.LogWarning("Menu Button Action belum di-assign!");
            return;
        }

        if (!menuButtonAction.action.enabled)
        {
            Debug.LogWarning("Menu Button Action belum aktif!");
            return;
        }

        if (menuButtonAction.action.WasPressedThisFrame())
        {
            Debug.Log("TOMBOL MENU DITEKAN");
            TogglePause();
        }
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }



    // Fungsi dipanggil saat tombol "Continue" ditekan
    public void OnContinuePressed()
    {
        pauseMenuUI.SetActive(false); // Menyembunyikan seluruh pause menu
        Time.timeScale = 1f; // Melanjutkan waktu permainan jika sebelumnya dipause
    }

    // Fungsi dipanggil saat tombol "Options" ditekan
    public void OnOptionsPressed()
    {
        menuPanel.SetActive(false);     // Sembunyikan menu utama
        optionsPanel.SetActive(true);   // Tampilkan menu opsi
    }

    // Fungsi dipanggil saat tombol "Main Menu" ditekan
    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f; // Pastikan waktu berjalan normal saat berpindah scene
        SceneManager.LoadScene(0); 
    }
    public void OnBackFromOptions()
    {
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

}
