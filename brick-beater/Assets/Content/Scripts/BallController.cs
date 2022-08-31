using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed;
    Vector2 startPosition;

    [SerializeField] private bool canLoseHP;

    [SerializeField] private GameObject bubble;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    public delegate void BallEvent();
    public static BallEvent TouchedGround;
    public static BallEvent BallSpawned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        canLoseHP = true;
        bubble.SetActive(false);

        UIManager.Instance.CountdownFinished += InitBall;
    }

    private void Start()
    {
        startPosition = transform.position;
        minSpeed = 65f;
        maxSpeed = 200f;
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.GameOn)
        {
            setMinimumVelocity();
            setMaximumVelocity();
        }
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
                canLoseHP = false;
                TouchedGround.Invoke();

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

        float speed = Vector2.SqrMagnitude(rb.velocity);

        if (speed > maxSpeed)
        {

            rb.velocity *= 0.95f;
        }

    }


    IEnumerator horizontalCheck()
    {
        //Debug.Log($"At: {this}, started horizontalCheck, vel: {rigidbody.velocity}");
        yield return new WaitForSeconds(3.5f);

        if (rb.velocity.x == 0)
        {
            rb.velocity = new Vector2(Random.Range(-1f, 1f), rb.velocity.y);
        }

        //Debug.Log($"At: {this}, ended horizontalCheck, vel: {rigidbody.velocity}");
    }

    public void InitBall()
    {
        GameManager.Instance.GameOn = true;
        rb.AddForce(Vector2.down.normalized * speed, ForceMode2D.Impulse);
    }

    private void OnEnable()
    {
        BallSpawned?.Invoke();
    }


}
