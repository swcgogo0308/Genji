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

	[Header("SkillSettings")]
	public float shiftForce = 6f;
	public float skillDelayTime = 3f;

	[Header("Mouse")]
	public float mouseSpeed = 2f;

	Rigidbody rigid;

	Vector3 movement;

	bool isOnGround;
	bool isjumpDelaying;
	bool isSkillDelaying;

	float rotateX;
	float rotateY;

	int currentJumpCount;

	// Use this for initialization
	void Awake () {
		rigid = GetComponent<Rigidbody> ();
		currentJumpCount = maxJumpCount;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		JumpPhysics ();
		SkillPhysics ();
	}

	void Update()
	{
		MovementPhysics ();
		RotatePhysics ();
		CheckOnGround ();
	}

	void MovementPhysics() {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		
		movement = new Vector3 (horizontal, 0, vertical);
		movement = movement.normalized * speed * Time.deltaTime;

		//rigid.MovePosition (transform.position + movement);
		transform.Translate (movement);
	}

	void RotatePhysics(){
		rotateX += Input.GetAxis ("Mouse X") * mouseSpeed;

		rotateY += Input.GetAxis ("Mouse Y") * mouseSpeed;

		rotateY = Mathf.Clamp (rotateY, -45, 45);

		transform.localEulerAngles = new Vector3(-rotateY, rotateX, 0); 
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
		rigid.AddForce (Vector3.up * (jumpForce * 100) * Time.fixedDeltaTime, ForceMode.Impulse);
		StartCoroutine ("JumpDelay");

	}

	IEnumerator JumpDelay() {
		isjumpDelaying = true;
		yield return new WaitForSeconds (jumpDelayTime);
		isjumpDelaying = false;
	}

	#endregion
		
	void SkillPhysics() {
		if (isSkillDelaying || !Input.GetKeyDown(KeyCode.LeftShift))
			return;
		
		//rigid.AddForce (Vector3.forward * shiftForce * Time.fixedDeltaTime, ForceMode.Impulse);
		transform.Translate(Vector3.forward * (shiftForce * 100) * Time.deltaTime);
		StartCoroutine ("SkillDelay");
	}

	IEnumerator SkillDelay(){
		Debug.Log ("SkillCoolTime is starting");
		isSkillDelaying = true;
		yield return new WaitForSeconds (skillDelayTime);
		isSkillDelaying = false;
		Debug.Log ("SkillCoolTime is ended");
	}

}
