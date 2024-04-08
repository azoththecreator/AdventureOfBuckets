using DG.Tweening;
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

	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Sprite defaultSprite, chargeSprite;
	[SerializeField] ParticleSystem splash;

	[SerializeField] int speed;
	[SerializeField] int jump;
	public bool isCharging = false;
	public bool isJumping = true;

	AudioSource audioSource;
	public AudioClip fxLand, fxJump;
	public AudioSource springAudioSource;

	public float dist;
	public DistanceJoint2D dj;
	float maxDist = 6, defaultDist = 3;
	public bool isFlying = false;
	public PlayerMovement opponent;

	public PlayerManager playerManager;

	public bool quicksand = false;

	public bool isEnding = false;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

		opponent = tr2.GetComponent<PlayerMovement>();

		audioSource = GetComponent<AudioSource>();
    }

	void Update()
	{
		if (!isEnding)
		{
			if (playerManager.isFollowing)
			{
				if (!isCharging)
				{
					if (quicksand)
					{
						if (Input.GetKey(left))
						{
							tr.Translate(-speed * .5f * Time.deltaTime, 0, 0);
							if (tr.localScale.x == 1)
								tr.DOScaleX(-1, 0);
						}
						if (Input.GetKey(right))
						{
							tr.Translate(speed * .5f * Time.deltaTime, 0, 0);
							if (tr.localScale.x == -1)
								tr.DOScaleX(1, 0);
						}

						if (Input.GetKeyDown(up))
						{
							if (!isJumping)
							{
								audioSource.clip = fxJump;
								audioSource.Play();
								rb.AddForce(Vector2.up * jump * .5f, ForceMode2D.Impulse);
							}
						}
					}
					else
					{
						// 좌우 움직임
						if (Input.GetKey(left))
						{
							tr.Translate(-speed * Time.deltaTime, 0, 0);
							if (tr.localScale.x == 1)
								tr.DOScaleX(-1, 0);
						}
						if (Input.GetKey(right))
						{
							tr.Translate(speed * Time.deltaTime, 0, 0);
							if (tr.localScale.x == -1)
								tr.DOScaleX(1, 0);
						}

						// 점프
						if (Input.GetKeyDown(up))
						{
							if (!isJumping)
							{
								audioSource.clip = fxJump;
								audioSource.Play();
								rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
							}
						}
					}
				}

				// 고정
				if (Input.GetKeyDown(down) && !opponent.isCharging && !isFlying && !opponent.isFlying)
				{
					isCharging = true;
					rb.constraints = RigidbodyConstraints2D.FreezeAll;
					dj.distance = maxDist;
					spriteRenderer.sprite = chargeSprite;
				}

				if (Input.GetKeyUp(down) && !opponent.isCharging && !isFlying && !opponent.isFlying)
				{
					isCharging = false;
					spriteRenderer.sprite = defaultSprite;
					if (dist > 4)
					{
						rb2.velocity = Vector2.zero;
						rb2.AddForce((tr.position - tr2.position) * (dist - 1), ForceMode2D.Impulse);
						isFlying = true;
						springAudioSource.Play();
					}
					else
					{
						rb.constraints = RigidbodyConstraints2D.FreezeRotation;
						tr.rotation = Quaternion.Euler(Vector3.zero);
						dj.distance = defaultDist;
						isFlying = true;
					}
				}

				// 거리가 줄보다 길고 멀어질 경우 이동속도 느려짐
				bool goingFarther = isGoingFarther();
				if (dist > 3 && speed == 4 && goingFarther)
					speed = 2;
				else if (speed == 2 && (dist <= 3 || !goingFarther))
					speed = 4;

				// 차지 후 발사
				if (isFlying && dj.distance == maxDist)
				{
					rb.constraints = RigidbodyConstraints2D.FreezeRotation;
					tr.rotation = Quaternion.Euler(Vector3.zero);
					isJumping = true;
					opponent.isJumping = true;
					dj.distance = defaultDist;
				}

				if (Input.GetKeyDown(KeyCode.R))
				{
					playerManager.Respawn();
				}
			}
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Respawn"))
		{
			collision.GetComponent<CheckpointManager>().Save(playerManager.respawnPos);
			playerManager.respawnPos = collision.transform.position;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Terrain"))
		{
			if (isJumping)
			{
				isJumping = false;
				audioSource.clip = fxLand;
				audioSource.Play();
				splash.Play();
			}
			if (isFlying)
				isFlying = false;
			if (opponent.isFlying)
				opponent.isFlying = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Terrain"))
			isJumping = true;
	}

	public void SuperJump(float strength)
	{
		rb.velocity = Vector3.zero;
		rb2.velocity = Vector3.zero;
		rb.AddForce(Vector2.up * strength, ForceMode2D.Impulse);
		rb2.AddForce(Vector2.up * strength, ForceMode2D.Impulse);

		if (isFlying)
			isFlying = false;
		if (opponent.isFlying)
			opponent.isFlying = false;
	}
}
