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
		if (this.rigidbody.constraints != RigidbodyConstraints.FreezeRotation) {
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}

		if (CurrentAction != PreviousAction && debug) {
			print("Lemming: " + CurrentAction);
		}

		switch (CurrentAction) {
			case Action.WALKING:
				this.rigidbody.useGravity = false;

				walkCount += Time.deltaTime;
				if (walkCount >= walkPeriod) {
					walkCount = 0;
					changeDirection("random");
					//print("CHANGE DIRECTION");
				}

				RaycastHit hit;
				if (!Physics.Raycast(this.transform.position + CurrentDirection * this.transform.localScale.y, -Vector3.up, out hit)) {
					walkCount = walkPeriod;
				} 
				
				if (!Physics.Raycast(this.transform.position, -Vector3.up, out hit, GameHandler.BLOCK_SIZE)) {
					setAction(Action.FALLING);
				} else {
					setAction(Action.WALKING);
				}

				move();
				break;
			case Action.CLIMBING_UP:
				this.rigidbody.useGravity = false;

				if (climbTarget != null) {
					if (this.collider.bounds.min.y > climbTarget.collider.bounds.max.y) {
						// done climbing up
						positionMarker = this.transform.position;
						setAction(Action.CLIMBING_ON);
					} else {
						// continue climbing
						CurrentDirection = Vector3.up;
						setAction(Action.CLIMBING_UP);
					}
				}

				move();
				break;
			case Action.CLIMBING_ON:
				this.rigidbody.useGravity = false;

				if (climbTarget != null) {
					CurrentDirection = directionMarker;
					if (Vector3.Distance(this.transform.position, positionMarker) > this.collider.bounds.size.x) {
						recordClimbedBlock(climbTarget);
						climbTarget = null;
						walkCount = 0;
						setAction(Action.WALKING);
					} else {
						setAction(Action.CLIMBING_ON);
					}
				} else {
					setAction(Action.FALLING);
				}

				move();
				break;
			case Action.FALLING:
				this.rigidbody.useGravity = true;

				if (this.transform.position.y < 0) {
					Destroy(this.gameObject);
				} else {
					setAction(Action.FALLING);
				}

				walkCount = walkPeriod;
				break;
		}
	}

	private void setAction(Action nextAction) {
		PreviousAction = CurrentAction;
		CurrentAction = nextAction;
	}

	void OnCollisionEnter(Collision collision) {
		switch (CurrentAction) {
			case Action.WALKING:
				if (collision.gameObject.tag == "Block") {
					GameObject block = collision.gameObject;
					if (block.transform.position.y < this.transform.position.y) {
						setAction(Action.WALKING);
					} else if (!block.GetComponent<block>().hasBlockAbove()) {
						climbTarget = block;
						directionMarker = CurrentDirection;
						setAction(Action.CLIMBING_UP);
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
				} else if (collision.gameObject.tag == "Floor") {
					setAction(Action.WALKING);
					blocksClimbed.Clear();
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

	private void recordClimbedBlock(GameObject block) {
		int index = (int) Mathf.Floor(block.transform.position.y / GameHandler.BLOCK_SIZE);
		//print("index = " + index);
		blocksClimbed.Remove(block);
		blocksClimbed.Add(block);

		block.GetComponent<block>().increasePriority(0.05f);
		

		bool isTallest = true;
		GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Block");
		foreach (GameObject b in allBlocks) {
			if (b.transform.position.y > block.transform.position.y) {
				isTallest = false;
				break;
			}
		}

		if (isTallest) {
			highlightClimbedBlocks();
		}
	}

	private void highlightClimbedBlocks() {
		foreach (GameObject b in blocksClimbed) {
			b.GetComponent<block>().increasePriority(0.2f);
		}
	}

	private void move() {
		Vector3 newPosition = this.transform.position + CurrentDirection * speed * Time.deltaTime;
		if (GameHandler.isInBounds(newPosition)) {
			this.transform.position = newPosition;
		} else {
			walkCount = walkPeriod;
		}

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
