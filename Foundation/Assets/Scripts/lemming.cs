using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lemming : MonoBehaviour {
	public float speed;
	public float walkPeriod;
	public Vector3 CurrentDirection;
	public Action CurrentAction;
	public Action PreviousAction;
	private float walkCount;
	private GameObject climbTarget;
	private Vector3 positionMarker;
	private Vector3 directionMarker;
	private List<GameObject> blocksClimbed;
	public bool debug;
	public int level;

	public enum Action {
		WALKING,
		CLIMBING_UP,
		CLIMBING_ON,
		FALLING
	}

	// Use this for initialization
	void Start () {
		setAction(Action.WALKING);
		CurrentDirection = Vector3.forward;
		walkCount = 0;
		blocksClimbed = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (this.rigidbody.constraints != (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ)) {
			this.rigidbody.constraints = (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ);
		}*/

		if (this.rigidbody.constraints != RigidbodyConstraints.FreezeRotation) {
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}

		switch (CurrentAction) {
			case Action.WALKING:
				this.rigidbody.useGravity = false;

				walkCount += Time.deltaTime;
				if (walkCount >= walkPeriod) {
					walkCount = 0;
					setRandomRotation();	
					//changeDirection("random");
					//print("CHANGE DIRECTION");
				}

				RaycastHit hit;
				if (!Physics.Raycast(this.transform.position + CurrentDirection * this.transform.localScale.y, -Vector3.up, out hit)) {
					walkCount = walkPeriod;
				} 
				
				if (!Physics.Raycast(this.transform.position, -Vector3.up, out hit, GameHandler.BLOCK_SIZE)) {
					setAction(Action.FALLING);
				}

				walk();
				break;
			case Action.CLIMBING_UP:
				this.rigidbody.useGravity = false;
				changeDirection("up");

				if (climbTarget != null) {
					if (this.collider.bounds.min.y > climbTarget.collider.bounds.max.y) {
						// done climbing up
						positionMarker = this.transform.position;
						setAction(Action.CLIMBING_ON);
					}
				}

				climb();
				break;
			case Action.CLIMBING_ON:
				this.rigidbody.useGravity = false;

				if (climbTarget != null) {
					CurrentDirection = directionMarker;
					if (Vector3.Distance(this.transform.position, positionMarker) > 2 * this.collider.bounds.size.x) {
						recordClimbedBlock(climbTarget);
						climbTarget = null;
						walkCount = 0;
						setAction(Action.WALKING);
					}
				}

				walk();
				break;
			case Action.FALLING:
				this.rigidbody.useGravity = true;

				if (this.transform.position.y < 0) {
					Destroy(this.gameObject);
				}

				walkCount = walkPeriod;
				break;
		}
	}

	private void setAction(Action NextAction) {
		PreviousAction = CurrentAction;
		CurrentAction = NextAction;
		if (PreviousAction != CurrentAction && debug) {
			print("Lemming: " + CurrentAction);
		}
	}

	void OnCollisionEnter(Collision collision) {
		switch (CurrentAction) {
			case Action.WALKING:
				if (collision.gameObject.tag == "Block") {
					GameObject block = collision.gameObject;
					if (block.transform.position.y < this.transform.position.y) {
						if (debug)	print("foo " + block.transform.position.y + " < " + this.transform.position.y);
						setAction(Action.WALKING);
					} else if (!block.GetComponent<block>().hasBlockAbove()) {
						if (debug)	print("bar");
						// No block above block I bumped into
						RaycastHit hit;
						bool somethingAboveMe = Physics.Raycast(this.transform.position, Vector3.up, out hit, GameHandler.BLOCK_SIZE);

						if (!somethingAboveMe || (somethingAboveMe && hit.transform.gameObject.tag != "Block")) {
								if (debug)	print("yo");
								// No block above me, so climb
								climbTarget = block;
								directionMarker = CurrentDirection;
								setAction(Action.CLIMBING_UP);
						}
					}
				}
				break;
			case Action.CLIMBING_UP:
				if (collision.gameObject.tag == "Block") {
					GameObject block = collision.gameObject;
					if (block != climbTarget) {
						climbTarget = null;
						setAction(Action.FALLING);
					}
				}
				break;
			case Action.FALLING:
				if (collision.gameObject.tag == "Block") {
					setAction(Action.WALKING);
					level = collision.gameObject.GetComponent<block>().level + 1;
				} else if (collision.gameObject.tag == "Floor") {
					setAction(Action.WALKING);
					blocksClimbed.Clear();
					level = 0;
				}
				break;
		}
	}

	void OnCollisionStay(Collision collision) {
		if (CurrentAction == Action.FALLING) {
			if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Floor") {
				setAction(Action.WALKING);
			}
		}
	}

	private void recordClimbedBlock(GameObject blockObject) {
		block block = blockObject.GetComponent<block>();
		level = block.level + 1;
		if (debug) 	print("index = " + block.level);
		blocksClimbed.Remove(blockObject);
		blocksClimbed.Add(blockObject);

		block.increasePriority(0.02f);

		if (block.level >= LemmingController.HIGHEST_LEVEL) {
			highlightClimbedBlocks();
		}
	}

	private void highlightClimbedBlocks() {
		foreach (GameObject b in blocksClimbed) {
			b.GetComponent<block>().increasePriority(0.2f);
		}
	}

	private void walk() {
		Vector3 newPosition = this.transform.position + this.transform.forward * speed * Time.deltaTime;
		if (GameHandler.isInBounds(newPosition)) {
			this.transform.position = newPosition;
		} else {
			CurrentDirection = -1 * this.transform.forward;
			walkCount = 0;
		}
	}

	private void climb() {
		Vector3 newPosition = this.transform.position + Vector3.up * speed * Time.deltaTime;
		this.transform.position = newPosition;
	}

	private void setRandomRotation() {
		//Vector3 newRotation = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
		Quaternion rot = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up);
		this.transform.rotation = rot;
		CurrentDirection = this.transform.forward;
	}

	//change to an action, default is a random one
	string changeDirection(string a = "random") {
		switch(a){
			case "random"://pick a random action
				int r = Random.Range(0,4);
				switch(r){
					case 0:
						return changeDirection("left");
					case 1:
						return changeDirection("forward");
					case 2:
						return changeDirection("right");
					case 3:
						return changeDirection("back");
				}
				break;
			case "left":
				CurrentDirection = Vector3.left;
				return "left";
			case "forward":
				CurrentDirection = Vector3.forward;
				return "forward";
			case "right":
				CurrentDirection = Vector3.right;
				return "right";
			case "back":
				CurrentDirection = Vector3.back;
				return "back";
			case "up":
				CurrentDirection = Vector3.up;
				return "up";
			case "down":
				CurrentDirection = Vector3.down;
				return "down";
		}
		return "failed";
	}
}
