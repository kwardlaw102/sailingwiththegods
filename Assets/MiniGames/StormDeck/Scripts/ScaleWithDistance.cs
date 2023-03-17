using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScaleWithDistance : MonoBehaviour
{
	private TextMeshProUGUI textMesh;
	public float hideDistance = 10f;
	public float farScale = 3.5f;
	public float closeScale = 9f;

    void Start()
    {
		textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
		if (textMesh == null) return;

		float cameraDistanceFromText = (Camera.main.transform.position - textMesh.transform.position).magnitude;
		SetTextVisibility(cameraDistanceFromText < hideDistance);
		UpdateTextScale(cameraDistanceFromText);
    }

	private void SetTextVisibility(bool isVisible) {
		textMesh.enabled = isVisible;
	}

	private void UpdateTextScale(float cameraDistanceFromText) {
		float fac = cameraDistanceFromText / hideDistance;
		float scale = Mathf.Lerp(closeScale, farScale, fac);
		textMesh.fontSize = scale * cameraDistanceFromText;
	}
}
