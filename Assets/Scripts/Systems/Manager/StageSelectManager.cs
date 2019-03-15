using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour {

	public static StageSelectManager instance;

	private int selectingStageNum;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void ChangeStage() {
		//selectingStageNumのステージに移動

	}
}
