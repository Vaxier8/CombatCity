using UnityEngine;

public class CoinColect : MonoBehaviour
{
    public float checkRadius = 0.5f; 
    public LayerMask playerLayer; 
    private ScoreBoardManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreBoardManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreBoardManager not found in the scene.");
        }
    }

    void Update()
    {
        CollectCoin();
    }

    void CollectCoin()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);
        if (playerCollider != null && playerCollider.CompareTag("Player"))
        {
            Debug.Log("Player has collected the coin");
            if (scoreManager != null)
            {
                scoreManager.addScore(10);
            }
            Destroy(gameObject);
        }
    }


}
