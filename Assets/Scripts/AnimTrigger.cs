using UnityEngine;

public class AnimTrigger : MonoBehaviour
{
    private Animator anim;
    //[SerializeField] private AudioSource sadAudioSource;
    //[SerializeField] private AudioSource clapAudioSource;
    private bool isSadPlayed = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimationSad()
    {
        anim.SetBool("IsSad", true);
        anim.SetBool("IsClap", false); // Reset IsClap to false

        //if (!sadAudioSource.isPlaying)
        //{
        //    sadAudioSource.Play();
        //}
        //if (clapAudioSource.isPlaying)
        //{
        //    clapAudioSource.Stop();
        //}
    }
    public void PlayAnimationClap()
    {
        anim.SetBool("IsSad", false); // Reset IsSad to false
        anim.SetBool("IsClap", true);

        //if (!clapAudioSource.isPlaying)
        //{
        //    clapAudioSource.Play();
        //}
        //if (sadAudioSource.isPlaying)
        //{
        //    sadAudioSource.Stop();
        //}
    }

    //public void PlayToggleAnimation()
    //{
    //    if (!isSadPlayed)
    //    {
    //        PlayAnimationSad();
    //        isSadPlayed = true;
    //    }
    //    else
    //    {
    //        PlayAnimationClap();
    //        isSadPlayed = false; // Reset jika ingin toggle terus
    //    }
    //}
}
