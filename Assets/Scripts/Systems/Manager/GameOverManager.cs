using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
	public static GameOverManager instance;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = new Ray();
			RaycastHit hit = new RaycastHit();
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {

				//ゲーム開始
				if (hit.collider.gameObject.tag == "StageRetry") {
#if UNITY_EDITOR
					StageRetry();
#endif
				}

				//タイトルへ戻る
				if (hit.collider.gameObject.tag == "StageSelect") {
					StageSelect();

				}
			}
		}
	}

	public void StageRetry() {
		PlayerPrefs.SetInt("stage", StageSelectManager.instance.selectingStageNum);
		SceneManager.LoadScene("Game");
	}

	public void StageSelect() {
		SceneManager.LoadScene("StageSelect");
	}

}