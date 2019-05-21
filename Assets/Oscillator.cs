using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;
	
	float movementPercent;
	Vector3 startingPos;

	// Use this for initialization
	void Start ()
	{
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (period <= Mathf.Epsilon)	//protect against division by 0
		{
			return;
		}
		float cycles = Time.time/period;

		const float tau = Mathf.PI * 2f;	//once around the unit circle
		float sineWave = Mathf.Sin(cycles * tau);
		movementPercent = (sineWave / 2f) + .5f;

		Vector3 offset = movementVector * movementPercent;
		transform.position = startingPos + offset;
	}
}
