using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Camera playerCamera;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 currentMovement;
    private Vector3 movementSmoothVelocity;
    private float currentSpeed;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleSprint();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();
        
        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = -9.81f;
        
        currentMovement = Vector3.SmoothDamp(currentMovement, velocity, ref movementSmoothVelocity, smoothTime);
        
        controller.Move(currentMovement * Time.deltaTime);
    }

    private void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }
}