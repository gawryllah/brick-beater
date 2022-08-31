using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] ControlsStats controlsStats;

    [SerializeField] private Vector2 startingPos;

    private float height;
    private float baseSpeed;
    private float baseWidth;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        baseSpeed = controlsStats.Speed;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        baseWidth = transform.localScale.x;
        height = transform.position.y;
 
        startingPos = transform.position;

        BrickManager.Instance.OnSpawnedBricks += RestartPos;
    }


    private void Update()
    {
        if (GameManager.Instance.GameOn)
        {
            Move();
            Interactions();
        }
    }

    void Interactions()
    {
        //Holdball();
        PauseGame();
    }

    void Move()
    {

        if (transform.position.x > controlsStats.MaxDelta)
        {
            transform.position = new Vector2(controlsStats.MaxDelta, height);
        }

        if (transform.position.x < -controlsStats.MaxDelta)
        {
            transform.position = new Vector2(-controlsStats.MaxDelta, height);

        }
        else
        {

            var newPos = new Vector2(transform.position.x + (playerControls.Controls.Movement.ReadValue<Vector2>().x * baseSpeed * Time.deltaTime), height);

            transform.position = newPos;
        }
    }

    void PauseGame()
    {
        if (playerControls.Controls.PauseMenu.triggered && !UIManager.Instance.GameOverViewOpened)
        {
            if (UIManager.Instance.PauseMenuOpened)
            {
                GameManager.Instance.ClosePauseMenu();
            }
            else
            {
                GameManager.Instance.OpenPauseMenu();
            }
        }
    }

    void RestartPos()
    {

        transform.position = startingPos;
        baseSpeed = controlsStats.Speed;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Heart")
        {
            //Debug.Log("heal trigger detected");
            Destroy(collision.gameObject);
            GameManager.Instance.AddHealth();

        }
        else if (collision.gameObject.tag == "Speed")
        {
            //Debug.Log("speed trigger detected");
            Destroy(collision.gameObject);
            StartCoroutine(speedUp());
        }else if (collision.gameObject.tag == "Shroom")
        {
            Destroy(collision.gameObject);
            var width = transform.localScale;

            width.x *= 1.25f;

            transform.localScale = width;
            StartCoroutine(expandCD());
        }
    }

    private IEnumerator speedUp()
    {
        
        baseSpeed *= 2f;
        yield return new WaitForSeconds(6f);
        baseSpeed = controlsStats.Speed;

    }

    private IEnumerator expandCD()
    {
        yield return new WaitForSeconds(5f);
        transform.localScale = new Vector3(baseWidth, transform.localScale.y, transform.localScale.z);   
    }


}
