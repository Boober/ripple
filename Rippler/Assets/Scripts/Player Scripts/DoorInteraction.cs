using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour {


    public Sprite closedDoor;
    public Sprite openDoor;
    private bool state; //Whether it's open or closed
    public bool locked; //Whether the door is locked or not.
    private SpriteRenderer mrenderer;
    private BoxCollider2D box;

    private AudioSource mSource; //Plays the audio when the door is opened or closed.
    public AudioClip openSound;
    public AudioClip closeSound;


	// Use this for initialization
	void Start () {
        state = false;
        mrenderer = GetComponent<SpriteRenderer>();
        box = this.gameObject.GetComponent<BoxCollider2D>();
        mSource = GetComponent<AudioSource>();
    }
	
    //Opens or closes the door when sent a message by the Player.
	void ToggleDoor()
    {
        if (!locked)
        {
            //flips the currClip to either the open or close door, then only plays when a sound isn't already occuring on this door.
            mSource.clip = (state) ? closeSound : openSound;
            if (!mSource.isPlaying)
            {
                mSource.Play();
            }

            //Flips the sprite, and disables the collider on the door.
            mrenderer.sprite = (state) ? closedDoor : openDoor;
            box.enabled = !box.enabled;
            state = !state;
        }
    }
}
