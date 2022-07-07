using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void DelegateAIAttack();   //Delegate FOR AI Damage
    public DelegateAIAttack delegateAIAttack;

    private IEnumerator StartAIAttackTemp;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #region AI

    public void SetStartAIAttack()
    {
        if (StartAIAttackTemp != null)
        {
            StopCoroutine(StartAIAttackTemp);
            StartAIAttackTemp = null;
        }
        if (StartAIAttackTemp == null)
        {
            StartAIAttackTemp = StartAIAttack();
            StartCoroutine(StartAIAttackTemp);
        }
    }

    private IEnumerator StartAIAttack()
    {
        while (CharacterManager.Instance.GetCharacterData().CurrentHealth > 0 && 
            CharacterManager.Instance.GetEnemyData().CurrentHealth > 0)
        {
            yield return new WaitForSeconds(Random.Range(
                CharacterManager.Instance.GetEnemyData().AIMinAttackDelay, CharacterManager.Instance.GetEnemyData().AIMaxAttackDelay));

            //End AI attack if game over during cooldown
            if (CharacterManager.Instance.GetEnemyData().CurrentHealth <= 0)
                yield break;

            AppValueController.Instance.DebugLog("SetAIAttack", Color.red);
            CharacterManager.Instance.GetCharacterData().TakeDamage(CharacterManager.Instance.GetEnemyData().SetDamageRandom());

            if (delegateAIAttack != null)
                delegateAIAttack();
        }
    }

    #endregion
}
