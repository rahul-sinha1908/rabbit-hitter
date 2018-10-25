using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RabbitHitter {
	public class RabbitSpawnerScript : MonoBehaviour {

		public GameObject rabbitPrefab;
		public Transform parent;

		public Transform[] walls;
		private bool isMovingWall;
		public float offset = 0;
		private List<GameObject> rabbits;

		private System.Random random;

		private int totalRabbits = 5;
		// Use this for initialization
		void Start() {
			random = new System.Random();
			rabbits = new List<GameObject>();

			loadRandomRabbits();
		}

		public void loadRandomRabbits() {
			for(int i = 0; i < totalRabbits; i++) {
				Quaternion quat = Quaternion.Euler(0, random.Next(-180, 180), 0);
				float xMin=0, xMax=0, zMin=0, zMax=0;
				foreach(var wall in walls) {
					var wallDir = wall.transform.right;
					var wallPos = wall.transform.position;
					Debug.Log("Wall Dir : " + wallDir);
					Debug.Log("Wall Pos : " + wallPos);
					if (wallDir.x == 0) {
						if (wallPos.z < 0) {
							zMin = wallPos.z + offset;
						} else {
							zMax = wallPos.z - offset;
						}
					}
					if (wallDir.z == 0) {
						if (wallPos.x < 0)
							xMin = wallPos.x + offset;
						else {
							xMax = wallPos.x - offset;
							Debug.Log("xMax Set : " + xMax + " , " + wallDir.x);
						}
					}
				}
				Debug.Log(xMin + "," + xMax + " : " + zMin + "," + zMax);
				//Vector3 vect = new Vector3((float)random.NextDouble() * spawnRadius - spawnRadius/2, 0, (float)random.NextDouble() * spawnRadius - spawnRadius/2);
				Vector3 vect = new Vector3((float)random.NextDouble() * (xMax - xMin) + xMin, 0, (float)random.NextDouble() * (zMax - zMin) + zMin);
				var go = GameObject.Instantiate(rabbitPrefab, parent);
				rabbits.Add(go);
				go.transform.position = vect;
				go.transform.rotation = quat;
			}
		}

		public void setMoveWall() {
			//TODO Activate all the walls
			isMovingWall = !isMovingWall;
			GameControllerScript.instance.setMovingWall(isMovingWall);

			foreach (Transform t in walls) {
				t.GetChild(0).gameObject.SetActive(isMovingWall);
			}
			if (isMovingWall) {
				//Destroy all rabbits;
				destroyAllRabbits();
			} else {
				//Respawn rabbits
				loadRandomRabbits();
			}
		}
		private void destroyAllRabbits() {
			foreach (var rabbit in rabbits) {
				Destroy(rabbit);
			}
			rabbits.Clear();
		}
	}
}
