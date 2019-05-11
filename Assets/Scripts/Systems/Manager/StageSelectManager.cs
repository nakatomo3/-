using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	public static StageSelectManager instance;

	public int selectingStageNum = 0;

	public GameObject hiddenStagePipe;

	public GameObject collectPartsObject;
	private GameObject[,] collectParts = new GameObject[5,2];

	public GameObject Door;
	public Transform doorParent;
	public GameObject[] doors = new GameObject[5];

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		for(int i = 0; i < 5; i++) {
			for(int j = 0; j < 2; j++) {
				collectParts[i, j] = Instantiate(collectPartsObject, new Vector3(21 + j * 3, 5.6f + 9.4f * i, -1.8f), Quaternion.identity) as GameObject;
			}
		}
		for(int i = 0; i < 5; i++) {
			doors[i] = Instantiate(Door, new Vector3(27,1.6f+9*i,1.8f), Quaternion.identity, doorParent);
		}
	}

	// Update is called once per frame
	void Update() {
		hiddenStagePipe.SetActive(!CheckCollectpartsAll());

		for(int i = 0;i <5; i++) {
			if(doors[i].transform.localRotation.y < 0) {
				doors[i].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
		}

	}

	public void ChangeStage(int n) {
		//selectingStageNumのステージに移動
		selectingStageNum = n;
		PlayerPrefs.SetInt("stage", selectingStageNum);
		SceneManager.LoadScene("Game");
	}

	private bool CheckCollectpartsAll() {
		bool isNotHaveCollectParts = true;
		for(int i = 1; i < 5+1; i++) {
			if(PlayerPrefs.GetInt(i.ToString() + 1) == 0) {
				isNotHaveCollectParts = false;
				collectParts[i-1, 0].SetActive(false);
			} else {
				collectParts[i-1, 0].SetActive(true);
			}
			if (PlayerPrefs.GetInt(i.ToString() + 2) == 0) {
				isNotHaveCollectParts = false;
				collectParts[i-1, 1].SetActive(false);

			} else {
				collectParts[i-1, 0].SetActive(true);

			}
		}
		return isNotHaveCollectParts;
	}

}
