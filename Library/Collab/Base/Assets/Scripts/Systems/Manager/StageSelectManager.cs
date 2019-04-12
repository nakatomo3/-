using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	public static StageSelectManager instance;

	private int selectingStageNum = 1;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
#if UNITY_EDITOR
		if (Input.anyKey) {
			PlayerPrefs.SetInt("stage", selectingStageNum);
			SceneManager.LoadScene("Game");
		}
#endif
	}

	public void ChangeStage() {
		//selectingStageNumのステージに移動

	}
}
