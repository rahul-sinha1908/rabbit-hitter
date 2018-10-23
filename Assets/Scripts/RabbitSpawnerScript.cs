using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	public class RabbitSpawnerScript : MonoBehaviour {

		public GameObject rabbitPrefab;
		public Transform parent;

		private System.Random random;

		private int totalRabbits = 5;
		// Use this for initialization
		void Start() {
			random = new System.Random();

			loadRandomRabbits();
		}

		public void loadRandomRabbits() {
			for(int i = 0; i < totalRabbits; i++) {
				Quaternion quat = Quaternion.Euler(0, random.Next(-180, 180), 0);
				Vector3 vect = new Vector3((float)random.NextDouble() * 2 - 1, 0, (float)random.NextDouble() * 2 - 1);
				var go = GameObject.Instantiate(rabbitPrefab, parent);
				go.transform.position = vect;
				go.transform.rotation = quat;
			}
		}
	}
}
