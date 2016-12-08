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

    //Sound Stuff
    private AudioSource mSource;
    private AudioSource pickupSounds;
    public AudioClip walkingSound; //TODO: Discuss whether to try and use both carpet/tile sounds. 
    public AudioClip pickupSound; 

    //Nodes+AI stuff
    public GameObject closestnode;
    public GameObject[] NodesAll;
    public bool tester = true;

	//Tasks
	private List<int> CurrentTasks;

	//Speech
	private UnityEngine.UI.Text txtRef;		//A reference to the Player's text UI Component
	private float onelinerTimer = 0.0f; 	//The countdown for how much time is left talking
	private int conversationIndex = 0;
	public struct ConversationStep {
		public string Speaker;
		public string Text;
		public float HowLong;
		public ConversationStep(string speaker, string text, float howLong) {
			Speaker = speaker;
			Text = text;
			HowLong = howLong;
		}
	}
		
	public List<ConversationStep> Conversation;
	public List<ConversationStep> ConvoBrewCoffee;
	public List<ConversationStep> ConvoGetCoffee;
	public List<ConversationStep> ConvoSoda;
	public List<ConversationStep> ConvoSandwich;
	public List<ConversationStep> ConvoBathroom;
	public List<ConversationStep> ConvoPrinter;
	public List<ConversationStep> ConvoFaucet;
	public List<ConversationStep> ConvoLightbulb;
	public List<ConversationStep> ConvoGoodJob;
	public List<List<ConversationStep>> AllConvos;
	public bool inConversation;
	public GameObject SpeakingTo;

    void Start () {
        NodesAll = GameObject.FindGameObjectsWithTag("DOORNODE");
        closestnode = GetClosestNode(NodesAll);
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        //Set up speaking system:
        //		Note!! if we rename the player, this next line needs to be changed:
        txtRef = GameObject.Find("/ExamplePlayer/Canvas/Text").GetComponent<UnityEngine.UI.Text>();
        txtRef.text = ""; //Set the Player's text to empty, initially
        mSource = GetComponent<AudioSource>();
        pickupSounds = gameObject.AddComponent<AudioSource>();
        mSource.loop = true;
        mSource.clip = walkingSound;

		//Initialize task list:
		CurrentTasks = new List<int>();

		//Set up speaking system:
		//		Note!! if we rename the player, this next line needs to be changed:
		txtRef = GameObject.Find ("/ExamplePlayer/Canvas/Text").GetComponent<UnityEngine.UI.Text>(); 
		//txtRef.text = ""; //Set the Player's text to empty, initially
		inConversation = false;
		InitConversations();
    }

	private void InitConversations() 
	//Initializes and stores all possbile conversations for future use
	{
		//Brew coffee:
		ConvoBrewCoffee = new List<ConversationStep>();
		ConvoBrewCoffee.Add (new ConversationStep ("other", "Hey, Blueboy!", 0.5f));
		ConvoBrewCoffee.Add (new ConversationStep ("other", "Brew up some coffee\nwould ya?", 1.5f));
		ConvoBrewCoffee.Add (new ConversationStep ("other", "Can't have my\nworkers dying on me...", 1.5f));
		ConvoBrewCoffee.Add (new ConversationStep ("other", "Hehe!", 0.8f));
		ConvoBrewCoffee.Add (new ConversationStep ("self", "Okay...", 1.5f));
		//Get coffee:
		ConvoGetCoffee = new List<ConversationStep>();
		ConvoGetCoffee.Add (new ConversationStep ("other", "Oops,", 0.5f));
		ConvoGetCoffee.Add (new ConversationStep ("other", "I left my coffee\nin the kitchen,", 1.5f));
		ConvoGetCoffee.Add (new ConversationStep ("other", "would you mind\ngetting it for me?", 1.5f));
		ConvoGetCoffee.Add (new ConversationStep ("self", "...", 1.0f));
		ConvoGetCoffee.Add (new ConversationStep ("self", "Do I get paid\nfor this?", 1.5f));
		//Soda:
		ConvoSoda = new List<ConversationStep>();
		ConvoSoda.Add (new ConversationStep ("other", "Hey Joe,", 0.8f));
		ConvoSoda.Add (new ConversationStep ("self", "My name is Ed!", 0.8f));
		ConvoSoda.Add (new ConversationStep ("other", "Can you buy a can\nof soda for me?", 1.5f));
		ConvoSoda.Add (new ConversationStep ("other", "Here's some change.", 1.0f));
		ConvoSoda.Add (new ConversationStep ("self", "...", 0.5f));
		ConvoSoda.Add (new ConversationStep ("self", "That's not enough\nfor a soda!", 1.5f));
		//Sandwich:
		ConvoSandwich = new List<ConversationStep>();
		ConvoSandwich.Add (new ConversationStep ("other", "Could you fetch\nmy sandwich", 1.0f));
		ConvoSandwich.Add (new ConversationStep ("other", "from the fridge,\nPaul?", 1.0f));
		ConvoSandwich.Add (new ConversationStep ("self", "...", 0.5f));
		ConvoSandwich.Add (new ConversationStep ("self", "That's not\nmy name...", 1.5f));
		ConvoSandwich.Add (new ConversationStep ("other", "I'm starving,\nthanks!", 1.0f));
		ConvoSandwich.Add (new ConversationStep ("self", "...", 0.5f));			
		ConvoSandwich.Add (new ConversationStep ("self", "Which sandwich?", 1.5f));
		ConvoSandwich.Add (new ConversationStep ("self", "Sigh...", 1.0f));
		//Bathroom:
		ConvoBathroom = new List<ConversationStep>();
		ConvoBathroom.Add (new ConversationStep ("other", "The bathroom is\nbeing weird,", 1.0f));
		ConvoBathroom.Add (new ConversationStep ("other", "can you take care\nof it? Thanks.", 1.5f));
		ConvoBathroom.Add (new ConversationStep ("self", "Where's the\njanitor?", 1.5f));
		ConvoBathroom.Add (new ConversationStep ("self", "Am I the\njanitor?!", 1.3f));
		//Print:
		ConvoPrinter = new List<ConversationStep>();
		ConvoPrinter.Add (new ConversationStep ("other", "Bob!\nDo me a favor,", 1.0f));
		ConvoPrinter.Add (new ConversationStep ("other", "I just sent\nsomething to print,", 1.0f));
		ConvoPrinter.Add (new ConversationStep ("other", "can you go\nget it for me?", 1.0f));
		ConvoPrinter.Add (new ConversationStep ("self", "Ok boss...", 1.0f));
		//Faucet:
		ConvoFaucet = new List<ConversationStep>();
		ConvoFaucet.Add (new ConversationStep ("other", "The faucet is\nmaking a mess,", 1.0f));
		ConvoFaucet.Add (new ConversationStep ("other", "could you go clean\nit up? Thanks.", 1.0f));
		ConvoFaucet.Add (new ConversationStep ("self", "...", 1.0f));
		//Lightbulb:
		ConvoLightbulb = new List<ConversationStep>();
		ConvoLightbulb.Add (new ConversationStep ("other", "The lightbulb\nblew out in the", 1.1f));
		ConvoLightbulb.Add (new ConversationStep ("other", "conference room,\ncould you", 1.1f));
		ConvoLightbulb.Add (new ConversationStep ("other", "get a spare in\nthe closet", 1.1f));
		ConvoLightbulb.Add (new ConversationStep ("other", "and replace it?", 1.1f));
		ConvoLightbulb.Add (new ConversationStep ("self", "OK~~", 1.0f));
		//Good job:
		ConvoGoodJob = new List<ConversationStep>();
		ConvoGoodJob.Add (new ConversationStep("other", "Good job.", 2.5f));
		//All of them together:
		AllConvos = new List<List<ConversationStep>>();
		AllConvos.Add (ConvoBrewCoffee);
		AllConvos.Add (ConvoGetCoffee);
		AllConvos.Add (ConvoSoda);
		AllConvos.Add (ConvoSandwich);
		AllConvos.Add (ConvoBathroom);
		AllConvos.Add (ConvoPrinter);
		AllConvos.Add (ConvoFaucet);
		AllConvos.Add (ConvoLightbulb);
	}

	public void SetConversation(int index) 
	//Set which conversation the player is currently in
	//Called by SetNextPossibleConversation in an NPC's ConversationManager.cs
	//Uses conversations created in InitConversation
	{
		Debug.Log ("Setting conversation to " + index.ToString());
		if (index != 8) { //The "Good job" convo is hard-coded to the 8-indexed conversation
			CurrentTasks.Add (index);
			Debug.Log ("Tasks length now: " + (CurrentTasks.Count).ToString ());
			Conversation = AllConvos [index];
		} else {
			Conversation = ConvoGoodJob;
		}
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
        if (other.gameObject.tag == "Item")
        {
            Debug.Log ("Over a " + other.name);
			itemsInRange.Add (other.gameObject);
		}
        else if (other.gameObject.tag == "Door")
        
            {
            //Just testing the one-liner possiblities on doors.
            //SayOneliner("Maybe If I Press E...");
			//SetOnelinerLength (1.0f);
            doorsInRange.Add(other.gameObject);
        }
       else if (other.gameObject.tag == "OnelinerZone") {
 		//This function will go to the OnelinerZone object, which will then call
 			//		SayOneliner and SetOnelinerLength (from this script), which lets
 			//		each object set a unique message and time limit for the player's text
 			other.gameObject.SendMessage("TellPlayerWhatToSay", gameObject);
 		}
		else if (other.gameObject.tag == "ConversationTrigger") {
			SpeakingTo = other.gameObject;
			SpeakingTo.SendMessage ("SetSpeakingTo", this.gameObject);
			SpeakingTo.SendMessage ("SetNextPossibleConversation", CurrentTasks);
			if (Conversation.Count > 0) {
				inConversation = true;
				conversationIndex = 0;
				DoConversationStep (Conversation [conversationIndex]);
			}
		}
	}

	//When the player is no longer in range of the item, forget the item
	void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Item")
        {
            itemsInRange.Remove(other.gameObject);
        }
        if (other.gameObject.tag == "Door")
        {
            doorsInRange.Remove(other.gameObject);
		} 
	}

	//These are called by any object with the "OnelinerData" script:
	//Note!! if you don't call SetOnelinerLength when you call SayOneliner, the text will never disappear
    private void SayOneliner(string message)
    {   //Sets the player's text
        txtRef.text = message;
    }
 	private void SetOnelinerLength(float howLong)
    {       //Sets how long the text will be displayed
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
        if (left == right && up == down)
        {
            mSource.Stop();
        }

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
            if (!mSource.isPlaying)
            {
                mSource.Play();
            }   
		}
		else if (right)
		{
			moveX = Mathf.Min(moveX + acceleration, 1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RightMove"))
			{
				anim.SetTrigger("Right");
			}
            if (!mSource.isPlaying)
            {
                mSource.Play();
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
            if (!mSource.isPlaying)
            {
                mSource.Play();
            }
        }
		else if (up)
		{
			moveY = Mathf.Min(moveY + acceleration, 1);
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("UpMove"))
			{
				anim.SetTrigger("Up");
			}
            if (!mSource.isPlaying)
            {
                mSource.Play();
            }
        }
		anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(moveX), Mathf.Abs(moveY)));
		rb.velocity = new Vector2(moveX * speed, moveY * speed);

		//Temporary press F to finish all tasks
		if (Input.GetKeyDown (KeyCode.F)) {
			Debug.Log ("Pressed F to complete all tasks.");
			Debug.Log ("Tasks: " + CurrentTasks.Count.ToString ());
			CurrentTasks.Clear ();
			Debug.Log ("Tasks: " + CurrentTasks.Count.ToString ());
		}

		//Pick up all items in range when the E key is pressed
		if (Input.GetKeyDown (KeyCode.E)) {
			while (itemsInRange.Count > 0) {
				GameObject item = itemsInRange [0];
                //This is a good spot to put the code to transfer
                //the item to the player's inventory
                pickupSounds.PlayOneShot(pickupSound, 1);
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
        
 		//If the character is saying a oneliner:
       else if (onelinerTimer > 0)
        {
                       //Reduce the timer every time FixedUpdate is called:
            onelinerTimer -= Time.deltaTime;
                       //If this is the update that ends the timer, remove text:
                     if (onelinerTimer <= 0)
            {
				txtRef.text = "";
				if (inConversation) {
					CallNextConversationStep ();
				}
                          }
               }

        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

	}

	public void CallNextConversationStep() 
	//This is called every time a conversation step has been completed
	//		to manage the transition to the next step in the conversation
	//This is called from FixedUpdate, when the onelinerTimer reaches 0
	{
		conversationIndex++; //Move to the next line in the conversation
		if (conversationIndex >= Conversation.Count) { 
			//If we've spoken everything in the conversation, end it
			inConversation = false;
			conversationIndex = 0;
		} else {
			//Otherwise, say the line we've just moved to
			DoConversationStep (Conversation [conversationIndex]);
		}
	}

	public void DoConversationStep(ConversationStep step) 
	//This is called from CallNextConversationStep
	//Reads from the Conversation list to find the words to be spoken next,
	//		sends those words to the speaker, 
	//		and tells the speaker how long to speak
	{
		Debug.Log ("DCS " + step.Text);
		string text = step.Text;
		string speaker = step.Speaker;
		float howLong = step.HowLong;
		if (speaker == "self") {
			//Debug.Log ("Self: " + text);
			SayOneliner (text);
			SetOnelinerLength (howLong);
		} else {
			//Debug.Log ("Other: " + text);
			SpeakingTo.SendMessage ("SetSpeech", text);
			SpeakingTo.SendMessage ("SetSpeechTimer", howLong);
		}
	}

	public void CompleteTask(int task) {
		Debug.Log ("Completed task #" + task.ToString ());
	}

}






