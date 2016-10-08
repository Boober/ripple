using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAI : MonoBehaviour {

	float speed = 5f;
	public Rigidbody2D rbnpc;
	public GameObject player;
	public bool lighton;
	private bool detected;

	void Start() {
		rbnpc = GetComponent<Rigidbody2D>();

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
		if (HitTop.collider == null ||HitMid.collider == null || HitBot.collider == null) {
			return true;
		} else {
			return false;
		}
	}

	Vector2 pathfind() {
		Vector2 pathdir = new Vector2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
		pathdir.Normalize ();
		return speed * pathdir;
	}

	void FixedUpdate() {
		if (lighton) {
			detected = seePlayer ();
			Debug.Log (detected);
			if (detected) {
				rbnpc.velocity = pathfind ();
			} else {
				rbnpc.velocity = new Vector2 (0f, 0f);
			}
		} else {
			rbnpc.velocity = new Vector2 (0f, 0f);
		}
	}
}