using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
	public GameManager gameManager;
	bool check = true;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && check == true)
		{
			check = false;
			gameManager.StageClear();
		}
	}
}
