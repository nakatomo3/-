using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	public enum TriggerType {
		Default,
		RightGear,
		LeftGear,
		Button,
		MinusButton,
		Electrical,
		Forever
	}
	//[HideInInspector]
    public TriggerType thisType = TriggerType.Default;

    [HideInInspector]
	public int connectNum = -1;

	public MeshRenderer mesh;

	private Transform thisTransform;

	//[HideInInspector]
	public bool isThisGimmick = false;

	//ボタンの時に使う
	private float defaultY;

	public Transform visualObject;

	[SerializeField]
	private bool isPlus = true;

	// Start is called before the first frame update
	void Start() {
		thisTransform = gameObject.GetComponent<Transform>();

		if(thisType == TriggerType.Default || connectNum == -1) {
			Debug.LogError("エラー文");
		}

		defaultY = thisTransform.position.y;
	}

	// Update is called once per frame
	void Update() {
        CheckTriggerPlus();
		CheckTriggerMinus();

	}

	private void CheckTriggerPlus() {
		var isTriggerOn = false;
		float buttonPushRange = 0;

		if (SystemManager.instance.GetGimmickValue(connectNum) <=1) {
			switch (thisType) {
				case TriggerType.RightGear:
					if (Input.GetKey(KeyCode.D) && Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;
					} else {
						isTriggerOn = false;
					}
					break;

				case TriggerType.LeftGear:
					if (Input.GetKey(KeyCode.A) && Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;

					} else {
						isTriggerOn = false;
					}
					break;

				case TriggerType.Button:
					if (isThisGimmick == true) {
						isTriggerOn = true;
					} else {
						isTriggerOn = false;
					}

					break;

				case TriggerType.Electrical:
					if (Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;
					}
					break;
				case TriggerType.Forever:
					if(isThisGimmick == true && isPlus) {
						isTriggerOn = true;
					}
					break;

				default:
					isTriggerOn = false;
					break;

			}

		}

		if (thisType == TriggerType.Button || thisType == TriggerType.Forever) {
			if (isThisGimmick) {
				buttonPushRange = 0.5f;
			} else {
				buttonPushRange = 0f;
			}
			visualObject.position = new Vector3(thisTransform.position.x, defaultY - buttonPushRange, thisTransform.position.z);
		}

		if(thisType == TriggerType.Forever && SystemManager.instance.GetGimmickValue(connectNum) >= 1) {
			isPlus = false;
		}

		if (isTriggerOn == true) {
			SystemManager.instance.ActionGimmickPlus(connectNum);
			SystemManager.instance.ChangeGimmickValue(true, connectNum);
		}
    }

	private void CheckTriggerMinus() {
		var isTriggerOn = false;
		float buttonPushRange = 0;

		if (SystemManager.instance.GetGimmickValue(connectNum) >= -1) {
			switch (thisType) {
				case TriggerType.RightGear:
					if (Input.GetKey(KeyCode.A) && Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;
					} else {
						isTriggerOn = false;
					}
					break;

				case TriggerType.LeftGear:
					if (Input.GetKey(KeyCode.D) && Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;
					} else {
						isTriggerOn = false;
					}
					break;

				case TriggerType.MinusButton:
					if (isThisGimmick == true) {
						isTriggerOn = true;
					} else {
						isTriggerOn = false;
					}

					visualObject.position = new Vector3(thisTransform.position.x, defaultY - buttonPushRange, thisTransform.position.z);
					break;

				case TriggerType.Electrical:
					if (Player.instance.isGimmickMode == true && isThisGimmick == true) {
						isTriggerOn = true;
					}
					break;

				case TriggerType.Forever:
					if (isThisGimmick == true && isPlus == false) {
						isTriggerOn = true;
					}
					if (SystemManager.instance.GetGimmickValue(connectNum) <= -1) {
						isPlus = true;
					}
					break;

				default:
					isTriggerOn = false;
					break;
			}
		}

		if (thisType == TriggerType.MinusButton || thisType == TriggerType.Forever) {
			if (isThisGimmick) {
				buttonPushRange = 0.5f;
			} else {
				buttonPushRange = 0f;
			}
			visualObject.position = new Vector3(thisTransform.position.x, defaultY - buttonPushRange, thisTransform.position.z);
		}

		if (thisType == TriggerType.Forever && SystemManager.instance.GetGimmickValue(connectNum) <= -1) {
			isPlus = true;
		}


		if (isTriggerOn == true) {
			SystemManager.instance.ActionGimmickMinus(connectNum);
			SystemManager.instance.ChangeGimmickValue(false, connectNum);
		}
	}


}
