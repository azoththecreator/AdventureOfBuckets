using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class EndingManager : MonoBehaviour
{
	bool endingStart = false;

	[SerializeField] ParticleSystem snowParticle;
	[SerializeField] SpriteRenderer[] snowBg;
	[SerializeField] Tilemap snowTile;

	[SerializeField] PlayerManager playerManager;
	[SerializeField] Transform p1, p2;
	[SerializeField] SpriteRenderer[] springSprites;
	[SerializeField] GameObject cannons;
	[SerializeField] ParticleSystem watering;

	[SerializeField] GameObject flower;
	[SerializeField] TextMeshProUGUI title;

	[SerializeField] GameManager gameManager;

	private void Update()
	{
		if (p2.position.x >= 119.2f && p2.localScale.x == 1)
		{
			StartCoroutine(EndingCutscene());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !endingStart)
		{
			endingStart = true;
			Ending();
		}
	}

	void Ending()
	{
		cannons.SetActive(false);
		playerManager.isEnding = true;
		Camera.main.DOOrthoSize(7, 1);
		DOTween.To(() => Camera.main.GetComponent<AudioSource>().volume, x => Camera.main.GetComponent<AudioSource>().volume = x, 0, 3);

		p1.GetComponent<PlayerMovement>().isEnding = true;
		p2.GetComponent<PlayerMovement>().isEnding = true;

		p1.DOScaleX(1, 0);
		p2.DOScaleX(1, 0);
		p1.GetComponent<DistanceJoint2D>().enabled = false;
		p1.DOMoveX(115.5f, 4).SetSpeedBased().SetEase(Ease.Linear);
		p2.DOMoveX(119.2f, 4).SetSpeedBased().SetEase(Ease.Linear);

		for (int i = 0; i < springSprites.Length; i++)
		{
			springSprites[i].DOColor(new Color(1, 1, 1, 0), 3);
		}

		snowParticle.Stop();
		for (int i = 0; i < snowBg.Length; i++)
		{
			snowBg[i].DOColor(new Color(1, 1, 1, 0), 5).SetDelay(3);
		}
		DOTween.To(() => snowTile.color, x => snowTile.color = x, new Color(1, 1, 1, 0), 5).SetDelay(3);
	}

	IEnumerator EndingCutscene()
	{
		p2.DOScaleX(-1, 0);
		playerManager.isFollowing = false;
		Camera.main.DOOrthoSize(5, 5);
		yield return new WaitForSeconds(1);
		watering.Play();
		yield return new WaitForSeconds(5);
		flower.SetActive(true);
		yield return new WaitForSeconds(2);
		Camera.main.transform.DOMoveY(10, 5).SetEase(Ease.Linear).SetRelative();

		yield return new WaitForSeconds(7);
		title.DOColor(new Color(1, 1, 1, 0), 2);
		yield return new WaitForSeconds(2);
		title.text = "wooslee\nyuna";
		title.DOColor(new Color(1, 1, 1, 1), 2);
		yield return new WaitForSeconds(4);
		title.DOColor(new Color(1, 1, 1, 0), 2);
		yield return new WaitForSeconds(2);
		gameManager.StageClear("Scenes/Menu");
	}
}
