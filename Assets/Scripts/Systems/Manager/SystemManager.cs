using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.SceneManagement;

//SystemManagerというよりゲーム部分のmanager
public class SystemManager : MonoBehaviour {

	public static SystemManager instance;

	/// <summary>
	/// 現在遊んでいるステージのナンバー
	/// </summary>
	public int stageNum { get; private set; }

	private struct Gimmick {
		public List<Trigger> triggers;
		public List<Parts> parts;
		public float value;
	}
	private Dictionary<int, Gimmick> gimmicks = new Dictionary<int, Gimmick>();

	private float width;
	private float height;

	private void Awake() {
		stageNum = PlayerPrefs.GetInt("stage");
		instance = this;
		DestroyStage();
		CreateStage();
	}

	void Start() {
		//OptionManager.instance.ChangeScreenSize(800,450);
	}

	private void OnEnable() {

	}

	void Update() {


    }

	/// <summary>
	/// ステージナンバーを変更する
	/// </summary>
	public void ChangeStageNum(int num) {
		stageNum = num;
	}

	/// <summary>
	/// ポーズ時の処理
	/// </summary>
	private void Pause() {
	 
	}

	/// <summary>
	/// ステージを読み込んで生成する
	/// </summary>
	private void CreateStage() {
		TextAsset xmlTextAsset;
		XmlDocument xmlDoc = new XmlDocument();

		//XMLの読み込み
		try {
			xmlTextAsset = Instantiate(Resources.Load("XMLs/Stages/"+stageNum.ToString())) as TextAsset;
			xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlTextAsset.text);
		} catch {
			Debug.LogError("XMLs/Stages/" + stageNum.ToString() + ".xmlが存在していないか、正しく読み込めません。");
			return;
		}

		//高さ横幅の読み込み
		try {
			width = int.Parse(xmlDoc.GetElementsByTagName("Width").Item(0).InnerText);
			height = int.Parse(xmlDoc.GetElementsByTagName("Height").Item(0).InnerText);
		} catch {
			Debug.LogError(stageNum+".xmlにWidth,Heightタグ内に値を記入してください");
			return;
		}


		var collectParts = Resources.Load("Prefabs/Systems/CollectParts") as GameObject;

		try {
			var collect1 = xmlDoc.GetElementsByTagName("Collect1");
			Debug.Log(collect1.Item(0).ChildNodes.Item(0).InnerText);

			var pos = new Vector3(float.Parse(collect1.Item(0).ChildNodes.Item(0).InnerText), float.Parse(collect1.Item(0).ChildNodes.Item(0).InnerText), 0);
			var newCollectParts = Instantiate(collectParts, pos, Quaternion.identity, transform);
			newCollectParts.name = "1";

			var collect2 = xmlDoc.GetElementsByTagName("Collect2");
			pos = new Vector3(float.Parse(collect2.Item(0).ChildNodes.Item(0).InnerText), float.Parse(collect2.Item(0).ChildNodes.Item(0).InnerText), 0);
			newCollectParts = Instantiate(collectParts, pos, Quaternion.identity, transform);
			newCollectParts.name = "2";
		} catch {
			Debug.LogError("CollectPartsが指定されていません");
		}

		//トリガーの追加
		var triggers = xmlDoc.GetElementsByTagName("Trigger");
		for(int i = 0; i < triggers.Count; i++) {
			int connectNum = int.Parse(triggers.Item(i).ChildNodes.Item(0).InnerText);
			string name = triggers.Item(i).ChildNodes.Item(1).InnerText;
			float x, y;
			x = float.Parse(triggers.Item(i).ChildNodes.Item(2).InnerText);
			y = float.Parse(triggers.Item(i).ChildNodes.Item(3).InnerText);

			GameObject triggerObject = new GameObject();
			Trigger.TriggerType triggerType = Trigger.TriggerType.Default;
			switch (name) {
				default:
					Debug.LogError("設定されていないトリガーが選ばれました\n名前："+name);
					continue;

				case "RightGear":
					triggerObject = Resources.Load("Prefabs/Triggers/RightGear") as GameObject;
					triggerType = Trigger.TriggerType.RightGear;
					break;
				case "LeftGear":
					triggerObject = Resources.Load("Prefabs/Triggers/LeftGear") as GameObject;
					triggerType = Trigger.TriggerType.LeftGear;
					break;
				case "Button":
					triggerObject = Resources.Load("Prefabs/Triggers/PlusButton") as GameObject;
					triggerType = Trigger.TriggerType.Button;
					break;
                case "MinusButton":
                    triggerObject = Resources.Load("Prefabs/Triggers/MinusButton") as GameObject;
                    triggerType = Trigger.TriggerType.MinusButton;
                    break;
                case "Electrical":
					triggerObject = Resources.Load("Prefabs/Triggers/Electrical") as GameObject;
					triggerType = Trigger.TriggerType.Electrical;
					break;
				case "Forever":
					triggerObject = Resources.Load("Prefabs/Triggers/ForeverButton") as GameObject;
					triggerType = Trigger.TriggerType.Forever;
					break;
			}
			var newTriggerObject = Instantiate(triggerObject, new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
			Trigger trigger = newTriggerObject.GetComponent<Trigger>();
			if (triggers.Item(i).ChildNodes.Count > 4) {
				var rotate = float.Parse(triggers.Item(i).ChildNodes.Item(4).InnerText);
				newTriggerObject.transform.Rotate(0, 0, rotate);
			}

			trigger.thisType = triggerType;
			trigger.connectNum = connectNum;
			if (gimmicks.ContainsKey(connectNum)) {
				gimmicks[connectNum].triggers.Add(trigger);
			} else {
				Gimmick newGimmick = new Gimmick();
				newGimmick.triggers = new List<Trigger>();
				newGimmick.parts = new List<Parts>();
				newGimmick.triggers.Add(trigger);
				newGimmick.value = -1;
				gimmicks.Add(connectNum, newGimmick);
			}
		}


		//パーツの追加
		var parts = xmlDoc.GetElementsByTagName("Parts");
		for(int i = 0; i < parts.Count; i++) {
			int connectNum = int.Parse(parts.Item(i).ChildNodes.Item(0).InnerText);
			if (gimmicks.ContainsKey(connectNum)) {
				string name = parts.Item(i).ChildNodes.Item(1).InnerText;
				float x, y;
				x = float.Parse(parts.Item(i).ChildNodes.Item(2).InnerText);
				y = float.Parse(parts.Item(i).ChildNodes.Item(3).InnerText);

				GameObject partsObject = new GameObject();
				Parts.PartsType partsType = Parts.PartsType.Default;
				switch (name) {
					default:
						Debug.LogError("設定されていないパーツが選ばれました\n名前："+name);
						continue;

					case "Door":
						partsObject = Resources.Load("Prefabs/Parts/Door") as GameObject;
						partsType = Parts.PartsType.Door;
						//他のパーツ
						//partsObject = hoge;
						//partsType = hoge;
						break;

                    case "Bridge":
                        partsObject = Resources.Load("Prefabs/Parts/Bridge") as GameObject;
                        partsType = Parts.PartsType.Bridge;
                        break;

                    case "Bomb":
                        partsObject = Resources.Load("Prefabs/Parts/Bomb") as GameObject;
                        partsType = Parts.PartsType.Bomb;
                        break;

                    case "ChangeScene":
                        partsObject = Resources.Load("Prefabs/Parts/ChangeScene") as GameObject;
                        partsType = Parts.PartsType.ChangeScene;
                        break;

                    case "MoveHorizontalObj":
                        partsObject = Resources.Load("Prefabs/Parts/MoveHorizontalObj") as GameObject;
                        partsType = Parts.PartsType.MoveHorizontalObj;
                        break;

                    case "MoveVerticalObj":
                        partsObject = Resources.Load("Prefabs/Parts/MoveVerticalObj") as GameObject;
                        partsType = Parts.PartsType.MoveVerticalObj;
                        break;

                    case "MoveDepthObj":
                        partsObject = Resources.Load("Prefabs/Parts/MoveDepthObj") as GameObject;
                        partsType = Parts.PartsType.MoveDepthObj;
                        break;

                    case "Slope":
                        partsObject = Resources.Load("Prefabs/Parts/Slope") as GameObject;
                        partsType = Parts.PartsType.Slope;
                        break;

                    case "Pitfall":
                        partsObject = Resources.Load("Prefabs/Parts/Pitfall") as GameObject;
                        partsType = Parts.PartsType.Pitfall;
                        break;

                    case "ObjFallTrap":
                        partsObject = Resources.Load("Prefabs/Parts/ObjFallTrap") as GameObject;
                        partsType = Parts.PartsType.ObjFallTrap;
                        break;

                    case "FlameThrower":
                        partsObject = Resources.Load("Prefabs/Parts/FlameThrower") as GameObject;
                        partsType = Parts.PartsType.FlameThrower;
                        break;

                    case "InterposeTrap":
                        partsObject = Resources.Load("Prefabs/Parts/InterposeTrap") as GameObject;
                        partsType = Parts.PartsType.InterposeTrap;
                        break;

                    case "ImpulseUp":
                        partsObject = Resources.Load("Prefabs/Parts/ImpulseUp") as GameObject;
                        partsType = Parts.PartsType.ImpulseUp;
                        break;

                    case "ImpulseLeft":
                        partsObject = Resources.Load("Prefabs/Parts/ImpulseLeft") as GameObject;
                        partsType = Parts.PartsType.ImpulseLeft;
                        break;

                    case "ImpulseRight":
                        partsObject = Resources.Load("Prefabs/Parts/ImpulseRight") as GameObject;
                        partsType = Parts.PartsType.ImpulseRight;
                        break;
                }
				var newPartsObject = Instantiate(partsObject, new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
				var newParts = newPartsObject.GetComponent<Parts>();
				if(parts.Item(i).ChildNodes.Count > 4) {
					var rotate = float.Parse(parts.Item(i).ChildNodes.Item(4).InnerText);
					newPartsObject.transform.Rotate(0, 0, rotate);
				}
				newParts.thisType = partsType;
				gimmicks[connectNum].parts.Add(newParts);

			} else {
				Debug.LogError(connectNum + "番のトリガーを登録してください");
				continue;
			}
		}

		var grounds = xmlDoc.GetElementsByTagName("Ground");
		var ground = Resources.Load("Prefabs/StageFrames/Ground") as GameObject;
		for (int i = 0; i < grounds.Count; i++) {
			float posX = float.Parse(grounds.Item(i).ChildNodes.Item(0).InnerText);
			float posY = float.Parse(grounds.Item(i).ChildNodes.Item(1).InnerText);
			float groundWidth = float.Parse(grounds.Item(i).ChildNodes.Item(2).InnerText);
			var groundObject = Instantiate(ground, new Vector3(posX, posY, 0), Quaternion.identity, transform);
			groundObject.transform.localScale = new Vector3(groundWidth, 1,2);
			if (grounds.Item(i).ChildNodes.Count > 4) {
				float rotate = float.Parse(grounds.Item(i).ChildNodes.Item(3).InnerText);
				groundObject.transform.Rotate(0, 0, rotate);
			}
		}

		var walls = xmlDoc.GetElementsByTagName("Wall");
		var wall = Resources.Load("Prefabs/StageFrames/Wall") as GameObject;
		for (int i = 0; i < walls.Count; i++) {
			float posX = float.Parse(walls.Item(i).ChildNodes.Item(0).InnerText);
			float posY = float.Parse(walls.Item(i).ChildNodes.Item(1).InnerText);
			float wallWidth = float.Parse(walls.Item(i).ChildNodes.Item(2).InnerText);
			var wallObj = Instantiate(wall, new Vector3(posX, posY, 0), Quaternion.identity, transform);
			wallObj.transform.localScale = new Vector3(wallWidth, 1, 2);
			wallObj.transform.Rotate(0, 0, 90);
			if (walls.Item(i).ChildNodes.Count > 4) {
				float rotate = float.Parse(walls.Item(i).ChildNodes.Item(3).InnerText);
				wallObj.transform.Rotate(0, 0, rotate);
			}
		}
		
		var missGround = Resources.Load("Prefabs/StageFrames/missGround") as GameObject;
		var player = Instantiate(Resources.Load("Prefabs/Systems/Player") as GameObject, new Vector3(int.Parse(xmlDoc.GetElementsByTagName("StartX").Item(0).InnerText), int.Parse(xmlDoc.GetElementsByTagName("StartY").Item(0).InnerText), 0), Quaternion.identity, transform);
		Instantiate(Resources.Load("Prefabs/Systems/Camera") as GameObject,new Vector3(int.Parse(xmlDoc.GetElementsByTagName("StartX").Item(0).InnerText), int.Parse(xmlDoc.GetElementsByTagName("StartY").Item(0).InnerText), -10), Quaternion.identity, transform);

		var wallObject = Instantiate(wall, new Vector3(0, height / 2, 0), Quaternion.identity, transform);
		wallObject.transform.localScale = new Vector3(1, height+1, 2);
		wallObject = Instantiate(wall, new Vector3(width, height / 2, 0), Quaternion.identity, transform);
		wallObject.transform.localScale = new Vector3(1, height+1, 2);

		var missGroundObject = Instantiate(missGround, new Vector3(width/2, 0, 0), Quaternion.identity, transform);
		missGroundObject.transform.localScale = new Vector3(width+1, 1, 2);
		missGroundObject = Instantiate(missGround, new Vector3(width / 2, height, 0), Quaternion.identity, transform);
		missGroundObject.transform.localScale = new Vector3(width+1, 1, 2);
		//Todo 背景オブジェクトの追加
	}

	/// <summary>
	/// Triggerがプラス方向に動作したときに全てのパーツを動かす関数
	/// </summary>
	/// <param name="num"></param>
	public void ActionGimmickPlus(int num) {
		foreach(var parts in gimmicks[num].parts) {
			parts.ActionPartsPlus();
		}
	}

	/// <summary>
	/// Triggerがマイナス方向に動作したときに全てのパーツを動かす関数
	/// </summary>
	/// <param name="num"></param>
	public void ActionGimmickMinus(int num) {
		foreach (var parts in gimmicks[num].parts) {
			parts.ActionPartsMinus();
		}
	}


	/// <summary>
	/// もしgimmicksに何かあった場合全て削除する関数
	/// </summary>
	private void DestroyStage() {
		//Destroy(Player.instance.gameObject);

		var keys = gimmicks.Keys;
		foreach(var key in keys) {
			foreach(Parts parts in gimmicks[key].parts) {
				Destroy(parts.gameObject);
				Debug.LogError("すでに生成されているステージの初期化ができていませんでした。報告してください");
			}
		}
		gimmicks.Clear();
	}

	public float GetGimmickValue(int num) {
		return gimmicks[num].value;
	}

	public void ChangeGimmickValue(bool isPlus, int num) {
		var gimmick = gimmicks[num];
		if (isPlus) {
			gimmick.value += Time.deltaTime/2;
		} else {
			gimmick.value -= Time.deltaTime/2;
		}
		gimmicks[num] = gimmick;
	}
}
