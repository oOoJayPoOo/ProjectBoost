using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Rotator : MonoBehaviour
{
	[Range(-100, 100)][SerializeField] float rotationSpeed = 100f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float frameRotation = rotationSpeed * Time.deltaTime;

		transform.Rotate(Vector3.back * frameRotation);
	}
}
