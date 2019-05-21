using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float rscMainThrust = 100f;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip finish;

	Rigidbody rigidBody;
	AudioSource audioSource;

	enum State {Alive, Dying, Transcending};
	State state = State.Alive;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (state == State.Alive)
		{
			HandleThrustInput();
			HandleRotationInput();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive)
		{
			return;
		}

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				break;
			case "Finish":
				StartFinishSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void StartFinishSequence()
	{
		state = State.Transcending;
		rigidBody.constraints = RigidbodyConstraints.FreezeAll;
		audioSource.Stop();
		audioSource.PlayOneShot(finish);
		Invoke("LoadNextScene", 1f);
	}

	private void StartDeathSequence()
	{
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		Invoke("LoadFirstLevel", 1);
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(1);
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void HandleThrustInput()
	{
		if (Input.GetKey(KeyCode.Space))	//Able to thrust while rotating
		{
			ApplyThrust();
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void ApplyThrust()
	{
		if (audioSource.isPlaying == false)
		{
			audioSource.PlayOneShot(mainEngine);
		}
		rigidBody.AddRelativeForce(Vector3.up * rscMainThrust);
	}

	private void HandleRotationInput()
	{
		rigidBody.freezeRotation = true;	//control rotation manually

		float frameRotation = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * frameRotation);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.back * frameRotation);
		}

		rigidBody.freezeRotation = false;	//control rotation with physics
	}
}
