using UnityEngine;
using System.Collections;

public class WhichToFlip : MonoBehaviour {
    public string flip;
	

    void Flip(bool above)
    {
        if (flip == "UpperWall")
        {
            SpriteRenderer renderer = GameObject.FindGameObjectWithTag("UWall").GetComponent<SpriteRenderer>();
            SpriteRenderer renderer2 = GameObject.FindGameObjectWithTag("Item").GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = (above) ? "BehindCharacter" : "AboveCharacter";
            renderer2.sortingLayerName = (above) ? "Objects" : "AboveWall";


            GameObject[] misc = GameObject.FindGameObjectsWithTag("Misc");
            for (int i = 0; i < misc.Length; i++)
            {
                SpriteRenderer r = misc[i].GetComponent<SpriteRenderer>();
                r.sortingLayerName = (above) ? "Objects" : "AboveWall";
            }
        }
        else if (flip == "LowerWall")
        {
            SpriteRenderer renderer = GameObject.FindGameObjectWithTag("LWall").GetComponent<SpriteRenderer>();
            SpriteRenderer renderer2 = GameObject.FindGameObjectWithTag("Item").GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = (above) ? "BehindCharacter" : "AboveCharacter";
            renderer2.sortingLayerName = (above) ? "Objects" : "AboveWall";
        }
        /* Taken out, doesn't seem to work well.
        else if (flip == "Item") //Part of the item layer, so we'll have to flip the player.
        {
            SpriteRenderer renderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
            renderer.sortingLayerName = (above) ? "BelowItems" : "AboveItems";
        } */
        else if (flip == "Door")
        {
            SpriteRenderer renderer = GetComponentInParent<SpriteRenderer>();
            renderer.sortingLayerName = (above) ? "BehindCharacter" : "AboveCharacter";
        }
    }
}
