using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public bool IsOpen => dialogAnim.GetBool("IsOpen");
    public bool TutorialOpen => tutorialAnim.GetBool("TutorialOpen");
    public TextMeshProUGUI name;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI name1;
    public TextMeshProUGUI dialogText1;
    public GameObject interaksi;
    public GameObject gambar;
    public GameObject dialogBox;
    public GameObject tutorialBox;

    private TextMeshProUGUI activeName;
    private TextMeshProUGUI activeDialogText;

    public Animator tutorialAnim;
    public Animator dialogAnim;

    public Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialog(Dialog dialog)
    {
        dialogAnim.SetBool("IsOpen", true); // Open the dialog UI
        tutorialAnim.SetBool("TutorialOpen", false); // Ensure the tutorial UI is closed

        activeName = name1;
        activeDialogText = dialogText1;
        activeName.text = dialog.name;
        //name1.text = dialog.name; // Set the name in the UI Text component

        interaksi.SetActive(false); // Hide the interaction UI element
        gambar.SetActive(false); // Hide the image if it is not part of the dialog
        if (dialogBox != null) dialogBox.SetActive(true);
        if (tutorialBox != null) tutorialBox.SetActive(false);

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void StartTutorial(Dialog dialog)
    {
        tutorialAnim.SetBool("TutorialOpen", true); // Open the dialog UI
        dialogAnim.SetBool("IsOpen", false); // Ensure the dialog UI is closed

        activeName = name;
        activeDialogText = dialogText;
        activeName.text = dialog.name;
        //name.text = dialog.name; // Set the name in the UI Text component
        interaksi.SetActive(false); // Hide the interaction UI element
        gambar.SetActive(true); // Show the image if it is part of the tutorial
        if (dialogBox != null) dialogBox.SetActive(false);
        if (tutorialBox != null) tutorialBox.SetActive(true);

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //gambar.SetActive(false); // Hide the image if it was shown previously
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // Stop any currently running typing effect
        StartCoroutine(TypeSentence(sentence)); // Start typing effect for the sentence
    }

    //IEnumerator TypeSentence(string sentence)
    //{
    //    dialogText.text = "";
    //    foreach (char letter in sentence.ToCharArray())
    //    {
    //        dialogText.text += letter;
    //        yield return null; // Wait for the next frame
    //    }
    //}

    IEnumerator TypeSentence(string sentence)
    {
        if (activeDialogText == null)
            yield break;

        activeDialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            activeDialogText.text += letter;
            yield return null;
        }
    }


    void EndDialog()
    {
        dialogAnim.SetBool("IsOpen", false);
        tutorialAnim.SetBool("TutorialOpen", false); // Close the tutorial UI
        interaksi.SetActive(true); // Show the interaction UI element again
    }
}
