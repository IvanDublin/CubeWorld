using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public float E_TIME = 3f;
	float time_shoot = 0;

	public AudioClip audioShoot;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		initSound();
	}

	void initSound() {
		audio = transform.gameObject.AddComponent<AudioSource>();
		audio.clip = audioShoot;
		audio.loop = false;
		audio.playOnAwake = false;
		audio.volume = 0.8f;
	}
	
	//Create the bullet and attach the rigidbody and capsule collider.
	void Update () {
		float current_time = Time.time;
		if (current_time - time_shoot > E_TIME && Input.GetKey(KeyCode.Space)) {
			time_shoot = current_time;
			GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Capsule);

			bullet.transform.position = transform.position + transform.forward*0.2f;
			bullet.transform.localScale = new Vector3(0.02f,0.02f,0.02f);
			bullet.transform.rotation = transform.rotation;		
			bullet.tag = "Bullet";
			bullet.AddComponent<MovementBullet>();
			Rigidbody bulletRigidBody = bullet.AddComponent<Rigidbody>(); // Add the rigidbody.
			bulletRigidBody.mass = 5;
			bulletRigidBody.useGravity = false;
			bullet.AddComponent<CapsuleCollider>();


			audio.Play();

		}
	}
}
