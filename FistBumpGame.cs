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
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        SocialConnector.Connect();
    }
    
    #endregion
}

