using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public static CameraManager instance;

	private Transform playerTransform;
	private Transform thisTransform;

	private float SideRange = 0;
	private const float SIDE_MAX_RAMGE = 3;

	float posX;
	float posY;

	private bool isWholeMode = false;

	private Vector3 originPosition = new Vector3();
	private Vector3 moveToPosition = new Vector3();

	private float wholeRange = -70;
	private float smallerLong = 0;

	private void Awake() {
		instance = this;
		thisTransform = transform;
		playerTransform = Player.instance.gameObject.transform;
	}


	// Start is called before the first frame update
	void Start() {
		thisTransform.Rotate(0, -0.8f, 0);
		thisTransform.position = playerTransform.position + new Vector3(0, 4.8f, -20f);
		moveToPosition = thisTransform.position;

		if(SystemManager.instance.width >= SystemManager.instance.height) {
			smallerLong = SystemManager.instance.height;
		} else {
			smallerLong = SystemManager.instance.width;
		}

		wholeRange = -70 * smallerLong/100;
	}

	// Update is called once per frame
	void Update() {
		if(isWholeMode == false) {
			thisTransform.position = new Vector3(playerTransform.position.x + SideRange, posY, thisTransform.position.z);
			thisTransform.localRotation = Quaternion.Euler(0,SideRange,0);
		}

		ChangeWholeMode();
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
		posY = Mathf.Lerp(thisTransform.position.y, playerTransform.position.y+4f, 0.1f);
	}

	private void ChangeWholeMode() {
		bool changeKey = Input.GetKey(KeyCode.Q);

		if (changeKey == false) {
			isWholeMode = false;
			moveToPosition = new Vector3(playerTransform.position.x,playerTransform.position.y+4.8f,-20);
			SideRange = 0;
		} else {
			isWholeMode = true;
			originPosition = thisTransform.position;
			moveToPosition = new Vector3(SystemManager.instance.width / 2, SystemManager.instance.height / 2, wholeRange);
		}

		if(isWholeMode == true) {
			SideRange = 0;
		}

		thisTransform.position += new Vector3((moveToPosition.x - thisTransform.position.x) /50, (moveToPosition.y - thisTransform.position.y) / 50, (moveToPosition.z - thisTransform.position.z) / 80);
		thisTransform.localRotation = Quaternion.Euler(new Vector3(0, SideRange, 0));
	}
}
