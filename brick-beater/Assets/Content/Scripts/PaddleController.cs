using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private PlayerControls playerControls;
    [SerializeField] ControlsStats controlsStats;

    private float height;



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
    }


    private void Update()
    {
        Move();
        Interactions();
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
        if (playerControls.Controls.PauseMenu.triggered)
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
}
