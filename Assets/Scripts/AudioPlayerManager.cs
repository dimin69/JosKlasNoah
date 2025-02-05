﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Story;
using Custom.GameManager;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayerManager : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        GameManager.CurrentAudioSource = _audioSource;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(_audioSource.clip != null)
        {
            if(!_audioSource.isPlaying)
            {
                _audioSource.clip = null;
                StoryEventManager.OnAudioFinished();
            }
        }
    }
}
