using UnityEngine;

public class BrickScript : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private int hits;
    Color orange = new Color(255, 165, 0, 1);

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
            //Debug.Log($"At {this.GetInstanceID()}, {hits}");
            Destroy(this.gameObject);
            GameManager.Instance.AddScore();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            TakeDamage();
        }
    }

}
