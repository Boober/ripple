using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour {


    public Sprite closedDoor;
    public Sprite openDoor;
    private bool state; //Whether it's open or closed
    public bool locked; //Whether the door is locked or not.
    private SpriteRenderer mrenderer;
    private BoxCollider2D box;
	// Use this for initialization
	void Start () {
        state = false;
        mrenderer = GetComponent<SpriteRenderer>();
        box = this.gameObject.GetComponent<BoxCollider2D>();
    }
	
    //Opens or closes the door when sent a message by the Player.
	void ToggleDoor()
    {
        if (!locked)
        {
            mrenderer.sprite = (state) ? closedDoor : openDoor;
            box.enabled = !box.enabled;
            state = !state;
        }
    }
}
