using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
	public Transform tr2;
	public Rigidbody2D rb2;
    public KeyCode left, right, up, down;

	[SerializeField] int speed;
	[SerializeField] int jump;
	[SerializeField] public bool canMove = true;
	[SerializeField] public bool isJumping = true;

	public float dist;
	public DistanceJoint2D dj;
	float maxDist = 6, defaultDist = 3;
	[SerializeField] bool isFlying = false;
	public PlayerMovement opponent;

	public PlayerManager playerManager;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

		opponent = tr2.GetComponent<PlayerMovement>();
    }

	void Update()
	{
		if (canMove)
		{
			// 좌우 움직임
			if (Input.GetKey(left))
				tr.Translate(-speed * Time.deltaTime, 0, 0);
			if (Input.GetKey(right))
				tr.Translate(speed * Time.deltaTime, 0, 0);

			// 점프
			if (Input.GetKeyDown(up))
			{
				if (!isJumping)
				{
					rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
					isJumping = true;
				}
			}
		}

		// 고정
		if (Input.GetKeyDown(down) && opponent.canMove)
		{
			canMove = false;
			rb.constraints = RigidbodyConstraints2D.FreezePosition;
			dj.distance = maxDist;
		}

		if (Input.GetKeyUp(down))
		{
			canMove = true;
			if (dist > 4)
			{
				rb2.velocity = Vector2.zero;
				rb2.AddForce((tr.position - tr2.position) * (dist - 1), ForceMode2D.Impulse);
				isFlying = true;
			}
			else
			{
				rb.constraints = RigidbodyConstraints2D.None;
				rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				dj.distance = defaultDist;
			}
		}

		// 거리가 줄보다 길고 멀어질 경우 이동속도 느려짐
		bool goingFarther = isGoingFarther();
		if (dist > 3 && speed == 4 && goingFarther)
			speed = 2;
		else if (speed == 2 && (dist <= 3 || !goingFarther))
			speed = 4;

		// 차지 후 발사
		if (isFlying && (Mathf.Abs(tr.position.x - tr2.position.x) < .5f && Mathf.Abs(tr.position.y - tr2.position.y) < .5f))
		{
			rb.constraints = RigidbodyConstraints2D.None;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			isJumping = true;
			opponent.isJumping = true;
			isFlying = false;
			dj.distance = defaultDist;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			playerManager.Respawn();
		}
    }

	bool isGoingFarther()
	{
		float distTemp = Vector3.Distance(tr.position, tr2.position);
		if (distTemp < dist)
		{
			dist = distTemp;
			return false;
		}
		else
		{
			dist = distTemp;
			return true;
		}
	}

	private void FixedUpdate()
    {
		// 점프 체크
		Debug.DrawRay(rb.position, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Terrain"));
        if (rayHit.collider != null && rb.velocity.y < 0)
            if (isJumping)
                isJumping = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Respawn"))
		{
			playerManager.respawnPos = collision.transform.position;
		}
	}
}
