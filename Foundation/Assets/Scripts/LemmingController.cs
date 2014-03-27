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
		Ray dRay = new Ray(transform.position, Vector3.down);
		if(!Physics.Raycast(dRay, 100)){
			oppositeDirection();
		}
		transform.position = new Vector3(transform.position.x + direction.x*speed, transform.position.y + direction.y*speed, transform.position.z + direction.z*speed);
		
		
		
		//change action
		if(actionTimer % actionInterval == 0){
			changeAction ("random");
		}
		actionTimer++;
	}
	//collision
	void OnCollisionEnter(Collision collision) {
		
		//hit a block
        if(collision.gameObject.tag == "Block"){
			oppositeDirection();
		}
    }
	
	void oppositeDirection(){
		direction.x = -direction.x;
		direction.y = -direction.y;
		direction.z = -direction.z;
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
