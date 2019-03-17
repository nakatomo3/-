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
		public Trigger trigger;
		public List<Parts> parts;
		public Gimmick(Trigger t) {
			trigger = t;
			parts = new List<Parts>();
		}
	}
	private Dictionary<int, Gimmick> gimmicks = new Dictionary<int, Gimmick>();


	private void Awake() {
		stageNum = PlayerPrefs.GetInt("stage");
		instance = this;
		DestroyStage();
		CreateStage();
	}

	// Start is called before the first frame update
	void Start() {
		//OptionManager.instance.ChangeScreenSize(800,450);
	}

	private void OnEnable() {

	}

	// Update is called once per frame
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

	private void CreateStage() {
		TextAsset xmlTextAsset;
		XmlDocument xmlDoc = new XmlDocument();

		try {
			xmlTextAsset = Instantiate(Resources.Load("XMLs/Stages/"+stageNum.ToString())) as TextAsset;
			xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlTextAsset.text);
		} catch {
			Debug.LogError("XMLs/Stages/" + stageNum.ToString() + ".xmlが存在していないか、正しく読み込めません。");
			return;
		}

		//トリガーの追加
		var triggers = xmlDoc.GetElementsByTagName("Trigger");
		for(int i = 0; i < triggers.Count; i++) {
			int connectNum = int.Parse(triggers.Item(i).ChildNodes.Item(0).InnerText);
			if(gimmicks.ContainsKey(connectNum) == true) {
				Debug.LogError(connectNum+"番はすでに登録されているconnectNumberです");
			}
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
					triggerType = Trigger.TriggerType.RighrtGear;
					break;
				//他のギミック
				//triggerObject = hoge;
				//triggerType = hoge;
				case "Button":
					triggerObject = Resources.Load("Prefabs/Triggers/ButtonTrigger") as GameObject;
					triggerType = Trigger.TriggerType.Button;
					break;
				case "Electrical":
					triggerObject = Resources.Load("Prefabs/Triggers/Electrical") as GameObject;
					triggerType = Trigger.TriggerType.Electrical;
					break;
			}
			var newTriggerObject = Instantiate(triggerObject, new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
			Gimmick newGimmick = new Gimmick(newTriggerObject.GetComponent<Trigger>());
			newGimmick.trigger.thisType = triggerType;
			newGimmick.trigger.connectNum = connectNum;
			gimmicks.Add(connectNum, newGimmick);
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
				}
				var newPartsObject = Instantiate(partsObject, new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
				var newParts = newPartsObject.GetComponent<Parts>();
				newParts.thisType = partsType;
				gimmicks[connectNum].parts.Add(newParts);

			} else {
				Debug.LogError(connectNum + "番のトリガーを登録してください");
				continue;
			}
		}

		var player = Instantiate(Resources.Load("Prefabs/Systems/Player") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, transform);

		//Todo 背景オブジェクトの追加
	}

	//Triggerが動作したときに全てのパーツを動かす関数
	public void ActionGimmick(int num) {
		foreach(var parts in gimmicks[num].parts) {
			parts.ActionParts();
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
			Destroy(gimmicks[key].trigger.gameObject);
		}
		gimmicks.Clear();
	}
}
