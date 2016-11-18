using UnityEngine;
using System.Collections;

public class CheckPlayerSight : MonoBehaviour {

    // Use this for initialization

    private string mSortLayer = "";
    private GameObject mPlayer;
    private SpriteRenderer BlockOut;
    private Transform Node;
    private int mlayerMask = (1 << 8) | (1 << 9); //Checks Walls (Layer8), and Characters (Level 9).


	void Start () {
        mSortLayer = "Blargh";
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        BlockOut = GetComponent<SpriteRenderer>();
        Node = this.transform.GetChild(0);
	}

    void Update()
    {
        //Sends a ray to see whether it can hit the player. If it can't, it blocks out the room so that it's not visible to the player.
        RaycastHit2D playerray = Physics2D.Raycast(Node.transform.position, mPlayer.transform.position - Node.transform.position, 200.0f, mlayerMask);
        if (playerray)
        {
            Debug.DrawRay(Node.transform.position, mPlayer.transform.position - Node.transform.position, Color.green);
            if (playerray.collider.tag == "Player")
            {
                BlockOut.sortingLayerName = "Underground";
            }
            else
            {
                if (playerray.collider.tag == "Wall") Debug.Log("Hit Wall");
                BlockOut.sortingLayerName = "Coverage";
            }

        }
        else
        {
            BlockOut.sortingLayerName = "Coverage";
        }

    }
    /*
	
    void FixedUpdate()
    {
        //Sends a ray to see whether it can hit the player. If it can't, it blocks out the room so that it's not visible to the player.
        RaycastHit2D playerray = Physics2D.Raycast(this.transform.position, mPlayer.transform.position - this.transform.position,200.0f, mlayerMask);
        if (playerray)
        {
            if (playerray.collider != null && playerray.collider.tag == "Player")
            {
                BlockOut.sortingLayerName = "Underground";
            }
            else
            {
                BlockOut.sortingLayerName = "Coverage";
            }
            
        } else
        {
            BlockOut.sortingLayerName = "Coverage";
        }
    } */
}
