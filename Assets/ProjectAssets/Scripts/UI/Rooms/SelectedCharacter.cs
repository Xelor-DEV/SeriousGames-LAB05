using UnityEngine;
using UnityEngine.UI;

public class SelectedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterSelector characterSelector;
    [SerializeField] private Image currentCharacter;

    public void UpdateCharacter()
    {
        currentCharacter.sprite = characterSelector.CurrentCharacter;
    }
}