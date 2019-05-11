using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
	public static GameOverManager instance;

    private bool isSelectingRetry = true;

    public Transform restartparent;
    public Transform returnParent;

    public Transform[] restartGears;
    public Transform[] returnGears;

    private float rotateSpeed = 0;

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

        if (isSelectingRetry)
        {
            for(int i = 0; i < restartGears.Length; i++)
            {
                restartGears[i].Rotate(new Vector3(0,0,100*Time.deltaTime*(-1 * i % 2) + 0.5f) * 2);
            }
        }
        else
        {
            for (int i = 0; i < returnGears.Length; i++)
            {
                returnGears[i].Rotate(new Vector3(0, 0, 100 * Time.deltaTime * (-1 * i % 2) + 0.5f) * 2);
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSelectingRetry = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isSelectingRetry = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(isSelectingRetry == true)
            {
                StageRetry();
            }
            else
            {
                StageSelect();
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