using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public int bulletsLeft; //balas que quedan en el cargador
    public bool isReloading;
    Animator animator;
    public PlayerMovement playerMovement;
    public enum ShootingMode
    {
        Single,
        Burst,
        Automatic
    }

    [Header ("Referencias")]
    public GameObject bulletPrefab; //Objeto que utilizaremos como bala
    public Transform bulletSpawn; //Desde donde aparecera la bala
    public Camera playerCamera;
    public TextMeshProUGUI ammoDisplay; //UI de la municion
    public GameObject muzzleEffect; //Efecto de luz al disparar
    public AudioManager audioManager;
    

    [Header ("Parametros Bala")]
    public float bulletSpeed = 30;
    public float bulletLifeTime = 3f; //Lo utilizaremos para eliminar la bala pasando el tiempo

    [Header ("Parametros de las armas")] //Al modificar estos podremos crear las distintas armas
    public int damagePerBullet; //Da�o por bala del arma
    public float shootingDelay = 0.5f; //Retraso entre disparo
    public int bulletsPerBurst = 3; //Cuantas balas se disparan al mismo tiempo en modo burst (escopeta)
    public int burstBulletsLeft; //Cuantas balas quedan para disparar en modo burst
    public float spreadIntensity; //"Precision del arma". A mayor menor precision
    public float reloadTime; //Tiempo de recarga
    public int maxBulletNum; //Balas por cargador
    public int totalAmmo; //Numero total de balas
    public int maxTotalAmmo; //Numero maximo de balas a llevar
    public ShootingMode currentShootingMode; //Tipo de disparo (ej: escopeta deberia tener burst y rifle automatic
    //public int dmgPerBullet;
    private float actualMovement;
    private bool isAiming = false; //boleano que se utilizara en la logica de apuntar
    private float actualSpread;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        bulletsLeft = maxBulletNum;
        totalAmmo = maxTotalAmmo;
        animator = GetComponent<Animator> ();//Tomamos el animator local como referencia
        actualMovement = playerMovement.speed; //Velocidad mientras se apunta
        actualSpread = spreadIntensity - (spreadIntensity / 4);
    }
    void Update()
    {
        Shot();
        Reload();
        DisplayAmmo();
        Aim();
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
        
        //Limitamos disparar cuando no hay balas
        if (readyToShoot && isShooting && (bulletsLeft  > 0) && isReloading == false)
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
    */
    {
        readyToShoot = false;

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 0)
        {
            for (int i = 0; i < bulletsPerBurst && bulletsLeft > 0; i++)
            {//He intentado seguir la l�gica del siguiente c�digo y adaptarlo al actual: https://discussions.unity.com/t/creating-a-shotgun/440919/2



                ShootBullet();
            }
            bulletsLeft--;
        }
        else
        {
            //Disminuimos las balas en el cargador
            bulletsLeft--;
            ShootBullet();
        }

            //Comprobamos si podemos seguir disparando
            if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
    }

    //He sacado fuera el instanciar la bala dado que con la nueva l�gica de la escopeta se repetir�a c�digo segun modo de disparo
    private void ShootBullet()
    {
        //Reproducimos el sonido del disparo

        if (currentShootingMode == ShootingMode.Single)
        {
            audioManager.PlaySFX(audioManager.pistolShot);
        }
        else if(currentShootingMode == ShootingMode.Automatic)
        {
            audioManager.PlaySFX(audioManager.rifleShot);
        }
        else
        {
            audioManager.PlaySFX(audioManager.shotgunShot);
        }
        //Activamos el efecto de luz al disparar
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        //Iniciamos la animacion de disparo
        animator.SetTrigger("Recoil");

        //Direcci�na la que se dispara la bala
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Instanciamos la bala
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Asignamos a la bala el valor del da�o
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damagePerBullet;

        //Apuntamos la bala a la direccion de disparo
        bullet.transform.forward = shootingDirection;

        //Le damos la fuerza/empujamos hacia adelante
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        AmmoManager ammoManager = bullet.AddComponent<AmmoManager>();

        ammoManager.shootingMode = currentShootingMode;

        //shootingDirection es la direccion aplicamos bullet speed como fuerza, ForceMode es el tipo de fuerza,
        //en este caso utilizamos el tipo impulso

        //Utilizamos una corutina para iniciar un temporizador pasado el cual se destruira la bala
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && (bulletsLeft < maxBulletNum) && isReloading == false && (totalAmmo > 0 || currentShootingMode == ShootingMode.Single))
        {
            //Reproducimos el sonido de recarga

            if (currentShootingMode == ShootingMode.Single)
            {
                audioManager.PlaySFX(audioManager.pistolReload);
                
            }
            else if (currentShootingMode == ShootingMode.Automatic)
            {
                audioManager.PlaySFX(audioManager.rifleReload);
            }
            else
            {
                audioManager.PlaySFX(audioManager.shotgunReload);
            }
            animator.SetTrigger("Reload");
            isReloading = true;
            animator.SetBool("isReloading", true);
            Invoke("ReloadCompleted", reloadTime);
        }
    }

    private void ReloadCompleted()
    {
        int bulletsNeeded = maxBulletNum - bulletsLeft; //numero de balas totales a cargar (tiene en cuenta las que ya tiene el cargador)
        int bulletsToLoad = Mathf.Min(bulletsNeeded, totalAmmo); //calculo de las balas a cargar

        if (currentShootingMode != ShootingMode.Single)
        {
            //Actualizamos las balas
            bulletsLeft += bulletsToLoad;
            totalAmmo -= bulletsToLoad;
        }
        else
        {
            bulletsLeft += bulletsToLoad; //Tan solo cargamos las balas si es la pistola
        }

        isReloading = false;
        animator.SetBool("isReloading", false);
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

    private void Aim()
    {
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!isAiming)
            {
                animator.SetBool("isAiming", true);
                playerMovement.speed = playerMovement.speed / 2;
                spreadIntensity -= spreadIntensity/4;
                isAiming = true;
            }
            
        }
        else
        {
            if (isAiming)
            {
                animator.SetBool("isAiming", false);
                playerMovement.speed = actualMovement;
                spreadIntensity = actualSpread;
                isAiming = false;
            }
            
        }
    }
    #endregion

    #region UI

    void DisplayAmmo()
    {
        ammoDisplay.richText = true;

        if (currentShootingMode == ShootingMode.Single) 
        {
            ammoDisplay.text = $"{bulletsLeft}/\u221E"; //\u221E es el unicode para infinito
                                                                                           //<voffset=5%>\u221E</voffset>  
                                                                                           //y voffset es una etiqueta para para modificar el formato
                                                                                           //En este caso levanta el s�mbolo un 5%  --> NO FUNCIONA
        }
        else
        {
            ammoDisplay.text = $"{bulletsLeft}/{totalAmmo}";
        }
    }

    #endregion
}
