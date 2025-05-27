using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    /*
    He intentado gestionar externamente la municion con el script AmmoManagment adjuntado al padre de las armas (Scope), pero no ha ido bien

    public int ammoAmount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        AutoAmmo,
        BurstAmmo
    }
    */

    public Weapon.ShootingMode ammoMode; //En el inspector asignaremos en el script para que modo de arma se añadira la municion
    public int ammoAmount; //municion que recuperara el arma
    private void OnTriggerEnter(Collider other)
    {
        //Si el jugador colisiona con la municion
        if (other.gameObject.CompareTag("Player"))
        {
            /*
            Weapon[] weapons = other.GetComponentsInChildren<Weapon>(true);

            A quien tiene que detectar la colision es al player, pero luego he de referenciar las armas que son nietas del player
            Esta opcion unicamente permite al hijo por lo que da error
            */

            Transform root = other.transform; //No lo acabo de comprender del todo pero hace que busque en todo el gameObject

            Weapon[] weapons = root.GetComponentsInChildren<Weapon>(true); //Busca los hijos que 

            foreach (Weapon weapon in weapons)
            {
                // Comparamos con el modo de disparo
                if (weapon.currentShootingMode == ammoMode)
                {
                    weapon.totalAmmo = Mathf.Min(weapon.totalAmmo + ammoAmount, weapon.maxTotalAmmo);
                    Destroy(gameObject);
                    break;
                }
            }
        }

    }

    
}
