using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
	Animator animator;
	public float firstDelay;
	public float shootDelay;
	public float shootStrength;
	public GameObject bomb;

	public enum direction
	{
		UP,
		LEFT,
		RIGHT,
		DOWN
	}
	public direction dir;
	AudioSource audioSource;
	float vol = .2f;

	private void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(Delay());
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(firstDelay);
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot()
	{
		yield return new WaitForSeconds(shootDelay);
		animator.SetTrigger("shoot");
		yield return new WaitForSeconds(.5f);
		ShootBomb();
		yield return new WaitForSeconds(.25f);
		StartCoroutine(Shoot());
	}

	void ShootBomb()
	{
		audioSource.Play();
		GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
		switch(dir)
		{
			case direction.UP:
				newBomb.GetComponent<Rigidbody2D>().AddForce(Vector2.up * shootStrength, ForceMode2D.Impulse);
				break;
			case direction.LEFT:
				newBomb.GetComponent<Rigidbody2D>().AddForce(Vector2.left * shootStrength, ForceMode2D.Impulse);
				break;
			case direction.RIGHT:
				newBomb.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootStrength, ForceMode2D.Impulse);
				break;
			case direction.DOWN:
				newBomb.GetComponent<Rigidbody2D>().AddForce(Vector2.down * shootStrength, ForceMode2D.Impulse);
				break;
		}
	}

    private void OnBecameVisible()
    {
		audioSource.volume = vol;
    }

    private void OnBecameInvisible()
    {
		audioSource.volume = 0;
    }
}
