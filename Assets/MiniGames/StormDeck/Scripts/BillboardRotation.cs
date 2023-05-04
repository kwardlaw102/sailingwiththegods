using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotation : MonoBehaviour
{
	public bool yawOnly = true;

    void Update()
    {
		if (yawOnly) {
			transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		}
		else {
			transform.rotation = Camera.main.transform.rotation;
		}
    }
}
