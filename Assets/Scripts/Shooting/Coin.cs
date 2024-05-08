using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float coinAnimationSpeed = 1f;

    public void AnimateCoin()
    {
        StartCoroutine(CoinAnimation());
    }

    private IEnumerator CoinAnimation()
    {
        Vector3 startPosition = transform.position;
        Vector3 coinCounterWorldPosition = startPosition;

        if (GameManager.UICoinPosition != null)
        {
            coinCounterWorldPosition = GameManager.UICoinPosition;
        }

        float timer = 0;
        float animationTime = Vector3.Distance(coinCounterWorldPosition, startPosition) / coinAnimationSpeed;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, coinCounterWorldPosition, timer / animationTime);
            yield return null;
        }

        transform.position = coinCounterWorldPosition;
        GameManager.AddCoins(1);
        Destroy(gameObject);
    }
}
