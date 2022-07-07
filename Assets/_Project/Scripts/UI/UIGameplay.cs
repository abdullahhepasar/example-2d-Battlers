using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    [Header("Text Components")]
    public Text LevelText;
    public Text CoinText;
    public Text AttackText;

    [Header("Image Components")]
    public Image CharacterSprite;
    public Image EnemySprite;

    [Header("Slider Components")]
    public Slider CharacterHealthSlider;
    public Slider EnemyHealthSlider;

    [Header("Gameobject Components")]
    public GameObject AttackButton;

    [Header("Animation")]
    public float WinnerAnimationSpeed = 2f;

    private void OnEnable()
    {
        UIUpdate();

        GameManager.Instance.delegateAIAttack += CheckHealths;
    }

    private void OnDisable()
    {
        GameManager.Instance.delegateAIAttack -= CheckHealths;

    }

    public void UIUpdate()
    {
        SetLanguage();
        GetCharacters();
    }

    private void SetLanguage()
    {
        LevelText.text = LanguageManager.Instance.LevelText;
        CoinText.text = AppValueController.Instance.GetMoney().ToString();
        AttackText.text = LanguageManager.Instance.AttackText;
    }

    /// <summary>
    /// Get Characters Datas
    /// </summary>
    private void GetCharacters()
    {
        //First Setting
        CharacterManager.Instance.GetCharacterData().SetData();
        CharacterManager.Instance.GetEnemyData().SetData();

        CharacterHealthSlider.maxValue = CharacterManager.Instance.GetCharacterData().CurrentHealth;
        CharacterHealthSlider.value = CharacterManager.Instance.GetCharacterData().CurrentHealth;

        EnemyHealthSlider.maxValue = CharacterManager.Instance.GetEnemyData().CurrentHealth;
        EnemyHealthSlider.value = CharacterManager.Instance.GetEnemyData().CurrentHealth;

        CharacterSprite.sprite = CharacterManager.Instance.GetCharacterData().CharSprite;
        EnemySprite.sprite = CharacterManager.Instance.GetEnemyData().CharSprite;

        //Automatic AI Attack Start
        GameManager.Instance.SetStartAIAttack();
    }

    private void CheckHealths()
    {
        CharacterHealthSlider.value = CharacterManager.Instance.GetCharacterData().CurrentHealth;
        EnemyHealthSlider.value = CharacterManager.Instance.GetEnemyData().CurrentHealth;

        if (CharacterManager.Instance.GetCharacterData().CurrentHealth <= 0)
        {
            //Fail
            StartCoroutine(CheckWinner(false));
        }
    }

    /// <summary>
    /// Play Winner animation
    /// </summary>
    private IEnumerator CheckWinner(bool win)
    {
        yield return StartCoroutine(CheckWinnerAnimation(win));

        if (win)
        {
            //Player Win
            AppValueController.Instance.DebugLog("LEVEL WIN", Color.cyan);
            UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.UILevelSuccess);
        }
        else
        {
            //Enemy win
            AppValueController.Instance.DebugLog("LEVEL FAIL", Color.cyan);
            UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.UILevelFail);
        }
    }

    IEnumerator CheckWinnerAnimation(bool win)
    {
        CharacterHealthSlider.gameObject.SetActive(false);
        EnemyHealthSlider.gameObject.SetActive(false);
        AttackButton.SetActive(false);

        CharacterSprite.gameObject.SetActive(win);
        EnemySprite.gameObject.SetActive(!win);

        RectTransform targetImage = (win) ? CharacterSprite.GetComponent<RectTransform>() : EnemySprite.GetComponent<RectTransform>();
        float reflect = (win) ? 1f : -1f;

        float time = 0;

        while (time < 3f)
        {
            time += Time.deltaTime;

            targetImage.anchoredPosition = Vector2.Lerp(targetImage.anchoredPosition, new Vector2(((float)AppValueController.Instance.ScreenWidth / 2f) * reflect, 0f), Time.deltaTime * WinnerAnimationSpeed);
            targetImage.localScale = Vector3.Lerp(targetImage.localScale, Vector3.one * 2f, Time.deltaTime * WinnerAnimationSpeed);

            yield return null;
        }

        yield return null;
    }

    #region SET BUTTONS

    public void OpenUIPopUpSettings()
    {
        UIManager.Instance.UIPopupCreatePrefabs((int)UIManager.UIPrefabNames.UIPopupSettings);
    }

    public void SetAttackButton()
    {
        AppValueController.Instance.DebugLog("SetAttackButton", Color.cyan);

        CharacterManager.Instance.GetEnemyData().TakeDamage(CharacterManager.Instance.GetCharacterData().SetDamageRandom());

        EnemyHealthSlider.value = CharacterManager.Instance.GetEnemyData().CurrentHealth;

        if (CharacterManager.Instance.GetEnemyData().CurrentHealth <= 0)
        {
            //Win
            StartCoroutine(CheckWinner(true));
        }
    }

    #endregion
}
