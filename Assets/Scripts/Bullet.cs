using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectHit)
    {
        /*
        if (objectHit.gameObject.CompareTag("Target"))
        {
            //Para pruebas, al entrar en collision compara el tag y en caso de tener target indicara el nombre del objeto
            print("hit " + objectHit.gameObject.name + " !");
            Destroy(gameObject);
        }
        */
        //Introduciendo efectos de impacto segun Tag
        if (objectHit.gameObject.CompareTag("Stone"))
        {
            CreateBulletImpactEffect(objectHit);
        }
        if (!objectHit.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        
    }
    //
    void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab, //que se instancia
            contact.point,//Lugar en que se va a instanciar
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectHit.gameObject.transform);
        Destroy(gameObject);
    }
}
