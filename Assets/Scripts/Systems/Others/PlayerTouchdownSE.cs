using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchdownSE : MonoBehaviour
{
    private AudioSource touchDownaudioSouce;
    private AudioSource moveAudioSource;
    private bool canSound=false;

    private Vector3 latestPos;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        touchDownaudioSouce = audioSources[0];
        moveAudioSource = audioSources[1];

    }


    // Update is called once per frame
    void Update()
    {
        if (speed <= 0 || Player.instance.wasGameOver == true || OptionManager.instance.isPause == true) {
            moveAudioSource.Stop();
            canSound = true;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (Player.instance.isJumping == true) {
            if (other.gameObject.CompareTag("Ground")) {
                touchDownaudioSouce.Play();
            }
        }
        if(Input.GetKeyUp(KeyCode.W) || Input.GetButtonUp("GamePadA")) {
            touchDownaudioSouce.Stop();
        }
    }

    private void OnTriggerExit(Collider other) {
        touchDownaudioSouce.Stop();
    }

    private void OnTriggerStay(Collider other) {
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("GamePadStickHolizontal") > 0.3;
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("GamePadStickHolizontal") < -0.3;
        if (other.gameObject.CompareTag("Ground")) {
            if (isRight == true || isLeft == true) {
                if (canSound == true) {
                    moveAudioSource.Play();
                    canSound = false;
                }
            }
        }
    }
}
