using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour {

    private Transform thisTransform;
    private float doorFirstPosY;

    public enum PartsType {
		Default,
		Door,
		Bridge,
	}
	[HideInInspector]
	public PartsType thisType = PartsType.Default;

	private const float DOOR_UP_VALUE = 5;
	private const float DOOR_UP_SPEED = 0.8f;

	private const float BRIDGE_SPEED = 20;

	// Start is called before the first frame update
	void Start() {
        thisTransform = gameObject.GetComponent<Transform>();
        doorFirstPosY = thisTransform.position.y;
        if (thisType == PartsType.Default) {
			Debug.Log("エラー文");
		}
	}

	// Update is called once per frame
	void Update() {
		
	}


	/// <summary>
	/// 各パーツが動作する条件を満たしたときに呼び出される関数
	/// </summary>
	public void ActionParts() {
        switch (thisType) {
            case PartsType.Door:
                if (thisTransform.position.y <= doorFirstPosY + DOOR_UP_VALUE) {
                    thisTransform.Translate(0, DOOR_UP_SPEED * Time.deltaTime, 0);
                }
                break;

            case PartsType.Bridge:
                if (thisTransform.rotation.eulerAngles.z <= 85) {
                    var rot = new Vector3(0, 0, BRIDGE_SPEED * Time.deltaTime);
                    transform.Rotate(rot);
                }
                break;

            default:
               
                break;

        }
	}
}
