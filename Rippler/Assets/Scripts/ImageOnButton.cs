using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageOnButton: MonoBehaviour {
    public PlayerInventory playerInv;
    public int buttonCount;
    public Button b0;
    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;
    public Button b5;
    private List<Button> buttons;
    private Dictionary<string, Sprite> sprites;

    void Start() {
        buttons = new List<Button>();
        sprites = new Dictionary<string, Sprite>();
        buttons.Add(b0);
        buttons.Add(b1);
        buttons.Add(b2);
        buttons.Add(b3);
        buttons.Add(b4);
        buttons.Add(b5);

        //the following depends on what elements wil be pickable
        Sprite taco = Resources.Load<Sprite>("Sprites/taco");  
        Sprite lightbulb = Resources.Load<Sprite>("Sprites/lightbulb");
        Sprite paper = Resources.Load<Sprite>("Sprites/paper");
        Sprite key = Resources.Load<Sprite>("Sprites/key");
        sprites.Add("taco", taco);
        sprites.Add("lightbulb", lightbulb);
        sprites.Add("paper", paper);
        sprites.Add("key", key);                            
        //
    }
	
	// Update is called once per frame
	void Update () {
        
        for (int i = 0; i < buttonCount; i++)
        {
            if(i < playerInv.objects.ToArray().Length)
            {
                string obj = playerInv.objects[i];
                
                //int count = playerInv.inventory[obj].getCount();    
                buttons[i].GetComponent<Image>().sprite = sprites[obj];
            } else
            {
                buttons[i].GetComponent<Image>().sprite = null;
            }

        }
         
        
	}
}