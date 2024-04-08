using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CheckpointManager : MonoBehaviour
{
	public RectTransform text;
	TextMeshProUGUI textTMP;
	AudioSource audioSource;

	private void Start()
	{
		textTMP = text.GetComponent<TextMeshProUGUI>();
		audioSource = GetComponent<AudioSource>();
	}

	public void Save(Vector3 respawnPos)
	{
		if (respawnPos != transform.position)
		{
			audioSource.Play();
			textTMP.DOColor(new Color(1, 1, 1, 1), 1);
			text.DOLocalMoveY(.5f, 1).SetRelative();
			textTMP.DOColor(new Color(1, 1, 1, 0), 1).SetDelay(1);
			text.DOLocalMoveY(-.5f, 0).SetRelative().SetDelay(2);
		}
	}
}
