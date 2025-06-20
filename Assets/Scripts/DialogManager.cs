using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public bool IsOpen => animator.GetBool("IsOpen");
    public TextMeshProUGUI name;
    public TextMeshProUGUI dialogText; // Reference to a UI Text component to display dialog
    public GameObject interaksi;
    public GameObject gambar;

    public Animator animator;

    public Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialog(Dialog dialog)
    {
        animator.SetBool("IsOpen", true); // Open the dialog UI
        name.text = dialog.name; // Set the name in the UI Text component
        interaksi.SetActive(false); // Hide the interaction UI element

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void StartTutorial(Dialog dialog)
    {
        animator.SetBool("IsOpen", true); // Open the dialog UI
        name.text = dialog.name; // Set the name in the UI Text component
        interaksi.SetActive(false); // Hide the interaction UI element
        gambar.SetActive(true); // Show the image if it is part of the tutorial

        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        gambar.SetActive(false); // Hide the image if it was shown previously
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); // Stop any currently running typing effect
        StartCoroutine(TypeSentence(sentence)); // Start typing effect for the sentence
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return null; // Wait for the next frame
        }
    }

    void EndDialog()
    {
        animator.SetBool("IsOpen", false);
        interaksi.SetActive(true); // Show the interaction UI element again
    }
}
