using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed;
    Vector2 startPosition;

    [SerializeField] private bool canLoseHP;

    [SerializeField] private GameObject bubble;

    public delegate void TouchedGroundEvent();
    public static TouchedGroundEvent TouchedGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canLoseHP = true;
        bubble.SetActive(false);
    }

    private void Start()
    {
        startPosition = transform.position;
    }


    private void FixedUpdate()
    {
        setMinimumVelocity();
        setMaximumVelocity();
        //Debug.Log(Vector2.SqrMagnitude(rigidbody.velocity));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Paddle"))
        {
            if (collision.collider.offset.x < 0)
            {
                rb.AddForce((Vector2.left.normalized * 3f), ForceMode2D.Force);
            }
            else
            {
                rb.AddForce((Vector2.right.normalized * 3f), ForceMode2D.Force);
            }

        }
        else if (collision.gameObject.tag.Equals("Brick"))
        {
            rb.velocity *= 1.02f;

        }
        else if (collision.gameObject.tag.Equals("Ground"))
        {
            if (canLoseHP)
            {

                TouchedGround.Invoke();
                canLoseHP = false;
                StartCoroutine(TakeDamageDelay(true, 1.5f));
            }
        }
    }

    private IEnumerator TakeDamageDelay(bool value, float time)
    {
        bubble.SetActive(true);
        yield return new WaitForSeconds(time);
        this.canLoseHP = value;
        bubble.SetActive(false);
    }

    void setMinimumVelocity()
    {
        float minSpeed = 65f;
        float speed = Vector2.SqrMagnitude(rb.velocity);

        if (speed < minSpeed)
        {

            rb.velocity *= 1.05f;
        }

        if (rb.velocity.y >= 0 && rb.velocity.y < 0.5f)
        {
            rb.AddForce(Vector2.up.normalized);
        }
        else if (rb.velocity.y < 0 && rb.velocity.y > -0.5f)
        {
            rb.AddForce(Vector2.down.normalized);
        }

        if (rb.velocity.x == 0)
        {
            StartCoroutine(horizontalCheck());
        }
    }

    void setMaximumVelocity()
    {
        float maxSpeed = 300f;
        float speed = Vector2.SqrMagnitude(rb.velocity);

        if (speed > maxSpeed)
        {

            rb.velocity *= 0.95f;
        }

    }


    IEnumerator horizontalCheck()
    {
        //Debug.Log($"At: {this}, started horizontalCheck, vel: {rigidbody.velocity}");
        yield return new WaitForSeconds(5f);

        if (rb.velocity.x == 0)
        {
            rb.velocity = new Vector2(Random.Range(-1f, 1f), rb.velocity.y);
        }

        //Debug.Log($"At: {this}, ended horizontalCheck, vel: {rigidbody.velocity}");
    }

    public void InitBall()
    {
        //rigidbody.AddForce(Vector2.down.normalized * speed, ForceMode2D.Impulse);
    }

    private void OnEnable()
    {
        // Debug.Log($"{rigidbody.velocity}");
        rb.AddForce(Vector2.down.normalized * speed, ForceMode2D.Impulse);
        //Debug.Log($"{rigidbody.velocity}");
    }


}
