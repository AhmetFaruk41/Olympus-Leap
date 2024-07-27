using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioClip backgroundMusic;

    private AudioSource audioSource;
    public bool isMuted { get; private set; } = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değiştiğinde yok olmasını engelle
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; // Müziğin döngüsel çalmasını sağla
            audioSource.playOnAwake = true; // Oyun başladığında müziği çal
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Eğer başka bir instance varsa bu gameObject'i yok et
        }
    }

    // Sesi açıp kapatmayı sağlayan metot
    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }
}