using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Disparos que necesita el enemigo para morir
    public int hits = 3;
    private int totalHits = 0;

    [Header("Efectos de sangre")]
    public GameObject bloodEffect;
    public Renderer enemyRenderer;
    private Color originalColor;

    //para la estadistica de enemigos muertos
    public static int enemigosMuertos = 0;

    void Start()
    {
        //aqui guardo el color original al empezar
        originalColor = enemyRenderer.material.color;
    }

    private void OnCollisionEnter(Collision other) {
        // al chocar con una bala
        if(other.gameObject.CompareTag("Bullet")){
            totalHits++;
            
            // aqui creo el efecto de la sangre en el punto exacto en el que colisiona la bala
            ContactPoint contact = other.contacts[0];
            Instantiate(bloodEffect, contact.point, Quaternion.identity);

            //esto cambia el color a rojo
            StartCoroutine(Damage());

            //para destruir la bala al colisionar
            Destroy(other.gameObject);

            // Si ha recibido todos los disparos, desaparece el enemigo
            if(totalHits >= hits){
                //aumento el contador cuando mata un enemigo
                enemigosMuertos++;

                //destruyo el gameobject del enemigo cuando muere
                Destroy(gameObject);
            }
        }
    }

    private System.Collections.IEnumerator Damage(){
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material.color = originalColor;
    }
    
}
