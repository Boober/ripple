using UnityEngine;
using System.Collections;

public class OnelinerData : MonoBehaviour {

	public string message;
	public float howLong;

	public void TellPlayerWhatToSay(GameObject obj) {
		obj.SendMessage ("SayOneliner", message);
		if (howLong <= 0.05f) //In case someone forgets to set the variable
			howLong = 0.5f;
		obj.SendMessage("SetOnelinerLength", howLong);
	}
}
