using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSounds : MonoBehaviour
{
    private AudioSource _source;
    public List<AudioClip> sounds;
    private float tmr = 0;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.volume = PlayerPrefs.GetFloat("volume");

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > tmr)
        {
            tmr = Time.time + Mathf.Max(4.0f, Random.value * 30);
            _source.clip = sounds[Random.Range(0, sounds.Count - 1)];
            _source.Play();
        }
    }
}
