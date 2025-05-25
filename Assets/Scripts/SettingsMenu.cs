using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Referencias")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public AudioMixer mainAudioMixer;
    public Slider brightSlider;
    public Image bright;
    public Toggle toggle;
    public TMP_Dropdown resolutionDropDown;
    public TMP_Dropdown qualityDropdown;
    Resolution[] resolutions;//Creamos un array para contener las resoluciones

    [Header("Valores de las Barras")]
    public float brightSliderValue;
    public float sliderVolumeValue;
    public int quality;



    void Start()
    {
        brightSlider.value = PlayerPrefs.GetFloat("bright", 0.5f); //Iniciamos la primera vez con unos valores predeterminados,
                                                                   //luego utilizaremos la preferencia

        bright.color = new Color(bright.color.r, bright.color.g, bright.color.b, brightSlider.value); //cambia la transparencia (4 valor) segun la barra

        quality = PlayerPrefs.GetInt("qualityNumber", 3);
        qualityDropdown.value = quality;
        AdjustQuality();

        //sliderVolume.value = PlayerPrefs.GetFloat("volumeAudio", 0.5f);
        //AudioListener.volume = sliderVolume.value;

        //Comprobamos si la casilla está marcada o no para activar la pantalla completa
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        CheckResolution();
    }
    
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", musicVolumeSlider.value);
    }

    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXVol", sfxVolumeSlider.value);
    }

    public void ChangeBright(float value)
    {
        brightSliderValue = value;
        PlayerPrefs.SetFloat("bright", brightSlider.value);
        PlayerPrefs.Save(); //Guardamos la preferencia
        bright.color = new Color(bright.color.r, bright.color.g, bright.color.b, brightSlider.value);
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
        PlayerPrefs.SetInt("qualityNumber", qualityDropdown.value);
        quality = qualityDropdown.value;
    }

    public void ActivateFullScreen(bool fullScreen) //Activa pantalla completa si el valor es verdadero
    {
        Screen.fullScreen = fullScreen;
    }

    void CheckResolution()
    /*
     Aquí comprobaremos las resoluciones que permite la pantalla, las añadiremos a la lista y
     comprobaremos la resolucion actual, pero no realizaremos el cambio de resolucion
    */
    {
        resolutions = Screen.resolutions; //Las resoluciones serán las que tenga la pantalla
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>(); //Creamos una lista
        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option); //Añadimos y mostramos las resoluciones que tiene la pantalla (ancho x alto)

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i; //Como estamos recorriendo la lista con el bucle, cuando la resolucion de la pantalla sea la misma que la opcion
                                       // la fijara como la actual
            }
        }
        //Añadimos las opciones a la lista vertical y fijamos la resolucion actual
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolution;
        resolutionDropDown.RefreshShownValue();

        resolutionDropDown.value = PlayerPrefs.GetInt("resolutionNumber", 0);
    }

    public void ChangeResolution(int indexResolution) //Cambiamos la resolucion
    {
        PlayerPrefs.SetInt("resolutionNumber", resolutionDropDown.value);

        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
