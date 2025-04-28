using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterSelector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Image characterImage;

    public UnityEvent OnCharacterSelected;

    private void Start()
    {
        if (characterDatabase.characters.Length > 0 && characterImage != null)
        {
            characterDatabase.SetDefaultCharacter();
            UpdateCharacterUI();
            OnCharacterSelected?.Invoke();
        }
    }

    public void ChangeCharacter(int direction)
    {
        if (characterDatabase.characters.Length == 0 || characterImage == null) return;

        int newIndex = characterDatabase.currentCharacterIndex + direction;

        if (newIndex < 0)
        {
            newIndex = characterDatabase.characters.Length - 1;
        }
        else if (newIndex >= characterDatabase.characters.Length)
        {
            newIndex = 0;
        }

        characterDatabase.currentCharacterIndex = newIndex;
        UpdateCharacterUI();
        OnCharacterSelected?.Invoke();
    }

    private void UpdateCharacterUI()
    {
        characterImage.sprite = characterDatabase.CurrentCharacter.characterIcon;
    }
}