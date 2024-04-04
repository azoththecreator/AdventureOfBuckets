using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Camera mainCam;
	Transform mainCamTr;
	float offsetY = 1;
	float camSpeed = 5;
	float defaultSize = 7;

	public Transform p1, p2;
	Rigidbody2D rb1, rb2;
	PlayerMovement playerMovement;

	public Transform spring;
	float radToDeg = 57.2958f;
	public SpriteRenderer[] springSprites;

	public Vector2 respawnPos;

	void Awake()
    {
		mainCamTr = mainCam.transform;
		playerMovement = p1.GetComponent<PlayerMovement>();

		rb1 = p1.GetComponent<Rigidbody2D>();
		rb2 = p2.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// 스프링 스프라이트 정렬
		spring.position = new Vector3((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2, 1);
		spring.rotation = Quaternion.Euler(0, 0, radToDeg * Mathf.Atan2(p1.position.y - p2.position.y, p1.position.x - p2.position.x));
		if (spring.localEulerAngles.z >= 90 && spring.localEulerAngles.z <= 270 && spring.localScale.y == 1)
			spring.localScale = new Vector3(1, -1);
		else if ((spring.localEulerAngles.z >= 270 || spring.localEulerAngles.z <= 90) && spring.localScale.y == -1)
			spring.localScale = new Vector3(1, 1);

		for (int i = 0; i < springSprites.Length; i++)
			springSprites[i].transform.position = new Vector3(p1.position.x * i / (springSprites.Length - 1) + p2.position.x * (springSprites.Length - 1 - i) / (springSprites.Length - 1), p1.position.y * i / (springSprites.Length - 1) + p2.position.y * (springSprites.Length - 1 - i) / (springSprites.Length - 1), 1);
	}

	private void FixedUpdate()
	{
		// 차지할 때 줌아웃
		if (playerMovement.dist > 3)
			mainCam.orthographicSize = defaultSize + (playerMovement.dist - 3);
	}

	private void LateUpdate()
    {
        // 카메라가 플레이어를 따라감
        Vector3 targetPos = new Vector3((p1.position.x + p2.position.x) / 2, (p1.position.y + p2.position.y) / 2 + offsetY, mainCamTr.position.z);
        
		// 차지할 때
		if (!playerMovement.canMove)
			targetPos = new Vector3(p1.position.x, p1.position.y, mainCamTr.position.z);
		else if (!playerMovement.opponent.canMove)
			targetPos = new Vector3(p2.position.x, p2.position.y, mainCamTr.position.z);

		mainCamTr.Translate((targetPos - mainCamTr.position) * camSpeed * Time.deltaTime);
	}

	public void Respawn()
	{
		rb1.velocity = Vector2.zero;
		rb2.velocity = Vector2.zero;
		p1.transform.position = new Vector3(respawnPos.x - .5f, respawnPos.y + 3, 0);
		p2.transform.position = new Vector3(respawnPos.x + .5f, respawnPos.y + 3, 0);
	}
}
