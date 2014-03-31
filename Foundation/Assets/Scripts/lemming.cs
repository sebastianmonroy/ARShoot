using UnityEngine;
using System.Collections;

public class lemming : MonoBehaviour {
	public float speed;
	public float walkPeriod;
	public Vector3 CurrentDirection;
	public Action CurrentAction;
	private float walkCount;
	private GameObject climbTarget;
	private Vector3 positionMarker;
	private Vector3 directionMarker;

	public enum Action {
		WALKING,
		CLIMBING_UP,
		CLIMBING_ON,
		FALLING
	}

	// Use this for initialization
	void Start () {
		CurrentAction = Action.WALKING;
		CurrentDirection = Vector3.forward;
		walkCount = 0;
		this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.rigidbody.constraints != RigidbodyConstraints.FreezeRotation) {
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}


		switch (CurrentAction) {
			case Action.WALKING:
				this.rigidbody.useGravity = false;

				walkCount += Time.deltaTime;
				if (walkCount >= walkPeriod) {
					walkCount = 0;
					changeDirection("random");
					print("CHANGE DIRECTION");
				}

				RaycastHit hit;
				if (!Physics.Raycast(this.transform.position + CurrentDirection * this.transform.localScale.y, -Vector3.up, out hit)) {
					walkCount = walkPeriod;
				} 
				
				if (!Physics.Raycast(this.transform.position, -Vector3.up, out hit, this.collider.bounds.extents.y + 1)) {
					CurrentAction = Action.FALLING;
				}

				move();
				print("Lemming: Walking");
				break;
			case Action.CLIMBING_UP:
				this.rigidbody.useGravity = false;

				if (climbTarget != null) {
					if (this.collider.bounds.min.y > climbTarget.collider.bounds.max.y) {
						// done climbing up
						positionMarker = this.transform.position;
						CurrentAction = Action.CLIMBING_ON;
					} else {
						// continue climbing
						changeDirection("up");
					}
				}

				move();
				print("Lemming: Climbing up");
				break;
			case Action.CLIMBING_ON:
				if (climbTarget != null) {
					CurrentDirection = directionMarker;
					if (Vector3.Distance(this.transform.position, positionMarker) > this.collider.bounds.size.x) {
						climbTarget = null;
						walkCount = 0;
						CurrentAction = Action.WALKING;
					}
				}

				move();
				print("Lemming: Climbing on");
				break;
			case Action.FALLING:
				this.rigidbody.useGravity = true;
				walkCount = walkPeriod;

				print("Lemming: Falling");
				break;
		}
	}

	void OnCollisionEnter(Collision collision) {
		switch (CurrentAction) {
			case Action.WALKING:
				if (collision.gameObject.tag == "Block") {
					GameObject block = collision.gameObject;
					if (block.transform.position.y < this.transform.position.y) {
						CurrentAction = Action.WALKING;
					} else if (!block.GetComponent<block>().hasBlockAbove()) {
						climbTarget = block;
						directionMarker = CurrentDirection;
						CurrentAction = Action.CLIMBING_UP;
					}
				}
				break;
			case Action.CLIMBING_UP:
				if (collision.gameObject.tag == "Block") {
					GameObject block = collision.gameObject;
					if (block != climbTarget) {
						climbTarget = null;
						CurrentAction = Action.FALLING;
					}
				}
				break;
			case Action.FALLING:
				if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Floor") {
					CurrentAction = Action.WALKING;
				}
				break;
		}
	}

	void OnCollisionStay(Collision collision) {
		if (CurrentAction == Action.FALLING) {
			if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Floor") {
				CurrentAction = Action.WALKING;
			}
		}
	}

	private void move() {
		this.transform.position += CurrentDirection * speed * Time.deltaTime;
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
