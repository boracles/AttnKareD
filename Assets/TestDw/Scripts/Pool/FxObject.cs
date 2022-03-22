using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class FxObject : MonoBehaviour, IResettable
{
	public Action actEnded;

	public void Init()
	{
		gameObject.SetActive(true);
		StartCoroutine(CheckIfAlive());
	}

	public void Reset()
    {
		gameObject.SetActive(false);
	}

	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();
		ps.Play();
		//var psDuration = ps.main.startLifetime.constantMax;
		
		while (true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);//min..
			if (!ps.IsAlive(true))
			{
				actEnded?.Invoke();
			}
		}
	}
}