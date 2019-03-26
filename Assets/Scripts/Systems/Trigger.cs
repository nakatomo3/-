using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	public enum TriggerType {
		Default,
		RighrtGear,
		LeftGear,
		Button,
		Electrical
	}
	[HideInInspector]
    public TriggerType thisType = TriggerType.Default;

    [HideInInspector]
	public int connectNum = -1;

	public MeshRenderer mesh;

	private Transform thisTransform;

	[HideInInspector]
	public bool isThisGimmick = false;

	//ボタンの時に使う
	private float defaultY;

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
        CheckTrigger();

	}

    private void CheckTrigger() {
        var isTriggerOn = false;

		switch (thisType) {
			case TriggerType.RighrtGear:
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
					isTriggerOn = true;
				}
				break;

			case TriggerType.Button:
				float buttonPushRange = 0.5f;
				if (isThisGimmick == true) {
					isTriggerOn = true;
					buttonPushRange = 0.5f;
				} else {
					buttonPushRange = 0;
				}

				thisTransform.position = new Vector3(thisTransform.position.x, defaultY - buttonPushRange, thisTransform.position.z);
				break;

			case TriggerType.Electrical:
				if (Player.instance.isGimmickMode == true && isThisGimmick == true) {
					isTriggerOn = true;
				}
				break;

			default:
				isTriggerOn = true;
				break;
		}
        
        if (isTriggerOn == true) {
            TriggerOn();
        }
    }

	/// <summary>
	/// 条件を満たしたときに呼び出される関数。
	/// </summary>
	private void TriggerOn() {
		SystemManager.instance.ActionGimmick(connectNum);
	}

	
}
