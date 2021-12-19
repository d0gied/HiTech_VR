using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSfx : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource _audioSource;
    public AudioClip _clip;
    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= 1)
        {
            _audioSource.PlayOneShot(_clip);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }
}
