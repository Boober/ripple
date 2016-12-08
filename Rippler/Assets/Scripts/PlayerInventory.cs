using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class PlayerInventory : MonoBehaviour {
    public ImageOnButton buttonScript;
    public Dictionary<string, InventoryObject> types;
    public List<string> objects;
	// Use this for initialization
	void Start () {
        types = new Dictionary<string, InventoryObject>();


        DirectoryInfo levelDirectoryPath = new DirectoryInfo("Assets/Resources/Sprites");
        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (FileInfo file in fileInfo)
        {
            if(file.Extension.Equals(".png"))
            {
                string object_name = file.Name.ToString().Substring(0, file.Name.ToString().Length - 4); // length('.png') == 4
                types.Add(object_name, new InventoryObject(object_name));
            }
        }

        //objects on the inventory bar max size is the size of the bar
        objects = new List<string>();

	}
	
	// Update is called once per frame
	void Update () {
        // removes empty parts of the inventory to make room for new objects
        removeEmpty();
	}

    // removes objects from the inventory bar when the count reaches zero
    public void removeEmpty()
    {
        foreach(string obj in types.Keys)
        {
            if(types[obj].getCount() <= 0)
            {
                objects.Remove(obj);
            }
        }
    }

    //method for adding objects to the inventory
    public void addObject(string obj)
    {
        if (!objects.Contains(obj) && objects.ToArray().Length <= buttonScript.buttonCount)
        {
            objects.Add(obj);
        }
        types[obj].increase();
    }

	public bool IsCarrying(string obj)
	{
		return objects.Contains (obj);
	}

    // removing objects from the inventory
    public void removeObject(string obj)
    {
        types[obj].decrease();
    }
}
