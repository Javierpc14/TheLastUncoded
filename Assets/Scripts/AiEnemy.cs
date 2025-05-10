using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{   
    [Header("Movimiento enemigo")]
    [SerializeField] public Transform player;
    public float velocity;
    public NavMeshAgent IA;

    private void Start() {
        IA = GetComponent<NavMeshAgent>();
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");

        if(playerobj != null){
            player = playerobj.transform;
        }
    }
    void Update()
    {
        if(player != null && IA.isOnNavMesh){
            IA.speed = velocity;
            //el objetivo de nuestra ia es la posicion del player
            IA.SetDestination(player.position);
        } 
    }
}
