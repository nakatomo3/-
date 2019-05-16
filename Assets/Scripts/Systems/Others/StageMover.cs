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

	// Start is called before the first frame update
	void Start() {

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

			Player.instance.gameObject.transform.position = transform.position;
			mesh.enabled = false;
		} else {
			mesh.enabled = true;
		}
		if(timer < 0) {
			timer = 0;
		}

		if(timer >= OPENING_TIME) {
			StageSelectManager.instance.ChangeStage(stageNum);
			Player.instance.isGimmickMode = false;
			Player.instance.rigidbody.useGravity = true;
			SceneManager.LoadScene("Game");
		}

	}
}
