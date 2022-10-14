using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource mainSoundtrack;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource rotateSound;
    [SerializeField] AudioSource explosionSound;
    [SerializeField] AudioSource gameOverSound;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void StartMainSoundtrack()
    {
        if (!mainSoundtrack.isPlaying)
        {
            mainSoundtrack.Play();
        }
    }

    public IEnumerator SpeedMainSoundTrack(float amount)
    {
        float endValue = mainSoundtrack.pitch + amount;
        while (true)
        {
            yield return null;
            if (mainSoundtrack.pitch < 1.5f)
            {
                mainSoundtrack.pitch += amount * Time.deltaTime * 0.35f;
                if (mainSoundtrack.pitch > 1.5f)
                {
                    mainSoundtrack.pitch = 1.5f;
                }
            }

            if (mainSoundtrack.pitch > endValue)
            {
                yield break;
            }
        }
    }

    public void StopMainSoundTrack()
    {
        mainSoundtrack.Stop();
    }

    public void HitSound()
    {
        if (hitSound.isPlaying)
        {
            hitSound.Stop();
        }

        hitSound.Play();
    }

    public void RotateSound()
    {
        if (rotateSound.isPlaying)
        {
            rotateSound.Stop();
        }

        rotateSound.Play();
    }

    public void GameOverSound()
    {
        if (gameOverSound.isPlaying)
        {
            gameOverSound.Stop();
        }
        StopMainSoundTrack();
        gameOverSound.Play();
    }

    public void ExplosionSound()
    {
        if (!explosionSound.isPlaying)
        {
            explosionSound.Play();
        }   
    }


}
