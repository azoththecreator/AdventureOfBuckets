using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuicksandManager : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (!collision.GetComponentInParent<PlayerMovement>().quicksand)
				collision.GetComponentInParent<PlayerMovement>().quicksand = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponentInParent<PlayerMovement>().quicksand = false;
		}
	}
}
