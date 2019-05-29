using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class CameraManager : MonoBehaviour {

	public static CameraManager instance;

	public Transform playerTransform;
	private Transform thisTransform;

	[SerializeField]
	private float SideRange = 0;
	private const float SIDE_MAX_RAMGE = 3;
    private const float CAMERA_POS_Z = -30;

	float posX;
	float posY;

	private bool isWholeMode = false;

	private Vector3 originPosition = new Vector3();
	private Vector3 moveToPosition = new Vector3();

	private float wholeRange = -70;
	private float smallerLong = 0;

	//[HideInInspector]
	public bool isFreeAnimation = false;

	private AudioSource audioSource;

    private bool isFilmingMode = false;

	private void Awake() {
		instance = this;
		thisTransform = transform;
	}


	// Start is called before the first frame update
	void Start() {
		playerTransform = Player.instance.gameObject.transform;

		thisTransform.Rotate(0, -0.8f, 0);
		thisTransform.position = playerTransform.position + new Vector3(0, 4.8f, CAMERA_POS_Z);
		moveToPosition = thisTransform.position;

		if (SceneManager.GetActiveScene().name == "Game") {
			if (SystemManager.instance.width >= SystemManager.instance.height) {
				smallerLong = SystemManager.instance.width;
			} else {

				smallerLong = SystemManager.instance.height;
			}
		} else {
			smallerLong = 50;
		}
		wholeRange = -70 * smallerLong/100;

		audioSource = GetComponent<AudioSource>();
		
	}

	// Update is called once per frame
	void Update() {
        if(isFilmingMode == false) {
            if(isFreeAnimation == false) {
                if(isWholeMode == false) {
                    if(SceneManager.GetActiveScene().name == "Game") {
                        thisTransform.position = new Vector3(posX + SideRange, posY, thisTransform.position.z);
                        thisTransform.localRotation = Quaternion.Euler(0, SideRange, 0);
                    } else {

                        thisTransform.position += new Vector3((playerTransform.position.x - thisTransform.position.x) / 3, (playerTransform.position.y + 4 - thisTransform.position.y) / 30, (playerTransform.position.z + CAMERA_POS_Z - thisTransform.position.z) / 80);

                    }

                }

                if(SceneManager.GetActiveScene().name == "Game") {
                    if(SystemManager.instance.isDeath == false) {
                        ChangeWholeMode();
                    }
                }

                posX = Mathf.Lerp(thisTransform.position.x, playerTransform.position.x, 0.1f);
            } else {
                thisTransform.position = Vector3.Lerp(thisTransform.position, new Vector3(5, -6, -30), 0.1f);
            }
        }else{
            thisTransform.position += Vector3.right * 10 * Time.deltaTime;
        }

    }

    public void MoveRight() {
		if (Player.instance.isGimmickMode == false) {
			SideRange += Time.deltaTime * Player.instance.moveSpeed;

			if (SideRange >= SIDE_MAX_RAMGE) {
				SideRange = SIDE_MAX_RAMGE;
			}
		}
	}

	public void MoveLeft() {
		if (Player.instance.isGimmickMode == false) {
			SideRange -= Time.deltaTime * Player.instance.moveSpeed;

			if (SideRange <= -SIDE_MAX_RAMGE) {
				SideRange = -SIDE_MAX_RAMGE;
			}
		}

	}

	private void FixedUpdate() {
		posY = Mathf.Lerp(thisTransform.position.y, playerTransform.position.y + 4f, 0.1f);
    }

	private void ChangeWholeMode() {
		bool changeKey = Input.GetKey(KeyCode.Q) || Input.GetButton("GamePadRB") || Input.GetButton("GamePadLB");

		if (changeKey == false) {
			isWholeMode = false;
			moveToPosition = new Vector3(playerTransform.position.x, playerTransform.position.y+4.8f, CAMERA_POS_Z);
			SideRange = 0;
		} else {
			isWholeMode = true;
			originPosition = thisTransform.position;
			moveToPosition = new Vector3(SystemManager.instance.width / 2, SystemManager.instance.height / 2, wholeRange);
		}

		if(isWholeMode == true) {
			SideRange = 0;
		}

		thisTransform.position += new Vector3((moveToPosition.x - thisTransform.position.x) /80, (moveToPosition.y - thisTransform.position.y) / 80, (moveToPosition.z - thisTransform.position.z) / 80);
		thisTransform.localRotation = Quaternion.Euler(new Vector3(0, SideRange, 0));
	}

	public void ChangeBGM(AudioClip audioClip) {
		audioSource.clip = audioClip;
		audioSource.Play();
	}
}
