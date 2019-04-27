using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour {

	public static StageSelectManager instance;

	public int selectingStageNum=0;

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}
        //#if UNITY_EDITOR
        //		if (Input.anyKey) {
        //			PlayerPrefs.SetInt("stage", selectingStageNum);
        //			SceneManager.LoadScene("Game");
        //		}
        //#endif


//        if (Input.GetMouseButtonDown(0)) {
//            Ray ray = new Ray();
//            RaycastHit hit = new RaycastHit();
//            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {

//                //ゲーム開始
//#if UNITY_EDITOR
//                if (hit.collider.gameObject.tag=="Game") {
//                    selectingStageNum = 1;
//                    PlayerPrefs.SetInt("stage", selectingStageNum);
//                    SceneManager.LoadScene("Game");
//                }
//#endif

//                //タイトルへ戻る
//                if (hit.collider.gameObject.tag=="Title") {
//                    SceneManager.LoadScene("Title");


//                }
//            }
//        }

    //}

	public void ChangeStage() {
		//selectingStageNumのステージに移動
		selectingStageNum = 1;
		PlayerPrefs.SetInt("stage", selectingStageNum);
		SceneManager.LoadScene("Game");
	}

    public void ChangeStage2() {
        selectingStageNum = 2;
        PlayerPrefs.SetInt("stage", selectingStageNum);
        SceneManager.LoadScene("Game");

    }

    public void ChangeStage3() {
        selectingStageNum = 3;
        PlayerPrefs.SetInt("stage", selectingStageNum);
        SceneManager.LoadScene("Game");
    }

    public void ChangeStage4() {
        selectingStageNum = 4;
        PlayerPrefs.SetInt("stage", selectingStageNum);
        SceneManager.LoadScene("Game");
    }
    public void ChangeStage5() {
        selectingStageNum = 5;
        PlayerPrefs.SetInt("stage", selectingStageNum);
        SceneManager.LoadScene("Game");
    }
}
