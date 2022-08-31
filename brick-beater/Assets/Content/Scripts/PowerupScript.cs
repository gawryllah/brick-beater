using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    enum PowerupLabel { HEAL, SPEED };

    [SerializeField] private PowerupLabel powerupLabel;

    float speed = 1.5f;



    private void FixedUpdate()
    {
        var pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;

        if (transform.position.y < -6)
        {
            Destroy(transform.gameObject);
        }
    }


}
