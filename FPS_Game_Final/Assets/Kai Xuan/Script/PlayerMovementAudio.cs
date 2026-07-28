using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovementAudio : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("Footsteps")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float walkInterval = 0.5f;
    [SerializeField] private float sprintInterval = 0.3f;

    [Header("Jump")]
    [SerializeField] private AudioClip jumpClip;

    private bool wasGrounded;

    private CharacterController controller;
    private AudioSource audioSource;
    private float footstepTimer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Debug.Log("PlayerMovementAudio Running");

        bool isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundMask
        );

        if (!isGrounded)
        {
            footstepTimer = 0f;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isMoving =
            Mathf.Abs(horizontal) > 0.1f ||
            Mathf.Abs(vertical) > 0.1f;

        Debug.Log("Moving: " + isMoving);

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;

            float interval = Input.GetKey(KeyCode.LeftShift)
                ? sprintInterval
                : walkInterval;

            if (footstepTimer <= 0f)
            {
                footstepTimer = interval;

                if (footstepClips.Length > 0)
                {
                    int index = Random.Range(0, footstepClips.Length);

                    Debug.Log("Footstep Played");

                    audioSource.PlayOneShot(footstepClips[index], 1.8f);
                }
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        HandleJump();
    }

    private void HandleJump()
    {
        bool isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundMask
        );

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpClip != null)
            {
                audioSource.PlayOneShot(jumpClip, 0.35f);
            }
        }
    }
}