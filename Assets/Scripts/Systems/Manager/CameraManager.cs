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

	private void Awake() {
		instance = this;
		thisTransform = transform;
		playerTransform = Player.instance.gameObject.transform;
	}


	// Start is called before the first frame update
	void Start() {
		thisTransform.Rotate(0, -0.8f, 0);
		thisTransform.position = playerTransform.position + new Vector3(0, 4.8f, -20f);
	}

	// Update is called once per frame
	void Update() {

		thisTransform.position = new Vector3(playerTransform.position.x + SideRange, posY, thisTransform.position.z);
		thisTransform.localRotation = Quaternion.Euler(0,SideRange,0);
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
}
