using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcAI : MonoBehaviour {

	//float speed = 5f;
	public Rigidbody2D rbnpc;
	public GameObject player;
	public bool lighton;
	private bool detected;
	private float detectiontimer;
	private bool pathexist;
	private Vector2 LastKnown;
	private Vector2 NPCPos;
	public List<Vector2> Nodes = new List<Vector2>();
	private float increment = 5f;

	void Start() {
		rbnpc = GetComponent<Rigidbody2D>();
		detectiontimer = 0f;
		pathexist = false;
		Debug.Log ("It started");
	}


	bool seePlayer() {
		Vector3 RayOrigin = new Vector3 (transform.position.x + 0.05f, transform.position.y + 0.5f, transform.position.z);
		Vector2 RayDirTop = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y + 0.75f - RayOrigin.y);
		Vector2 RayDirMid = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y - RayOrigin.y);
		Vector2 RayDirBot = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y - 0.75f - RayOrigin.y);

		RaycastHit2D HitTop = Physics2D.Raycast(RayOrigin, RayDirTop, RayDirTop.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		RaycastHit2D HitMid = Physics2D.Raycast(RayOrigin, RayDirMid, RayDirMid.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		RaycastHit2D HitBot = Physics2D.Raycast(RayOrigin, RayDirBot, RayDirBot.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));

		/*
		//Debug.Log (RayDirTop);
		Debug.Log (HitTop.point.x - player.transform.position.x);
		Debug.Log (HitTop.point.y - player.transform.position.y);
		//Debug.Log (RayDirMid);
		Debug.Log (HitMid.point.x - player.transform.position.x);
		Debug.Log (HitMid.point.y - player.transform.position.y);
		//Debug.Log (RayDirBot);
		Debug.Log (HitBot.point.x - player.transform.position.x);
		Debug.Log (HitBot.point.y - player.transform.position.y);
		//Debugging log code
		*/
		//If any one of the three returns null, then it is posisble for the npc to see the player
		if (HitTop.collider == null ||HitMid.collider == null || HitBot.collider == null) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}

	void followpath(List<Vector2> Nodes, Vector2 NPCPos) {
		Vector2 Next = Nodes [0];
		if (Math.Abs(Vector2.Distance(Next,NPCPos)) == 0f) {
			Nodes.RemoveAt (0);
		}
		Vector2 Target = Nodes [0];
		transform.Translate ((Target - NPCPos) * Time.deltaTime, Space.World);
	}

	bool isvalidpath(List<Vector2> Nodes) {
		if (pathexist) 
		{
			return true;
		}
		return false;
	}

	public static Vector2 Rotate(Vector2 v, float degrees)
	{
		return Quaternion.Euler(0, 0, degrees) * v;
	}

	void pathfind(Vector2 LastKnown, Vector2 NPCPos) {
		RaycastHit2D Path = Physics2D.Raycast (NPCPos, LastKnown - NPCPos, Vector2.Distance(LastKnown,NPCPos), 1 << LayerMask.NameToLayer ("ActualWalls"));
		if (Path.collider == null) {
			Nodes.Add (LastKnown);
			pathexist = true;
		} else {
			float rotate = 0f;

			while (!pathexist) {
				Debug.Log (rotate);
				Vector2 PosNode1 = NPCPos + Rotate (LastKnown - NPCPos, rotate);
				Vector2 PosNode2 = NPCPos + Rotate (LastKnown - NPCPos, -rotate);
				RaycastHit2D PosSeg11 = Physics2D.Raycast (NPCPos, PosNode1 - NPCPos, Vector2.Distance (LastKnown, NPCPos), 1 << LayerMask.NameToLayer ("ActualWalls"));
				RaycastHit2D PosSeg12 = Physics2D.Raycast (NPCPos, LastKnown - PosNode1, Vector2.Distance (LastKnown, NPCPos), 1 << LayerMask.NameToLayer ("ActualWalls"));
				RaycastHit2D PosSeg21 = Physics2D.Raycast (NPCPos, PosNode2 - NPCPos, Vector2.Distance (LastKnown, NPCPos), 1 << LayerMask.NameToLayer ("ActualWalls"));
				RaycastHit2D PosSeg22 = Physics2D.Raycast (NPCPos, LastKnown - PosNode2, Vector2.Distance (LastKnown, NPCPos), 1 << LayerMask.NameToLayer ("ActualWalls"));
				if (PosSeg11.collider == null && PosSeg12.collider == null) {
					pathexist = true;
					Nodes.Add (PosNode1);
					Nodes.Add (LastKnown);
				} else if (PosSeg21.collider == null && PosSeg22.collider == null) {
					pathexist = true;
					Nodes.Add (PosNode2);
					Nodes.Add (LastKnown);
				} else if (rotate == 180) {
					pathexist = true;
					Nodes.Add (NPCPos);
				}
				else {
					rotate = rotate + increment;
				}
				Debug.Log ("if it fails its infinite loop");
			}
		}
		return;
		//return speed * pathdir;
	}

	void FixedUpdate() {
		NPCPos = new Vector2 (transform.position.x, transform.position.y);
		Debug.Log ("Get Location of NPC");
		Debug.Log (increment);
		if (lighton) 
		{
			//Debug.Log ("PreNodeRef");
			Nodes.Add (NPCPos);
			//Debug.Log (Nodes[0]);
			//Debug.Log("PostNodeRef");
			detected = seePlayer ();
			//Debug.Log (detected);
			//Debug.Log("Initial Fail");
			//Debug.Log(isvalidpath(Nodes));
			//Debug.LogFormat ("First Fail Point");
			/*if (detected) {
				LastKnown = new Vector2 (player.transform.position.x, player.transform.position.y); 
				Debug.Log ("Detected");
			}
			//Debug.Log ("Second Fail Point");*/
			bool isvalid = isvalidpath (Nodes);
			if (detected && !isvalid) {
				LastKnown = new Vector2 (player.transform.position.x, player.transform.position.y);
				Debug.Log (LastKnown);
				Debug.Log ("Detected no path");
				Debug.Log (NPCPos);
				Nodes.Clear ();
				Debug.Log (isvalid);
				pathfind(LastKnown, NPCPos);
			}
			/*if (detected && !isvalid) 
			{
				Debug.Log ("Pathfinding");
				Debug.Break ();
				Nodes.Clear ();
				//Debug.Log (Nodes [0]);
				Debug.Break();
				pathfind (LastKnown, NPCPos);
				Debug.Break ();
				//detectiontimer = 4f;
				//followpath (Nodes, NPCPos);
			} */
			else if (detected && isvalid) 
			{
				Debug.Log ("Following Path");
				detectiontimer = 4f;
				followpath (Nodes, NPCPos);
			} 
			else if (isvalidpath(Nodes) && detectiontimer >= 0) 
			{
				Debug.Log ("Following outdated path");
				followpath (Nodes, NPCPos);
				detectiontimer -= Time.deltaTime;
			} 
			else 
			{
				Debug.Log ("Exited Detection Range");
				detectiontimer = 0f;
				rbnpc.velocity = new Vector2 (0f, 0f);
				pathexist = false;
			}
		} 
		else 
		{
			Debug.Log ("Lights Off");
			rbnpc.velocity = new Vector2 (0f, 0f);
			detectiontimer = 0f;
			pathexist = false;
		}
	}
}