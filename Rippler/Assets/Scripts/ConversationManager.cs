using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationManager : MonoBehaviour {

	private float textTime;
	private UnityEngine.UI.Text txtRef;
	private GameObject SpeakingTo;
	private int index;
	private int task;

	void Start () {
		index = 0;
		task = -1;
		textTime = 0.0f;
		txtRef = GetComponentInChildren<UnityEngine.UI.Text> ();
		txtRef.text = "!";
	}

	void Reset () {
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

	void SetNextPossibleConversation (List<int> CurrentTasks) {
		if (task == -1) { 
			//TODO: use conditionals to set Watson's initial BrewCoffee convo,
			//and also the Follower's conversations.
			//Note: 8 hardcoded as # of poss convos - if we had time it'd be nice to 
			//		pass this in in a logical way from PlayerController, but...
			task = Random.Range (0, 8); 
			Debug.Log ("Task set to #" + task.ToString ());
			SpeakingTo.SendMessage ("SetConversation", task);
		} else {
			if (!CurrentTasks.Contains (task)) {
				SpeakingTo.SendMessage ("SetConversation", 8); //Sorry about the hard-coding...
				Reset ();
			}
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
