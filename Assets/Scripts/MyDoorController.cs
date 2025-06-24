using UnityEngine;

public class MyDoorController : MonoBehaviour
{
    private Animator doorAnim;
    public AudioSource doorAudio;
    private bool doorOpen = false;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            doorAnim.Play("DoorOpen", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("DoorClose", 0, 0.0f);
            doorOpen = false;
        }
        if (doorAudio != null)
        {
            doorAudio.Play();
        }
    }
    public void PlayAnimation1()
    {
        if (!doorOpen)
        {
            doorAnim.Play("DoorOpen1", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("DoorClose1", 0, 0.0f);
            doorOpen = false;
        }
    }
}
