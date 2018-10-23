using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	public class GameControllerScript : MonoBehaviour {

		public GameObject ballPrefab, ballPosition;
		public Vector3 ballOffset;
		public float throwForce;

		private GameObject ballObj;
		private Rigidbody ballRigid;

		private Vector2 beganPos;

		// Use this for initialization
		void Start() {

			ballPosition.transform.localPosition = ballOffset;
			instantiateBall();
		}

		private void instantiateBall() {
			ballObj = GameObject.Instantiate(ballPrefab, ballPosition.transform);
			ballRigid = ballObj.GetComponent<Rigidbody>();
			ballRigid.useGravity = false;
		}

		private void throwBall(Vector3 force) {
			ballRigid.useGravity = true;
			ballRigid.AddForce(force, ForceMode.Impulse);
			Destroy(ballObj, 5);
			StartCoroutine(enumBallInstantiate());
			
		}
		private IEnumerator enumBallInstantiate() {
			yield return new WaitForSeconds(1);
			instantiateBall();
		}
		private void calculateForceDirection(Vector2 delta) {
			if (delta.y < 0)
				return;
			Vector3 throwDir = Vector3.Lerp(ballPosition.transform.forward, ballPosition.transform.up, 0.5f).normalized;
			throwDir = throwDir * delta.y;
			Vector3 rVect = ballPosition.transform.right * delta.x;
			throwDir = throwDir + rVect;
			throwDir = throwDir.normalized * delta.magnitude * throwForce;
			throwBall(throwDir);
		}
		// Update is called once per frame
		void Update() {

			if (Input.touchCount == 1) {
				Touch touch = Input.touches[0];
				if (touch.phase == TouchPhase.Began) {
					beganPos = touch.position;
				}else if (touch.phase == TouchPhase.Ended) {
					Vector3 deltaPos = touch.position - beganPos;
					calculateForceDirection(deltaPos);
				}
			}
		}
	}

}
