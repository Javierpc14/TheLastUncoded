using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Transform scope;

    [Header ("Par�metros del Rat�n")]
    public float mouseSensitivity = 500f;
    float xRotation = 0f;
    float yRotation = 0f;
    public float bottomClamp = 90f;
    public float topClamp = -90f;

    [Header("Par�metros de movimiento del Jugador")]
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    [Header ("Detecci�n del suelo")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header ("Variables de Vida")]
    public int maxVida = 5;
    public int vidaActual;
    public DamageEffect damageEffect;
    
    Vector3 velocity;

    bool isGrounded;
    bool isMoving; //Puede usarse mas tarde para realizar acciones cuando este quieto

    private Vector3 lastPosition = new Vector3(0f,0f, 0f);
    void Start()
    {
        //Bloqueamos el cursor en el centro de la c�mara
        Cursor.lockState = CursorLockMode.Locked;

        //Asignamos el controlador
        controller = GetComponent<CharacterController>();

        //inicializo la vida actual a la vida maxima
        vidaActual = maxVida;
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
        Movement();
    }

    // para que colisione con el arma del enemigo
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("arma")){
            Debug.Log("el prota ha recibido dano");
            RecibirDanio(1);
        }   
    }

    void MouseMovement()
    {
        //Detectamos los inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotamos la camara
        xRotation -= mouseY;
        yRotation += mouseX;

        //Limitamos la rotacion para que no de vuelta completas mas alla del eje X
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Aplicamos la rotac�on al personaje
        //transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        scope.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        /*
        Al aplicar la rotacion vertical al personaje mueve hitbox y el groundCheck
        �Aplicar horizontal a personaje y vertical solo al arma?
        */
    }
    void Movement()
    {
        //Comprobamos si est� en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reiniciamos la velocidad en el eje Y cada vez que toca suelo
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        //Detectamos los inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Guardamos los inputs en un vector
        Vector3 move = transform.right * x + transform.forward * z;

        //Movemos el jugador
        controller.Move(move * speed * Time.deltaTime);

        //Si el jugador est� en el suelo puede saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Iniciar el salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Velocidad de ca�da
        velocity.y += gravity * Time.deltaTime;

        //Control del salto
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void RecibirDanio(int cantidad){
        vidaActual -= cantidad;

        //esto sube en 0.2 la intensidad de la imagen roja de danio
        damageEffect.AddDamageEffect(0.4f);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir(){
        Debug.Log("¡El jugador ha muerto!");
    }
}
