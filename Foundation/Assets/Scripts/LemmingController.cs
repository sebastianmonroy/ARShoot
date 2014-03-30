using UnityEngine;
using System.Collections;

public class LemmingController : MonoBehaviour {
	public float speed = 1;
	//enum directions {left, up, right, down};
	private string action;
	private Vector3 direction;
	private bool isClimbing;
	private block climbTarget;

	
	public float actionInterval = 30;//interval between changing directions
	private float actionTimer = 0;
	
	

	// Use this for initialization
	void Start () {
		action = "stand";
		isClimbing = false;
		climbTarget = null;
		direction = Vector3.forward;
	}
	
	// Update is called once per frame
	void Update () {
		Ray downRay = new Ray(transform.position, Vector3.down);
		if(!Physics.Raycast(downRay, 15) && !isClimbing){
			//don't move
			direction = Vector3.down;
			print ("FALLING");
		}else{//work normally
			
			standUpStright();
			lookDownTurnAround();
			moveForward();
			
			
			if(climbTarget != null){
				moveUpToBlock(climbTarget);
			}else{
				RaycastHit hit;
				Ray directionRay = new Ray(transform.position, direction);//check to see if we're about to hit a wall
				if(Physics.Raycast(directionRay,out hit, 20)){//did we see something
					Transform t = hit.transform;
					if(t.tag == "Block"){//make sure we're hitting a block
						block b = t.GetComponent<block>();
						if(!b.hasBlockAbove()){
							climbTarget = b;
							isClimbing = true;
						}
					}
				}
			}
		}

		
		
		
		
		
		//change action
		if(actionTimer % actionInterval == 0){
			changeAction ("random");
		}
		actionTimer++;
	}
	//collision
	/*void OnCollisionEnter(Collision collision) {
		print ("collided with "+collider.tag);
		GameObject collided = collider.gameObject;
		//hit a block
        if(collided.tag == "Block" || collided.tag == "Tetris"){
			//oppositeDirection();
			print ("BLOCK HIT");
			block blockscript = collided.GetComponent<block>();
			if(!blockscript.hasBlockAbove()){//check if there's another block on this block
				transform.position = new Vector3(collided.transform.position.x, collided.transform.position.y-100, collided.transform.position.z);
			}
		}
    }*/
	void moveForward(){
		transform.position = new Vector3(transform.position.x + direction.x*speed, transform.position.y + direction.y*speed, transform.position.z + direction.z*speed);
	}
	void moveUpToBlock(block b){
		float targetY = b.transform.position.y + this.transform.lossyScale.y + 10;
		Rigidbody g = this.transform.GetComponent<Rigidbody>();
		
		if(this.transform.position.y < targetY){
			transform.position = new Vector3(this.transform.position.x, this.transform.position.y+10, this.transform.position.z);
			direction = Vector3.up;
			isClimbing = true;
			g.useGravity = false;
		}else{
			transform.position = new Vector3(b.transform.position.x, targetY, b.transform.position.z);
			isClimbing = false;
			climbTarget = null;
			g.useGravity = true;
		}
	}
	
	void lookDownTurnAround(){//turn around if no floor in front
		Ray forwardDown = new Ray(transform.position, Vector3.down + (direction*0.2f));//check to see if there is a floor in front
		if(!Physics.Raycast(forwardDown, 30)){
			oppositeDirection();
		}
	}
	
	void oppositeDirection(){
		direction.x = -direction.x;
		direction.y = -direction.y;
		direction.z = -direction.z;
	}
	
	void standUpStright(){
		Vector3 rotationVector = transform.rotation.eulerAngles;
		rotationVector.x = 0;
		rotationVector.z = 0;
		transform.rotation = Quaternion.Euler(rotationVector);
	}
	
	//change to an action, default is a random one
	string changeAction(string a = "random"){
		switch(a){
		case "random"://pick a random action
			int r = Random.Range(0,4);
			switch(r){
			case 0:
				return changeAction("left");
				break;
			case 1:
				return changeAction("forward");
				break;
			case 2:
				return changeAction("right");
				break;
			case 3:
				return changeAction("back");
				break;
			}
			
			break;
		case "left":
			direction = Vector3.left;
			return "left";
			break;
		case "forward":
			direction = Vector3.forward;
			return "forward";
			break;
		case "right":
			direction = Vector3.right;
			return "right";
			break;
		case "back":
			direction = Vector3.back;
			return "back";
			break;
		case "up":
			direction = Vector3.up;
			return "up";
			break;
		case "down":
			direction = Vector3.down;
			return "down";
			break;
		}
		return "failed";
	}
}
