using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player instance;

	private bool isRight = true;

    //壊れた歯車
    private GameObject gearobject;


	[HideInInspector]
	public float moveSpeed { private set; get; } = MOVE_SPEED;
	private const float MOVE_SPEED = 10f;
	private float rotateSpeed = 225f;

	/// <summary>
	/// transformを使う際にはこれを使用すること
	/// </summary>
	private Transform thisTransform;
	public Transform visualTransform;
	public Transform underTransform;

	[HideInInspector]
	public bool isGimmickMode = false;
	[HideInInspector]
	public Rigidbody rigidbody;
	private Collider thisCollider;

	//[SerializeField]
	private bool isJumping;
	private float jumpRotateSpeed;
	private float jumpRange;
	private const float JUMP_RANGE_ADD_VALUE = 10f;
	private const float ROTATE_ADD_SPEED = 20f;

	public bool canRightMove = true;
	public bool canLeftMove = true;

    public bool[] isCollectGets = { false, false };

   // [HideInInspector]
    public float springSpeed = 0;
    private const float SPRING_COEFFICIENT = 0.9f;

	[SerializeField]
	public bool canMove = true;

    [HideInInspector]
    public bool wasGameOver = false;

    private void Awake() {
        instance = this;
		//Destroy(Camera.main.gameObject);
    }

    // Start is called before the first frame update
    void Start() {
		thisTransform = GetComponent<Transform>();
		rigidbody = gameObject.GetComponent<Rigidbody>();
		thisCollider = gameObject.GetComponent<Collider>();
	}

	// Update is called once per frame
	void Update() {
		if (canMove == true) {
			MovePlayer();
			JumpPlayer();

			springSpeed *= SPRING_COEFFICIENT;
			if (springSpeed < 0) {
				if (canLeftMove == true) {
					thisTransform.position += new Vector3(springSpeed, 0, 0);
				} else {
					springSpeed = 0;
				}
			}
			if (springSpeed > 0) {
				if (canLeftMove == true) {
					thisTransform.position += new Vector3(springSpeed, 0, 0);
				} else {
					springSpeed = 0;
				}
			}
		}
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        float distance = 1.5f;
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance)) {
            if (hit.collider.CompareTag("Ground")) {
                isJumping = false;
            }
        }
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
		if (Input.GetKey(KeyCode.W) && isJumping == false|| Input.GetButton("GamePadA")&& isJumping == false) {
			moveSpeed = MOVE_SPEED / 5;

		} else if (isJumping) {
			moveSpeed = MOVE_SPEED * 0.8f;
		} else { 
		
			moveSpeed = MOVE_SPEED;
		}

		bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("GamePadStickHolizontal") >0.3;
		bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("GamePadStickHolizontal") < -0.3;

		if (isRight) {
			MoveRight();
		}

		if (isLeft) {
			MoveLeft();
		}

		//if(canLeftMove == false) {
		//	transform.position += Vector3.right * 0.1f * Time.deltaTime;
		//}
		//if(canRightMove == false) {
		//	transform.position += Vector3.left * 0.1f * Time.deltaTime;
		//}
	}

	/// <summary>
	/// コントローラーとキーボード押したらジャンプ
	/// </summary>
	private void JumpPlayer() {
		if (Input.GetKeyDown(KeyCode.W) && isJumping != true|| Input.GetButtonDown("GamePadA") && isJumping != true) {
			jumpRange += 2f;
			jumpRotateSpeed = 0;
		}

		if (Input.GetKey(KeyCode.W) && isJumping != true|| Input.GetButton("GamePadA") && isJumping != true) {
			if (jumpRange < 10) {
				jumpRange += JUMP_RANGE_ADD_VALUE * Time.deltaTime;
				thisTransform.localScale -= new Vector3(0, 0.005f, 0f);
				visualTransform.position -= (visualTransform.position - underTransform.position) * 0.015f;
				jumpRotateSpeed += ROTATE_ADD_SPEED * Time.deltaTime;
			}

		}
		if (Input.GetKeyUp(KeyCode.W) || Input.GetButtonUp("GamePadA")) {
			if (isJumping != true) {
				rigidbody.velocity = new Vector3(0, jumpRange, 0);
			}
			thisTransform.localScale = new Vector3(1, 1, 1);
			visualTransform.localPosition = new Vector3(0, 0, 0);
			isJumping = true;
			jumpRange = 0;
			if (isRight == false) {
				jumpRotateSpeed *= -1;
			}
		}

		if (isJumping == true) {
			visualTransform.Rotate(0, -jumpRotateSpeed, 0);
			jumpRotateSpeed *= 0.98f;
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

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("Ground")) {
			rigidbody.AddForce(new Vector3(0, 1, 0));

			if (collision.gameObject.transform.position.y <= thisTransform.position.y) {
				isJumping = false;
			}
		}

         if (collision.gameObject.name == "ObjFallTrap(Clone)") {
            if (collision.gameObject.transform.position.y-1 > thisTransform.position.y) {
                wasGameOver = true;
               // Instantiate(gearobject, Player.instance.visualTransform.position, Quaternion.Euler(90, 0, 0)); //rigidbodyの空気抵抗を変えてアニメーションさせてる
                Instantiate(Resources.Load("Prefabs/Systems/GameOver") as GameObject, transform.position, Quaternion.identity);
                Instantiate(Resources.Load("Prefabs/Systems/BreakPlayer") as GameObject, transform.position, Quaternion.Euler(90, 0, 0));
                this.gameObject.SetActive(false);
            }
        }
		
	}

	private void OnCollisionExit(Collision collision) {
		//if (collision.gameObject.CompareTag("Wall")) {
		//	canLeftMove = true;
		//	canRightMove = true;
		//}
        if (collision.gameObject.CompareTag("Ground")) {
            isJumping = true;
			canLeftMove = true;
			canRightMove = true;
        }
		if (collision.gameObject.CompareTag("Wall")) {
			canRightMove = true;
			canLeftMove = true;
		}
	}

	private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            rigidbody.AddForce(new Vector3(0, 1, 0));

            if (collision.gameObject.transform.position.y <= thisTransform.position.y) {
                isJumping = false;
            }
        }

		if (collision.gameObject.CompareTag("Wall")) {
			if (collision.gameObject.transform.position.x >= thisTransform.position.x) {
				//右側に行けない
				canRightMove = false;
			} else {
				canLeftMove = false;
				//左側
			}
		}

		if (collision.gameObject.CompareTag("Ground") && collision.gameObject.name != "Bridge") {
			if(collision.gameObject.transform.position.y < thisTransform.position.y + 1 && collision.gameObject.transform.position.y > thisTransform.position.y - 1
				&& collision.gameObject.transform.localRotation == Quaternion.Euler(0,0,0)) {
				if (collision.gameObject.transform.position.x > thisTransform.position.x) {
					//右側に行けない
					canRightMove = false;
				} else {
					canLeftMove = false;
					//左側
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Trigger")) {
			Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if (trigger.thisType == Trigger.TriggerType.Forever) {
				trigger.isThisGimmick = !trigger.isThisGimmick;
			}
		}
	}

	private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Trigger")) {
			Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if(trigger.thisType == Trigger.TriggerType.Button|| trigger.thisType == Trigger.TriggerType.MinusButton) {
				trigger.isThisGimmick = true;

            }

			if (Input.GetKeyDown(KeyCode.Space)|| Input.GetButtonDown("GamePadB")) {

				if (trigger.thisType == Trigger.TriggerType.Electrical ||
					trigger.thisType == Trigger.TriggerType.LeftGear ||
					trigger.thisType == Trigger.TriggerType.RightGear) {

					trigger.isThisGimmick = true;
					isGimmickMode = !isGimmickMode;
				}
			}

		}

		if (other.gameObject.CompareTag("StageMover") || Input.GetButtonDown("GamePadB")) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				StageMover stageMover = other.transform.parent.gameObject.GetComponent<StageMover>();
				stageMover.isConnect = !stageMover.isConnect;
				rigidbody.useGravity = !rigidbody.useGravity;
				rigidbody.velocity = Vector3.zero;
				isGimmickMode = !isGimmickMode;
				thisTransform.position = other.gameObject.transform.position;
			}
		}

        //落下死
		if (other.gameObject.CompareTag("MissGround")) {
            wasGameOver = true;
            Instantiate(Resources.Load("Prefabs/Systems/GameOver") as GameObject, transform.position, Quaternion.identity);
			Destroy(this);
		}
        //圧死
        if (other.gameObject.CompareTag("SquashGround"))
        {
            Vector3 Gearb = GameObject.FindGameObjectWithTag("Player").transform.position;
            wasGameOver = true;
            Destroy(this.gameObject);

            Instantiate(gearobject, new Vector3(Gearb.x, Gearb.y, Gearb.z), Quaternion.identity);

            Instantiate(Resources.Load("Prefabs/Systems/GameOver") as GameObject, transform.position, Quaternion.identity);
           
        }
        //挟死
        if (other.gameObject.CompareTag("SandwichedGround"))
        {
            Vector3 Gearb = GameObject.FindGameObjectWithTag("Player").transform.position;
            wasGameOver = true;
            Destroy(this.gameObject);

            Instantiate(gearobject, new Vector3(Gearb.x, Gearb.y, Gearb.z), Quaternion.identity);

            Instantiate(Resources.Load("Prefabs/Systems/GameOver") as GameObject, transform.position, Quaternion.identity);
           
        }
        //爆死
        //出現方法は一緒で複数のパーツをそれぞれ座標をずらして出現させる回転も加えてもいいが時間がかかる
        if (other.gameObject.CompareTag("ExplosivedeathGround"))
        {
            Vector3 Gearb = GameObject.FindGameObjectWithTag("Player").transform.position;
            wasGameOver = true;
            Destroy(this.gameObject);

            Instantiate(gearobject, new Vector3(Gearb.x, Gearb.y, Gearb.z), Quaternion.identity);

            Instantiate(Resources.Load("Prefabs/Systems/GameOver") as GameObject, transform.position, Quaternion.identity);
            
        }
    }

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Trigger")) {
			Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if (trigger.thisType != Trigger.TriggerType.Forever) {
				trigger.isThisGimmick = false;
			}
				isGimmickMode = false;

		}
    }

	public void MoveRight() {
		if (isGimmickMode == false && canRightMove) {
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		}
		var rot = new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
		visualTransform.Rotate(rot);
		isRight = true;

		CameraManager.instance.MoveRight();
	}

	public void MoveLeft() {
		if (isGimmickMode == false && canLeftMove) {
			transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
		}
		var rot = new Vector3(0, rotateSpeed * Time.deltaTime, 0);
		visualTransform.Rotate(rot);
		isRight = false;

		CameraManager.instance.MoveLeft();
	}

    public void CollisionMove(bool isRight, bool isLeft) {
        if (isRight == true) {
            thisTransform.Translate(12 * Time.deltaTime, 0, 0);

        }else if (isLeft == true) {
            thisTransform.Translate(-12 * Time.deltaTime, 0, 0);
        }
        
    }

}