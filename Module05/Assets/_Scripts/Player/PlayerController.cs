using System.Collections;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
	[SerializeField] float jumpForce = 5f;
	[SerializeField] float damageForce = 5f;
    [SerializeField] float damageJump = 2f;
	[SerializeField] GameObject foot;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;
	private bool isGrounded = true;
	private bool isStun = false;
	private BoxCollider2D bc;
	private bool isRespawn = true;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		if (isRespawn)
			animator.SetTrigger("Respawn");
		bc = GetComponent<BoxCollider2D>();
		foot.GetComponent<Foot>().onGrounded += GroundEnter;
		GameManager.OnPlayerDeath += PlayDieAnimation;
		GameManager.OnStageClear += SetStun;
		Debug.Log("PlayerController Start");
    }

    void Update()
    {
		if (isStun)
			return;
		float move = Input.GetAxis("Horizontal");
        animator.SetFloat("Move", Mathf.Abs(move));
		if (move != 0)
		{
			rb.velocity = new Vector2(move * speed, rb.velocity.y);
		}

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			AudioManager.instance.PlayJump();
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			animator.SetBool("IsJump", true);
			isGrounded = false;
		}

		//debug
		// if (Input.GetKeyDown(KeyCode.Alpha1))
		// {
		// 	TakeDamage(1);
		// }

		//낙사 처리
		if (transform.position.y < -10f)
		{
			GameManager.instance.FallDeath();
			isStun = true;
		}

		FollowingCamera();
	}
	
	public void SetStun()
	{
		Debug.Log("PlayerController SetStun");
		rb.velocity = new Vector2(0, 0);
		isStun = true;
	}

	void OnDestroy()
	{
		Debug.Log("PlayerController OnDestroy");
		GameManager.OnPlayerDeath -= PlayDieAnimation;
		foot.GetComponent<Foot>().onGrounded -= GroundEnter;
		GameManager.OnStageClear -= SetStun;
	}

	public void SetIsRespawn(bool isRespawn)
	{
		this.isRespawn = isRespawn;
	}

	void FollowingCamera()
	{
		float camY = transform.position.y;
		if (camY < StageManager.instance.GetMapBottomCameraLimit())
			camY = StageManager.instance.GetMapBottomCameraLimit();
		else if (camY > StageManager.instance.GetMapTopCameraLimit())
			camY = StageManager.instance.GetMapTopCameraLimit();
		float camX = transform.position.x;
		if (camX < StageManager.instance.GetMapLeftCameraLimit())
			camX = StageManager.instance.GetMapLeftCameraLimit();
		else if (camX > StageManager.instance.GetMapRightCameraLimit())
			camX = StageManager.instance.GetMapRightCameraLimit();

		Camera.main.transform.position = new Vector3(camX, camY, Camera.main.transform.position.z);
	}

	void GroundEnter()
	{
		isGrounded = true;
		animator.SetBool("IsJump", false);
		bc.offset = new Vector2(0, 0.5081537f);
		bc.size = new Vector2(4.26f, 1.024271f);
	}

	void TakeDamage(int direction)
	{
		if (isStun)
			return;
		AudioManager.instance.PlayTakeDamage();
		rb.velocity = new Vector2(0, rb.velocity.y);
		animator.SetBool("IsJump", false);
		animator.SetBool("TakeDamage", true);
		if (facingRight && direction == 1 || !facingRight && direction == -1)
			Flip();
		Vector2 damageDirection = new Vector2(direction * damageForce, damageJump);
		rb.velocity = new Vector2(rb.velocity.x + damageDirection.x, rb.velocity.y + damageDirection.y);
		GameManager.instance.TakeDamage();
		StartCoroutine(Stun(1f));
	}

	IEnumerator Stun(float time)
	{
		isStun = true;
		yield return new WaitForSeconds(time);
		animator.SetBool("TakeDamage", false);
		isStun = false;
	}

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			Transform enemy = other.gameObject.transform;
			int d = transform.position.x > enemy.position.x ? 1 : -1;
			TakeDamage(d);
		} else if (other.gameObject.tag == "Leaf") {
			StageManager.instance.GetLeaf(other.gameObject.GetComponent<Leaf>().GetNum());
			AudioManager.instance.PlayGetLeaf();
		}
	}

	public void JumpCollider1()
	{
		bc.offset = new Vector2(-.008757353f, 1.147425f);
		bc.size = new Vector2(3.769601f, 2.302814f);
	}

	public void JumpCollider2()
	{
		bc.offset = new Vector2(0.004378557f, 1.432032f);
		bc.size = new Vector2(3.49813f, 2.872027f);
	}

	public void JumpCollider3()
	{
		bc.offset = new Vector2(-0.03048396f, 1.920112f);
		bc.size = new Vector2(2.452243f, 3.848188f);
	}

	public void PlayDieAnimation()
	{
		Debug.Log("PlayerController PlayDieAnimation");
		rb.velocity = new Vector2(0, 0);
		isStun = true;
		if (animator != null)
			animator.SetTrigger("Die");
		bc.enabled = false;
	}

	public bool GetIsStun()
	{
		return isStun;
	}
}
