using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private float moveSpeed = 1.0f;
	private float jumpSpeed = 2f;

	/// <summary>
	/// transformを使う際にはこれを使用すること
	/// </summary>
	private Transform thisTransform;

	private bool isGimmickMode = false;

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

		//moveSpeedを使用
	}

	/// <summary>
	/// コントローラーとキーボード押したらジャンプ
	/// </summary>
	private void JumpPlayer() {

		//jumpSpeedを使用
	}

	private void ActionGimmick(string gimmickName) {
		switch (gimmickName) {
			default:
				Debug.LogError("エラー文"+gimmickName);
				break;
		}
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
}
