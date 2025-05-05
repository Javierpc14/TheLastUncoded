using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Configuracion del Spawner")]
    public GameObject enemy;
    public Transform[] spawnPoints;
    public float timeRounds = 10f;

    public int roundNumber = 1;

    private int enemigosPorOleada;
    private bool isSpawning = false;

    private void Start() {
        StartWaves();
    }

    private void StartWaves(){
        // Con esto inicio la corutina creada mas abajo
        StartCoroutine(SpawnearOleadas());
    }

    private void SpawnearEnemigo(){
        // aqui elijo un punto de spawn aleatorio de todos los que hay disponible
        int indice = Random.Range(0, spawnPoints.Length);
        Transform puntoSpawn = spawnPoints[indice];

        // aqui instancio el prefab del enemigo para que aparezca en ese punto
        Instantiate(enemy, puntoSpawn.position, Quaternion.identity);
    }

    // metodo que define cuantos enemigos se generan por ronda
    int CalcularCantidadEnemigos(int oleada){
        // por oleada, 3 enemigos en oleada1, 6 en la 2, etc.
        return oleada * 3;
    }

    //aqui implemento una corutina, que es un tipo especial de funcion que permite pausar la ejecucion y continuarla mas adelante sin congelar el juego
    // esta implementada para poder esperar en la salida de enemigos y los temporizadores de las rondas.
    // Esto se define con el IEnumerator y dentro de esto con yield return para indicar cuando queremos que se pause
    IEnumerator SpawnearOleadas(){
        //bucle while para que las oleadas se reproduzcan de forma infinita
        while(true){
            // para ver en la consola por que oleada va
            Debug.Log("Oleada " + roundNumber + " comenzando");

            enemigosPorOleada = CalcularCantidadEnemigos(roundNumber);
            
            //bucle for para generar todos los enemigos de la oleada con un tiempo de espera entre medias para que no salgan todos de golpe
            for(int i = 0; i < enemigosPorOleada; i++){
                SpawnearEnemigo();

                // tiempo de espera entre enemigos
                yield return new WaitForSeconds(0.5f);
            }

            roundNumber++;
            yield return new WaitForSeconds(timeRounds);
        }
    }
}
