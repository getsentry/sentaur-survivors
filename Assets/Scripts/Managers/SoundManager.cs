using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _enemyHitSound;

    private float _timeOfLastHitSound = 0f;

    [SerializeField]
    private float _hitSoundCooldown = 0.1f;

    private static SoundManager _instance;

    public static SoundManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        Init();
    }

    public void Init()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void PlayHitSound()
    {
        // don't play the sound if it's too soon
        if (Time.time - _timeOfLastHitSound < _hitSoundCooldown)
        {
            return;
        }

        // NOTE: don't use PlayOneShot on hit sounds because so many hits can
        // happen that it can cause the audio source to die (no more sound for
        // remainder of game session). Better to play manually.
        _audioSource.clip = _enemyHitSound;
        _audioSource.Play();
        _timeOfLastHitSound = Time.time;
    }
}
