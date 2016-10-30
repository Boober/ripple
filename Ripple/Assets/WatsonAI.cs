using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatsonAI : MonoBehaviour {

	public Rigidbody2D rbnpc;
	public GameObject player;
	public GameObject[] Nodes;
	private Vector2 LastKnown;
	public bool lighton;
	private float detectiontimer;
	private bool isvalid;
	private Vector3 initialpos;
	private bool triggerhit;
	private GameObject playerClosestNode;
	private GameObject npcClosestNode;
	private PlayerController scriptcontrol;
	float speed = 5.0f;
	// Use this for initialization

	void Start () {
		rbnpc = GetComponent<Rigidbody2D>();
		initialpos = transform.position;
		Debug.Log (initialpos);
		isvalid = false;
		detectiontimer = 0f;
		triggerhit = false;
		player = GameObject.Find("Player");
		scriptcontrol = player.GetComponent<PlayerController>();
		// playerClosestNode = 0;
	}

	private bool seePlayer() {
		Vector3 RayOrigin = new Vector3 (transform.position.x, transform.position.y - 1f, transform.position.z);
		Vector2 RayDirTop = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y + 0.75f - RayOrigin.y);
		Vector2 RayDirMid = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y - RayOrigin.y);
		Vector2 RayDirBot = new Vector2 (player.transform.position.x - RayOrigin.x, player.transform.position.y - 0.75f - RayOrigin.y);

		RaycastHit2D HitTop = Physics2D.Raycast(RayOrigin, RayDirTop, RayDirTop.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		RaycastHit2D HitMid = Physics2D.Raycast(RayOrigin, RayDirMid, RayDirMid.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		RaycastHit2D HitBot = Physics2D.Raycast(RayOrigin, RayDirBot, RayDirBot.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));

		if (HitTop.collider == null ||HitMid.collider == null || HitBot.collider == null) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}

	private bool canwalktoplayer() {
		Vector2 Direction = (Vector2)(player.transform.position - transform.position);
		RaycastHit2D test = Physics2D.Raycast((Vector2)(transform.position), Direction, Direction.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		if (test.collider == null) {
			return true;
		}
		return false;
	}

	/*public bool validRoom(GameObject RoomNode) {
		Vector3 RayOrigin = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		Vector2 RayDir = new Vector2(RoomNode.transform.position.x - RayOrigin.x , RoomNode.transform.position.y - RayOrigin.y);
		RaycastHit2D WallCheck = Physics2D.Raycast(RayOrigin, RayDir, RayDir.magnitude, 1 << LayerMask.NameToLayer("ActualWalls"));
		if (WallCheck.collider == null) {
			return true;
		}
		return false;
	}*/

	private Vector3 GetClosestNode(GameObject[] nodes, Vector3 position)
	{
		GameObject closest = null;
		//Debug.Log (nodes[0]);
		float distance = Mathf.Infinity;
		foreach (GameObject go in nodes) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		//Debug.Log (closest.transform.position);
		return closest.transform.position;
	}

	private void pathfind(Vector3 tarpos, List<Vector3> nodepos) {
		Nodes = scriptcontrol.NodesAll;
		nodepos.Clear ();
		Vector2 CastDirection = (Vector2)(tarpos - transform.position);
		Vector3 wall2 = new Vector3 (transform.position.x + 0.1f, transform.position.y + 0.1f, 0);
		Vector3 wall3 = new Vector3 (transform.position.x - 0.1f, transform.position.y + 0.1f, 0);
		Vector2 CastDir2 = (Vector2)(wall2 - transform.position);
		Vector2 CastDir3 = (Vector2)(wall3 - transform.position);
		RaycastHit2D Wallcheck = Physics2D.Raycast (transform.position, CastDirection, CastDirection.magnitude, 1 << LayerMask.NameToLayer ("ActualWalls"));
		RaycastHit2D Wallcheck2 = Physics2D.Raycast (wall2, CastDir2, CastDir2.magnitude, 1 << LayerMask.NameToLayer ("ActualWalls"));
		RaycastHit2D Wallcheck3 = Physics2D.Raycast (wall3, CastDir3, CastDir3.magnitude, 1 << LayerMask.NameToLayer ("ActualWalls"));
		Vector3 cornerhit = new Vector3 (Wallcheck.point.x, Wallcheck.point.y, 0);
		Vector3 Doornode1 = GetClosestNode (Nodes, cornerhit);
		Vector3 Doornode2 = GetClosestNode (Nodes, Doornode1);
		Vector3 Dist1 = Doornode1 - transform.position;
		Vector3 Dist2 = Doornode2 - transform.position;
		Vector3 tarDist = tarpos - transform.position;
		if (Dist1.magnitude > tarDist.magnitude) {
			nodepos.Add (tarpos);
			Debug.Log ("joy");
			return;
		}
		if (Wallcheck.collider == null && Wallcheck2.collider == null && Wallcheck3.collider == null) {
			nodepos.Add (tarpos);
			Debug.Log ("joy2");
			return;
		}
		if (Dist1.magnitude > Dist2.magnitude) {
			nodepos.Add (Doornode2);
			nodepos.Add (Doornode1);
		} else {
			nodepos.Add (Doornode1);
			nodepos.Add (Doornode2);
		}
		Vector3 tarClosestNode = GetClosestNode(Nodes, tarpos);
		if (tarClosestNode != Doornode1 || tarClosestNode != Doornode2) {
			nodepos.Add (tarClosestNode);
		}
		if (tarpos != tarClosestNode) {
			nodepos.Add (tarpos);
		}
		return;
	}

	private void followpath(List<Vector3> nodes) {
		Debug.Log (nodes [0]);
		if (triggerhit && nodes [0] != null	) {
			nodes.RemoveAt (0);
			Debug.Log ("if");
		} else if (nodes [0] != null) {
			Vector3 finalvelocity = nodes [0] - transform.position;
			finalvelocity.Normalize();
			rbnpc.velocity = (Vector2)(finalvelocity)*speed;
			Debug.Log ("else if");
		} else {
			Vector3 finalvelocity = initialpos - transform.position;
			finalvelocity.Normalize();
			rbnpc.velocity = (Vector2)(finalvelocity)*speed;
			Debug.Log ("else");
		}
		//Debug.Log ("ActuallyMoving");

	}

	IEnumerator Example() {
		yield return new WaitForSeconds(15);
	}

	void FixedUpdate() {
		if (!lighton) 
		{
			if (seePlayer()) {
				/*Vector3 finalvelocity = player.transform.position - transform.position;
				finalvelocity.Normalize();
				rbnpc.velocity = (Vector2)(finalvelocity)*speed;*/
				List<Vector3> nodepos = new List<Vector3>();
				//Debug.Log ("Follow player for a bit");
				pathfind (player.transform.position, nodepos);
				followpath (nodepos);
			}
			else 
			{
				Vector3 spawnDistance = player.transform.position - transform.position;
				transform.position = transform.position + spawnDistance / 15;
				StartCoroutine (Example ());
			}
		} 
		else 
		{
			
		}
	}
}
