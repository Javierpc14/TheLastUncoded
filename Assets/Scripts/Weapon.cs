using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header ("Referencias")]
    public GameObject bulletPrefab; //Objeto que utilizaremos como bala
    public Transform bulletSpawn; //Desde donde aparecera la bala
    public Camera playerCamera;

    [Header ("Parametros Bala")]
    public float bulletSpeed = 30;
    public float bulletLifeTime = 3f; //Lo utilizaremos para eliminar la bala pasando el tiempo

    [HideInInspector]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public enum ShootingMode
    {
        Single,
        Burst,
        Automatic
    }

    [Header ("Parametros de las armas")] //Al modificar estos podremos crear las distintas armas
    public float shootingDelay = 0.5f; //Retraso entre disparo
    public int bulletsPerBurst = 3; //Cuantas balas se disparan al mismo tiempo en modo burst
    public int burstBulletsLeft; //Cuantas balas quedan en el cargador
    public float spreadIntensity; //"Precision del arma"
    public ShootingMode currentShootingMode; //Tipo de disparo (ej: escopeta deberia tener burst y rifle automatic

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }
    void Update()
    {
        Shot();
    }

    #region Cuando se dispara
    void Shot()
    {
        //En caso de estar en automatico se dispara mientras se mantenga el click izq
        if (currentShootingMode == ShootingMode.Automatic)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        //En caso de tener otro modo se dispara solo al hacer click izq
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }
    #endregion

    #region Dinamica de las Armas
    private void FireWeapon()
        /*
        Va a funcionar instanciando un objeto (bala) y dandole una fuerza hacia adelante para que salga disparada
        Existe otra forma de hacerlo via Raycast, podemos preguntarle a Edu si quedaria mejor
        */
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        //Instanciamos la bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Apuntamos la bala a la direccion de disparo
        bullet.transform.forward = shootingDirection;

        //Le damos la fuerza/empujamos hacia adelante
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        //shootingDirection es la direccion aplicamos bullet speed como fuerza, ForceMode es el tipo de fuerza,
        //en este caso utilizamos el tipo impulso

        //Utilizamos una corutina para iniciar un temporizador pasado el cual se destruira la bala
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));

        //Comprobamos si podemos seguir disparando
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) //Miramos si es mayor que 1 porque ya hemos disparado una
        {
            burstBulletsLeft--;
            Invoke("FireWapon", shootingDelay);
        }
    }

    //Con este metodo evitamos que se reinicie el disparo varias veces antes que transcurra el retraso entre disparos
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    private Vector3 CalculateDirectionAndSpread()
    {
        //El ray (linea invisible) para detectar la colision parte de mitad de la camara
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        //La direccion se encuentra entre el punto de spawn de la bala y el punto de objetivo (mirilla, scope o como queramos llamarlo)
        Vector3 direction = targetPoint - bulletSpawn.position;

        //Calculamos la dispersion en eje X y en Y
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0); //calculamos la trayectoria de la bala segun la direccion y la dispersion (spread)
    }

    //Temporizador para destruir la bala
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        //Una vez pasado el tiempo indicado (delay) pasara a la siguiente linea y destruira la bala
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    #endregion
}
