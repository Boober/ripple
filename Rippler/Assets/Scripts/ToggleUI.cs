using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleUI : MonoBehaviour {
    private GameObject Inventory;
    PlayerInventory playerInv;
   
	// Use this for initialization
	void Start () {
        Inventory = GameObject.Find("Grid");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            ToggleInventory();
        }

	
	}

    private void ToggleInventory(){
        Inventory.SetActive(!Inventory.activeInHierarchy);
    }

    private void UpdateInventorySprites()
    {


    }

}
