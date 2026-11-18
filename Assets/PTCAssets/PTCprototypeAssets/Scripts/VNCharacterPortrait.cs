using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class VNCharacterPortrait : MonoBehaviour
{
    [Header("Character Sprites")] 
    public Sprite neutralSprite;
    public Sprite happySprite;
    public Sprite angrySprite;

    [Header("Other Character")] 
    public VNCharacterPortrait otherCharacter;
    
    [Header("Animation")]
    public float backDist = 60f;
    public float backScale = 0.9f;
    public float animationSpeed = 8f;
    
    private Image characterImage;
    private RectTransform characterRectTransform;
    
    private Vector2 ogPosition;
    private Vector3 ogSize;
    
    private bool isSpeaking = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        characterImage = GetComponent<Image>();
        characterRectTransform = GetComponent<RectTransform>();
        
        ogPosition = characterRectTransform.anchoredPosition;
        ogSize = characterRectTransform.localScale;

        SetExpression("neutral");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos;
        Vector3 targetSize;
        Color targetColor;

        if (isSpeaking)
        {
            targetPos = ogPosition; 
            targetSize = ogSize;
            targetColor = Color.white;
        }
        else
        {
            targetPos = ogPosition + Vector2.down * backDist;
            targetSize = ogSize * backScale;
            targetColor = Color.gray;
        }
        
        float t = 1f - Mathf.Exp(-animationSpeed * Time.deltaTime);
        
        characterRectTransform.anchoredPosition = Vector2.Lerp(characterRectTransform.anchoredPosition, targetPos, t);
        characterRectTransform.localScale = Vector3.Lerp(characterRectTransform.localScale, targetSize, t);
        characterImage.color = Color.Lerp(characterImage.color, targetColor, t);
    }

    public void SetSpeaking(bool speaking)
    {
        isSpeaking = speaking;
    }

    [YarnCommand("speak")]
    public void Speak(string expression = "")
    {
        SetSpeaking(true);

        if (otherCharacter != null)
        {
            otherCharacter.SetSpeaking(false);
        }

        if (!string.IsNullOrEmpty(expression))
        {
            SetExpression(expression);
        }
    }
    
    
    [YarnCommand("expression")]
    public void SetExpression(string expression)
    {
        switch (expression)
        {
            case "neutral":
                characterImage.sprite = neutralSprite;
                break;
            case  "happy":
                characterImage.sprite = happySprite;
                break;
            case "angry" :
                characterImage.sprite = angrySprite;
                break;
        }
    }
    
}
