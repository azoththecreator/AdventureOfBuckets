using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Camera mainCam;
	Transform mainCamTr;
	public Transform p1, p2;
	PlayerMovement playerMovement;

	public Transform spring;

    void Awake()
    {
		mainCamTr = mainCam.transform;
		playerMovement = p1.GetComponent<PlayerMovement>();
	}

	private void Update()
	{
		spring.position = new Vector3((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2, 0);
		spring.rotation = Quaternion.Euler(0, 0, 57.2958f * Mathf.Atan2(p1.position.y - p2.position.y, p1.position.x - p2.position.x));
	}

	private void FixedUpdate()
	{
		// 차지할 때 줌아웃
		if (playerMovement.dist > 3)
			mainCam.orthographicSize = 5 + (playerMovement.dist - 3);
	}

	private void LateUpdate()
	{
		// 카메라가 플레이어를 따라감
		Vector3 targetPos = new Vector3((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2 + 2, mainCamTr.position.z);
		mainCamTr.position = targetPos;
	}
}
