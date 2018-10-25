using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	public class GameControllerScript : MonoBehaviour {

		public static GameControllerScript instance;

		public GameObject ballPrefab, ballPosition;
		public Vector3 ballOffset;
		public float throwForce;
		private bool isMovingWall=false;

		private GameObject ballObj;
		private Rigidbody ballRigid;
		private BallScript ballScript;
		private SphereCollider collider;

		private Transform movableWall;

		private Vector2 beganPos;

		private void Awake() {
			instance = this;
		}

		// Use this for initialization
		void Start() {
			ballPosition.transform.localPosition = ballOffset;
			instantiateBall();
		}

		private void instantiateBall() {
			ballObj = GameObject.Instantiate(ballPrefab, ballPosition.transform);
			ballRigid = ballObj.GetComponent<Rigidbody>();
			ballScript = ballObj.GetComponent<BallScript>();
			collider = ballObj.GetComponent<SphereCollider>();

			ballRigid.useGravity = false;
			collider.isTrigger = true;
		}

		private void throwBall(Vector3 force) {
			if (ballObj == null)
				return;

			ballRigid.useGravity = true;
			collider.isTrigger = false;

			ballObj.transform.parent = null;
			ballRigid.AddForce(force, ForceMode.Impulse);
			ballScript.setThrown();

			ballObj = null;
			StartCoroutine(enumBallInstantiate());
		}
		private IEnumerator enumBallInstantiate() {
			yield return new WaitForSeconds(1);
			instantiateBall();
		}
		private void calculateForceDirection(Vector2 delta) {
			if (delta.y < 0)
				return;
			Vector3 throwDir = Vector3.Lerp(ballPosition.transform.forward, ballPosition.transform.up, 0.7f).normalized;
			throwDir = throwDir * delta.y;
			Vector3 rVect = ballPosition.transform.right * delta.x;
			throwDir = throwDir + rVect;
			throwDir = throwDir.normalized * delta.magnitude * throwForce;
			throwBall(throwDir);
		}
		// Update is called once per frame
		void Update() {
			if (!isMovingWall) {
				if (Input.touchCount == 1) {
					Touch touch = Input.touches[0];
					if (touch.phase == TouchPhase.Began) {
						beganPos = touch.position;
					} else if (touch.phase == TouchPhase.Ended) {
						Vector3 deltaPos = touch.position - beganPos;
						calculateForceDirection(deltaPos);
					}
				}
			} else {
				//Move the Wall
				if (Input.touchCount == 1) {
					Touch touch = Input.touches[0];
					// Construct a ray from the current touch coordinates
					if(touch.phase == TouchPhase.Began) {
						Ray ray = Camera.main.ScreenPointToRay(touch.position);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask(new string[] { "WallMove" }))) {
							movableWall = hit.collider.transform.parent;
						}
					}else if(touch.phase == TouchPhase.Moved) {
						if (movableWall == null)
							return;

						Ray ray = Camera.main.ScreenPointToRay(touch.position);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask(new string[] { "Floor" }))) {
							Vector3 point = hit.point;
							Vector3 moveDirection = movableWall.right;
							Vector3 wallPosition = movableWall.position;
							point.y = wallPosition.y;
							Debug.Log("Move Direction : " + moveDirection);
							if (moveDirection.x == 0) {
								point.x = wallPosition.x;

								if (point.z * wallPosition.z <= 0) {
									point.z = wallPosition.z;
								}
							}
							if (moveDirection.z == 0) {
								point.z = wallPosition.z;

								if (point.x * wallPosition.x <= 0) {
									point.x = wallPosition.x;
								}
							}
							movableWall.position = point;
						}
					}
				}
			}
		}

		public void addScore() {
			//TODO do the work for scoring
		}
		public void setMovingWall(bool b) {
			isMovingWall = b;
			if (b) {
				Destroy(ballObj);
			} else {
				instantiateBall();
			}
		}
	}

}
