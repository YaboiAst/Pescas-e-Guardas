using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

using UnityEngine;

public class WaterSoundScript : MonoBehaviour {
    [Header("------ Referências ------")]
    [SerializeField] private AudioSource musicSource;     // Música de fundo
    [SerializeField] private AudioSource SFXSource;       // Som do movimento
    [Header("------ Áudios ------")]
    public AudioClip background;
    public AudioClip shipMovement;

    private bool isMoving = false;

    private void Start()
    {
        // Música de fundo
        if (musicSource != null && background != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.volume = 0.05f;
            musicSource.Play();
        }

        // Som de movimento
        if (SFXSource != null && shipMovement != null)
        {
            SFXSource.clip = shipMovement;
            SFXSource.loop = true;
            SFXSource.volume = 0.05f;
        }
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool inputDetected = h != 0 || v != 0;

    
        if (inputDetected && !isMoving)
        {
            isMoving = true;
            if (!SFXSource.isPlaying)
                SFXSource.Play();
        }
        else if (!inputDetected && isMoving)
        {
            isMoving = false;
            if (SFXSource.isPlaying)
                SFXSource.Pause();
        }
    }
}
