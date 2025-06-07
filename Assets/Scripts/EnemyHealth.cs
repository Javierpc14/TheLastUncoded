using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Vida del enemigo
    public int maxHP = 100;
    private int currentHP;
    

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
        currentHP = maxHP;
    }

    private void OnCollisionEnter(Collision other) {
        // al chocar con una bala
        if(other.gameObject.CompareTag("Bullet")){
            
            
            // aqui creo el efecto de la sangre en el punto exacto en el que colisiona la bala
            ContactPoint contact = other.contacts[0];
            Instantiate(bloodEffect, contact.point, Quaternion.identity);

            //esto cambia el color a rojo
            StartCoroutine(Damage());

            AmmoManager ammoManager = other.gameObject.GetComponent<AmmoManager>();

            bool isSingleMode = ammoManager != null && ammoManager.shootingMode == Weapon.ShootingMode.Single;

            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            int damage = bullet.damage;

            currentHP -= damage;

            // Si ha recibido todos los disparos, desaparece el enemigo
            if (currentHP <= 0){
                //aumento el contador cuando mata un enemigo
                enemigosMuertos++;

                //suelta la municion
                if (isSingleMode)
                {
                    DropAmmo();
                }
                
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

    //Metodo para que el enemigo reciba daño
    /*
    private void TakeDamage()
    {
        Bullet bullet = 
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        AmmoManager ammoManager = other.gameObject.GetComponent<AmmoManager>();

        bool isSingleMode = ammoManager != null && ammoManager.shootingMode == Weapon.ShootingMode.Single;

        //aumento el contador cuando mata un enemigo
        enemigosMuertos++;

        //suelta la municion
        if (isSingleMode)
        {
            DropAmmo();
        }

        //destruyo el gameobject del enemigo cuando muere
        Destroy(gameObject);
    }
    */
}
