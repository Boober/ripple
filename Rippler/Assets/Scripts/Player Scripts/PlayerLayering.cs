using UnityEngine;
using System.Collections;

public class PlayerLayering : MonoBehaviour {
    public SpriteRenderer playerRender;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () { 
	  
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("flip_layer"))
        {
            other.gameObject.SendMessage("Flip", false);
        }
    }

    //When the player is no longer in range of the item, forget the item
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("flip_layer"))
        {
            other.gameObject.SendMessage("Flip", true);
        }
       
    } 
}
