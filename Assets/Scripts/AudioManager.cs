﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explosionClip;

    private AudioSource _audioSource;

    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source in the Audio_Manager is NULL.");
        }
        else
        {
            _audioSource.clip = _explosionClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExplosion()
    {
        _audioSource.Play();
    }
}
