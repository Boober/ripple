using UnityEngine;
using System.Collections;

public class OnLightBulbPickup : MonoBehaviour {


    public GameObject lsource; //The lightsource that this lightbulb is connected to. 

    void OnDestroy()
    {
        lsource.SendMessage("ToggleLight");
    }
}
