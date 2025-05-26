using UnityEngine;
using UnityEngine.SceneManagement;

public class PausarJuego : MonoBehaviour
{
    public GameObject menuPausa;
    public bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (juegoPausado)
            {
                Reanudar();
                //el cursor se vuelve a bloquear
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Pausar();
                //desbloquea el cursos
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void Reanudar(){
        menuPausa.SetActive(false);
        //velocidad a la que el juego se ejecuta
        Time.timeScale = 1;

        //el cursor se vuelve a bloquear
        Cursor.lockState = CursorLockMode.Locked;

        juegoPausado = false;
    }

    public void Pausar(){
        menuPausa.SetActive(true);
        Time.timeScale = 0;

        //desbloquea el cursos
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        juegoPausado = true;
    }

    public void VolverMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
