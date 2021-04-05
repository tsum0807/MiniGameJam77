using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;

    [SerializeField] private AudioClip ambience;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip enemyHurt;
    [SerializeField] private AudioClip heartBeat;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip powerUp;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip walk;

    private AudioSource audioSource;

    void Awake()
    {
        if (AM != null)
        {
            Destroy(AM);
        }
        else
        {
            AM = this;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void PlaySound(AudioClip clipToPlay, float volume = 1f)
    {
        if (volume == 0)
        {
            // volume is muted so dont try to play anything
            return;
        }
        // Randomize sound volume slightly
        float roll = Random.Range(-0.15f, 0.15f);
        float volToPlayAt = volume + roll <= 0 ? volume : volume + roll;

        audioSource.PlayOneShot(clipToPlay, volToPlayAt);
    }

    public void PlayAmbience()
    {
        audioSource.Play();
    }

    public void PlayAttackSound()
    {
        PlaySound(attack);
    }
    public void PlayEnemyHurtSound()
    {
        PlaySound(enemyHurt);
    }
    public void PlayHeartBeatSound()
    {
        PlaySound(heartBeat);
    }
    public void PlayPlayerHurtSound()
    {
        PlaySound(playerHurt);
    }
    public void PlayPowerUpSound()
    {
        PlaySound(powerUp);
    }
    public void PlayRunSound()
    {
        PlaySound(run);
    }
    public void PlayScreamSound()
    {
        PlaySound(scream);
    }
    public void PlayWalkSound()
    {
        PlaySound(walk);
    }
}
