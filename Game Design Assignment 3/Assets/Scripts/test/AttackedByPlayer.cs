using UnityEngine;
using System.Collections;
/// <summary>
/// Attacked by player.
/// This class just make camera shake.
/// 
/// </summary>
public class AttackedByPlayer : MonoBehaviour {
	private GameObject theCamera;
	// Use this for initialization
	void Start () {
		theCamera = Camera.main.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collisionInfo) {
		//Debug.Log (222222222222222);
		if (collisionInfo.gameObject.tag.Equals ("Weapon")) {

			//GameObject player = GameObject.Find("Player");
			//Vector3 target = player.transform.position;
			//Debug.Log("x: " + target.x + " y: " + target.y + " z: " + target.z);
			//target.x = 21;
			//target.y = 17;
			//target.z = 57;
				//Debug.Log("x: " + target.x + " y: " + target.y + " z: " + target.z);
			//player.transform.Translate(target);
			//GameObject.Find("Player").transform.position = Vector3.Lerp(player.transform.position, target, 1);
			//GameObject.Destroy(collisionInfo.gameObject);
			//Debug.Log("boss attavck");
			//GameObject weapon = GameObject.Find("WeaponCube 1");
			//Vector3 t = weapon.transform.position;
			//t.x = 40;
			//t.y = 40;
			//t.z = 69;
			//t.y = 100;
			//weapon.transform.position = Vector3.Lerp(weapon.transform.position, t, 1);
			//GameObject.Find("WeaponCube 1").GetComponent<Rigidbody>().useGravity = false;
			//weapon.GetComponent<Rigidbody> ().useGravity = false;
			//weapon.GetComponent<Rigidbody>().isKinematic = true;
			theCamera.GetComponent<CameraShake>().Shake();
		}
	}
}
