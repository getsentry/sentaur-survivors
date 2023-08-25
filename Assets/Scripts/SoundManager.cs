using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _enemyHitSound;
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

        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    public void Init() {
        _enemyHitSound = GetComponent<AudioSource>();

    }

    public void PlayHitSound() {
        // don't play the sound if it's too soon
        if (Time.time - _timeOfLastHitSound < _hitSoundCooldown) {
            return;
        }
        
        _enemyHitSound.Play();
        _timeOfLastHitSound = Time.time;
    }
}
