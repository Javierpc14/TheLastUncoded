using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Disparos que necesita el enemigo para morir
    public int hits = 3;
    private int totalHits = 0;

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
}
