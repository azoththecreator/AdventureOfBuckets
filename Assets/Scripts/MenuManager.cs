using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    BoolManager boolManager;
    
    public RectTransform fadeOut;
	public RectTransform fadeIn;

	[SerializeField] GameObject flower;

	[SerializeField] GameObject skipOn, skipOff;
	[SerializeField] TMP_InputField cowardInput;

    private void Start()
    {
	    boolManager = GameObject.Find("BoolManager").GetComponent<BoolManager>();
	    if (boolManager.isFinished)
		    flower.SetActive(true);
	    if (boolManager.isCoward)
	    {
		    skipOn.SetActive(true);
		    skipOff.SetActive(false);
	    }
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
	    if (Input.GetKeyDown(KeyCode.Return))
	    {
		    if (cowardInput.text.ToLower() == "i really suck at this game")
		    {
			    cowardInput.text = "";
			    skipOff.SetActive(false);
			    skipOn.SetActive(true);
			    boolManager.isCoward = true;
		    }
	    }
    }

    public void SceneLoad(string stage)
	{
		StartCoroutine(FadeOut(stage));
    }

    public void ExitGame()
    {
	    StartCoroutine(Exit());
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

	IEnumerator Exit()
	{
		for (int i = 0; i <= 16; i++)
		{
			fadeOut.DOLocalMoveX(128, 0).SetRelative();
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(.5f);
		
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
	
	public void MenuAppear(Transform menu)
	{
		menu.DOMoveX(1000, .5f);
	}
	public void MenuDisappear(Transform menu)
	{
		menu.DOMoveX(-1000, .5f);
	}
}
