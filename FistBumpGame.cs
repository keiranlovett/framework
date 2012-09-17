#region Using statements

using FistBump.Framework;
using UnityEngine;

#endregion

public abstract class FistBumpGame : SingletonMonoBehaviour<FistBumpGame>
{
    public TextAsset StatDefinitions;
    public TextAsset AchievementDescriptions;

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

        SocialConnector.LocalAchievementDescriptions = AchievementDescriptions;
        SocialConnector.Connect();
    }
    
    #endregion
}

