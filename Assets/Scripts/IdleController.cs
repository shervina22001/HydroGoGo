    using UnityEngine;

public class IdleController : MonoBehaviour
{
    private Animator idleAnim;
    private void Awake()
    {
        idleAnim = GetComponent<Animator>();
    }
}
