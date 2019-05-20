using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
	Rigidbody rigidBody;
	AudioSource thrusterSound;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		thrusterSound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))	//Able to thrust while rotating
		{
			if (thrusterSound.isPlaying == false)
			{
				thrusterSound.Play();
			}
			rigidBody.AddRelativeForce(Vector3.up);
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.back);
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			thrusterSound.Stop();
		}
	}
}
