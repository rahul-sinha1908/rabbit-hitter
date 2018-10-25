using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class RabbitController : MonoBehaviour {

		private enum AnimAction {
			idlemotion, movemotion, idle, move, death
		}

		private CharacterController controller;
		private Animator animator;

		[SerializeField]
		private float walkSpeed, gallopSpeed;
		private int moveIndex;

		private bool isMoving;
		private bool isDead = false;
		private Vector3 moveDirection;
		private System.Random random;

		// Use this for initialization
		void Start() {
			controller = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();

			random = new System.Random();

			setRandomIdleMotion();

			StartCoroutine(enumMoveIdleLoop());
		}

		private void setRandomIdleMotion() {
			isMoving = false;
			animator.SetBool(AnimAction.idle.ToString(), true);
			animator.SetBool(AnimAction.move.ToString(), false);
			animator.SetInteger(AnimAction.idlemotion.ToString(), random.Next(0, 4));
			StartCoroutine(switchVariable(AnimAction.idle.ToString()));
		}
		private void setRandomMoveMotion() {
			isMoving = true;
			animator.SetBool(AnimAction.idle.ToString(), false);
			animator.SetBool(AnimAction.move.ToString(), true);
			moveIndex = random.Next(0, 2);
			animator.SetInteger(AnimAction.movemotion.ToString(), moveIndex);
		}

		private IEnumerator enumMoveIdleLoop() {
			while (true) {
				float waitTime = (float)random.NextDouble() * 5;
				yield return new WaitForSeconds(waitTime);
				if (isMoving) {
					setRandomIdleMotion();
				} else {
					setRandomMoveMotion();
				}
			}
		}
		private IEnumerator switchVariable(string var, bool b = false, float time=1) {
			yield return new WaitForSeconds(time);
			animator.SetBool(var, b);
		}
		// Update is called once per frame
		void Update() {
			if (isDead)
				return;

			if (isMoving) {
				float turn = random.Next(-10, 100);
				if (turn > 10)
					turn = 0;
				if (moveIndex == 0) {
					//Walk
					controller.SimpleMove(transform.forward*walkSpeed);
				} else {
					//Gallop
					controller.SimpleMove(transform.forward * gallopSpeed);
				}
				transform.Rotate(new Vector3(0, turn, 0));
			}
		}

		public void killRabbit() {
			isDead = true;
			animator.SetBool(AnimAction.death.ToString(), true);
			Destroy(gameObject, 3);
		}

		private void OnControllerColliderHit(ControllerColliderHit hit) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls")) {
				Debug.Log("Collided");
				setRandomIdleMotion();
				transform.LookAt(Vector3.zero);
			}
		}
		//private void OnTriggerEnter(Collider other) {
		//	Debug.Log("Triggered");
		//	setRandomIdleMotion();
		//	transform.LookAt(Vector3.zero);
		//}
	}
}
