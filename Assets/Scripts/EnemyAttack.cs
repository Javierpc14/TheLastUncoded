using UnityEngine;

public class EnemyAttack : MonoBehaviour
{   
    public Animator animator;
    private Transform objetivo; // el jugador
    public float distanciaAtaque = 2f;
    public float tiempoEntreAtaques = 1.5f;
    public Collider manoCollider;

    private UnityEngine.AI.NavMeshAgent agente;
    private float temporizador;

    private void Start()
    {
        agente = GetComponent<UnityEngine.AI.NavMeshAgent>();
        objetivo = GameObject.FindGameObjectWithTag("Player").transform;
        temporizador = 0f;
        manoCollider.enabled = false;
    }

    private void Update()
    {
        float distancia = Vector3.Distance(transform.position, objetivo.position);
        temporizador += Time.deltaTime;

        // Si está cerca y ha pasado suficiente tiempo
        if (distancia <= distanciaAtaque && temporizador >= tiempoEntreAtaques)
        {
            agente.isStopped = true; // se detiene
            transform.LookAt(objetivo); // mirar al jugador
            animator.SetTrigger("Atacar"); // activar animación
            temporizador = 0f;
        }
        else if (distancia > distanciaAtaque)
        {
            agente.isStopped = false; // seguir persiguiendo
        }
    }

    // Llamado desde un Animation Event
    public void ActivarGolpe()
    {
        manoCollider.enabled = true;
    }

    // Llamado desde un Animation Event
    public void DesactivarGolpe()
    {
        manoCollider.enabled = false;
    }
}
