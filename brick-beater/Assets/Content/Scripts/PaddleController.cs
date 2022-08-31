using System.Collections;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] ControlsStats controlsStats;

    [SerializeField] private Vector2 startingPos;

    private float height;
    private float startingSpeed;



    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        height = transform.position.y;
        startingSpeed = controlsStats.Speed;
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
        Holdball();
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

            var newPos = new Vector2(transform.position.x + (playerControls.Controls.Movement.ReadValue<Vector2>().x * controlsStats.Speed * Time.deltaTime), height);

            transform.position = newPos;
        }
    }

    void Holdball()
    {
        if (playerControls.Controls.Holdball.IsPressed())
        {
            Debug.Log($"at {this}, {playerControls.Controls.Holdball.name} {playerControls.Controls.Holdball.triggered}");
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
        controlsStats.Speed = startingSpeed;
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
        }
    }

    private IEnumerator speedUp()
    {
        controlsStats.Speed *= 2f;
        yield return new WaitForSeconds(6f);
        controlsStats.Speed = startingSpeed;

    }

}
