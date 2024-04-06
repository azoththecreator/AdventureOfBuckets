using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public RectTransform fadeIn, fadeOut;

	public Transform mainCam;
	public PlayerManager playerManager;

	private void Start()
	{
		Time.timeScale = 0;
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn()
	{
		yield return new WaitForSecondsRealtime(1);
		for (int i = 0; i <= 16; i++)
		{
			fadeIn.DOLocalMoveX(128, 0).SetRelative().SetUpdate(true);
			yield return new WaitForSecondsRealtime(.05f);
		}
		Time.timeScale = 1;
	}

	public void StageClear()
	{
		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut()
	{
		for (int i = 0; i <= 16; i++)
		{
			fadeOut.DOLocalMoveX(128, 0).SetRelative();
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(.5f);
		Debug.Log("next stage");
	}

	public void Unlock(GameObject key, Animator lockAnim)
	{
		StartCoroutine(UnlockCoroutine(key, lockAnim));
	}

	IEnumerator UnlockCoroutine(GameObject key, Animator lockAnim)
	{
		Time.timeScale = 0;
		playerManager.isFollowing = false;
		Vector3 prevPos = mainCam.position;
		yield return new WaitForSecondsRealtime(1);
		mainCam.DOMove(new Vector3(lockAnim.transform.position.x, lockAnim.transform.position.y, -10), 1).SetUpdate(true);
		yield return new WaitForSecondsRealtime(1.5f);
		key.SetActive(false);
		lockAnim.SetBool("unlock", true);
		yield return new WaitForSecondsRealtime(2);
		mainCam.DOMove(prevPos, 1).SetUpdate(true);
		yield return new WaitForSecondsRealtime(1);
		playerManager.isFollowing = true;
		Time.timeScale = 1;
	}
}
