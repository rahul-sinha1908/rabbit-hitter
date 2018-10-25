using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	public class BallScript : MonoBehaviour {

		private bool isThrown = false;
		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {
			
		}

		public void setThrown() {
			isThrown = true;
		}

		private void OnCollisionEnter(Collision collision) {
			if (!isThrown)
				return;

			if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Rabbit")) {
				//Kill the Rabbit
				var rabbit = collision.collider.gameObject.GetComponent<RabbitController>();
				rabbit.killRabbit();
			}else if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor")) {
				//Destroy the object
				isThrown = false;
				Destroy(gameObject, 1);
			}
		}
	}
}
