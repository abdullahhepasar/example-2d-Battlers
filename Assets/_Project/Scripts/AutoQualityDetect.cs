using System.Collections;
using UnityEngine;

public class AutoQualityDetect : MonoBehaviour
{
    public static AutoQualityDetect Instance;

    /// <summary>
    /// The number of data points to calculate the average FPS over.
    /// </summary>
    int numberOfDataPoints;
    /// <summary>
    /// The current average fps.
    /// </summary>
    public float currentAverageFps;
    /// <summary>
    /// The time interval in which the class checks for the framerate and adapts quality accordingly.
    /// </summary>
    public float TimeIntervalToAdaptQualitySettings = 10f;
    /// <summary>
    /// The lower FPS threshold. Decrease quality when FPS falls below this.
    /// </summary>
    public float LowerFPSThreshold = 30f;
    /// <summary>
    /// The upper FPS threshold. Increase quality when FPS is above this.
    /// </summary>
    public float UpperFPSThreshold = 50f;
    /// <summary>
    /// The stability of the current quality setting. Below 0 if changes have been
    /// made, otherwise positive.
    /// </summary>
    int stability;
    /// <summary>
    /// Tracks whether quality was improved or worsened.
    /// </summary>
    bool lastMovementWasDown;
    /// <summary>
    /// Counter that keeps track when the script can't decide between lowering or increasing quality.
    /// </summary>
    int flickering;

    [Header("Min Width: 1920 altı ve RAM: 4500 Altı telefonlar")]
    public bool veryLowDeviceActive = false;
    public int veryLowDeviceWidthMinLimit = 1920;
    public int veryLowDeviceRAMMinLimit = 4500;

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

    private void Start()
    {
        //Application.lowMemory += OnLowMemory;
        Application.targetFrameRate = 60;
    }

    private void OnLowMemory()
    {
        veryLowDeviceActive = true;
        // release all cached textures
        Resources.UnloadUnusedAssets();

        StartCoroutine(SetUnloadUnusedAssets());
    }

    IEnumerator SetUnloadUnusedAssets()
    {
        yield return new WaitForSeconds(3f);
        Resources.UnloadUnusedAssets();
    }

    void StartAdaptQuality()
    {
        StartCoroutine(AdaptQuality());
    }


    void Update()
    {
        //UpdateCumulativeAverageFPS(1 / Time.deltaTime);
    }


    /// <summary>
    /// Updates the cumulative average FPS.
    /// </summary>
    /// <param name="newFPS">New FPS.</param>
    float UpdateCumulativeAverageFPS(float newFPS)
    {
        ++numberOfDataPoints;
        currentAverageFps += (newFPS - currentAverageFps) / numberOfDataPoints;

        return currentAverageFps;
    }


    /// <summary>
    /// Sets the quality accordingly to the current thresholds.
    /// </summary>
    IEnumerator AdaptQuality()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeIntervalToAdaptQualitySettings);

            if (Debug.isDebugBuild)
            {
                //Debug.Log("Current Average Framerate is: " + currentAverageFps);
            }

            // Decrease level if framerate too low
            if (currentAverageFps < LowerFPSThreshold)
            {
                QualitySettings.DecreaseLevel();
                --stability;
                if (!lastMovementWasDown)
                {
                    ++flickering;
                }
                lastMovementWasDown = true;
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Reducing Quality Level, now " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
                }

                // In case we are "flickering" (switching between two quality settings),
                // stop it, using the lower quality level.
                if (flickering > 1)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log(string.Format(
                          "Flickering detected, staying at {0} to stabilise.",
                          QualitySettings.names[QualitySettings.GetQualityLevel()]));
                    }

                    QualitySettings.SetQualityLevel(0);
                    //Destroy(this);
                }

            }
            else
              // Increase level if framerate is too high
              if (currentAverageFps > UpperFPSThreshold)
            {
                //Maximum Medium a çeksin otomatik
                if (QualitySettings.GetQualityLevel() < 1)
                    QualitySettings.IncreaseLevel();
                --stability;
                if (lastMovementWasDown)
                {
                    ++flickering;
                }
                lastMovementWasDown = false;
                if (Debug.isDebugBuild)
                {
                    //Debug.Log("Increasing Quality Level, now " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
                }
            }
            else
            {
                ++stability;
            }

            // If we had a framerate in the range between 25 and 60 frames three times
            // in a row, we consider this pretty stable and remove this script.
            if (stability > 3)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log("Framerate is stable now, removing automatic adaptation.");
                }
                //Destroy(this);
            }

            QualityChange();

            // Reset moving average
            numberOfDataPoints = 0;
            currentAverageFps = 0;
        }
    }

    public void QualityChange()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(AppValueController.Instance.QualityIndexText));

        float ScaleScreen = (float)AppValueController.Instance.ScreenHeight / (float)AppValueController.Instance.ScreenWidth;

        float middleScale = 1920f * ScaleScreen;
        float lowScale = 1280f * ScaleScreen;
        float verylowScale = 960f * ScaleScreen;

        switch (QualitySettings.GetQualityLevel())
        {
            case 0://LOW
                Screen.SetResolution(1280, (int)lowScale, true);
                break;
            case 1://MEDIUM
                   //Cok Dusuk Modeller
                if (Screen.width <= 1920 || SystemInfo.systemMemorySize <= 3000)
                    Screen.SetResolution(1280, (int)lowScale, true);
                else
                    Screen.SetResolution(1920, (int)middleScale, true);
                break;
            case 2://HIGH
                   //Screen.SetResolution(AppValueController.Instance.ScreenWidth, AppValueController.Instance.ScreenHeight, true);

                if (Screen.width <= 1920 || SystemInfo.systemMemorySize <= 3000)
                    Screen.SetResolution(1280, (int)lowScale, true);
                else
                    Screen.SetResolution(1920, (int)middleScale, true);
                break;
            default:
                //Screen.SetResolution(AppValueController.Instance.ScreenWidth, AppValueController.Instance.ScreenHeight, true);

                if (Screen.width <= 1920 || SystemInfo.systemMemorySize <= 3000)
                    Screen.SetResolution(1280, (int)lowScale, true);
                else
                    Screen.SetResolution(1920, (int)middleScale, true);
                break;
        }

        //ÇOK DÜŞÜK TELEFONLAR İÇİN
        if (Screen.width <= veryLowDeviceWidthMinLimit && SystemInfo.systemMemorySize <= veryLowDeviceRAMMinLimit)
        {
            veryLowDeviceActive = true;
            //960:540 -> 1 GB ye yaklaşık telefonlar her türlü buradan çıkmasınlar
            if(Screen.width < 1000)
                Screen.SetResolution(960, (int)verylowScale, true);
            else if (Screen.width <= 1920)
            {
                Screen.SetResolution(1280, (int)lowScale, true);
            }
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
            Application.targetFrameRate = 300;
        else
            Application.targetFrameRate = 60;

        /*if (QualitySettings.GetQualityLevel() <= 2)
            PlayerPrefs.SetInt(AppValueController.Instance.QualityIndexText, QualitySettings.GetQualityLevel());*/
    }
}
