using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reel : MonoBehaviour
{
    public Sprite[] symbolSprites;
    public Image[] symbolImages;
    private RectTransform[] symbolRects;
    private int[] currentIndices;
    private bool isSpinning = false;
    private float spinDuration = 2f;
    private float moveDistance = 100f;
    private float moveTime = 0.1f;
    private const float MASK_POSITION = 300f;
    private const float RESET_POSITION = 300f;

    void Awake()
    {
        symbolRects = new RectTransform[6];
        for (int i = 0; i < 6; i++)
        {
            symbolRects[i] = symbolImages[i].GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        currentIndices = new int[6];
        float[] initialPositions = { 300f, 200f, 100f, 0f, -100f, -200f };
        for (int i = 0; i < 6; i++)
        {
            currentIndices[i] = Random.Range(0, symbolSprites.Length);
            symbolImages[i].sprite = symbolSprites[currentIndices[i]];
            symbolRects[i].anchoredPosition = new Vector2(0, initialPositions[i]);
        }
    }

    public void StartSpinning()
    {
        if (!isSpinning)
        {
            isSpinning = true;
            StartCoroutine(SpinCoroutine());
        }
    }

    private IEnumerator SpinCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            float moveElapsed = 0f;
            Vector2[] startPositions = new Vector2[6];
            Vector2[] targetPositions = new Vector2[6];

            for (int i = 0; i < 6; i++)
            {
                startPositions[i] = symbolRects[i].anchoredPosition;
                targetPositions[i] = startPositions[i] - new Vector2(0, moveDistance);
            }

            while (moveElapsed < moveTime)
            {
                moveElapsed += Time.deltaTime;
                float t = moveElapsed / moveTime;
                for (int i = 0; i < 6; i++)
                {
                    symbolRects[i].anchoredPosition = Vector2.Lerp(startPositions[i], targetPositions[i], t);
                }
                yield return null;
            }

            for (int i = 0; i < 6; i++)
            {
                symbolRects[i].anchoredPosition = targetPositions[i];
                if (Mathf.Approximately(symbolRects[i].anchoredPosition.y, -300f))
                {
                    symbolRects[i].anchoredPosition = new Vector2(0, RESET_POSITION);
                    ShuffleSprites();
                    currentIndices[i] = 0;
                    symbolImages[i].sprite = symbolSprites[currentIndices[i]];
                }
            }

            elapsedTime += moveTime;
            yield return null;
        }

        float alignTime = 0f;
        Vector2[] finalPositions = new Vector2[6];
        for (int i = 0; i < 6; i++)
        {
            finalPositions[i] = new Vector2(0, 300f - (i * 100f));
        }

        isSpinning = false;
    }

    private void ShuffleSprites()
    {
        int n = symbolSprites.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Sprite temp = symbolSprites[i];
            symbolSprites[i] = symbolSprites[j];
            symbolSprites[j] = temp;
        }
    }

    public string GetCurrentSymbol()
    {
        return symbolSprites[currentIndices[2]].name;
    }
}