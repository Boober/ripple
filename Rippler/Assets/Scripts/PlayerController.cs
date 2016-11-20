using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	public bool wasdControls;
    public PlayerInventory playerInv;
    private List<GameObject> doorsInRange = new List<GameObject>(); 
	private Rigidbody2D rb;
	private Animator anim;
	float speed = 10.0f;
	public float moveX = 0;
	public float moveY = 0;
	float acceleration = 0.1f;
	private List<GameObject> itemsInRange = new List<GameObject> ();

    //Nodes+AI stuff
    public GameObject closestnode;
    public GameObject[] NodesAll;
    public bool tester = true;

	//Speech
	private UnityEngine.UI.Text txtRef;		//A reference to the Player's text UI Component
	private float onelinerTimer = 0.0f; 	//The countdown for how much time is left talking

    void Start () {
        NodesAll = GameObject.FindGameObjectsWithTag("DOORNODE");
        closestnode = GetClosestNode(NodesAll);
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		//Set up speaking system:
		//		Note!! if we rename the player, this next line needs to be changed:
		txtRef = GameObject.Find ("/ExamplePlayer/Canvas/Text").GetComponent<UnityEngine.UI.Text>(); 
		txtRef.text = "..."; //Set the Player's text to empty, initially

	}


    //Gets the closest Node. :P
    private GameObject GetClosestNode(GameObject[] nodes)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in nodes)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    //When the player walks over an item, register the item as "in range"
    //To qualify as an item, a GameObject must 
    //		1. have a "Box Collider 2D" component with the "Is Trigger" box checked
    //		2. be tagged "Item"
   //Also when the player encounters a door.

    void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Item") {
			Debug.Log ("Over a " + other.name);
			itemsInRange.Add (other.gameObject);
		} else if (other.gameObject.tag == "Door") {
            doorsInRange.Add(other.gameObject);
		} else if (other.gameObject.tag == "OnelinerZone") {
			//This function will go to the OnelinerZone object, which will then call
			//		SayOneliner and SetOnelinerLength (from this script), which lets
			//		each object set a unique message and time limit for the player's text
			other.gameObject.SendMessage("TellPlayerWhatToSay", gameObject);
		}
	}

	//When the player is no longer in range of the item, forget the item
	void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Item")
        {
            itemsInRange.Remove(other.gameObject);
        } else if (other.gameObject.tag == "Door")
        {
            doorsInRange.Remove(other.gameObject);
		} 
	}

	//These are called by any object with the "OnelinerData" script
	//	The script is triggered in OnTriggerEnter, when we SendMessge "TellPlayerWhatToSay"
	private void SayOneliner(string message) {	//Sets the player's text
		txtRef.text = message;
	}
	private void SetOnelinerLength(float howLong) {		//Sets how long the text will be displayed
		onelinerTimer = howLong;
	}

    void FixedUpdate()
	{
        closestnode = GetClosestNode(NodesAll);

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
                playerInv.addObject(item.name);
				Destroy (item);
				itemsInRange.Remove(item);
			}
            for (int i = 0; i < doorsInRange.Count; i++)
            {
                doorsInRange[i].SendMessage("ToggleDoor");
            }
		}
			
		//If the character is saying a oneliner:
		else if (onelinerTimer > 0) {
			//Reduce the timer every time FixedUpdate is called:
			onelinerTimer -= Time.deltaTime;
			//If this is the update that ends the timer, remove text:
			if (onelinerTimer <= 0) {
				txtRef.text = "";
			}
		}
	}



}






