using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour
{
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float rscMainThrust = 100f;
	[SerializeField] float levelLoadDelay = 1f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip death;
	[SerializeField] AudioClip finish;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem finishParticles;
	[SerializeField] ParticleSystem deathParticles;

	Rigidbody rigidBody;
	AudioSource audioSource;

	private bool isTransitioning = false;
	private int maxSceneNumber;
	private int currentSceneNumber;
	private bool bCollisionToggle = true;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
		maxSceneNumber = SceneManager.sceneCountInBuildSettings - 1;	//build index starts at 0
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Debug.isDebugBuild)
		{
			HandleDebugKeysInput();
		}

		if (isTransitioning == false)
		{
			HandleThrustInput();
			HandleRotationInput();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (isTransitioning || bCollisionToggle == false)
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
		isTransitioning = true;
		rigidBody.constraints = RigidbodyConstraints.FreezeAll;
		audioSource.Stop();
		audioSource.PlayOneShot(finish);
		finishParticles.Play();
		Invoke("LoadNextScene", levelLoadDelay);
	}

	private void StartDeathSequence()
	{
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		deathParticles.Play();
		Invoke("LoadFirstLevel", levelLoadDelay);
	}

	private void LoadNextScene()
	{
		if (currentSceneNumber < maxSceneNumber)
		{
			SceneManager.LoadScene(currentSceneNumber + 1);
		}
		else
		{
			LoadFirstLevel();;
		}
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void HandleDebugKeysInput()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			bCollisionToggle = !bCollisionToggle;
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextScene();
		}
	}

	private void HandleThrustInput()
	{
		if (Input.GetKey(KeyCode.Space))	//Able to thrust while rotating
		{
			ApplyThrust();
		}
		else
		{
			StopThrust();
		}
	}

	private void ApplyThrust()
	{
		rigidBody.AddRelativeForce(Vector3.up * rscMainThrust * Time.deltaTime);
		if (audioSource.isPlaying == false)
		{
			audioSource.PlayOneShot(mainEngine);
		}
		mainEngineParticles.Play();
	}

	private void StopThrust()
	{
		audioSource.Stop();
		mainEngineParticles.Stop();
	}
	private void HandleRotationInput()
	{
		if (Input.GetKey(KeyCode.A))
		{
			RotateManually(rcsThrust * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			RotateManually(-rcsThrust * Time.deltaTime);
		}
	}

	private void RotateManually(float frameRotation)
	{
		rigidBody.freezeRotation = true;	//control rotation manually
		transform.Rotate(Vector3.forward * frameRotation);
		rigidBody.freezeRotation = false;	//control rotation with physics
	}
}
