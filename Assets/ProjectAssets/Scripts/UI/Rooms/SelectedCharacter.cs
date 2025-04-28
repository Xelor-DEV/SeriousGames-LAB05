using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Image currentCharacter;

    public void UpdateCharacter()
    {
        currentCharacter.sprite = characterDatabase.CurrentCharacter.characterIcon;
    }
}