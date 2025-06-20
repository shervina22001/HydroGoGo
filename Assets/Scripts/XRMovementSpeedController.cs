using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRMovementSpeedController : MonoBehaviour
{
    [Header("Move Provider")]
    public ActionBasedContinuousMoveProvider moveProvider;

    [Header("Speed Settings")]
    public float defaultSpeed = 1.0f;
    public float sprintSpeed = 3.0f;

    private void Start()
    {
        if (moveProvider == null)
        {
            moveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        }

        SetMoveSpeed(defaultSpeed);
    }

    private void Update()
    {
        // Tekan tombol joystick kanan untuk sprint
        if (Input.GetKey(KeyCode.JoystickButton3))  // Biasanya tombol Y di Xbox controller
        {
            SetMoveSpeed(sprintSpeed);
        }
        else
        {
            SetMoveSpeed(defaultSpeed);
        }
    }

    public void SetMoveSpeed(float speed)
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed = speed;
        }
    }
}
