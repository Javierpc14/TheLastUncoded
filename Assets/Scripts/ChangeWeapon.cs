using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public int selectedWeapon = 0; //Arma seleccionada, su el número de cada arma depende del orden que se encuentren como hijo de scoupe  
    void Start()
    {
        SelectWeapon(); //funcion para activar el arma
    }
    
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon; //Guardamos el arma equipada actual

        //Vamos cambiando el arma seleccionada con la rueda
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1) //tomamos como límite de valores el número de hijos de scoupe
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
            
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }

        }

        //Camabiamos el arma seleccionada con el teclado
        //Lo he hecho un poco a la fuerza, posiblemente se pueda mejorar el codigo con un switch
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) //De nuevo fijamos un limite para no dar lugar a no tener ningun arma seleccionada
        {
            selectedWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        //Cambiamos el arma cuando el numero seleccionado no coincide con el numero de arma equipada actualmente
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
    /*
    Activa el arma segun el numero asignado 
    */
    void SelectWeapon()
    {
        int i = 0;

        //Realiza un for para buscar las armas segun el numero asignado
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else 
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
