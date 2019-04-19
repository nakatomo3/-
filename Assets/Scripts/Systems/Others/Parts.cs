using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parts : MonoBehaviour {

    private Transform thisTransform;

    private float doorFirstPosY;
    private float floorFirstPosX;

    private Transform pitfallLeftAxis;
    private Transform pitfallRightAxis;
    private float objFirstPosY;
    private float objFirstPosX;
    private float objFirstPosZ;


    private float explosionCounter = 0;
    private bool explosionActive = false;

    private float changeSceneTime = 0;

    private float pitfallRotateCounter = 0;

    private bool trapAction = false;
    private bool trapActionStop = false;

    public enum PartsType {
		Default,
		Door,
		Bridge,
        Bomb,
        ChangeScene,
        MoveFordBackFloor,
        MoveHorizontalObj,
        MoveVerticalObj,
        MoveDepthObj,
        Slope,
        Pitfall    //＋トリガー動作で停止、-トリガー動作で動く
    }
	[HideInInspector]
	public PartsType thisType = PartsType.Default;

	private const float DOOR_UP_RANGE = 5;
	private const float DOOR_UP_SPEED = 0.8f;

	private const float BRIDGE_SPEED = 45;

    private const float MOVE_HORIZONTAL_OBJ_RANGE = 7;
    private const float MOVE_HORIZONTAL_OBJ_SPEED = 2;

    private const float MOVE_VIRTICAL_OBJ_RANGE = 7;
    private const float MOVE_VIRTICAL_OBJ_SPEED = 2;

    private const float MOVE_DEPTH_OBJ_RANGE = 7;
    private const float MOVE_DEPTH_OBJ_SPEED = 2;

    private const float SLOPE_UP_SPEED = 1;
    private const float SLOPE_SIDE_SPEED = 1;
    private const float SLOPE_SIDE_RANGE = 10;

    private const float PITFALL_ROTATE_SPEED = 300;
    private const float PITFALL_RANGE = 90;

    private const float FLOOR_FRONT_RANGE = 7;
    private const float FLOOR_MOVE_FRONT_SPEED = 2;

	// Start is called before the first frame update
	void Start() {
        thisTransform = gameObject.GetComponent<Transform>();
        doorFirstPosY = thisTransform.position.y;
        floorFirstPosX = thisTransform.position.x;
        objFirstPosY = thisTransform.position.y;
        objFirstPosX = thisTransform.position.x;
        objFirstPosZ = thisTransform.position.z;

        if (thisType == PartsType.Pitfall) {
            pitfallLeftAxis = transform.GetChild(0).transform;
            pitfallRightAxis = transform.GetChild(1).transform;
        }

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
        TrapAction();
    }


	/// <summary>
	/// 各パーツが+方向にプラス方向に動作する条件を満たしたときに呼び出される関数
	/// </summary>
	public void ActionPartsPlus() {
        switch (thisType) {
            case PartsType.Door:
                if (thisTransform.position.y <= objFirstPosY + DOOR_UP_RANGE) {
                    thisTransform.Translate(0, DOOR_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Bridge:
                
                    var rot = new Vector3(0, 0, BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rot);
                
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

            case PartsType.MoveHorizontalObj:
                if (thisTransform.position.x <= objFirstPosX + MOVE_HORIZONTAL_OBJ_RANGE) {
                    thisTransform.Translate(MOVE_HORIZONTAL_OBJ_SPEED * Time.deltaTime, 0, 0);
                }
                break;

            case PartsType.MoveVerticalObj:
                if (thisTransform.position.y <= objFirstPosY + MOVE_VIRTICAL_OBJ_RANGE) {
                    thisTransform.Translate(0, MOVE_VIRTICAL_OBJ_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.MoveDepthObj:
                if (thisTransform.position.z <= objFirstPosZ + MOVE_DEPTH_OBJ_RANGE) {
                    thisTransform.Translate(0,0, MOVE_DEPTH_OBJ_SPEED * Time.deltaTime);
                }
                break;

            case PartsType.Slope:
                if (thisTransform.position.x <= objFirstPosX + SLOPE_SIDE_RANGE) {
                    thisTransform.Translate(SLOPE_SIDE_SPEED * Time.deltaTime, SLOPE_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Pitfall:
                trapActionStop = true;
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
                if (thisTransform.position.y >= doorFirstPosY)
                {
                    if (thisTransform.position.y >= objFirstPosY)
                    {
                        thisTransform.Translate(0, -DOOR_UP_SPEED * Time.deltaTime, 0);
                    }
                    break;
                }
                break;
			case PartsType.Bridge:
                if (thisTransform.rotation.eulerAngles.z >= -90) {
                    var rotn = new Vector3(0, 0, -BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rotn);
                }
                
                    var rot = new Vector3(0, 0, -BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rot);
                
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
            case PartsType.MoveHorizontalObj:
				if (thisTransform.position.x >= objFirstPosX) {
					thisTransform.Translate(-MOVE_HORIZONTAL_OBJ_SPEED * Time.deltaTime, 0, 0);
				}
				break;

            case PartsType.MoveVerticalObj:
                if (thisTransform.position.y >= objFirstPosY) {
                    thisTransform.Translate(0, -MOVE_VIRTICAL_OBJ_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.MoveDepthObj:
                if (thisTransform.position.z >= objFirstPosZ) {
                    thisTransform.Translate(0, 0, -MOVE_DEPTH_OBJ_SPEED * Time.deltaTime);
                }
                break;

            case PartsType.Slope:
                if (thisTransform.position.x >= objFirstPosX) {
                    thisTransform.Translate(-SLOPE_SIDE_SPEED * Time.deltaTime, -SLOPE_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Pitfall:
                trapActionStop = false;
                break;

            default:

				break;

		}
	}

    public void TrapAction() {
        switch (thisType) {
            case PartsType.Pitfall:
                if (trapActionStop == false) {
                    if (pitfallRotateCounter <= PITFALL_RANGE && trapAction == true) {
                        pitfallLeftAxis.Rotate(0, 0, -PITFALL_ROTATE_SPEED * Time.deltaTime);
                        pitfallRightAxis.Rotate(0, 0, PITFALL_ROTATE_SPEED * Time.deltaTime);
                        pitfallRotateCounter += PITFALL_ROTATE_SPEED * Time.deltaTime;

                    } else if (pitfallRotateCounter >= 5 && trapAction == false) {
                        pitfallLeftAxis.Rotate(0, 0, PITFALL_ROTATE_SPEED * Time.deltaTime);
                        pitfallRightAxis.Rotate(0, 0, -PITFALL_ROTATE_SPEED * Time.deltaTime);
                        pitfallRotateCounter -= PITFALL_ROTATE_SPEED * Time.deltaTime;

                    }
                }

                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            trapAction = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            trapAction = false;
        }
    }
}
