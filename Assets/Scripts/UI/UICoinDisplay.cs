using UnityEngine;
using TMPro;

public class UICoinDisplay : MonoBehaviour
{
    [SerializeField] private int MaxValueCoin = 1000;
    [SerializeField] private Transform coinTransform;


    private TextMeshProUGUI coinCounterText;
    private Vector3 coinWorldPos;
    public Vector3 CoinWorldPos => coinWorldPos;


    private void Awake()
    {
        coinCounterText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {        
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, coinTransform.position);
        coinWorldPos = Camera.main.ScreenToWorldPoint(screenPos);   
    }
    public void UpdateText()
    {
        coinCounterText.text = GameManager.Coins.ToString();
    }
}
