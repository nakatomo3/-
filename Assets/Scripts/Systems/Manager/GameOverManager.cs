using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {
	public static GameOverManager instance;

    private bool isSelectingRetry = true;

    public Transform restartparent;
    public Transform returnParent;

    private Transform[] restartGears;
    private Transform[] returnGears;

    private float rotateSpeed = 100;

	public Transform[] LightParent = new Transform[2];
	private Image[,] light = new Image[2,2];

    private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
        restartGears = new Transform[restartparent.childCount];
        returnGears = new Transform[returnParent.childCount];

        for(int i = 0; i < restartparent.childCount; i++)
        {
            restartGears[i] = restartparent.GetChild(i);
        }
        for (int i = 0; i < returnParent.childCount; i++)
        {
            returnGears[i] = returnParent.GetChild(i);
        }

		for(int i = 0; i < 2; i++) {
			light[i, 0] = LightParent[i].GetChild(0).GetComponent<Image>();
			light[i, 1] = LightParent[i].GetChild(1).GetComponent<Image>();
		}
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
					StageRetry();
				}

				//タイトルへ戻る
				if (hit.collider.gameObject.tag == "StageSelect") {
					StageSelect();

				}
			}
		}

		if (isSelectingRetry) {
			for (int i = 0; i < restartGears.Length; i++) {
				restartGears[i].Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime * (-1 * i % 2) + 0.5f) * 2);
			}
			for(int i = 0; i < 2; i++) {
				if(i == 0) {
					light[i, 0].color = Color.yellow;
					light[i, 1].color = Color.yellow;
				} else {
					light[i, 0].color = Color.white;
					light[i, 1].color = Color.white;
				}
			}
		} else {
			for (int i = 0; i < returnGears.Length; i++) {
				returnGears[i].Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime * (-1 * i % 2) + 0.5f) * 2);
			}
			for (int i = 0; i < 2; i++) {
				if (i == 1) {
					light[i, 0].color = Color.yellow;
					light[i, 1].color = Color.yellow;
				} else {
					light[i, 0].color = Color.white;
					light[i, 1].color = Color.white;
				}
			}

		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			isSelectingRetry = false;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			isSelectingRetry = true;
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			if (isSelectingRetry == true) {
				StageRetry();
			} else {
				StageSelect();
			}
		}
	}

	public void StageRetry() {
		PlayerPrefs.SetInt("stage", SystemManager.instance.stageNum);
		SceneManager.LoadScene("Game");
	}

	public void StageSelect() {
		if(PlayerPrefs.GetInt("isTutorial") == 1) {
			SceneManager.LoadScene("Title");
			return;
		}
		SceneManager.LoadScene("StageSelect");
	}


}