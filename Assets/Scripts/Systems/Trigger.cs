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

	[HideInInspector]
	public bool isThisGimmick = false;

	// Start is called before the first frame update
	void Start() {
		if(thisType == TriggerType.Default || connectNum == -1) {
			Debug.LogError("エラー文");
		}
	}

	// Update is called once per frame
	void Update() {
        CheckTrigger();

	}

    private void CheckTrigger() {
        var isTriggerOn = false;

        if (Player.instance.isGimmickMode == true && isThisGimmick == true) {
            switch (thisType) {
                case TriggerType.RighrtGear:
                    if (Input.GetKey(KeyCode.D)) {
                        isTriggerOn = true;

                    } else {
                        isTriggerOn = false;
                    }
                    break;

                case TriggerType.LeftGear:
                    if (Input.GetKey(KeyCode.A)) {
                        isTriggerOn = true;

                    } else {
                        isTriggerOn = true;
                    }
                    break;

                case TriggerType.Button:
                    isTriggerOn = true;
                    break;

                case TriggerType.Electrical:
                    isTriggerOn = true;
                    break;

                default:
                    isTriggerOn = true;
                    break;
            }
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
