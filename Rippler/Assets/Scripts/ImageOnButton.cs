using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

        DirectoryInfo levelDirectoryPath = new DirectoryInfo("Assets/Resources/Sprites");
        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (FileInfo file in fileInfo)
        {
            if (file.Extension.Equals(".png"))
            {
                string object_name = file.Name.ToString().Substring(0, file.Name.ToString().Length - 4); // length('.png') == 4
                Sprite obj = Resources.Load<Sprite>("Sprites/" + object_name);
                sprites.Add(object_name, obj);
            }
        }
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