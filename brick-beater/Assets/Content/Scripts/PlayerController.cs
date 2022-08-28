using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerContrls;
    [SerializeField] ControlsStats controlsStats;

    private float height;



    private void Awake()
    {
        playerContrls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerContrls.Enable();
    }

    private void OnDisable()
    {
        playerContrls.Disable();
    }

    private void Start()
    {
        height = transform.position.y;
    }


    private void Update()
    {

        Holdball();
        Move();
    }



    void Move()
    {

        if (transform.position.x > controlsStats.MaxDelta) {
            var newPos2 = new Vector2(transform.position.x - 0.03f, height);
            transform.position = newPos2;
        }

        if(transform.position.x < -controlsStats.MaxDelta)
        {
            var newPos2 = new Vector2(transform.position.x + 0.03f, height);
            transform.position = newPos2;

        }

        var newPos = new Vector2(transform.position.x + (playerContrls.Controls.Movement.ReadValue<Vector2>().x * controlsStats.Speed * Time.deltaTime), height);

        transform.position = newPos;

    }

    void Holdball()
    {
        if (playerContrls.Controls.Holdball.IsPressed())
        {
            Debug.Log($"at {this}, {playerContrls.Controls.Holdball.name} {playerContrls.Controls.Holdball.triggered}");
        }
    }
}
