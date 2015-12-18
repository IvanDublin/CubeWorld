using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ShipController : MonoBehaviour {

	Vector3 position;
	Vector3 look;
    Vector3 right;
    Vector3 up;
    Quaternion orientation;
    public float speed = 4f;
	public AudioClip audioProp;
	AudioSource audio;
    float mouseX, mouseY;

	public float PAUSE_SOUND_TIME = 1;
	float time;		//Used to know when start a sound, if there is someone


    // Use this for initialization
	void Start () 
	{ 
		initSound();
		time = Time.time;
	}

	void initSound() {
		audio = transform.gameObject.AddComponent<AudioSource>();
		audio.clip = audioProp;
		audio.loop = true;
		audio.playOnAwake = false;
		audio.volume = 0.2f;
	}

	void Yaw(float angle)
	{
		Quaternion rot = Quaternion.AngleAxis (angle, Vector3.up);
		orientation = rot * orientation;
	}

	void Pitch(float angle)
	{
		float invcosTheta1 = Vector3.Dot(look, Vector3.up);
		float threshold = 0.95f;
		if ((angle > 0 && invcosTheta1 < (-threshold)) || (angle < 0 && invcosTheta1 > (threshold)))
		{
			return;
		}

		// A pitch is a rotation around the right vector
		Quaternion rot = Quaternion.AngleAxis(angle, right);

		orientation = rot * orientation;
	}

	void UpdateMe()
	{
		position = gameObject.transform.position;
		orientation = gameObject.transform.rotation;
		look = gameObject.transform.forward;
		right = gameObject.transform.right;
        up = gameObject.transform.up;
	}

	void UpdateGameObject()
	{
		gameObject.transform.position = position;
		gameObject.transform.rotation = orientation;
	}
	// Update is called once per frame
	void Update () 
	{
        float speed = this.speed;

		UpdateMe();

		if (audio.isPlaying && Time.time - time > PAUSE_SOUND_TIME) 
			audio.Pause();

        if (Input.GetKey(KeyCode.W))
        {
            speed *= 2.0f;
        }
        
        if (Input.GetKey(KeyCode.W)) 
		{
			position +=  gameObject.transform.forward * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
		}

        if (Input.GetKey(KeyCode.S))
        {
            position -= gameObject.transform.forward * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position -= gameObject.transform.right * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
        }

        if (Input.GetKey(KeyCode.D))
        {
            position += gameObject.transform.right * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
        }
        if (Input.GetKey(KeyCode.R))
        {
            position += gameObject.transform.up * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
        }

        if (Input.GetKey(KeyCode.F))
        {
            position -= gameObject.transform.up * Time.deltaTime * speed;
			if (!audio.isPlaying) audio.Play();
			time = Time.time;
        }



        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

		float invcosTheta1 = Vector3.Dot(look, Vector3.up);
		float angle = Mathf.Acos (invcosTheta1);

		Yaw(mouseX);
		Pitch(-mouseY);

		UpdateGameObject();
	}
}