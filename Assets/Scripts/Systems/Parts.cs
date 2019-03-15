using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour {

	public enum PartsType {
		Default,
		Door,
		Bridge
	}
	[HideInInspector]
	public PartsType thisType = PartsType.Default;

	// Start is called before the first frame update
	void Start() {
		if(thisType == PartsType.Default) {
			Debug.Log("エラー文");
		}
	}

	// Update is called once per frame
	void Update() {

	}
}
