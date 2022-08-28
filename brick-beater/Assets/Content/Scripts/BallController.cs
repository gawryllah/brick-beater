using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    [SerializeField] float speed;
    Vector2 direction;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.AddForce(Vector2.down.normalized * speed * Time.deltaTime, ForceMode2D.Impulse);

    }


    private void FixedUpdate()
    {
        setMinimumVelocity();
        setMaximumVelocity();
        Debug.Log(Vector2.SqrMagnitude(rigidbody.velocity));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Paddle"))
        {
            if (collision.collider.offset.x < 0)
            {

                rigidbody.AddForce((Vector2.left.normalized * 3f), ForceMode2D.Force);

            }
            else
            {


                rigidbody.AddForce((Vector2.right.normalized * 3f), ForceMode2D.Force);

            }

        }
        else if (collision.gameObject.tag.Equals("Brick"))
        {
            rigidbody.velocity *= 1.02f;
        }
    }

    void setMinimumVelocity()
    {
        if (rigidbody.velocity.y > 0 && rigidbody.velocity.y < 0.35f)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 1f);
        }
        else if (rigidbody.velocity.y < 0 && rigidbody.velocity.y > -0.35f)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, -1f);
        }

        if (rigidbody.velocity.x == 0)
        {
            StartCoroutine(horizontalCheck());
        }
    }

    void setMaximumVelocity()
    {
        float maxSpeed = 300f;
        float speed = Vector2.SqrMagnitude(rigidbody.velocity);

        if (speed > maxSpeed)
        {

            rigidbody.velocity *= 0.95f;
        }

    }
    

    IEnumerator horizontalCheck()
    {
        Debug.Log($"At: {this}, started horizontalCheck, vel: {rigidbody.velocity}");
        yield return new WaitForSeconds(5f);

        if (rigidbody.velocity.x == 0)
        {
            rigidbody.velocity = new Vector2(Random.Range(-1f, 1f), rigidbody.velocity.y);
        }

        Debug.Log($"At: {this}, ended horizontalCheck, vel: {rigidbody.velocity}");
    }


}
