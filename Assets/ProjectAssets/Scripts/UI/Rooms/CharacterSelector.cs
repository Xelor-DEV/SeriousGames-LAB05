using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelector : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Image characterImage;

    [SerializeField] private int currentIndex = 0;
    [SerializeField] private Sprite currentCharacter;

    public Sprite CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
    }

    public UnityEvent OnCharacterSelected;

    private void Start()
    {
        if (characterSprites.Length > 0 && characterImage != null)
        {
            characterImage.sprite = characterSprites[currentIndex];
            currentCharacter = characterSprites[currentIndex];
            OnCharacterSelected?.Invoke();
        }
    }

    public void ChangeCharacter(int direction)
    {
        if (characterSprites.Length == 0 || characterImage == null) return;

        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = characterSprites.Length - 1;
        }
        else if (currentIndex >= characterSprites.Length)
        {
            currentIndex = 0;
        }

        characterImage.sprite = characterSprites[currentIndex];
        currentCharacter = characterSprites[currentIndex];
        OnCharacterSelected?.Invoke();
    }
}