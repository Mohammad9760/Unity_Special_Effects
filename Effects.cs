using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Effects : MonoBehaviour
{
	public Material screenDamageMat;
	public CinemachineImpulseSource impulseSource;
	private Coroutine screenDamageTask;

	private static Effects instance;
 
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(1))
			SpecialEffects.ScreenDamageEffect(Random.Range(0.1f, 1));
	}

	private void ScreenDamageEffect(float intensity) 
	{
		if(screenDamageTask != null)
			StopCoroutine(screenDamageTask);

		screenDamageTask = StartCoroutine(screenDamage(intensity));
	}
	private IEnumerator screenDamage(float intensity)
	{
		// Cinemachine Camera shake
		var velocity = new Vector3(0, -0.5f, -1);
		velocity.Normalize();
		impulseSource.GenerateImpulse(velocity * intensity * 0.4f);
		// Screen Effect
		var targetRadius = Remap(intensity, 0, 1, 0.4f, -0.1f);
		var curRadius = 1f;
		for(float t = 0; curRadius != targetRadius; t += Time.deltaTime)
		{
			curRadius = Mathf.Clamp(Mathf.Lerp(1, targetRadius, t), 1, targetRadius);
			screenDamageMat.SetFloat("_Vignette_radius", curRadius);
			yield return null;
		}
		for(float t = 0; curRadius < 1; t += Time.deltaTime)
		{
			curRadius = Mathf.Lerp(targetRadius, 1, t);
			screenDamageMat.SetFloat("_Vignette_radius", curRadius);
			yield return null;
		}
		
	}

	private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
	{
		return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
	}

	public static class SpecialEffects
	{
		public static void ScreenDamageEffect(float intensity) => instance.ScreenDamageEffect(intensity);
	}
}
