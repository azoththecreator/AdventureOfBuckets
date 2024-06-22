using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolManager : MonoBehaviour
{
	public bool isCoward;
	public bool isFinished;
	
	void Awake()
	{
		DontDestroyOnLoad(this);
	}
}
