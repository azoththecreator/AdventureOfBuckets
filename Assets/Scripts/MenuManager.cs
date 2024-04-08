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
    bool screenOn = false;

    public RectTransform fadeOut;
	public RectTransform fadeIn;

    bool escTimerStart = false;
    [SerializeField] float escTimer = 0;
    public TextMeshProUGUI escText;
    public Image escTextImage;

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Start()
    {
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
        fadeIn.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (screenOn)
            {
                screenOn = false;
                OnClickCloseButton();
            }
            else if (escTimerStart)
            {
                escText.gameObject.SetActive(false);
                escTextImage.gameObject.SetActive(false);
                ExitGame();
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
        screenOn = true;
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
        //if (inputField.text == "I'm really bad at this game.")
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
