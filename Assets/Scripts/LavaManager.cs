using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaManager : MonoBehaviour
{
	AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.name != "Sprite")
		{
			audioSource.Play();
			collision.transform.parent.GetComponent<PlayerManager>().Respawn();
		}
	}
}
