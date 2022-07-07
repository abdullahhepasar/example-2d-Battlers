using UnityEngine;
using UnityEngine.UI;

public class UILevelFailed : MonoBehaviour
{
    [Header("Text Language")]
    public Text LevelFailedText;
    public Text BackMenuText;

    private void OnEnable()
    {
        UIUpdate();
    }

    private void Start()
    {
        UIManager.Instance.UIPopupDestroyPrefabTransform();
        AppValueController.Instance.GameMoneyEconomy(AppValueController.Instance.CurrentLevelWinCoin / 2);
    }

    public void UIUpdate()
    {
        SetLanguage();
    }

    private void SetLanguage()
    {
        LevelFailedText.text = LanguageManager.Instance.LevelFailedText;
        BackMenuText.text = LanguageManager.Instance.BackMenuText;
    }

    #region SET BUTTONS

    public void UIChange(int index)
    {
        UIManager.Instance.UICreatePrefabs(index);
    }

    public void SetNoThanksButton()
    {
        UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.MainMenu);
    }

    #endregion
}
