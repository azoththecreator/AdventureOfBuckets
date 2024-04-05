using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public RectTransform transition;

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) //юс╫ц
			StartCoroutine(Transition());
    }

	IEnumerator Transition()
	{
		for (int i = 0; i <= 16; i++)
		{
			transition.DOLocalMoveX(128, 0).SetRelative();
			yield return new WaitForSeconds(.1f);
		}
	}
}
