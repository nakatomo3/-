using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMover : MonoBehaviour {

	public int stageNum;

	[HideInInspector]
	public float timer = 0;

	public MeshRenderer mesh;

	[HideInInspector]
	public bool isConnect = false;

	private const float OPENING_TIME = 3;

    private GameObject handle;
    private const float HANDLE_ROTATE_SPEED = 170;

    // Start is called before the first frame update
    void Start() {
        handle = transform.GetChild(1).gameObject;
	}

	// Update is called once per frame
	void Update() {
		if(isConnect == true) {
			if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				timer += Time.deltaTime;
				StageSelectManager.instance.doors[stageNum - 1].transform.Rotate(0, Time.deltaTime * 55, 0);
			}
			if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				if(timer > 0) {
					timer -= Time.deltaTime;
					StageSelectManager.instance.doors[stageNum - 1].transform.Rotate(0, Time.deltaTime * -55, 0);
				}
			}
            //ハンドルが回転するアニメーション
            if (Input.GetKey(KeyCode.D)) {
                handle.transform.Rotate(0, 0, HANDLE_ROTATE_SPEED * Time.deltaTime);

            }else if (Input.GetKey(KeyCode.A)) {
                handle.transform.Rotate(0, 0, -HANDLE_ROTATE_SPEED * Time.deltaTime);
            }

			Player.instance.gameObject.transform.position = transform.position;
			mesh.enabled = false;
		} else {
			mesh.enabled = true;
		}
		if(timer < 0) {
			timer = 0;
		}

		if(timer >= OPENING_TIME) {
			if (stageNum == 7) {
				SceneManager.LoadScene("StaffCredit");
			} else {
				StageSelectManager.instance.ChangeStage(stageNum);
				Player.instance.isGimmickMode = false;
				Player.instance.rigidbody.useGravity = true;
				SceneManager.LoadScene("Game");

			}
		}

	}
}
