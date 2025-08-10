using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEwalk : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSource.PlayOneShot(sound1);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            GetComponent<AudioSource>().Stop();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSource.PlayOneShot(sound1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
