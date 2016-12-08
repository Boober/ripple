using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour {

	private float textTime;
	private UnityEngine.UI.Text txtRef;
	private GameObject SpeakingTo;
	private int task;
	private int[] options;
	private string[] TaskNames;

	[SerializeField]
	public string convoType;

	void Start () {
		task = -1;
		textTime = 0.0f;
		txtRef = GetComponentInChildren<UnityEngine.UI.Text> ();
		txtRef.text = "!";
		InitOptions ();
	}

	public void Reset () {
		task = -1;
	}

	void SetSpeech(string text) {
		txtRef.text = text;
	}

	void SetSpeechTimer(float time) {
		textTime = time;
	}

	void SetSpeakingTo(GameObject speakingTo) {
		SpeakingTo = speakingTo;
	}

	/*
	IT: sandwich, paulcoffee
	HR: checkbathroom, talkwatson, breakroom, lockeddoor
	FD: fetchforms, stolencoffee
	Boss: brewcoffee, runhr, gofront
	Walker: lightbulb, checkit
	Else: printing, soda, getcoffee

	0. ConvoGoodJob
	ConvoBrewCoffee
	ConvoGetCoffee
	ConvoSoda
	ConvoSandwich
	5. ConvoBathroom
	ConvoPrinter
	ConvoFaucet
	ConvoLightbulb
	ConvoPaulCoffee
	10. ConvoCheckBathroom
	ConvoTalkWatson
	ConvoBreakroom
	ConvoLockedDoor
	ConvoFetchForms
	15. ConvoStolenCoffee
	ConvoRunToHR
	ConvoGoFrontDesk
	ConvoCheckIT
	*/

	void InitOptions() {
		if (convoType == "IT") {
			options = new int[]{ 4, 9 };
		} else if (convoType == "HR") {
			options = new int[]{ 10, 11, 12, 13 };
		} else if (convoType == "FD") {
			options = new int[]{ 14, 15 };
		} else if (convoType == "Boss") {
			options = new int[]{ 1, 16, 17 };
		} else if (convoType == "Walker") {
			options = new int[]{ 8, 18 };
		} else {
			options = new int[] { 6, 3, 2 };
		}
	}



	int SelectTask() {
		Debug.Log("Selecting task from " + options.Length.ToString() + " options...");
		return options [Random.Range (0, options.Length)];
	}

	void SetNextPossibleConversation (List<int> CurrentTasks) {
		Debug.Log ("Current task: " + task.ToString ());
		if (task <= 0) { 
			//TODO: use conditionals to set Watson's initial BrewCoffee convo,
			//and also the Follower's conversations.
			//Note: 8 hardcoded as # of poss convos - if we had time it'd be nice to 
			//		pass this in in a logical way from PlayerController, but...

			task = SelectTask (); //Random.Range (0, 8); 
			Debug.Log ("Task set to #" + task.ToString ());
			SpeakingTo.SendMessage ("SetConversation", task);
		} else {
			/*
			if (!CurrentTasks.Contains (task)) {
				SpeakingTo.SendMessage ("SetConversation", 0);
				Reset ();
			}
			*/
			SpeakingTo.SendMessage ("ReturnToConversation", task);
			SpeakingTo.SendMessage ("CheckIfReadyToTurnIn", task);
		}
	}

	void Update () {
		if (textTime > 0) {
			textTime -= Time.deltaTime;
			if (textTime <= 0) {
				txtRef.text = "";
				SpeakingTo.SendMessage ("CallNextConversationStep");
			}
		}
	}

}
