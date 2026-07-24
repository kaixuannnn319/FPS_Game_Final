using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    [SerializeField]
    private Camera playerCamera;
    // Mouse Look
    [Header("Mouse Settings")]
    [SerializeField]
    private float moveSpeed = 5f;
    // Movement
    [Header("Movement Settings")]
    [SerializeField]
    private float mouseSensitivity = 2f;
    private float xRotation = 0f;
    [SerializeField]
    private float sprintSpeed = 15f;
    //Jump
    [Header("Jump Settings")]
    [SerializeField]
    private float jumpHeight = 2.2f;
    //Crouch
    [Header("Crouch Settings")]
    [SerializeField]
    private float standingHeight = 2f;
    [SerializeField]
    private float crouchHeight = 0.8f;
    [SerializeField]
    private float crouchSpeed = 2.5f;
    //Gravity
    [Header("Gravity Settings")]
    [SerializeField]
    private float gravity = -18f;
    private Vector3 velocity;

    [Header("Look Clamp")]
    [SerializeField]
    private float maxLookUp = 70f;
    [SerializeField]
    private float maxLookDown = 30f;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck; // empty GameObject placed at the feet
    [SerializeField]
    private float groundCheckRadius = 0.3f;
    [SerializeField]
    private LayerMask groundMask; // set this to your "Ground" layer in Inspector

    private bool isGrounded;
    private bool isCrouching;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        Look();
        Move();
        Jump();
        Crouch();
        Gravity();
    }
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookDown, maxLookUp); //Restrict camera angle
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        float speed = new Vector2(horizontal, vertical).magnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
    private void Gravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Jump()
    {
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
            controller.center = new Vector3(0, crouchHeight / 2f, 0);
        }
        else
        {
            isCrouching = false;
            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 2f, 0);
        }
    }

    // Optional: visualize the ground check sphere in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}