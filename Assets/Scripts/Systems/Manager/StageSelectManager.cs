using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	public static StageSelectManager instance;

	public int selectingStageNum = 0;

	public GameObject hiddenStagePipe;
	public GameObject appearObjectParent;
	public GameObject gradation;

	public GameObject collectPartsObject;
	private GameObject[,] collectParts = new GameObject[5,2];

	public GameObject Door;
	public Transform doorParent;
	[HideInInspector]
	public GameObject[] doors = new GameObject[7];

	private bool isAnimation = false;
	private float animationTimer = 0;
	private const float ANIMATION_DOWN_MAX = 3;
	public GameObject Gradation;

	private bool isEndAnimation = false;
	private float endAnimationTimer = 0;
	private const float END_ANIMATION_MAX = 4;
	public GameObject endSign;
	private List<Transform> endSignGears = new List<Transform>();
	private bool isCrackerPoped = false;
	public GameObject crackerPaperA;
	public GameObject crackerPaperB;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		var collectpartsParent = new GameObject("CollectParts");
		collectpartsParent.transform.parent = transform;
		for(int i = 0; i < 5; i++) {
			for(int j = 0; j < 2; j++) {
				collectParts[i, j] = Instantiate(collectPartsObject, new Vector3(20 + j * 2, 5.6f + 9.4f * i, -1.8f), Quaternion.identity,collectpartsParent.transform) as GameObject;
				collectParts[i, j].SetActive(false);
			}
		}
		for(int i = 0; i < 5; i++) {
			doors[i] = Instantiate(Door, new Vector3(27,1.6f+9*i,1.8f), Quaternion.identity, doorParent);
		}
		doors[6] = Instantiate(Door, new Vector3(-18, 1.6f, 1.8f), Quaternion.identity, doorParent);

		if(CheckCollectpartsAll() == true) {
			if (PlayerPrefs.GetInt("freeAnimation",0) == 0) {
				isAnimation = true;
				CameraManager.instance.isFreeAnimation = true;
				PlayerPrefs.SetInt("freeAnimation", 1);
			} else {
				doors[5] = Instantiate(Door, new Vector3(27, -7, 1.8f), Quaternion.identity, doorParent);
				hiddenStagePipe.SetActive(false);
				appearObjectParent.SetActive(true);
				gradation.transform.position += Vector3.down * 15;
			}
		}
		PlayerPrefs.SetInt("isTutorial", 0);

		for(int i = 1; i < 6; i++) {
			if(PlayerPrefs.GetInt("Clear"+i.ToString(),0) == 0) {
				return;
			}
		}
		for(int i = 0; i < endSign.transform.GetChild(3).childCount; i++) {
			endSignGears.Add(endSign.transform.GetChild(3).GetChild(i));
		}
		isEndAnimation = true;
	}

	// Update is called once per frame
	void Update() {
		for(int i = 0;i <5; i++) {
			if(doors[i].transform.localRotation.y < 0) {
				doors[i].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
		}

		if (isAnimation == true && isEndAnimation == false) {
			animationTimer += Time.deltaTime;
			if (animationTimer < ANIMATION_DOWN_MAX) {
				Gradation.transform.position += Vector3.down * Time.deltaTime * 5;
			} else if (animationTimer > ANIMATION_DOWN_MAX + 0.5f) {
				doors[5] = Instantiate(Door, new Vector3(27, -7, 1.8f), Quaternion.identity, doorParent);
				hiddenStagePipe.SetActive(false);
				appearObjectParent.SetActive(true);
			}
			if (animationTimer > ANIMATION_DOWN_MAX + 1f){
				isAnimation = false;
				CameraManager.instance.isFreeAnimation = false;
			}
		}

		if(isEndAnimation == true && isAnimation == false) {
			endAnimationTimer += Time.deltaTime;
			if(endAnimationTimer < END_ANIMATION_MAX) {
				endSign.transform.position += Vector3.down * 5 * Time.deltaTime;
				for(int i = 0; i < endSignGears.Count; i++) {
					endSignGears[i].Rotate(0, 0, 100 * Time.deltaTime*((-1 * i % 2) + 0.5f) * 2);
				}
			} else if(endAnimationTimer < END_ANIMATION_MAX + 0.05f){
				endSign.transform.position += Vector3.down * 50 * Time.deltaTime;
			}else if(endAnimationTimer < END_ANIMATION_MAX + 5) {
				if (isCrackerPoped == false) {
					for(int i =  0; i < 200; i++) {
						Instantiate(crackerPaperA,CameraManager.instance.transform);
					}
					isCrackerPoped = true;
				}
			}else if(endAnimationTimer < END_ANIMATION_MAX * 2 + 5) {
				endSign.transform.position -= Vector3.down * 5 * Time.deltaTime;
			} else {
				isEndAnimation = false;
			}
		}

	}

	public void ChangeStage(int n) {
		//selectingStageNumのステージに移動
		selectingStageNum = n;
		PlayerPrefs.SetInt("stage", selectingStageNum);
		SceneManager.LoadScene("Game");
	}

	/// <summary>
	/// 全部持っているか
	/// </summary>
	/// <returns>全部持っていない</returns>
	private bool CheckCollectpartsAll() {
		bool isNotHaveCollectParts = true;
		for(int i = 1; i < 5+1; i++) {
			if(PlayerPrefs.GetInt(i.ToString() + "1",0) == 0) {
				isNotHaveCollectParts = false;
				collectParts[i-1, 0].SetActive(false);
			} else {
				collectParts[i-1, 0].SetActive(true);
			}
			if (PlayerPrefs.GetInt(i.ToString() + "2",0) == 0) {
				isNotHaveCollectParts = false;
				collectParts[i-1, 1].SetActive(false);

			} else {
				collectParts[i-1, 1].SetActive(true);

			}
		}
		return isNotHaveCollectParts;
	}

}
