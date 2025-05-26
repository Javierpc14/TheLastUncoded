using System.Collections.Generic;
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

    //referencia al scope (padre de las armas) -> Necesario para que suelten municion unicamente cuando se use la pistola
    //public Transform weaponParent;

    //para la estadistica de enemigos muertos
    public static int enemigosMuertos = 0;
    
    //objetos que dropea el enemigo al morir (unicamente municion por el momento)
    public List<GameObject> ammoDrops;

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

                //suelta la municion
                DropAmmo();
                
                /*
                foreach (Transform child in weaponParent)
                {
                    if (child.gameObject.activeInHierarchy && child.name == "Pistol")
                    {
                        DropAmmo();
                        break;
                    }
                }
                */

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
    
    //Metodo para que suelten municion
    void DropAmmo()
    {
        int index = Random.Range(0, 2); //Elije un numero al azar entre 0 y 1. Al tener solo 2 objetos tendra un 50% de soltar cada tipo de municion
        Instantiate(ammoDrops[index], transform.position, Quaternion.identity); //instanciamos el objeto con la misma posicíon y rotacion
    }
}
