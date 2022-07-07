using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [Header("Text Components")]
    public Text LevelText;
    public Text CoinText;
    public Text ToBattleText;
    public Text NextText;
    public Text BackText;

    [Header("Image Components")]
    public Image CharacterPreview;

    private void OnEnable()
    {
        UIUpdate();
    }

    public void UIUpdate()
    {
        SetLanguage();
        GetCharacter();

        CoinText.text = AppValueController.Instance.GetMoney().ToString();
    }

    private void SetLanguage()
    {
        LevelText.text = LanguageManager.Instance.LevelText;
        ToBattleText.text = LanguageManager.Instance.ToBattleText;
        NextText.text = LanguageManager.Instance.NextText;
        BackText.text = LanguageManager.Instance.BackText;
    }

    private void GetCharacter()
    {
        CharacterPreview.sprite = CharacterManager.Instance.GetCharacterData().CharSprite;
    }

    #region SET BUTTONS

    public void UIChange(int index)
    {
        UIManager.Instance.UICreatePrefabs(index);
    }

    public void SettingButton()
    {
        UIManager.Instance.UIPopupCreatePrefabs((int)UIManager.UIPrefabNames.UIPopupSettings);
    }

    public void SetPlayButton()
    {
        UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.UIGameplay);
    }

    /// <summary>
    /// Set UIMainMenu Next-Back Buttons
    /// +1-> Next
    /// -1 -> Back
    /// </summary>
    /// <param name="index"></param>
    public void SetCharacterChange(int index)
    {
        CharacterManager.Instance.SetCharacterData(index);
        GetCharacter();
    }

    #endregion
}
