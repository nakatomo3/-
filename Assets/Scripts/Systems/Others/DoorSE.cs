using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSE : MonoBehaviour {
    private GameObject soundDecision;
    private AudioSource audioSource;
    private bool canSound = false;
    private float speed;
    private Vector3 latestPos;

    private const float DOOR_PITCH = 1.25f;
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        soundDecision = transform.GetChild(1).gameObject;
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("GamePadStickHolizontal") > 0.3) {
            audioSource.pitch = DOOR_PITCH;
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("GamePadStickHolizontal") < -0.3) {
            audioSource.pitch = -DOOR_PITCH;

        }

        speed = ((soundDecision.transform.position - latestPos) / Time.deltaTime).magnitude;
        latestPos = soundDecision.transform.position;
        if (speed <= 0 || Player.instance.wasGameOver == true || OptionManager.instance.isPause == true) {
            audioSource.Pause();
            canSound = true;
        }

        if (canSound == true && speed > 0) {
            audioSource.Play();
            canSound = false;
            Debug.Log("play");
        }
    }
}