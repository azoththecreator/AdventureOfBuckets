using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
	bool isExplodingP1 = false, isExplodingP2 = false;
	bool isEnabled = false;
	public float explosion;
	SpriteRenderer sr;
	AudioSource audioSource;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (sr.enabled)
			if (!isEnabled)
			{
				audioSource.Play();
				transform.parent.rotation = Quaternion.identity;
				transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

				isEnabled = true;
				isExplodingP1 = true;
				isExplodingP2 = true;
			}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.name != "Sprite")
		{
			if (collision.name == "P1" && isExplodingP1)
			{
				isExplodingP1 = false;
				collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * explosion, ForceMode2D.Impulse);
			}
			if (collision.name == "P2" && isExplodingP2)
			{
				isExplodingP2 = false;
				collision.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position) * explosion, ForceMode2D.Impulse);
			}
		}
	}
}
