using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Scriptable Objects/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public CharacterData[] characters;
    public int currentCharacterIndex = 0;
    public CharacterData CurrentCharacter
    {
        get
        {
            return characters[currentCharacterIndex];
        }
        set
        {
            characters[currentCharacterIndex] = value;
        }
    }

    public void SetDefaultCharacter()
    {
        currentCharacterIndex = 0;
    }
}