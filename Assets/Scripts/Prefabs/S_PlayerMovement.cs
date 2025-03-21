using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private Camera playerCamera;
    
    [Header("Head Bob Settings")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    
    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 currentMovement;
    private Vector3 movementSmoothVelocity;
    private float currentSpeed;
    
    private float defaultYPos = 0;
    private float timer;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        currentSpeed = walkSpeed;
        defaultYPos = playerCamera.transform.localPosition.y;
    }
    
    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleSprint();
        HandleHeadBob();
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
    
    private void HandleHeadBob()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            float bobSpeed = currentSpeed == sprintSpeed ? sprintBobSpeed : walkBobSpeed;
            float bobAmount = currentSpeed == sprintSpeed ? sprintBobAmount : walkBobAmount;
            
            timer += Time.deltaTime * bobSpeed;
            Vector3 camPos = playerCamera.transform.localPosition;
            
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            playerCamera.transform.localPosition = new Vector3(camPos.x, newY, camPos.z);
        }
        else
        {
            timer = 0;
            Vector3 camPos = playerCamera.transform.localPosition;
            float newY = Mathf.Lerp(camPos.y, defaultYPos, Time.deltaTime * 5f);
            playerCamera.transform.localPosition = new Vector3(camPos.x, newY, camPos.z);
        }
    }
}