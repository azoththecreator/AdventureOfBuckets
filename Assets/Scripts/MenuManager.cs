using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject screen_Help;
    public GameObject screen_StageSelect;

    bool userIsFool = false;

	public RectTransform fadeOut;

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void OnClickStartButton()
	{
		// 스테이지 1 시작
		StartCoroutine(FadeOut("Scenes/Stage1"));
    }

    public void OnClickExitButton()
    {
        ExitGame();
    }

    public void OnClickHelpButton()
    {
        if (userIsFool)
        {
            // stage select
            screen_StageSelect.SetActive(true);
        }
        else
        {
            // 나는 바보다 입력창
            screen_Help.SetActive(true);
        }

    }

    public void OnClickDoneButton()
    {
        if (inputField.text == "나는 게임을 굉장히 못 합니다.")
        {
            userIsFool = true;
            screen_Help.SetActive(false);
            // 스테이지 선택 창
            screen_StageSelect.SetActive(true);
        }
        else
        {
            // "틀렸습니다!"

        }
    }

    public void OnClickStage1Button()
    {
        StartCoroutine(FadeOut("Scenes/Stage1"));
    }

    public void OnClickStage2Button()
    {
        StartCoroutine(FadeOut("Scenes/Stage2"));
    }

    public void OnClickStage3Button()
    {
        StartCoroutine(FadeOut("Scenes/Stage3"));
    }

    public void OnClickStage4Button()
    {
        StartCoroutine(FadeOut("Scenes/Stage4"));
    }

    public void OnClickCloseButton()
    {
        screen_Help.SetActive(false);
        screen_StageSelect.SetActive(false);
        //input field 초기화
        inputField.text = string.Empty;
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
}
