using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : MonoBehaviour
{
	private List<AudioSource> sounds = new List<AudioSource>();

	private void Start()
	{
		sounds.AddRange(GetComponentsInChildren<AudioSource>());
	}

	public void Play()
	{
		foreach(var sound in sounds)
			sound.Play();
	}

	public void Pause()
	{
		foreach (var sound in sounds)
			sound.Pause();
	}

	public void Stop()
	{
		foreach (var sound in sounds)
			sound.Stop();
	}
}
