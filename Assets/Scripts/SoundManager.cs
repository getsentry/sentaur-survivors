using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _enemyHitSound;


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
        _enemyHitSound.Play();
    }
}
