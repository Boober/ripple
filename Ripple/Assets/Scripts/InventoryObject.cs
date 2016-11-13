using UnityEngine;
using System.Collections;

public class InventoryObject {
    private int count;
    private string nombre;

    public InventoryObject(string name)
    {
        nombre = name;
        count = 0;
    }

    public string getName()
    {
        return nombre;
    }

    public int getCount()
    {
        return count;
    }

    public void increase()
    {
        count++;
    }
    public void decrease()
    {
        if (count > 0)
        {
            count--;
        }
    }
}
