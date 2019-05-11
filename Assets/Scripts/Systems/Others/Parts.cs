using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parts : MonoBehaviour {

	[HideInInspector]
	public string id = "";

    private Transform thisTransform;
    private Transform leftRotateAxis;
    private Transform rightRotateAxis;
    private Transform impulseObj;
    private GameObject Flame;
    private GameObject goalAnimation;
    private GameObject goalCanvas;
    private GameObject clearImage;
    private Vector3 impulseVector;
    private float thisFirstPosY;
    private float thisFirstPosX;
    private float thisFirstPosZ;
    private float clearFirstPosY;

    private float explosionCounter = 0;
    private bool explosionActive = false;

    private float changeSceneTime = 0;

    private bool isTrapAction = false;
    private bool isTrapActionStop = false;

    private float trapRotateCounter = 0;

    private float positionResetInterval = 0;
    private bool willPositionReset = false;

    private bool isImpulse = true;

    private float goalCounter = 0;
    private bool willGoal = false;

    private bool isMoveRight = false;
    private bool isMoveLeft = false;

    private bool isPlayerMoveRight = false;
    private bool isPlayerMoveLeft = false;

    private bool isGenelate = false;
    public enum PartsType {
        Default,
        Door,
        Bridge,
        Bomb,
        ChangeScene,
        MoveHorizontalObj,
        MoveVerticalObj,
        MoveDepthObj,
        Slope,
        Pitfall,        //落とし穴
        ObjFallTrap,   //落下してくるトラップ
        FlameThrower,  //炎が出るトラップ
        InterposeTrap,   //挟むトラップ
        ImpulseUp,
        ImpulseUp_2,
        ImpulseLeft,
        ImpulseRight,
        Goal
    }

    public PartsType thisType = PartsType.Default;

    
    private const float DOOR_UP_RANGE = 5;
    private const float DOOR_UP_SPEED = 0.8f;

    private const float BRIDGE_SPEED = 45;

	[HideInInspector]
    public float MOVE_HORIZONTAL_OBJ_RANGE = 50;
    //private const float MOVE_HORIZONTAL_OBJ_SPEED = 20;

	[HideInInspector]
    public float MOVE_VIRTICAL_OBJ_RANGE = 20;
    //private const float MOVE_VIRTICAL_OBJ_SPEED = 8;

	[HideInInspector]
    public float MOVE_DEPTH_OBJ_RANGE = 7;
    //private const float MOVE_DEPTH_OBJ_SPEED = 2;

    private const float SLOPE_UP_SPEED = 1;
    private const float SLOPE_SIDE_SPEED = 1;
    private const float SLOPE_SIDE_RANGE = 10;

    private const float TRAP_ROTATE_SPEED = 300;
    private const float TRAP_ROTATE_RANGE = 90;

    private const float FALL_SPEED = 15;
    private const float POSITION_RESET_INTERVAL = 1.5f;
    private const float POSITION_RESET_MOVE_SPEED = 2;

    private const float IMPULSE_UP_POWER = 22;
    private const float IMPULSE_UP_POWER_2 = 10;
    private const float IMPULSE_VIRTICAL_POWER = 85.0f;
    private const float IMPULSE_ACTION_SPEED = 17;
    private const float IMPULSE_ACTION_RESET_SPEED = 5;
    private const float IMPULSE_ACTION_RANGE = 0.2f;

    private const float GOAL = 3.8f;
    private const float CLEAR_IMAGE_RANGE = 100;
    private const float CLEAR_IMAGE_SPEED = 80;
    [HideInInspector]
    public int connectNumber = 0;

    // Start is called before the first frame update
    void Start() {
        thisTransform = gameObject.GetComponent<Transform>();
        thisFirstPosY = thisTransform.position.y;
        thisFirstPosX = thisTransform.position.x;
        thisFirstPosZ = thisTransform.position.z;

        switch (thisType) {
            case PartsType.Pitfall:
                leftRotateAxis = transform.GetChild(0).transform;
                rightRotateAxis = transform.GetChild(1).transform;
                break;

            case PartsType.FlameThrower:
                Flame = transform.GetChild(0).gameObject;
                break;

            case PartsType.InterposeTrap:
                leftRotateAxis = transform.GetChild(0).transform;
                rightRotateAxis = transform.GetChild(1).transform;
                break;

            case PartsType.ImpulseUp:
                impulseVector = new Vector3(0, IMPULSE_UP_POWER * Time.deltaTime, 0);
                impulseObj = transform.GetChild(0).transform;
                thisFirstPosY = impulseObj.position.y;
                break;
            case PartsType.ImpulseLeft:
                impulseObj = transform.GetChild(0).transform;
                thisFirstPosX = impulseObj.position.x;
                break;
            case PartsType.ImpulseRight:
                impulseObj = transform.GetChild(0).transform;
                thisFirstPosX = impulseObj.position.x;
                break;
            case PartsType.Goal:
                goalAnimation = (GameObject)Resources.Load("Prefabs/Systems/GameOvers/GoalAnimation");
                goalCanvas = transform.GetChild(0).gameObject;
                clearImage = goalCanvas.transform.GetChild(0).gameObject;
                break;
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

        if (thisType == PartsType.MoveHorizontalObj) {
            transform.position = new Vector3(thisFirstPosX + (SystemManager.instance.GetGimmickValue(connectNumber)+1) * MOVE_HORIZONTAL_OBJ_RANGE/2, thisTransform.position.y, thisTransform.position.z);

        }else if (thisType == PartsType.MoveVerticalObj) {
            transform.position = new Vector3(thisTransform.position.x, thisFirstPosY + (SystemManager.instance.GetGimmickValue(connectNumber)+1) * MOVE_VIRTICAL_OBJ_RANGE/2, thisTransform.position.z);

        } else if (thisType == PartsType.MoveDepthObj) {
            transform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, thisFirstPosZ + (SystemManager.instance.GetGimmickValue(connectNumber)+1) * MOVE_DEPTH_OBJ_RANGE/2);

        }

    }

    private void FixedUpdate() {
        //if (isMoveRight == true&& thisTransform.position.x <= thisFirstPosX + MOVE_HORIZONTAL_OBJ_RANGE) {
        //    thisTransform.Translate(MOVE_HORIZONTAL_OBJ_SPEED * Time.deltaTime, 0, 0);
        //    isMoveRight = false;

        //}
        //if (isMoveLeft == true&& thisTransform.position.x >= thisFirstPosX - MOVE_HORIZONTAL_OBJ_RANGE) {
        //    thisTransform.Translate(-MOVE_HORIZONTAL_OBJ_SPEED * Time.deltaTime, 0, 0);
        //    isMoveLeft = false;
        //}
    }

    /// <summary>
    /// 各パーツが+方向にプラス方向に動作する条件を満たしたときに呼び出される関数
    /// </summary>
    public void ActionPartsPlus() {
        switch (thisType) {
            case PartsType.Door:
                if (thisTransform.position.y <= thisFirstPosY + DOOR_UP_RANGE) {
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
                    Instantiate(Resources.Load("Prefabs/Systems/Clear"), SystemManager.instance.transform);
                    Destroy(this);
                    Destroy(Player.instance);
                }
                break;

            case PartsType.MoveHorizontalObj:
                if (thisTransform.position.x <= thisFirstPosX + MOVE_HORIZONTAL_OBJ_RANGE) {
                    isMoveRight = true;
                    isPlayerMoveRight = true;
                    isMoveLeft = false;

                } else {
                    isMoveRight = false;
                    isPlayerMoveRight = false;
                }
                break;

            case PartsType.MoveVerticalObj:
                if (thisTransform.position.y <= thisFirstPosY + MOVE_VIRTICAL_OBJ_RANGE) {
                    //thisTransform.Translate(0, MOVE_VIRTICAL_OBJ_SPEED * Time.deltaTime, 0);
                   // transform.position = new Vector3(thisTransform.position.x, thisFirstPosY + SystemManager.instance.GetGimmickValue(connectNumber) * MOVE_VIRTICAL_OBJ_RANGE, thisTransform.position.z);
                }
                break;

            case PartsType.MoveDepthObj:
                if (thisTransform.position.z <= thisFirstPosZ + MOVE_DEPTH_OBJ_RANGE) {
                    //transform.position = new Vector3(thisTransform.position.x, thisTransform.position.y, thisFirstPosZ + SystemManager.instance.GetGimmickValue(connectNumber) * MOVE_DEPTH_OBJ_RANGE);
                }
                break;

            case PartsType.Slope:
                if (thisTransform.position.x <= thisFirstPosX + SLOPE_SIDE_RANGE) {
                    thisTransform.Translate(SLOPE_SIDE_SPEED * Time.deltaTime, SLOPE_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Pitfall:
                isTrapActionStop = true;
                break;

            case PartsType.ObjFallTrap:
                isTrapActionStop = true;
                break;

            case PartsType.FlameThrower:
                isTrapAction = false;
                break;

            case PartsType.InterposeTrap:
                isTrapActionStop = true;
                break;

            case PartsType.ImpulseUp:
                isTrapActionStop = true;
                break;

            case PartsType.ImpulseUp_2:
                isTrapActionStop = true;
                break;

            case PartsType.ImpulseLeft:
                isTrapActionStop = true;
                break;

            case PartsType.ImpulseRight:
                isTrapActionStop = true;
                break;

            case PartsType.Goal:
                if (goalCounter<=GOAL-1.0f) {
                    clearImage.transform.position += new Vector3(0, -CLEAR_IMAGE_SPEED * Time.deltaTime, 0);
                }

                goalCounter += Time.deltaTime;
                Debug.Log(goalCounter);
                if (goalCounter >= GOAL) {
                    if (Player.instance.isCollectGets[0] == true) {
                        PlayerPrefs.SetInt(SystemManager.instance.stageNum + "1", 1);
                    }
                    if (Player.instance.isCollectGets[1] == true) {
                        PlayerPrefs.SetInt(SystemManager.instance.stageNum + "2", 1);
                    }
                    if (isGenelate == false) {
                        Instantiate(goalAnimation, thisTransform.position, Quaternion.identity);
                        isGenelate = true;
                    }
                    this.gameObject.SetActive(false);
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
                if (thisTransform.position.y >= thisFirstPosY) {
                    thisTransform.Translate(0, -DOOR_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Bridge:
                var rot = new Vector3(0, 0, -BRIDGE_SPEED * Time.deltaTime);
                transform.Rotate(rot);
                break;

            case PartsType.Bomb:
                //none
                break;

            case PartsType.ChangeScene:
                //none
                break;

            case PartsType.MoveHorizontalObj:
                if (thisTransform.position.x >= thisFirstPosX- MOVE_HORIZONTAL_OBJ_RANGE) {
                    isMoveLeft = true;
                    isPlayerMoveLeft = true;
                    isMoveRight = false;

                } else {
                    isMoveLeft = false;
                    isPlayerMoveLeft = false;
                }
                break;

            case PartsType.MoveVerticalObj:
                //if (thisTransform.position.y >= thisFirstPosY- MOVE_VIRTICAL_OBJ_RANGE) {
                //    thisTransform.Translate(0, -MOVE_VIRTICAL_OBJ_SPEED * Time.deltaTime, 0);
                //}
                break;

            case PartsType.MoveDepthObj:
                //if (thisTransform.position.z >= thisFirstPosZ- MOVE_DEPTH_OBJ_RANGE) {
                //    thisTransform.Translate(0, 0, -MOVE_DEPTH_OBJ_SPEED * Time.deltaTime);
                //}
                break;

            case PartsType.Slope:
                if (thisTransform.position.x >= thisFirstPosX) {
                    thisTransform.Translate(-SLOPE_SIDE_SPEED * Time.deltaTime, -SLOPE_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Pitfall:
                isTrapActionStop = false;
                break;

            case PartsType.ObjFallTrap:
                isTrapActionStop = false;
                break;

            case PartsType.FlameThrower:
                isTrapAction = true;
                break;

            case PartsType.InterposeTrap:
                isTrapActionStop = false;
                break;

            case PartsType.ImpulseUp:
                isTrapActionStop = false;
                break;

            case PartsType.ImpulseUp_2:
                isTrapActionStop = false;
                break;

            case PartsType.ImpulseLeft:
                isTrapActionStop = true;
                break;

            case PartsType.ImpulseRight:
                isTrapActionStop = true;
                break;
            case PartsType.Goal:
                if (goalCounter>=0) {
                    clearImage.transform.position += new Vector3(0, CLEAR_IMAGE_SPEED * Time.deltaTime, 0);
                }
                goalCounter -= Time.deltaTime;
                Debug.Log(goalCounter);
                if (goalCounter <= GOAL) {
                } else if (goalCounter >= GOAL) {
                    if (Player.instance.isCollectGets[0] == true) {
                        PlayerPrefs.SetInt(SystemManager.instance.stageNum + "1", 1);
                    }
                    if (Player.instance.isCollectGets[1] == true) {
                        PlayerPrefs.SetInt(SystemManager.instance.stageNum + "2", 1);
                    }
                    if (isGenelate == false) {
                        Instantiate(goalAnimation, thisTransform.position, Quaternion.identity);
                        isGenelate = true;
                    }
                    this.gameObject.SetActive(false);
                }
                break;
            default:

                break;

        }
    }

    public void TrapAction() {
        switch (thisType) {
            case PartsType.Pitfall:
                if (isTrapActionStop == false) {
                    if (trapRotateCounter <= TRAP_ROTATE_RANGE && isTrapAction == true) {
                        leftRotateAxis.Rotate(0, 0, -TRAP_ROTATE_SPEED * Time.deltaTime);
                        rightRotateAxis.Rotate(0, 0, TRAP_ROTATE_SPEED * Time.deltaTime);
                        trapRotateCounter += TRAP_ROTATE_SPEED * Time.deltaTime;

                    } else if (trapRotateCounter >= 5 && isTrapAction == false) {
                        leftRotateAxis.Rotate(0, 0, TRAP_ROTATE_SPEED * Time.deltaTime);
                        rightRotateAxis.Rotate(0, 0, -TRAP_ROTATE_SPEED * Time.deltaTime);
                        trapRotateCounter -= TRAP_ROTATE_SPEED * Time.deltaTime;

                    }
                }

                break;

            case PartsType.ObjFallTrap:
                if (isTrapActionStop == false) {
                    if (isTrapAction == true) {
                        thisTransform.Translate(0, -FALL_SPEED * Time.deltaTime, 0);
                        positionResetInterval = 0;

                    } else if (willPositionReset == true) {
                        if (positionResetInterval >= POSITION_RESET_INTERVAL) {
                            if (thisFirstPosY <= thisTransform.position.y) {
                                willPositionReset = false;
                            }
                            thisTransform.Translate(0, POSITION_RESET_MOVE_SPEED * Time.deltaTime, 0);
                        }
                        positionResetInterval += Time.deltaTime;
                    }

                }
                break;

            case PartsType.FlameThrower:
                Flame.SetActive(!isTrapAction);
                break;

            case PartsType.InterposeTrap:
                if (isTrapActionStop == false) {
                    if (trapRotateCounter <= TRAP_ROTATE_RANGE && isTrapAction == true) {
                        leftRotateAxis.Rotate(0, 0, -TRAP_ROTATE_SPEED * Time.deltaTime);
                        rightRotateAxis.Rotate(0, 0, TRAP_ROTATE_SPEED * Time.deltaTime);
                        trapRotateCounter += TRAP_ROTATE_SPEED * Time.deltaTime;

                    } else if (trapRotateCounter >= 5 && isTrapAction == false) {
                        leftRotateAxis.Rotate(0, 0, TRAP_ROTATE_SPEED * Time.deltaTime);
                        rightRotateAxis.Rotate(0, 0, -TRAP_ROTATE_SPEED * Time.deltaTime);
                        trapRotateCounter -= TRAP_ROTATE_SPEED * Time.deltaTime;

                    }
                }
                break;

            case PartsType.ImpulseUp:
                if (isTrapAction == true) {

                    if (isImpulse == true && willPositionReset == false) {
                        Player.instance.rigidbody.velocity = new Vector3(0, IMPULSE_UP_POWER, 0);
                        //Player.instance.rigidbody.AddForce(impulseVector, ForceMode.VelocityChange);
                        isImpulse = false;
                    }
                    if (impulseObj.position.y <= thisFirstPosY + IMPULSE_ACTION_RANGE && willPositionReset == false) {
                        impulseObj.Translate(0, IMPULSE_ACTION_SPEED * Time.deltaTime, 0);

                        if (impulseObj.position.y >= thisFirstPosY + IMPULSE_ACTION_RANGE) {
                            willPositionReset = true;
                        }
                    }
                    if (willPositionReset == true) {
                        if (positionResetInterval >= POSITION_RESET_INTERVAL) {
                            impulseObj.Translate(0, -IMPULSE_ACTION_RESET_SPEED * Time.deltaTime, 0);

                        }
                        if (impulseObj.position.y <= thisFirstPosY) {
                            willPositionReset = false;
                            isTrapAction = false;
                            isImpulse = true;
                            positionResetInterval = 0;
                        }
                        positionResetInterval += Time.deltaTime;
                    }

                }
                break;

            case PartsType.ImpulseUp_2:
                if (isTrapAction == true)
                {

                    if (isImpulse == true && willPositionReset == false)
                    {
                        Player.instance.rigidbody.velocity = new Vector3(0, IMPULSE_UP_POWER_2, 0);
                        //Player.instance.rigidbody.AddForce(impulseVector, ForceMode.VelocityChange);
                        isImpulse = false;
                    }
                    if (impulseObj.position.y <= thisFirstPosY + IMPULSE_ACTION_RANGE && willPositionReset == false)
                    {
                        impulseObj.Translate(0, IMPULSE_ACTION_SPEED * Time.deltaTime, 0);

                        if (impulseObj.position.y >= thisFirstPosY + IMPULSE_ACTION_RANGE)
                        {
                            willPositionReset = true;
                        }
                    }
                    if (willPositionReset == true)
                    {
                        if (positionResetInterval >= POSITION_RESET_INTERVAL)
                        {
                            impulseObj.Translate(0, -IMPULSE_ACTION_RESET_SPEED * Time.deltaTime, 0);

                        }
                        if (impulseObj.position.y <= thisFirstPosY)
                        {
                            willPositionReset = false;
                            isTrapAction = false;
                            isImpulse = true;
                            positionResetInterval = 0;
                        }
                        positionResetInterval += Time.deltaTime;
                    }

                }
                break;

            case PartsType.ImpulseLeft:
                if (isTrapAction == true) {

                    Debug.Log(isImpulse + ":" + willPositionReset);
                    if (isImpulse == true && willPositionReset == false) {
                        Player.instance.springSpeed = -IMPULSE_VIRTICAL_POWER * Time.deltaTime;
                        isImpulse = false;
                    }
                    if (impulseObj.position.x >= thisFirstPosX - IMPULSE_ACTION_RANGE && willPositionReset == false) {
                        impulseObj.Translate(-IMPULSE_ACTION_SPEED * Time.deltaTime, 0, 0);

                        if (impulseObj.position.x <= thisFirstPosX - IMPULSE_ACTION_RANGE) {
                            willPositionReset = true;
                        }
                    }
                    if (willPositionReset == true) {
                        if (positionResetInterval >= POSITION_RESET_INTERVAL) {
                            impulseObj.Translate(IMPULSE_ACTION_RESET_SPEED * Time.deltaTime, 0, 0);
                        }
                        if (impulseObj.position.x >= thisFirstPosX) {
                            willPositionReset = false;
                            isTrapAction = false;
                            isImpulse = true;
                            positionResetInterval = 0;
                        }
                        positionResetInterval += Time.deltaTime;
                    }
                    Debug.Log(isImpulse + ":" + willPositionReset);
                }
                break;

            case PartsType.ImpulseRight:
                if (isTrapAction == true) {

                    Debug.Log(isImpulse + ":" + willPositionReset);
                    if (isImpulse == true && willPositionReset == false) {
                        Player.instance.springSpeed = IMPULSE_VIRTICAL_POWER*Time.deltaTime;
                        isImpulse = false;
                    }
                    if (impulseObj.position.x <= thisFirstPosX + IMPULSE_ACTION_RANGE && willPositionReset == false) {
                        impulseObj.Translate(IMPULSE_ACTION_SPEED * Time.deltaTime, 0, 0);

                        if (impulseObj.position.x >= thisFirstPosX + IMPULSE_ACTION_RANGE) {
                            willPositionReset = true;
                        }
                    }
                    if (willPositionReset == true) {
                        if (positionResetInterval >= POSITION_RESET_INTERVAL) {
                            impulseObj.Translate(-IMPULSE_ACTION_RESET_SPEED * Time.deltaTime, 0, 0);
                        }
                        if (impulseObj.position.x <= thisFirstPosX) {
                            willPositionReset = false;
                            isTrapAction = false;
                            isImpulse = true;
                            positionResetInterval = 0;
                        }
                        positionResetInterval += Time.deltaTime;
                    }

                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player") {
            switch (thisType) {
                case PartsType.ObjFallTrap:
                    willPositionReset = true;
                    isTrapAction = false;
                    break;
            }

        }

    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            switch (thisType) {
                case PartsType.ImpulseUp:
                    if (isTrapAction == true) {
                        isImpulse = true;
                    }
                    if (willPositionReset == false && isTrapActionStop == false) {
                        isTrapAction = true;
                    }
                    break;

                case PartsType.ImpulseUp_2:
                    if (isTrapAction == true)
                    {
                        isImpulse = true;
                    }
                    if (willPositionReset == false && isTrapActionStop == false)
                    {
                        isTrapAction = true;
                    }
                    break;

                case PartsType.ImpulseLeft:
                    if (isTrapAction == true) {
                        isImpulse = true;
                    }
                    break;

                case PartsType.ImpulseRight:
                    if (isTrapAction == true) {
                        isImpulse = true;
                    }
                    break;

                case PartsType.MoveHorizontalObj:
                    Player.instance.CollisionMove(isPlayerMoveRight, isPlayerMoveLeft);
                    isPlayerMoveLeft = false;
                    isPlayerMoveRight = false;
                    break;

            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            switch (thisType) {
                case PartsType.Pitfall:
                    isTrapAction = true;
                    break;

                case PartsType.InterposeTrap:
                    isTrapAction = true;
                    break;

                case PartsType.ImpulseUp:
                    if (willPositionReset == false && isTrapActionStop == false) {
                        isTrapAction = true;
                    }
                    break;

                case PartsType.ImpulseUp_2:
                    if (willPositionReset == false && isTrapActionStop == false)
                    {
                        isTrapAction = true;
                    }
                    break;

                case PartsType.ImpulseLeft:
                    if (willPositionReset == false && isTrapActionStop == false) {
                        isTrapAction = true;
                    }
                    break;

                case PartsType.ImpulseRight:
                    if (willPositionReset == false && isTrapActionStop == false) {
                        isTrapAction = true;
                    }
                    break;


            }
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            switch (thisType) {
                case PartsType.Pitfall:
                    isTrapAction = false;
                    break;

                case PartsType.InterposeTrap:
                    isTrapAction = false;
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            switch (thisType) {
                case PartsType.ObjFallTrap:
                    if (willPositionReset == false) {
                        isTrapAction = true;
                    }
                    break;


            }
        }
    }

}
