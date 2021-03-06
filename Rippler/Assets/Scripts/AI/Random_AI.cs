﻿using UnityEngine;
using System.Collections;

public class Random_AI : MonoBehaviour
{

    public GameObject[] nodes;
    public float walkSpeed;
    private GameObject curNode;
    private GameObject nextNode;
    static Random rnd = new Random();
    private int numNodes;
    private bool reachedNewNode;
    private float totalTravel;
    public bool interactingWithPlayer;

    private int currNodeIndex; //Keeping track of the current node

    // Use this for initialization
    void Start()
    {
        numNodes = nodes.Length;
        int r = (int)Random.Range(0, numNodes);
        currNodeIndex = r;
        curNode = nodes[r];
        nextNode = chooseNextNode();
        Vector3 startPos = new Vector3(curNode.transform.position.x, curNode.transform.position.y);
        transform.position = startPos;
        reachedNewNode = true;
        totalTravel = 0;
        interactingWithPlayer = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (interactingWithPlayer == false)
        {
            if (reachedNewNode == true)
            {
                curNode = nextNode;
                nextNode = chooseNextNode();
                reachedNewNode = false;
            }
            else
            {
                float difX = nextNode.transform.position.x - curNode.transform.position.x;
                float difY = nextNode.transform.position.y - curNode.transform.position.y;
                float distToReach = Mathf.Sqrt(difX * difX + difY * difY);
                Vector2 move = new Vector2((difX / distToReach) * walkSpeed / 100, (difY / distToReach) * walkSpeed / 100);
                transform.Translate(move);
                totalTravel += move.magnitude;
                if (totalTravel >= distToReach)
                {
                    transform.position = nextNode.transform.position;
                    totalTravel = 0;
                    reachedNewNode = true;
                }
            }
        }
    }

    bool isLegal(GameObject curNode, GameObject nextNode)
    {
        float difX = nextNode.transform.position.x - curNode.transform.position.x;
        float difY = nextNode.transform.position.y - curNode.transform.position.y;
        float dist = Mathf.Sqrt(difX * difX + difY * difY);
        Vector2 dir = new Vector2(difX, difY);
        RaycastHit2D ray = Physics2D.Raycast(curNode.transform.position, dir);
        if (ray.distance > dist)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Randomly chooses the next node from a set of nodes attached to this Script.

    GameObject chooseNextNode()
    {
        while (true)
        {
            int r = (int)Random.Range(0, numNodes);
            if (currNodeIndex != r)
            {
                nextNode = nodes[r];
                currNodeIndex = r;
                break;
            }
        }
        return nextNode;
    }
}