using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithDistance : MonoBehaviour
{
	public float hideDistance = 10f;
	public float farScale = 3.5f;
	public float nearScale = 9f;

	private TMPro.TextMeshProUGUI textComponent;

	void Awake()
    {
		textComponent = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
		if (textComponent == null) return;
		float cameraDistanceFromText = (Camera.main.transform.position - textComponent.transform.position).magnitude;
		SetTextVisibility(cameraDistanceFromText < hideDistance);
		UpdateTextScale(cameraDistanceFromText);
    }

	private void SetTextVisibility(bool isVisible) {
		textComponent.enabled = isVisible;
	}

	private void UpdateTextScale(float cameraDistanceFromText) {
		float fac = cameraDistanceFromText / hideDistance;
		float scale = Mathf.Lerp(nearScale, farScale, fac);
		textComponent.fontSize = scale * cameraDistanceFromText;
	}
}
