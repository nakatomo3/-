using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class PlaySound : MonoBehaviour
{
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        System.Random rnd = new System.Random();    // インスタンスを生成
        int intResult = rnd.Next(5);        // 0～9の乱数を取得
        switch (intResult)
        {
            case 0:
                audioSource.clip = audioClip1;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = audioClip2;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = audioClip3;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = audioClip4;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = audioClip5;
                audioSource.Play();
                break;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StageSelect")
        {
            CameraManager.instance.GetComponent<AudioSource>().enabled = false;
        }
    }

}