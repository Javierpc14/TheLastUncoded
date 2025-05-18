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

    //Metodo para reproducir la musico o efectos
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
