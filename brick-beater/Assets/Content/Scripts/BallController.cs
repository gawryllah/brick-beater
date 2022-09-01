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

        canLoseHP = true;
        bubble.SetActive(false);



        startPosition = transform.position;
        minSpeed = 65f;
        maxSpeed = 200f;



        //UIManager.Instance.CountdownFinished += InitBall;
        //GameManager.Instance.OnCountdownEnd += InitBall;
    }

    private void OnEnable()
    {

        rb = GetComponent<Rigidbody2D>();
        BallSpawned?.Invoke();

        GameManager.Instance.OnBallSpawned += RestartBall;
        UIManager.Instance.CountdownFinished += InitBall;

    }

    private void OnDisable()
    {
        GameManager.Instance.OnBallSpawned -= RestartBall;
        UIManager.Instance.CountdownFinished -= InitBall;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameOn)
        {
            setMinimumVelocity();
            setMaximumVelocity();
        }
   ;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Paddle"))
        {
            if (collision.collider.offset.x < 0)
            {

                rb.AddForce((Vector2.left.normalized * 5f), ForceMode2D.Force);
            }
            else
            {

                rb.AddForce((Vector2.right.normalized * 5f), ForceMode2D.Force);
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

        yield return new WaitForSeconds(3.5f);

        if (rb.velocity.x == 0)
        {
            rb.velocity = new Vector2(Random.Range(-1f, 1f), rb.velocity.y);
        }


    }

    public void InitBall()
    {

        GameManager.Instance.ResumeGame();
        GameManager.Instance.GameOn = true;


        rb.AddForce(Vector2.down.normalized * speed / 2, ForceMode2D.Impulse);

    }

    public void RestartBall()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;

        BallSpawned?.Invoke();
    }
}
