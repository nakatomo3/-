﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMover : MonoBehaviour {

	public int stageNum;

	[HideInInspector]
	public float timer = 0;

	public MeshRenderer mesh;

	[HideInInspector]
	public bool isConnect = false;

	private const float OPENING_TIME = 3;

    private GameObject handle;
    private const float HANDLE_ROTATE_SPEED = 170;

    private Vector3 latestPos;
    private float speed;
    private AudioSource audioSource;
    private GameObject soundDecision;
    private bool canSound = false;

    // Start is called before the first frame update
    void Start() {
        handle = transform.GetChild(1).gameObject;
        soundDecision = handle.transform.GetChild(1).gameObject;
        audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		if(isConnect == true) {
			if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("GamePadStickHolizontal") > 0.3) {
				timer += Time.deltaTime;
				StageSelectManager.instance.doors[stageNum - 1].transform.Rotate(0, Time.deltaTime * 55, 0);
			}
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("GamePadStickHolizontal") < -0.3) {
				if(timer > 0) {
					timer -= Time.deltaTime;
					StageSelectManager.instance.doors[stageNum - 1].transform.Rotate(0, Time.deltaTime * -55, 0);
				}
			}
            //ハンドルが回転するアニメーション
            if (Input.GetKey(KeyCode.D) || Input.GetAxis("GamePadStickHolizontal") > 0.3) {
                handle.transform.Rotate(0, 0, HANDLE_ROTATE_SPEED * Time.deltaTime);

            }else if (Input.GetKey(KeyCode.A) || Input.GetAxis("GamePadStickHolizontal") < -0.3) {
                handle.transform.Rotate(0, 0, -HANDLE_ROTATE_SPEED * Time.deltaTime);
            }

			Player.instance.gameObject.transform.position = transform.position;
			mesh.enabled = false;
            //SE再生
            if (canSound == true) {
                audioSource.Play();
                canSound = false;
            }
        } else {
			mesh.enabled = true;
		}
		if(timer < 0) {
			timer = 0;
		}

		if(timer >= OPENING_TIME) {
			if (stageNum == 7) {
				SceneManager.LoadScene("StaffCredit");
			} else {
				StageSelectManager.instance.ChangeStage(stageNum);
				Player.instance.isGimmickMode = false;
				Player.instance.rigidbody.useGravity = true;
				SceneManager.LoadScene("Game");

			}
		}
        //SEストップ
        speed = ((soundDecision.transform.position - latestPos) / Time.deltaTime).magnitude;
        latestPos = soundDecision.transform.position;
        if (speed <= 0 || Player.instance.wasGameOver == true || OptionManager.instance.isPause == true) {
            canSound = true;
            audioSource.Stop();

        }
     
    }
}
