using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parts : MonoBehaviour {

    private Transform thisTransform;
    private float doorFirstPosY;
    private float floorFirstPosX;

    private float explosionCounter = 0;
    private bool explosionActive = false;

    private float changeSceneTime = 0;

    public enum PartsType {
		Default,
		Door,
		Bridge,
        Bomb,
        ChangeScene,
        MoveFordBackFloor,
    }
	[HideInInspector]
	public PartsType thisType = PartsType.Default;

	private const float DOOR_UP_RANGE = 5;
	private const float DOOR_UP_SPEED = 0.8f;

	private const float BRIDGE_SPEED = 20;

    private const float FLOOR_FRONT_RANGE = 7;
    private const float FLOOR_MOVE_FRONT_SPEED = 2;

	// Start is called before the first frame update
	void Start() {
        thisTransform = gameObject.GetComponent<Transform>();
        doorFirstPosY = thisTransform.position.y;
        floorFirstPosX = thisTransform.position.x;
        if (thisType == PartsType.Default) {
			Debug.Log("エラー文");
		}
	}

	// Update is called once per frame
	void Update() {

        //一定時間爆弾の爆発が残る
        if (explosionActive == true) {
            if (explosionCounter >= 0.5f) {

                foreach (Transform child in transform) {
                    child.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                    explosionActive = false;
                }
            }
            explosionCounter += 1 * Time.deltaTime;
        }
    }


	/// <summary>
	/// 各パーツが+方向にプラス方向に動作する条件を満たしたときに呼び出される関数
	/// </summary>
	public void ActionPartsPlus() {
        switch (thisType) {
            case PartsType.Door:
                if (thisTransform.position.y <= doorFirstPosY + DOOR_UP_RANGE) {
                    thisTransform.Translate(0, DOOR_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Bridge:
                if (thisTransform.rotation.eulerAngles.z <= 90) {
                    var rot = new Vector3(0, 0, BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rot);
                }
                break;

            case PartsType.Bomb:
                foreach (Transform child in transform) {
                    child.gameObject.SetActive(true);
                    explosionActive = true;
                }
                break;

            case PartsType.ChangeScene:
                changeSceneTime += Time.deltaTime;
                
                if (changeSceneTime >= 1.0) {
					Instantiate(Resources.Load("Prefabs/Systems/Clear"),SystemManager.instance.transform);
					Destroy(this);
					Destroy(Player.instance);
                }
                break;

            case PartsType.MoveFordBackFloor:
                if (thisTransform.position.x <= floorFirstPosX + FLOOR_FRONT_RANGE) {
                    thisTransform.Translate(FLOOR_MOVE_FRONT_SPEED * Time.deltaTime, 0, 0);
                }
                break;

            default:
               
                break;

        }
	}

	/// <summary>
	/// パーツがマイナス方向に動作するときの関数
	/// もしなかったらその旨をコメントに残しておくこと
	/// </summary>
	public void ActionPartsMinus() {
		switch (thisType) {
			case PartsType.Door:
                if (thisTransform.position.y >= doorFirstPosY) {
                    thisTransform.Translate(0, -DOOR_UP_SPEED * Time.deltaTime, 0);
                }
                break;

			case PartsType.Bridge:
                if (thisTransform.rotation.eulerAngles.z >= -90) {
                    var rot = new Vector3(0, 0, -BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rot);
                }
                break;

			case PartsType.Bomb:
                //none
				break;

            case PartsType.ChangeScene:
               //none
                break;

            case PartsType.MoveFordBackFloor:
                //if (thisTransform.position.x >= floorFirstPosX - FLOOR_FRONT_RANGE) {
                //    thisTransform.Translate(-FLOOR_MOVE_FRONT_SPEED * Time.deltaTime, 0, 0);
                //}
                break;

            default:

				break;

		}
	}
}
