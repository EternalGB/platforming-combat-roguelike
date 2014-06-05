using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilitiesController : MonoBehaviour
{

	public float[] abInput;
	public Ability[] abilities;
	public Transform channeller;

	void Awake()
	{
		abInput = new float[4];

	}

	void Start()
	{


	}

	void Update()
	{
		abInput[0] = Input.GetAxisRaw("A1");
		abInput[1] = Input.GetAxisRaw("A2");
		abInput[2] = Input.GetAxisRaw("A3");
		abInput[3] = Input.GetAxisRaw("A4");

		for(int i = 0; i < abInput.Length; i++) {
			if(abInput[i] > 0 && abilities[i] != null) {
				abilities[i].triggerActive(transform);
			}
		}
	}

}

