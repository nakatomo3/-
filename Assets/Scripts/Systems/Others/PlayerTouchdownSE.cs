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
        touchDownaudioSouce = GetComponent<AudioSource>();
        moveAudioSource = transform.GetChild(0).GetComponent<AudioSource>();

    }


    // Update is called once per frame
    void Update()
    {
        speed = (this.transform.position.x - latestPos.x) / Time.deltaTime;
        latestPos = this.transform.position;
        if (speed == 0 || Player.instance.wasGameOver == true || OptionManager.instance.isPause == true) {
            moveAudioSource.Stop();
            canSound = true;
        }
        if(Player.instance.isJumping == true) {
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
        if (other.gameObject.CompareTag("Ground")) {
            if (speed != 0) {
                if (canSound == true) {
                    moveAudioSource.Play();
                    canSound = false;
                }
            }
        }
    }
}
