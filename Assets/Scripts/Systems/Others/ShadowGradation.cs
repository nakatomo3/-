using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowGradation : MonoBehaviour
{

	private Transform thisTransform;

    // Start is called before the first frame update
    void Start()
    {
		thisTransform = transform;   
    }

    // Update is called once per frame
    void Update()
    {
		thisTransform.position = new Vector3(CameraManager.instance.transform.position.x, CameraManager.instance.transform.position.y, 3);

		thisTransform.localScale = new Vector3(0.16f*CameraManager.instance.transform.position.z/-30,0.09f * CameraManager.instance.transform.position.z / -30, 1);
    }
}
