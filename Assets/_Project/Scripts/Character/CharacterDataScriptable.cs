using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Project/CharacterData")]
public class CharacterDataScriptable : ScriptableObject
{
    public CharacterManager.CharacterID CharacterID = CharacterManager.CharacterID.None;
    public string CharName;
    public Sprite CharSprite;

    public float HealthAmount;
    public float DamageMaxAmount;
    public float DamageMinAmount;

    [Header("AI Attack Time")]
    public float AIMaxAttackDelay;
    public float AIMinAttackDelay;

    [HideInInspector]
    public float CurrentDamage, CurrentHealth;

    public void SetData()
    {
        CurrentHealth = HealthAmount;
        CurrentDamage = Random.Range(DamageMinAmount, DamageMaxAmount);
    }

    /// <summary>
    /// Set-Get Random Damage 
    /// </summary>
    public float SetDamageRandom()
    {
        CurrentDamage = Random.Range(DamageMinAmount, DamageMaxAmount);
        return CurrentDamage;
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
            CurrentHealth = 0;
    }
}
