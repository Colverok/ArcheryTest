using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int maxCoinValue = 1000;
    [SerializeField] private UICoinDisplay coinDisplay;
    private int coins;
    public static int Coins
    {
        get { return Instance.coins; }
        set { Instance.coins = value; }
    }
    public static Vector3 UICoinPosition
    {
        get { return Instance.coinDisplay.CoinWorldPos;}
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetCoins();
    }

    public static void AddCoins(int count)
    {
        Coins = Mathf.Clamp(Coins + count, 0, Instance.maxCoinValue);
        Instance.coinDisplay.UpdateText();
    }

    public static void ResetCoins()
    {
        Coins = 0;
        Instance.coinDisplay.UpdateText();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }


}
