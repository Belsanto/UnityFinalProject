// Sección de importaciones de librerías y paquetes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Asegurarse de que el GameObject tenga un CharacterController adjunto
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Controls Enable")]
    [SerializeField] private bool isAbleToMove = true; // Duración máxima de la carrera
    [SerializeField] private bool isAbleToLook = true; // Duración máxima de la carrera
    
    // Variables relacionadas con el movimiento del jugador
    [Header("Movement")]
    [SerializeField] [Range(0f, 10f)] private float walkingSpeed = 7.5f;
    [SerializeField] [Range(0f, 12f)] private float runningSpeed = 11.5f;
    [SerializeField] private float maxRunDuration = 50f; // Duración máxima de la carrera
    [SerializeField] private float minRunDuration = 10f; // Duración mínima de la carrera
    private float playerCurrentSpeed;

    // Variables relacionadas con la vista y sensibilidad de la cámara
    [Header("Look")]
    [SerializeField] [Range(0f, 10f)] private float lookSensitivity = 2f;
    private float lookVerticalMaxAngle = 90f;
    private float rotationX = 0;

    // Variables relacionadas con el salto del jugador
    [Header("Jump")]
    [SerializeField] private float coyoteTime = 0.1f; // Define el tiempo coyote en segundos
    [SerializeField] [Range(0f, 15f)] private float jumpForce = 6f;
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private Transform groundCheck; // Punto de verificación para determinar si el jugador está en el suelo
    [SerializeField] private LayerMask groundMask; // Máscara para definir la capa del suelo
    private bool isRunning;
    private bool ableToRun;
    private float currentRunDuration;
    private bool isGrounded;
    
    [Header("Audio")]
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;


    // Variable para el tiempo coyote que permite un breve periodo para saltar después de dejar el suelo
    private float coyoteTimeCounter;

    // Parámetro de tiempo entre saltos
    private float timeBetweenJumps = 0.5f; 

    // Variables para el contenedor de la cámara y el controlador de personajes
    private Transform cameraContainer;
    private CharacterController characterController;

    // Vectores para el movimiento y la vista del personaje
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVectorMovement;
    private Vector2 inputVectorLook;
    private Vector3 forwardDirection;
    private Vector3 rightDirection;
    private Vector3 movementVector;

    public void SetIsAbleToLook(bool active)
    {
        isAbleToLook = active;
    }
    
    // Método que se ejecuta al iniciar
    private void Start()
    {
        // Asignar el contenedor de la cámara y el controlador de personajes
        cameraContainer = transform.GetChild(0);
        characterController = GetComponent<CharacterController>();
        currentRunDuration = maxRunDuration;
        ableToRun = true;
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        Cursor.visible = false; // Hacer el cursor invisible
    }

    // Método que se ejecuta en cada cuadro
    private void Update()
    {
        // Leer la entrada del jugador
        ReadInput();

        if (isAbleToMove)
        {
            // Controlar el movimiento del personaje
            CharacterMovement();
        }

        if (isAbleToLook)
        {
            // Controlar la rotación de la cámara
            CameraRotation();
        }

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

    // Método para leer la entrada del jugador
    private void ReadInput()
    {
        // Obtener las entradas para el movimiento y la vista
        inputVectorMovement.x = Input.GetAxisRaw("Horizontal");
        inputVectorMovement.y = Input.GetAxisRaw("Vertical");
        inputVectorLook.x = Input.GetAxisRaw("Mouse X");
        inputVectorLook.y = Input.GetAxisRaw("Mouse Y");

        // Normalizar el vector de movimiento si su magnitud es mayor que 1
        if (inputVectorMovement.magnitude > 1f)
        {
            inputVectorMovement = inputVectorMovement.normalized;
        }

        // Controlar la lógica de la carrera y la carga de la carrera
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (currentRunDuration >= 0 && ableToRun)
            {
                isRunning = true;
                playerCurrentSpeed = runningSpeed;
                currentRunDuration -= Time.deltaTime;
                //Debug.Log("Is running");
                if (currentRunDuration < 1)
                {
                    ableToRun = false;
                    isRunning = false;
                    playerCurrentSpeed = walkingSpeed;
                    //Debug.Log("Is not running");
                }
            }
        }
        else
        {
            //Debug.Log("Is Walking");
            isRunning = false;
            playerCurrentSpeed = walkingSpeed;
        }

        // Controlar el tiempo de carga de la carrera y la posibilidad de correr
        if (isRunning == false)
        {
            if (currentRunDuration < maxRunDuration)
            {
                currentRunDuration += Time.deltaTime;
                //Debug.Log("Is charging run");
            }
            if (currentRunDuration > minRunDuration && ableToRun == false)
            {
                //Debug.Log("Is able to run");
                ableToRun = true;
            }
        }
        
        // Verificar si el jugador está en el suelo usando un objeto de esfera para la detección de colisiones
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        // Reproducir sonido de caminar y correr
        if (isGrounded && inputVectorMovement.magnitude > 0.01f)
        {
            if (isRunning)
            {
                if (!runSound.isPlaying)
                {
                    runSound.Play();
                }
            }
            else
            {
                if (!walkSound.isPlaying)
                {
                    walkSound.Play();
                }
            }
        }
        else
        {
            walkSound.Stop();
            runSound.Stop();
        }
    }

    // Método para controlar el movimiento del personaje
    private void CharacterMovement()
    {
        // Obtener las direcciones hacia adelante y hacia la derecha del personaje
        forwardDirection = transform.forward;
        rightDirection = transform.right;

        // Obtener la dirección del movimiento en el eje Y
        float movementDirectionY = moveDirection.y;

        // Calcular el vector de movimiento utilizando la velocidad actual del jugador
        movementVector = inputVectorMovement * playerCurrentSpeed;

        // Calcular la dirección de movimiento en función de las direcciones hacia adelante y hacia la derecha
        moveDirection = (forwardDirection * movementVector.y) + (rightDirection * movementVector.x);

        // Mantener la dirección del movimiento en el eje Y
        moveDirection.y = movementDirectionY;

        // Controlar la lógica de salto y la aplicación de la gravedad
        if ((isGrounded || coyoteTimeCounter > 0) && timeBetweenJumps <= 0)
        {
            if (Input.GetButton("Jump") && isGrounded)
            {
                // Reproducir sonido de salto
                if (!jumpSound.isPlaying)
                {
                    jumpSound.Play();
                    Debug.Log("jump");
                }
                moveDirection.y = jumpForce;
                coyoteTimeCounter = 0;
                timeBetweenJumps = 1f; // Reiniciar el tiempo entre saltos
            }
        }
        else
        {
            // Reproducir sonido de aterrizaje
            if (!landSound.isPlaying && moveDirection.y <= 0.1f && isGrounded)
            {
                landSound.Play();
            }
            moveDirection.y += gravityMultiplier * Physics.gravity.y * Time.deltaTime;
        }

        // Actualizar el tiempo entre saltos
        if (timeBetweenJumps > 0)
        {
            timeBetweenJumps -= Time.deltaTime;
        }

        // Mover al personaje usando el controlador de personajes y el vector de movimiento
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Método para controlar la rotación de la cámara
    private void CameraRotation()
    {
        // Calcular la rotación en el eje X
        rotationX += -inputVectorLook.y * lookSensitivity;

        // Limitar la rotación vertical de la cámara
        rotationX = Mathf.Clamp(rotationX, -lookVerticalMaxAngle, lookVerticalMaxAngle);

        // Aplicar la rotación al contenedor de la cámara
        cameraContainer.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Aplicar la rotación horizontal al GameObject
        transform.rotation *= Quaternion.Euler(0, inputVectorLook.x * lookSensitivity, 0);
    }
}
