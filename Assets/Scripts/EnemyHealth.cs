using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Disparos que necesita el enemigo para morir
    public int hits = 3;
    private int totalHits = 0;

    // [Header("Configuracion de movimiento")]
    // public int routine;
    // public float chronometer;
    // // para el rango de vision
    // public Quaternion angle;
    // public float degreeAngle;
    // //para cuando metamos animaciones
    // // public Animator ani;
    // public GameObject player;

    // [Header("Configuracion de ataque")]
    // public bool attacking;


    // private void Start() {
    //     // para la animacion cuando este
    //     // ani = GetComponent<Animator>();
    //     player = GameObject.Find("Prota");
    // }

    //  private void Update() {
    //     //metodo que gestiona el movimiento del enemigo
    //     enemyMovement(); 
    // }

    private void OnCollisionEnter(Collision other) {
        // al chocar con una bala
        if(other.gameObject.CompareTag("Bullet")){
            totalHits++;

            //para destruir la bala al colisionar
            Destroy(other.gameObject);

            // Si ha recibido todos los disparos, desaparece el enemigo
            if(totalHits >= hits){
                Destroy(gameObject);
            }
        }
    }

    // public void enemyMovement(){
    //     // si el aliado esta muy alejado del enemigo, el enemigo se quedara moviendose aleatoriamente por el mapa
    //     if(Vector3.Distance(transform.position, player.transform.position) >5 ){
    //         //cancelo la animacion de correr para el else
    //         // ani.SetBool("run", false);

    //         //aumenta en 1 nuestro cronometro respecto a ltiempo real
    //         chronometer += 1 * Time.deltaTime;

    //         if(chronometer >= 4){
    //             // rutina saca al azar un numero entre 0 y 2
    //             routine = Random.Range(0, 2);
    //             // y despues reiniciop el cronometro
    //             chronometer = 0;

    //             switch (routine)
    //             {
    //                 case 0:
    //                     //aqui queremos que el enemigo se quede quieto, entonces cancelo la animacion de caminar
    //                     // ani.SetBool("walk", false);
    //                     break;
    //                 case 1:
    //                     //aqui determino la direccion en la que se va a mover
    //                     degreeAngle = Random.Range(0, 360);
    //                     angle = Quaternion.Euler(0, degreeAngle, 0);
    //                     routine++;
    //                     break;
    //                 case 2:
    //                     // La rotacion del enemigo sera igual al angulo establecido
    //                     transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
    //                     //se movera hacia delante con una velocidad de 1
    //                     transform.Translate(Vector3.forward * 1 * Time.deltaTime);
    //                     // activo la animacion de caminar
    //                     // ani.SetBool("walk", true);
    //                     break;
    //                 default:
    //                     break;
    //             }
                
    //         }
    //     }else{
    //         //este if es que cuando el player este a una distancia mayor a 1 y atacando sea falso, se ejecutara el codigo para que el enemigo nos siga
    //         if(Vector3.Distance(transform.position, player.transform.position) > 1 && !attacking){
    //             //aqui hacemos que se mueva hacia el jugador
    //             var lookPos = player.transform.position - transform.position;
    //             lookPos.y = 0;
    //             var rotation = Quaternion.LookRotation(lookPos);
    //             //con esto el enemigo rotara segun wl angulo de la variable rotacion y tendra una velocidad de 2
    //             transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
    //             //cancelo la animacion de caminar
    //             // ani.setBool("run", true);
    //             //Con esto se mueve hacia delante a una velocidad mayor a la de caminar
    //             transform.Translate(Vector3.forward * 2 * Time.deltaTime);

    //             // ani.SetBool("attack", false);
    //         }else{
    //             // ani.SetBool("walk", false);
    //             // ani.SetBool("run", false);

    //             // ani.SetBool("attack", true);
    //             attacking = true;
    //         }
    //     }
    // }

    //metodo para cancelar tanto la animacion de atacar como el booleano de atacando
    // public void EndAni(){
    //     ani.SetBool("attack", false);
    //     attacking = false;
    // }
}
