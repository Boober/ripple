using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	public bool wasdControls;
    public PlayerInventory playerInv;
    /// <summary>
    /// must modify the pickupItems list so that this script knows what objects to consider
    /// as a pick up item
    /// </summary>

    public List<string> pickupItems;
	private Rigidbody2D rb;
	private Animator anim;
	float speed = 10.0f;
	public float moveX = 0;
	public float moveY = 0;
	float acceleration = 0.1f;
	private List<GameObject> itemsInRange = new List<GameObject> ();

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	//When the player walks over an item, register the item as "in range"
	//To qualify as an item, a GameObject must 
	//		1. have a "Box Collider 2D" component with the "Is Trigger" box checked
	//		2. be tagged "Item"
	void OnTriggerEnter2D(Collider2D other) {
		if (pickupItems.Contains(other.gameObject.tag)) {
			Debug.Log ("Over a " + other.name);
			itemsInRange.Add (other.gameObject);
		}
	}

	//When the player is no longer in range of the item, forget the item
	void OnTriggerExit2D(Collider2D other) {
		itemsInRange.Remove (other.gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}
	void FixedUpdate()
	{
		//Checks for input from either wasd or arrow keys.
		bool left = wasdControls ? Input.GetKey(KeyCode.A) : Input.GetKey(KeyCode.LeftArrow);
		bool right = wasdControls ? Input.GetKey(KeyCode.D) : Input.GetKey(KeyCode.RightArrow);
		bool down = wasdControls ? Input.GetKey(KeyCode.S) : Input.GetKey(KeyCode.DownArrow);
		bool up = wasdControls ? Input.GetKey(KeyCode.W) : Input.GetKey(KeyCode.UpArrow);

		/*
        //Checks for input from gamepad.
		float ljoystickx = Input.GetAxis("LeftJoystickX");
		float ljoysticky = Input.GetAxis("LeftJoystickY");
		*/

		//Moves the character.
		if (left == right)
		{
			moveX = 0;
		}
		else if (left)
		{
			moveX = Mathf.Max(moveX - acceleration, -1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("LeftMove"))
			{
				anim.SetTrigger("Left");
			}   
		}
		else if (right)
		{
			moveX = Mathf.Min(moveX + acceleration, 1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RightMove"))
			{
				anim.SetTrigger("Right");
			}
		}

		if (down == up)
		{
			moveY = 0;
		}
		else if (down)
		{
			moveY = Mathf.Max(moveY - acceleration, -1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DownMove"))
			{
				anim.SetTrigger("Down");
			}

		}
		else if (up)
		{
			moveY = Mathf.Min(moveY + acceleration, 1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("UpMove"))
			{
				anim.SetTrigger("Up");
			}
		}
		anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(moveX), Mathf.Abs(moveY)));
		rb.velocity = new Vector2(moveX * speed, moveY * speed);

		//Pick up all items in range when the E key is pressed
		if (Input.GetKeyDown (KeyCode.E)) {
			while (itemsInRange.Count > 0) {
				GameObject item = itemsInRange [0];
                //This is a good spot to put the code to transfer
                //the item to the player's inventory
                playerInv.addObject(item.tag.ToString());
				Destroy (item);
				itemsInRange.Remove(item);
			}
		}

	}



}






