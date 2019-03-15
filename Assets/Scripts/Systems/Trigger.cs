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

	// Start is called before the first frame update
	void Start() {
		if(thisType == TriggerType.Default || connectNum == -1) {
			Debug.LogError("エラー文");
		}
	}

	// Update is called once per frame
	void Update() {

	}

	private void TriggerOn() {
		SystemManager.instance.ActionGimmick(connectNum);
	}
}
