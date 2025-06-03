using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private PlayerMovement playerMovement;
    private int vidaMaxima;

    void Start()
    {
        playerMovement = GameObject.Find("Prota").GetComponent<PlayerMovement>();
        vidaMaxima = playerMovement.maxVida;
    }

    void Update()
    {
        rellenoBarraVida.fillAmount = (float)playerMovement.vidaActual / vidaMaxima;
    }
}


