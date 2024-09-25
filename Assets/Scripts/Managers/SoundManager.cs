using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _enemyHitSound;

    [SerializeField]
    private AudioClip _pickupSound;

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

    public void PlayPickupSound()
    {
        // _audioSource.clip = _pickupSound;
        _audioSource.PlayOneShot(_pickupSound);
    }

    public void PlayHitSound()
    {
        // don't play the sound if it's too soon
        if (Time.time - _timeOfLastHitSound < _hitSoundCooldown)
        {
            return;
        }

        // _audioSource.clip = _enemyHitSound;
        _audioSource.PlayOneShot(_enemyHitSound);
        _timeOfLastHitSound = Time.time;
    }
}
