﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerInventory : MonoBehaviour {
    public ImageOnButton buttonScript;
    public Dictionary<string, InventoryObject> inventory;
    public List<string> objects;
	// Use this for initialization
	void Start () {
        inventory = new Dictionary<string, InventoryObject>();

        //the following depends on the number of pickable items we have
        inventory.Add("taco", new InventoryObject("taco"));
        inventory.Add("lightbulb", new InventoryObject("lightbulb"));
        inventory.Add("key", new InventoryObject("key"));
        inventory.Add("paper", new InventoryObject("paper"));
        //

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
        foreach(string obj in inventory.Keys)
        {
            if(inventory[obj].getCount() <= 0)
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
        inventory[obj].increase();
    }

    // removing objects from the inventory
    public void removeObject(string obj)
    {
        inventory[obj].decrease();
    }
}
