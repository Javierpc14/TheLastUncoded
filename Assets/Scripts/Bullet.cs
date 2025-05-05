using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            //Para pruebas, al entrar en collision compara el tag y en caso de tener target indicara el nombre del objeto
            print("hit " + collision.gameObject.name + " !");
            Destroy(gameObject);
        }
    }

}
