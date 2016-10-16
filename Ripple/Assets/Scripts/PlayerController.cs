using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public bool wasdControls;
    private Rigidbody2D rb;
    private Animator anim;
    float speed = 10.0f;
    public float moveX = 0;
    public float moveY = 0;
	public GameObject closestnode;
    float acceleration = 0.1f;
	public GameObject[] NodesAll;
	public bool tester = true;

    void Start () {
		NodesAll = GameObject.FindGameObjectsWithTag ("DOORNODE");
		closestnode = GetClosestNode (NodesAll);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}

	private GameObject GetClosestNode(GameObject[] nodes)
	{
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in nodes) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}

    // Update is called once per frame
    void Update()
    {
		closestnode = GetClosestNode (NodesAll);
    }

    void FixedUpdate()
    {
        //Checks for input from either wasd or arrow keys.
        bool left = wasdControls ? Input.GetKey(KeyCode.A) : Input.GetKey(KeyCode.LeftArrow);
        bool right = wasdControls ? Input.GetKey(KeyCode.D) : Input.GetKey(KeyCode.RightArrow);
        bool down = wasdControls ? Input.GetKey(KeyCode.S) : Input.GetKey(KeyCode.DownArrow);
        bool up = wasdControls ? Input.GetKey(KeyCode.W) : Input.GetKey(KeyCode.UpArrow);

        //Checks for input from gamepad.
        //float ljoystickx = Input.GetAxis("LeftJoystickX");
        //float ljoysticky = Input.GetAxis("LeftJoystickY");

        //Moves the character.
        if (left == right)
        {
            moveX = 0;
        }
        else if (left)
        {
            moveX = Mathf.Max(moveX - acceleration, -1);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("LeftMove"))
            {
                anim.SetTrigger("Left");
            }   
        }
        else if (right)
        {
            moveX = Mathf.Min(moveX + acceleration, 1);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RightMove"))
            {
                anim.SetTrigger("Right");
            }
        }

        if (down == up)
        {
            moveY = 0;
        }
        else if (down)
        {
            moveY = Mathf.Max(moveY - acceleration, -1);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DownMove"))
            {
                anim.SetTrigger("Down");
            }

        }
        else if (up)
        {
            moveY = Mathf.Min(moveY + acceleration, 1);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("UpMove"))
            {
                anim.SetTrigger("Up");
            }
        }
        anim.SetFloat("Speed", Mathf.Max(Mathf.Abs(moveX), Mathf.Abs(moveY)));
        rb.velocity = new Vector2(moveX * speed, moveY * speed);
    }
}



    

