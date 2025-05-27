using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    //Aqui introducimos la musica y efectos
    [Header("Audio Clio")]
    public AudioClip pistolShot;
    public AudioClip pistolReload;
    public AudioClip rifleShot;
    public AudioClip backgroundMusic;


    void Start()
    {
        PlayMusic(backgroundMusic);
    }
    
    //Metodo para reproducir la musico o efectos
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        /*
         PlayOneShot lo que hace es reproducir el clip sin interrumpir lo que ya esta sonando en el audiosource
         No permite loopear, parar u otras opciones
        */
    }
    //Metodo para reproducir musica
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip; //Asignamos el clip
        musicSource.loop = true; //Cancion en bucle
        musicSource.Play(); //Reproduce
    }
}
