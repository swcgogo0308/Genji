using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour {
	public float speed = 10f; 

	[Header("JumpSettings")]
	public int maxJumpCount = 2;
	public float jumpForce = 5f;
	public float jumpDelayTime = 1f;

	[Space(10)]
	public float onGroundRange = 2f;

	Rigidbody rigid;

	Vector3 movement;

	bool isOnGround;
	bool isjumpDelaying;

	int currentJumpCount;

	// Use this for initialization
	void Awake () {
		rigid = GetComponent<Rigidbody> ();
		currentJumpCount = maxJumpCount;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		MovementPhysics ();
		JumpPhysics ();
	}

	void Update()
	{
		CheckOnGround ();
	}

	void MovementPhysics() {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		
		movement = new Vector3 (horizontal, 0, vertical);
		movement = movement.normalized * speed * Time.deltaTime;

		rigid.MovePosition (transform.position + movement);
	}

	void CheckOnGround() {
		if (Physics.Raycast (transform.position, Vector3.down, onGroundRange, 1 << 8)) {
			isOnGround = true;
			currentJumpCount = maxJumpCount;
		} else
			isOnGround = false;
	}

	#region JumpMathod

	void JumpPhysics() {
		if (!Input.GetKeyDown (KeyCode.Space) || isjumpDelaying || (!isOnGround && currentJumpCount <= 0))
			return;
		
		currentJumpCount--;
		rigid.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
		StartCoroutine ("JumpDelay");

	}

	IEnumerator JumpDelay() {
		isjumpDelaying = true;
		yield return new WaitForSeconds (jumpDelayTime);
		isjumpDelaying = false;
	}

	#endregion
		
	void SkillPhysics() {
		//TODO Skills
	}

}
