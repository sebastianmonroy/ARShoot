using UnityEngine;
using System.Collections;

public class serve : MonoBehaviour {
	public GameObject ballPrefab;
	public float serveSpeed;
	private bool roundStarted;
	public float waitDuration;
	private float waitCount;
	public GameObject paddle;
	public float swingSpeed;
	public float swingDistance;
	private Vector3 paddleTarget;
	private Vector3 paddleSource;
	private bool paddleFired;
	private float swingProgress;
	public GameObject paddleOrigin;
	// Use this for initialization
	void Start () {
		roundStarted = false;
		waitCount = waitDuration;
	}
	
	// Update is called once per frame
	void Update () {
		waitCount -= Time.deltaTime;
		

		if (waitCount <= 0) {
			if (!roundStarted) {
				// round not started, allow new ball serve
				if (Input.touchCount > 0) {
					GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation) as GameObject;
					ball.rigidbody.velocity = this.transform.forward * serveSpeed;
					Destroy(ball, 20);
					roundStarted = true;
					waitCount = waitDuration;
				} else if (Input.GetMouseButtonDown(0)) {
					GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation) as GameObject;
					ball.rigidbody.velocity = this.transform.forward * serveSpeed;
					Destroy(ball, 20);
					roundStarted = true;
					waitCount = waitDuration;
				}
			} else {
				if (!paddleFired) {
					// round started and paddle ready, allow paddle swinging
					if (Input.touchCount > 0) {
						paddleSource = paddle.transform.position;
						paddleTarget = paddleSource + this.transform.forward * swingDistance;
						swingProgress = 0;
						paddleFired = true;
						waitCount = waitDuration;
					} else if (Input.GetMouseButtonDown(0)) {
						paddleSource = paddle.transform.position;
						paddleTarget = paddleSource + this.transform.forward * swingDistance;
						swingProgress = 0;
						paddleFired = true;
						waitCount = waitDuration;
					}
				} else {
					// paddle swinging
					swingProgress += Time.deltaTime * swingSpeed;
					paddle.transform.position = Vector3.Lerp(paddleSource, paddleTarget, swingProgress);
					if (swingProgress >= 1) {
						paddle.transform.position = paddleOrigin.transform.position;
						paddleFired = false;
					}
				}
			}
		}
	}

	private void ResetPaddle() {
		paddle.transform.position = paddleOrigin.transform.position;
		paddleFired = false;
	}

	public void RoundOver() {
		roundStarted = false;
		ResetPaddle();
	}
}
