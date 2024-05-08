using UnityEngine;
using Spine.Unity;
/// <summary>
/// This class manages arrow animation and movement
/// </summary>
public class Arrow : MonoBehaviour
{
    /// <summary>
    /// Arrow SkeletonAnimation component
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    /// <summary>
    /// Arrow attack animation
    /// </summary>
    [SerializeField] private AnimationReferenceAsset attack;

    [SerializeField] private Coin CoinPrefab;

    private Rigidbody2D rb;
    private Spine.AnimationState animationState;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationState = skeletonAnimation.AnimationState;      

    }
    void FixedUpdate()
    {
        if (rb)
        {
            // rotating the arrow while it's moving
            transform.right = rb.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        animationState.AddAnimation(0, attack, false, 0);
        GetComponent<Collider2D>().enabled = false;
        if (rb)
        {
            rb.velocity = Vector2.zero;
        }
        if (CoinPrefab)
        {
            CreateCoin();
        }
        Destroy(gameObject, attack.Animation.Duration);
    }

    private void CreateCoin()
    {
        Coin coin = Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        coin.AnimateCoin();
    }
}
