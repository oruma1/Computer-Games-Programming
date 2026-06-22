using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Run Balance")]
    public float baseSpeed = 8f;
    public float maxSpeed = 18f;
    public float difficultyRampPerSecond = 0.035f;
    public int coinValue = 10;
    public float factBoostDuration = 3.2f;

    [Header("UI")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text coinText;
    public TMP_Text bestText;
    public TMP_Text factText;
    public TMP_Text boostText;
    public TMP_Text gameOverText;

    [Header("Nairobi Facts")]
    [TextArea] public List<string> nairobiFacts = new()
    {
        "Nairobi means ‘cool water’ from the Maasai phrase Enkare Nairobi.",
        "Nairobi began as a railway depot in 1899 before becoming Kenya’s capital.",
        "Nairobi National Park sits just outside the city centre.",
        "KICC is one of Nairobi’s most recognizable skyline landmarks.",
        "Matatus are famous for bold art, music, and Nairobi street style.",
        "Karura Forest is one of Nairobi’s major urban forests.",
        "Nairobi is often called the Green City in the Sun.",
        "The Giraffe Centre helps protect endangered Rothschild’s giraffes."
    };

    public bool IsRunning { get; private set; }
    public bool IsGameOver { get; private set; }
    public float CurrentSpeed { get; private set; }
    public float FactBoostRemaining { get; private set; }

    int score;
    int coins;
    int best;
    int streak;
    float runTime;
    float factMessageTimer;

    const string BestKey = "MatatuCoinRushBest";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        best = PlayerPrefs.GetInt(BestKey, 0);
        CurrentSpeed = baseSpeed;
        Time.timeScale = 0f;
        ShowStart();
        UpdateUI();
    }

    void Update()
    {
        if (!IsRunning) return;

        runTime += Time.deltaTime;
        CurrentSpeed = Mathf.Min(maxSpeed, baseSpeed + runTime * difficultyRampPerSecond * baseSpeed);
        score += Mathf.RoundToInt((CurrentSpeed * Time.deltaTime) * (1 + streak / 10));

        if (FactBoostRemaining > 0f)
            FactBoostRemaining -= Time.deltaTime;

        if (factMessageTimer > 0f)
            factMessageTimer -= Time.deltaTime;

        UpdateUI();
    }

    public void StartRun()
    {
        score = 0;
        coins = 0;
        streak = 0;
        runTime = 0f;
        FactBoostRemaining = 0f;
        IsRunning = true;
        IsGameOver = false;
        Time.timeScale = 1f;

        if (startPanel) startPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (factText) factText.gameObject.SetActive(false);
    }

    public void RestartRun()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CollectCoin(Vector3 worldPosition)
    {
        streak++;
        coins++;
        int multiplier = Mathf.Clamp(1 + streak / 8, 1, 5);
        int boostMultiplier = FactBoostRemaining > 0f ? 2 : 1;
        score += coinValue * multiplier * boostMultiplier;
        UpdateUI();
    }

    public void CollectFactPowerUp()
    {
        FactBoostRemaining = factBoostDuration;
        score += 50;

        if (nairobiFacts.Count > 0 && factText)
        {
            factText.text = nairobiFacts[Random.Range(0, nairobiFacts.Count)];
            factText.gameObject.SetActive(true);
            factMessageTimer = 4.5f;
        }

        UpdateUI();
    }

    public void HitObstacle()
    {
        if (!IsRunning || IsGameOver) return;

        if (FactBoostRemaining > 0f)
        {
            score += 35;
            return;
        }

        IsRunning = false;
        IsGameOver = true;
        Time.timeScale = 0f;

        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt(BestKey, best);
            PlayerPrefs.Save();
        }

        if (gameOverText)
            gameOverText.text = $"Score: {score}\nBest: {best}";

        if (gameOverPanel) gameOverPanel.SetActive(true);
        UpdateUI();
    }

    void ShowStart()
    {
        if (startPanel) startPanel.SetActive(true);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = score.ToString();
        if (coinText) coinText.text = coins.ToString();
        if (bestText) bestText.text = best.ToString();

        if (boostText)
        {
            bool boosted = FactBoostRemaining > 0f;
            boostText.gameObject.SetActive(boosted);
            if (boosted) boostText.text = $"Fact boost {FactBoostRemaining:0.0}s";
        }

        if (factText && factText.gameObject.activeSelf && factMessageTimer <= 0f)
            factText.gameObject.SetActive(false);
    }
}
