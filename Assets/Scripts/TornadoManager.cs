using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoManager : MonoBehaviour
{
	public float strength = 25;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.name != "Sprite")
		{
			collision.GetComponent<PlayerMovement>().SuperJump(strength);
		}
	}
}
