using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSound : MonoBehaviour
{
    private AudioSource sound01;
    private AudioSource steam;
    private float steamPlaySoundCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound01 = audioSources[0];
        steam = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (steamPlaySoundCounter >= 10) {
            steam.PlayOneShot(steam.clip);
            steamPlaySoundCounter = 0;

        }
        steamPlaySoundCounter += Time.deltaTime;
    }
}
