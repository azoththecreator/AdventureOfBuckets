using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public RectTransform fadeIn, fadeOut;

	public Transform mainCam;
	public PlayerManager playerManager;

	bool escTimerStart = false;
	[SerializeField] float escTimer = 0;
	public TextMeshProUGUI escText;
	public Image escTextImage;

    AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		Time.timeScale = 0;
		StartCoroutine(FadeIn());
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (escTimerStart)
            {
				escText.gameObject.SetActive(false);
				escTextImage.gameObject.SetActive(false);
                StartCoroutine(FadeOut("Scenes/Menu"));
			}
			else
            {
                escTimerStart = true;
				escText.DOColor(new Color(1, 1, 1, 1), .5f);
				escTextImage.DOColor(new Color(56 / 255f, 56 / 255f, 56 / 255f, 210 / 255f), .5f);
            }
		}

		if (escTimerStart)
		{
            escTimer += Time.deltaTime;

			if (escTimer > 3)
            {
                escText.DOColor(new Color(1, 1, 1, 0), .5f);
                escTextImage.DOColor(new Color(56 / 255f, 56 / 255f, 56 / 255f, 0), .5f);
                escTimer = 0;
                escTimerStart = false;
            }
        }
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

	public void StageClear(string stage)
	{
		StartCoroutine(FadeOut(stage));
	}

	IEnumerator FadeOut(string stage)
	{
		for (int i = 0; i <= 16; i++)
		{
			fadeOut.DOLocalMoveX(128, 0).SetRelative();
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(.5f);

		SceneManager.LoadScene(stage);
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
		audioSource.Play();
		yield return new WaitForSecondsRealtime(2);
		mainCam.DOMove(prevPos, 1).SetUpdate(true);
		yield return new WaitForSecondsRealtime(1);
		playerManager.isFollowing = true;
		Time.timeScale = 1;
	}
}
