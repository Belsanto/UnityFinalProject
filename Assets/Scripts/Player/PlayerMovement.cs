using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ImprovedPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(0f, 10f)] private float walkingSpeed = 7.5f;
    [SerializeField] [Range(0f, 12f)] private float runningSpeed = 11.5f;
    private float playerCurrentSpeed;

    [Header("Look")]
    [SerializeField] [Range(0f, 10f)] private float lookSensitivity = 2f;
    private float lookVerticalMaxAngle = 90f;
    private float rotationX = 0;

    [Header("Jump")]
    [SerializeField] [Range(0f, 15f)] private float jumpForce = 6f;
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    // Coyote time variables
    [SerializeField] private float coyoteTime = 0.1f; // Define el tiempo coyote en segundos
    private float coyoteTimeCounter;

    [Header("Time Between Jumps")]
    [SerializeField] [Range(0f, 1f)] private float timeBetweenJumps = 0.5f; // Nuevo parámetro de tiempo entre saltos

    private Transform cameraContainer;
    private CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVectorMovement;
    private Vector2 inputVectorLook;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;

    private void Start()
    {
        cameraContainer = transform.GetChild(0);
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();

        CharacterMovement();

        CameraRotation();

        // Actualizar el tiempo coyote
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void ReadInput()
    {
        inputVectorMovement.x = Input.GetAxisRaw("Horizontal");
        inputVectorMovement.y = Input.GetAxisRaw("Vertical");
        inputVectorLook.x = Input.GetAxisRaw("Mouse X");
        inputVectorLook.y = Input.GetAxisRaw("Mouse Y");

        if (inputVectorMovement.magnitude > 1f)
        {
            inputVectorMovement = inputVectorMovement.normalized;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerCurrentSpeed = runningSpeed;
        }
        else
        {
            playerCurrentSpeed = walkingSpeed;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
    }

    private void CharacterMovement()
    {
        forwardDirection = transform.forward;
        rightDirection = transform.right;

        var movementDirectionY = moveDirection.y;

        var movementVector = inputVectorMovement * playerCurrentSpeed;

        moveDirection = (forwardDirection * movementVector.y) + (rightDirection * movementVector.x);

        moveDirection.y = movementDirectionY;

        // Aplicar la fuerza de salto si el jugador está en el suelo o en el tiempo coyote y se ha pasado el tiempo entre saltos
        if ((isGrounded || coyoteTimeCounter > 0) && Input.GetButton("Jump") && timeBetweenJumps <= 0)
        {
            moveDirection.y = jumpForce;
            coyoteTimeCounter = 0;
            timeBetweenJumps = 1f; // Reiniciar el tiempo entre saltos
        }
        else
        {
            moveDirection.y += gravityMultiplier * Physics.gravity.y * Time.deltaTime;
        }

        // Reducir el tiempo entre saltos
        if (timeBetweenJumps > 0)
        {
            timeBetweenJumps -= Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void CameraRotation()
    {
        rotationX += -inputVectorLook.y * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookVerticalMaxAngle, lookVerticalMaxAngle);
        cameraContainer.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, inputVectorLook.x * lookSensitivity, 0);
    }
}
