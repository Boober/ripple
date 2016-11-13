using UnityEngine;
using System.Collections;

public class LightDetector : MonoBehaviour {

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
    void OnTriggerStay2D(Collider2D other)
    {
        //Making sure that objects that are within the radius of the light source
        //Are including in the light disperser.
        if (other.gameObject.tag == "LightSource")
        {
            LightDisperser l = other.gameObject.GetComponent<LightDisperser>();
            if (l.getObjects().IndexOf(this.gameObject) == -1)
            {
                l.addObject(this.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "LightSource")
        {
            LightDisperser l = other.gameObject.GetComponent<LightDisperser>();
            if (l.getObjects().IndexOf(this.gameObject) != -1)
            {
                l.removeObject(this.gameObject);
            }
        }
    } */
}
