using UnityEngine;
using UnityEngine.UI;

public class OptionSetting : MonoBehaviour
{
    public Image bright;

    void Start()
    {
        float brightness = PlayerPrefs.GetFloat("bright", 0.5f); // Carga el brillo (0.5 por defecto)
        bright.color = new Color(bright.color.r, bright.color.g, bright.color.b, brightness);
    }

}
