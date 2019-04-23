using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{

    public enum ThingType {
        Default,
        Cube,
        ElectricalThing
    }

    public ThingType thisType = ThingType.Default;
    private bool isTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if (thisType == ThingType.Default) {
            Debug.Log("エラー文");
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Trigger")) {
            Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if (thisType == ThingType.Cube) {
				if (trigger.thisType == Trigger.TriggerType.Button || trigger.thisType == Trigger.TriggerType.MinusButton) {
					trigger.isThisGimmick = true;

				}
			}

			if (thisType == ThingType.ElectricalThing) {
				if (trigger.thisType == Trigger.TriggerType.Electrical) {
					trigger.isThisGimmick = true;
				}
			}
        }

    }

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Trigger")) {
			Trigger trigger = other.gameObject.transform.parent.gameObject.GetComponent<Trigger>();
			if (thisType == ThingType.Cube) {
				if (trigger.thisType == Trigger.TriggerType.Button || trigger.thisType == Trigger.TriggerType.MinusButton) {
					trigger.isThisGimmick = false;

				}
			}

			if (thisType == ThingType.ElectricalThing) {
				if (trigger.thisType == Trigger.TriggerType.Electrical) {
					trigger.isThisGimmick = false;
				}
			}
		}
	}
}
