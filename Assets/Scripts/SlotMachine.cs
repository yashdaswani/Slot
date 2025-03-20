using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    public Reel[] reels;
    public Button spinButton;
    public Button resetButton;
    public TMP_Text balanceText;
    public TMP_Text resultText;
    public TMP_Text betText;
    public Slider betSlider;
    public TMP_Text spinButtonText;
    public ParticleSystem[] particleSystems;

    private int balance = 100;
    private int currentBet = 10;
    private int minBet = 5;
    private int maxBet = 50;
    private bool isSpinning = false;
    private float reelStartDelay = 0.5f;

    void Start()
    {
        UpdateBalanceUI();
        UpdateBetUI();
        spinButton.onClick.AddListener(Spin);
        resetButton.onClick.AddListener(ResetGame);
        betSlider.minValue = minBet;
        betSlider.maxValue = maxBet;
        betSlider.value = currentBet;
        betSlider.onValueChanged.AddListener(UpdateBetAmount);
        SetButtonsInteractable(true);
        DisableParticleEffects();
    }

    private void UpdateBetAmount(float value)
    {
        currentBet = Mathf.RoundToInt(value);
        UpdateBetUI();
    }

    public void Spin()
    {
        if (balance < currentBet || isSpinning) return;

        balance -= currentBet;
        UpdateBalanceUI();
        resultText.text = "";
        isSpinning = true;
        SetButtonsInteractable(false);
        spinButtonText.text = "Spinning";
        AudioManager.Instance?.PlayUIButtonSound();
        AudioManager.Instance?.PlaySpinReelSound();
        StartCoroutine(SpinReels());
    }

    private IEnumerator SpinReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StartSpinning();
            yield return new WaitForSeconds(reelStartDelay);
        }
        float totalSpinTime = 2.5f + (reelStartDelay * (reels.Length - 1));
        yield return new WaitForSeconds(totalSpinTime);
        AudioManager.Instance?.StopSpinReelSound();
        CheckWin();
        isSpinning = false;
        SetButtonsInteractable(true);
        spinButtonText.text = "Spin";
    }

    private void CheckWin()
    {
        string firstSymbol = reels[0].GetCurrentSymbol();
        bool won = true;

        for (int i = 1; i < reels.Length; i++)
        {
            if (reels[i].GetCurrentSymbol() != firstSymbol)
            {
                won = false;
                break;
            }
        }

        if (won)
        {
            int winnings = currentBet * 5;
            balance += winnings;
            AudioManager.Instance?.PlayWinSound();
            resultText.text = $"You Win! +{winnings} credits";
            EnableParticleEffects();
        }
        else
        {
            resultText.text = "Try Again";
            AudioManager.Instance?.PlayLoseSound();
            DisableParticleEffects();
        }
        UpdateBalanceUI();
    }

    public void ResetGame()
    {
        balance = 100;
        currentBet = 10;
        betSlider.value = currentBet;
        isSpinning = false;
        resultText.text = "";
        UpdateBalanceUI();
        UpdateBetUI();
        SetButtonsInteractable(true);
        spinButtonText.text = "Spin";
        AudioManager.Instance?.PlayUIButtonSound();
        DisableParticleEffects();
    }

    private void UpdateBalanceUI()
    {
        balanceText.text = $"Balance: {balance}";
    }

    private void UpdateBetUI()
    {
        betText.text = $"Bet: {currentBet}";
    }

    private void SetButtonsInteractable(bool interactable)
    {
        spinButton.interactable = interactable;
        resetButton.interactable = interactable;
    }

    private void EnableParticleEffects()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps != null)
            {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
        }
    }

    private void DisableParticleEffects()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps != null)
            {
                ps.Stop();
                ps.gameObject.SetActive(false);
            }
        }
    }
}