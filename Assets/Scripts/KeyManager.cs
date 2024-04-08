using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
	GameManager gameManager;
	public Animator lockAnim;
	AudioSource audioSource;

	private void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			gameManager.Unlock(gameObject, lockAnim);
			audioSource.Play();
		}
	}
}
