using System.Collections;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    private static bool canRespPowerup;
    private static bool isCooldownStarted;
    //[SerializeField] private List<GameObject> powerupList = new List<GameObject>();

    private SpriteRenderer sr;
    [SerializeField] private int hits;
    Color orange = new Color(255, 165, 0, 1);

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        canRespPowerup = true;
        isCooldownStarted = false;
    }



    public void SetHits(int hits)
    {
        this.hits = hits;
        SetColor(hits);
    }



    public void TakeDamage()
    {
        hits--;

        SetColor(hits);
        CheckIfCanPlay(hits);
    }
    void SetColor(int hits)
    {
        switch (hits)
        {
            case 1: sr.color = Color.blue; break;
            case 2: sr.color = Color.yellow; break;
            case 3: sr.color = orange; break;
            case 4: sr.color = Color.red; break;

        }

    }

    void CheckIfCanPlay(int hits)
    {
        if (hits <= 0)
        {
            BrickManager.Instance.DeleteBrickFromList(this.gameObject);
            //Debug.Log($"At {this.GetInstanceID()}, {hits}");
            var pos = transform.position;
            Destroy(this.gameObject);
            DrawPowerUp(pos);

            GameManager.Instance.AddScore();
            BrickManager.Instance.CheckBricksOnScene();

        }
    }

    void DrawPowerUp(Vector2 pos)
    {
        if (canRespPowerup)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                Debug.Log("Powerup resped");
                Instantiate(GameManager.Instance.PowerUpsList[Random.Range(0, GameManager.Instance.PowerUpsList.Count)], pos, Quaternion.identity);
                canRespPowerup = false;
                isCooldownStarted = true;
                StartCoroutine(PowerUpCooldown());
            }
        }
    }

    static IEnumerator PowerUpCooldown()
    {
        if (!isCooldownStarted)
        {
            yield return new WaitForSeconds(10f);
            canRespPowerup = true;
            isCooldownStarted = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            TakeDamage();
        }
    }

    private void OnEnable()
    {
        BrickManager.Instance.AddToBricksList(this.gameObject);
    }

}
