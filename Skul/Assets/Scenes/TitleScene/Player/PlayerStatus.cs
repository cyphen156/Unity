using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class PlayerStatus : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHP;                    // Health
    public float baseStemina;               // Stemina
    public float baseAttackPower;           // ATKPoint
    public float baseAttackSpeed;           // AS
    public float baseCriticalChance;         // CriticalPercent
    public float baseCriticalMultiplier;    // CriticalAP Multiple
    public float cooldownReduction;         // Cooldown 

    [Header("Current Stats")]
    public float maxHP;
    public float currentHP;
    public float maxStemina;
    public float currentStemina;
    public float currentAttackPower;
    public float currentAttackSpeed;
    public float currentCriticalChance;
    public float currentCriticalMultiplier;
    public float currentCooldownReduction;
    
    private void Awake()
    {
        baseHP = 150f;
        baseStemina = 100f;
        baseAttackPower = 10f;
        baseAttackSpeed = 1.0f;
        baseCriticalChance = 10f;          
        baseCriticalMultiplier = 1.5f; 
        cooldownReduction = 1f;            // 100 %
    }

    private void OnEnable()
    {
        maxHP = baseHP;
        currentHP = baseHP;
        maxStemina = baseStemina;
        currentStemina = baseStemina;
        currentAttackPower = baseAttackPower;
        currentAttackSpeed = baseAttackSpeed;
        currentCriticalChance = baseCriticalChance;
        currentCriticalMultiplier = baseCriticalMultiplier;
        currentCooldownReduction = cooldownReduction;
    }
    public void ApplyDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        if (currentHP <= 0)
        {
            currentHP = 0f;
        }
    }
    public float GetHPRatio()
    {
        return currentHP / maxHP;
    }
}