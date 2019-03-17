using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;

    private float moveSpeed = 1.0f;
	private float jumpSpeed = 2f;

	/// <summary>
	/// transformを使う際にはこれを使用すること
	/// </summary>
	private Transform thisTransform;

	//[HideInInspector]
	public bool isGimmickMode = false;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
		thisTransform = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update() {
		MovePlayer();
	}

	/// <summary>
	/// プレイヤーが死んだときの処理
	/// </summary>
	/// <param name="reason">死因</param>
	private void PlayerDead(string reason) {
		switch (reason) {
			default:
				//reasonをエラー文に書く
				Debug.LogError("エラー文");
				break;

				//他死因によって処理や描画を変える
		}

	}

	/// <summary>
	/// コントローラーとキーボード押したら移動
	/// </summary>
	private void MovePlayer() {

		if (Input.GetKey(KeyCode.D)) {
			if(isGimmickMode == false) {

			thisTransform.position += Vector3.right * moveSpeed * 10 * Time.deltaTime;
			}
			var rot = new Vector3(0, 0, -moveSpeed * 225 * Time.deltaTime);
			transform.Rotate(rot);

		}

		if (Input.GetKey(KeyCode.A) && isGimmickMode == false) {
			if (isGimmickMode == false) {

				thisTransform.position -= Vector3.right * moveSpeed * 10 * Time.deltaTime;
			}
			var rote = new Vector3(0, 0, moveSpeed * 225 * Time.deltaTime);
			transform.Rotate(rote);
		}

	}

	/// <summary>
	/// コントローラーとキーボード押したらジャンプ
	/// </summary>
	private void JumpPlayer() {

		//jumpSpeedを使用
	}


	private void ShiftGimmickMode() {
		//判定は当たり判定側などで取ること
		isGimmickMode = !isGimmickMode;
	}


	/// <summary>
	/// ゴールギミックを動作させたときに使う
	/// </summary>
	private void StageClear() {	
		
	}

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Trigger")) { 
			Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if(trigger.thisType == Trigger.TriggerType.Button) {
				trigger.isThisGimmick = true;
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				isGimmickMode = !isGimmickMode;
				trigger.isThisGimmick = true;
				trigger.mesh.enabled = !isGimmickMode;

				if (trigger.thisType == Trigger.TriggerType.Electrical ||
					trigger.thisType == Trigger.TriggerType.LeftGear ||
					trigger.thisType == Trigger.TriggerType.RighrtGear) {
					thisTransform.position = other.gameObject.transform.position;
				}
			}

		}

    }

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Trigger")) {
			isGimmickMode = false;
            other.gameObject.transform.parent.gameObject.GetComponent<Trigger>().isThisGimmick = false;

		}
    }

   
}
