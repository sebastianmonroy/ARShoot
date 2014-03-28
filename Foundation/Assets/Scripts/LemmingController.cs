using UnityEngine;
using System.Collections;

public class LemmingController : MonoBehaviour {
	public float speed = 1;
	//enum directions {left, up, right, down};
	private Vector3 direction;

	
	public float actionInterval = 30;//interval between changing directions
	private float actionTimer = 0;
	
	

	// Use this for initialization
	void Start () {
		direction = Vector3.forward;
	}
	
	// Update is called once per frame
	void Update () {
		Ray downRay = new Ray(transform.position, Vector3.down);//check to see if there is anything below to stand on
		if(!Physics.Raycast(downRay, 20)){
			oppositeDirection();
		}
		transform.position = new Vector3(transform.position.x + direction.x*speed, transform.position.y + direction.y*speed, transform.position.z + direction.z*speed);
		
		
		RaycastHit hit;
		Ray directionRay = new Ray(transform.position, direction);//check to see if we're about to hit a wall
		if(Physics.Raycast(directionRay,out hit, 20)){//did we see something
			Transform t = hit.transform;
			block b = t.GetComponent<block>();
			if(!b.hasBlockAbove()){
				print("moving up");
				moveUpToBlock(b);
			}
		}
		
		standUpStright();
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
	
	void moveUpToBlock(block b){
		transform.position = new Vector3(b.transform.position.x, b.transform.position.y+22, b.transform.position.z);
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
