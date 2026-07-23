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

    //Ground Check
    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.2f;
    [SerializeField]
    private LayerMask groundMask;

    [Header("Look Clamp")]
    [SerializeField]
    private float maxLookUp = 70f;
    [SerializeField]
    private float maxLookDown = 30f;

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
        GroundCheck();
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
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

        if(isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        controller.Move(move * currentSpeed * Time.deltaTime);
    }
    private void Gravity()
    {
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void Jump()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
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
}
