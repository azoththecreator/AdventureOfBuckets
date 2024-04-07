using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
	Vector3 initPos;
	[SerializeField] bool isFalling = false;

	private void Start()
	{
		initPos = transform.position;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (!isFalling)
				isFalling = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isFalling = false;
		}
	}

	private void Update()
	{
		if (isFalling)
		{
			transform.Translate(Vector2.down * Time.deltaTime);
		}

		if (!isFalling)
		{
			transform.Translate((initPos - transform.position) * Time.deltaTime * .5f);
		}
	}
}
