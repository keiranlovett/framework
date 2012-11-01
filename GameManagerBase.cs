#region Using statements

using System.Collections;
using FistBump.Framework;
using UnityEngine;

#endregion

public abstract class GameManagerBase : SingletonMonoBehaviour<GameManagerBase>
{
    public TextAsset StatDefinitions;
    public TextAsset AchievementDescriptions;

    public GameObject DynamicObjects { get; private set; }
    
    public int FramesPerSec { get; private set; }
    private float m_UpdateFPSFrequency = 0.5f;

    #region Implementation of MonoBehaviour
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {
        InitDynamicObjects();

        DontDestroyOnLoad(gameObject);

        if (StatDefinitions != null)
        {
            StatisticManager.Instance.LoadDefinitions(StatDefinitions);
        }
        else
        {
            Debug.Log("[GameManagerBase] No stat definition file provided. You'll have to add Statistic Definition manually to use this module");
        }

        SocialPlatformSelector.LocalAchievementDescriptions = AchievementDescriptions;
        SocialPlatformSelector.Select();
    }
    protected virtual void Start()
    {
        StartCoroutine(UpdateFPS());
    }

    #endregion

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

    private void InitDynamicObjects()
    {
        DynamicObjects = GameObject.Find("Dynamic Objects");
        if (DynamicObjects == null)
        {
            DynamicObjects = new GameObject("Dynamic Objects");
        }
    }
}

