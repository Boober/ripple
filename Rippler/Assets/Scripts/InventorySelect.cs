using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySelect : MonoBehaviour {

    public int selection;
    public int capacity;

    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyUse;

    public Transform gridTrans { get; private set; }

    public KeyButton currButton;

	// Use this for initialization
	void Start () {
        gridTrans = GetComponent<Transform>();
        selection = 0;
        capacity = gridTrans.childCount;
        setCurrButton();
        highlightKey();
        //Debug.Log("this should happen second");
    }
	
	// Update is called once per frame
	void Update ()
        {
        if (Input.GetKeyDown(keyUse)) {
            activateKey();
        } else if (Input.GetKeyUp(keyUse)) {
            deactivateKey();
        }
        if (Input.GetKeyDown(keyLeft)) {
            backward();
        } else if (Input.GetKeyUp(keyRight)) {
            forward();
        }
    }

    void setCurrButton() {
        currButton = gridTrans.GetChild(selection).gameObject.GetComponent<KeyButton>();
    }

    void forward() {
        unhighlightKey();
        //if (capacity == 0) selection = 0;
        //else selection = (selection + 1) % capacity;
        selection = (selection + 1) % capacity;
        setCurrButton();
        highlightKey();
    }

    void backward() {
        unhighlightKey();
        //if (capacity == 0) selection = 0;
        //else selection = (selection - 1) % capacity;
        selection = (selection + capacity - 1) % capacity;
        setCurrButton();
        highlightKey();
    }

    void unhighlightKey() {
        if (currButton != null)
        {
            currButton.isHighlighted = false;
            currButton.Up();
        }
    }

    void highlightKey() {
        // secection . isHighlighted = true;
        if (currButton != null)
        {
            //Debug.Log("so what's the problem?");
            currButton.isHighlighted = true;
            currButton.Up();
        }
        else Debug.Log("fuck me!");
    }

    void activateKey() {
        if (currButton != null)
        {
            currButton.Down();
        }
    }

    void deactivateKey()
    {
        if (currButton != null)
        {
            currButton.Up();
        }
    }
}
