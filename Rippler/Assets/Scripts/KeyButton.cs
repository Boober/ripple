using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class KeyButton : MonoBehaviour {

    public KeyCode key;

    public Button button { get; private set;}

    public bool isHighlighted;

    Graphic targetGraphic;
    Color normalColor;

    void Awake() {
        button = GetComponent<Button>();
        button.interactable = false;
        targetGraphic = GetComponent<Graphic>();

        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        button.colors = cb;
    }

	void Start () {
        //isHighlighted = false;
        Debug.Log("this should happen first");
        button.targetGraphic = null;
        Up();
	}
	
	void Update () {
        if (Input.GetKeyDown(key)) {
            Down();
        } else if (Input.GetKeyUp(key)) {
            Up();
        }
	}

    public void Up()
    {
        if (isHighlighted) {
            StartColorTween(button.colors.highlightedColor, false);
        } else {
            StartColorTween(button.colors.normalColor, false);
        }
    }

    public void Down() {
        StartColorTween(button.colors.pressedColor, false);
        button.onClick.Invoke();
    }

    void StartColorTween(Color targetColor, bool instant) {
        if (targetGraphic == null)
            return;

        targetGraphic.CrossFadeColor(targetColor, instant ? 0f :
            button.colors.fadeDuration, true, true);
    }

    void OnApplicationFocus() {
        Up();
    }

    public void LogOnClick() {
        Debug.Log("LogOnClick() - " + GetComponentInChildren<Text>().text);
    }
}
