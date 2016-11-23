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
        txtRef = GetComponentInChildren<UnityEngine.UI.Text>();
		txtRef.text = "";
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
		if (pickupItems.Contains(other.gameObject.tag)) {
			Debug.Log ("Over a " + other.name);
			itemsInRange.Add (other.gameObject);
		}
        if (other.gameObject.tag == "Door")
        {
            //Just testing the one-liner possiblities on doors.
            SayOneliner("Maybe If I Press E...");
            doorsInRange.Add(other.gameObject);
        }
		else if (other.gameObject.tag == "OnelinerZone") 
		//A OnelinerZone is any GameObject that has a Box Collider 2D, has Is Trigger selected, and
		//		is tagged OnelinerZone.  It will cause the player to say text whenever the player 
		//		first enters the zone.
		{
			SayOneliner ("Hm..."); //We could also change this to the object's name,
								   //so each zone can cause the player to say different things
		} 
	}

	//When the player is no longer in range of the item, forget the item
	void OnTriggerExit2D(Collider2D other) {
        if (pickupItems.Contains(other.gameObject.tag))
        {
            itemsInRange.Remove(other.gameObject);
        }
        if (other.gameObject.tag == "Door")
        {
            doorsInRange.Remove(other.gameObject);
		} 
	}


	private void SayOneliner(string msg) 
	//The player says one line of text
	{
		txtRef.text = msg;
		onelinerTimer = 2.0f;
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
                playerInv.addObject(item.tag.ToString());
				Destroy (item);
				itemsInRange.Remove(item);
			}
            for (int i = 0; i < doorsInRange.Count; i++)
            {
                doorsInRange[i].SendMessage("ToggleDoor");
            }
		}

		//If the character is saying a oneliner:
		if (onelinerTimer > 0) {
			//Reduce the timer every time FixedUpdate is called:
			onelinerTimer -= Time.deltaTime;
			//If this is the update that ends the timer, remove text:
			if (onelinerTimer <= 0) {
				txtRef.text = "";
			}
		}

        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

	}
		
}






