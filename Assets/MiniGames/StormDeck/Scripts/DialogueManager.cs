using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	private TextMeshProUGUI textComponent;
	private Color textColor;
	private Coroutine activeCoroutine;

	public float fadeInSeconds = 1f;
	public float visibleSecondsBase = 2f;
	public float visibleSecondsPerCharacter = 0.01f;
	public float fadeOutSeconds = 1f;

	private static DialogueManager instance;

    private void Awake()
    {
		textComponent = GetComponent<TextMeshProUGUI>();
		instance = this;
	}

	private void Start() {
		HideText();
	}

	public static void DisplayText(string text) {
		instance.DisplayTextHelper(text);
	}

	private void DisplayTextHelper(string text) {
		HideText();
		textComponent.text = text;
		if (activeCoroutine != null) {
			StopCoroutine(activeCoroutine);
		}
		float visibleSeconds = visibleSecondsBase + text.Length * visibleSecondsPerCharacter;
		activeCoroutine = StartCoroutine(FadeCoroutine(fadeInSeconds, visibleSeconds, fadeOutSeconds));
	}

	private void HideText() {
		textColor = textComponent.color;
		textColor.a = 0;
		textComponent.color = textColor;
	}

	private IEnumerator FadeCoroutine(float fadeInSeconds, float visibleSeconds, float fadeOutSeconds) {
		textColor = textComponent.color;
		textColor.a = 0;

		// fade in
		while (true) {
			textColor.a += Time.deltaTime / fadeInSeconds;
			Debug.Log(textColor.a);
			if (textColor.a > 1) {
				textColor.a = 1;
				textComponent.color = textColor;
				break;
			}
			textComponent.color = textColor;
			
			yield return new WaitForEndOfFrame();
		}

		// remain visible
		float elapsedTime = 0f;
		while (elapsedTime < visibleSeconds) {
			Debug.Log(elapsedTime);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		// fade out
		while (true) {
			textColor.a -= Time.deltaTime / fadeInSeconds;
			Debug.Log(textColor.a);
			if (textColor.a < 0) {
				textColor.a = 0;
				textComponent.color = textColor;
				break;
			}
			textComponent.color = textColor;
			yield return new WaitForEndOfFrame();
		}
	}
}
