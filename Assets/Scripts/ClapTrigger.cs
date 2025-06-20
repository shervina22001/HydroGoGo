using UnityEngine;

public class ClapTrigger : MonoBehaviour
{
    private Animator clapAnim;
    private AudioSource audioSource;

    private void Awake()
    {
        clapAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAnimation()
    {
        clapAnim.SetBool("IsClap", true);

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
