#region Using statements

using System.Collections;
using FistBump.Framework;
using UnityEngine;

#endregion

public abstract class FistBumpGame : SingletonMonoBehaviour<FistBumpGame>
{
    public TextAsset StatDefinitions;
    public TextAsset AchievementDescriptions;
    
    public int FramesPerSec { get; private set; }
    private float m_UpdateFPSFrequency = 0.5f;

    #region Implementation of MonoBehaviour
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (StatDefinitions != null)
        {
            StatisticManager.Instance.LoadDefinitions(StatDefinitions);
        }
        else
        {
            Debug.Log("[FistBumpGame] No stat definition file provided. You'll have to add Statistic Definition manually to use this module");
        }

        SocialPlatformSelector.LocalAchievementDescriptions = AchievementDescriptions;
        SocialPlatformSelector.Select();
    }

    protected virtual void Start()
    {
        StartCoroutine(UpdateFPS());
    }

    private IEnumerator UpdateFPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(m_UpdateFPSFrequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
        }
    }
    
    #endregion
}

