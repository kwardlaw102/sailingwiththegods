using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	[Header("Configuration")]
	public float fadeInSeconds = 1f;
	public float visibleSecondsBase = 2f;
	public float visibleSecondsPerCharacter = 0.01f;
	public float fadeOutSeconds = 1f;

	[Header("Object References")]
	[SerializeField] private TMPro.TextMeshProUGUI textComponent = null;

	public static NotificationManager instance { get; private set; }

    private void Awake()
    {
		if (instance != null) {
			Debug.LogError("Only one instance of " + GetType() + " should exist at a time.");
			return;
		}
		instance = this;
	}

	private void Start() {
		HideText();
	}

	/// <summary>
	/// Displays a temporary notification in the top left of the screen (or wherever textComponent is located on the screen)
	/// </summary>
	public void DisplayNotification(string text) {
		if (text == null) {
			text = "null";
		}
		HideText();
		textComponent.text = text;
		StopAllCoroutines();
		float visibleSeconds = visibleSecondsBase + text.Length * visibleSecondsPerCharacter;
		StartCoroutine(FadeCoroutine(fadeInSeconds, visibleSeconds, fadeOutSeconds));
	}

	private void HideText() {
		Color textColor = textComponent.color;
		textColor.a = 0;
		textComponent.color = textColor;
	}

	private IEnumerator FadeCoroutine(float fadeInSeconds, float visibleSeconds, float fadeOutSeconds) {
		Color textColor = textComponent.color;
		textColor.a = 0;

		// fade in
		while (true) {
			textColor.a += Time.deltaTime / fadeInSeconds;
			if (textColor.a > 1) {
				textColor.a = 1;
				textComponent.color = textColor;
				break;
			}
			textComponent.color = textColor;
			yield return new WaitForEndOfFrame();
		}

		// remain visible
		yield return new WaitForSeconds(visibleSeconds);

		// fade out
		while (true) {
			textColor.a -= Time.deltaTime / fadeOutSeconds;
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
