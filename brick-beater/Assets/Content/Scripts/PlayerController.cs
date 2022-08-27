using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerContrls;
    [SerializeField] ControlsStats controlsStats;

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

    }


    private void Update(){
        

        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        
        var newPos = new Vector2(transform.position.x + (playerContrls.Controls.Movement.ReadValue<Vector2>().x * controlsStats.Speed * Time.deltaTime), transform.position.y);

        transform.position = newPos;


    }

   


    


}
