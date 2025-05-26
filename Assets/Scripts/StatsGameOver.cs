using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsGameOver : MonoBehaviour
{
    //los textos del canvas
    public Text txtTotalWaves;
    public Text txtTotalEnemy;


    private void Start()
    {
        //cargo los datos obtenidos en las otras clases
        int oleadas = PlayerPrefs.GetInt("Oleadas", 0);
        int enemigos = PlayerPrefs.GetInt("Enemigos", 0);

        Debug.Log("OLEADAS (PlayerPrefs): " + PlayerPrefs.GetInt("Oleadas", -1));
        Debug.Log("ENEMIGOS (PlayerPrefs): " + PlayerPrefs.GetInt("Enemigos", -1));

        //muestro los datos en los textos del canvas
        txtTotalWaves.text = "" + oleadas;
        txtTotalEnemy.text = "" + enemigos;
    }
    
    public void VolverMenu(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
