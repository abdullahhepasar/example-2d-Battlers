using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public static UILoading Instance;

    [Header("Text Language")]
    public Text LoadingText;

    [Header("Slider Component")]
    public Slider LoadingSlider;

    [Header("Car Info Sliders Animation Speeds")]
    public float sliderSmoothSpeed = 1.5f;

    [Header("Slider Component")]
    public GameObject UILoadingContainer;

    [HideInInspector]
    public float TargetSliderValue;
    public float waitLoadingScreenForAnim = 1f;

    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
        {
            //if not, set instance to this
            Instance = this;
        }
        //If instance already exists and it's not this:
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadingActive();
        UIUpdate();
    }

    public void UIUpdate()
    {
        SetLanguage();

        //Loading set false
        LoadingSlider.gameObject.SetActive(false);
    }

    private void SetLanguage()
    {
        LoadingText.text = LanguageManager.Instance.LoadingText;
    }

    // Update is called once per frame
    void Update()
    {
        LoadingSlider.value = Mathf.Lerp(LoadingSlider.value, TargetSliderValue, sliderSmoothSpeed * Time.deltaTime);
    }

    public void LoadingActive()
    {
        UILoadingContainer.SetActive(true);
    }

    public void ResetLoading()
    {
        UILoadingContainer.SetActive(false);
        LoadingSlider.value = 0f;
        TargetSliderValue = 0f;
    }

    public IEnumerator WaitLoading()
    {
        yield return new WaitForSeconds(waitLoadingScreenForAnim);

        ResetLoading();
    }
}
