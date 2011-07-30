#region Using statements

using UnityEngine;

#endregion

[RequireComponent(typeof(FistBumpGameManager))]
[RequireComponent(typeof(FistBumpScreenManager))]
[RequireComponent(typeof(FistBumpAudioManager))]
[RequireComponent(typeof(FistBumpMissionManager))]
[RequireComponent(typeof(FistBumpPlayerManager))]
[RequireComponent(typeof(FistBumpStatisticManager))]
public abstract class FistBumpGame : MonoBehaviour
{
    #region Private Variables

    public static string Version { get { return "0.1.0 Alpha"; } }

    protected static FistBumpGame s_Instance;
    private FistBumpGameService m_GameService;
    private FistBumpGameManager m_GameManager;
    private FistBumpScreenManager m_ScreenManager;
    private FistBumpAudioManager m_AudioManager;
    private FistBumpMissionManager m_MissionManager;
    private FistBumpPlayerManager m_PlayerManager;
    private FistBumpStatisticManager m_StatisticManager;
    private FistBumpControlSchemeManager m_ControlSchemeManager;

    #endregion

    #region Public Properties

    public static FistBumpGame Instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first FistBumpGame object in the scene.
                s_Instance = FindObjectOfType(typeof(FistBumpGame)) as FistBumpGame;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("FistBumpGame");
                s_Instance = obj.AddComponent(typeof(FistBumpGame)) as FistBumpGame;
                Debug.Log("Could not locate an FistBumpGame object. \n FistBumpGame was Generated automatically.");
            }

            return s_Instance;
        }
    }
    public  FistBumpGameService GameService { get { return Instance.m_GameService; } }
    public FistBumpGameManager Game { get { return Instance.m_GameManager; } }
    public FistBumpScreenManager Screen { get { return Instance.m_ScreenManager; } }
    public FistBumpAudioManager Audio { get { return Instance.m_AudioManager; } }
    public FistBumpMissionManager Mission { get { return Instance.m_MissionManager; } }
    public FistBumpPlayerManager PlayerManager { get { return Instance.m_PlayerManager; } }
    public FistBumpControlSchemeManager ControlSchemes { get { return Instance.m_ControlSchemeManager; } }
    public FistBumpStatisticManager Stats { get { return Instance.m_StatisticManager; } }
    #endregion

    #region Implementation of MonoBehaviour
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected void Awake()
    {
        InitializeManagers();

        InitializeGameServices();

        //Make this game object persistant
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// Sent to all game objects before the application is quit.
    /// </summary>
    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
    
    #endregion

    #region Private Methods
    private void InitializeManagers()
    {
        m_GameManager = GetComponent<FistBumpGameManager>() ?? gameObject.AddComponent<FistBumpGameManager>();
        m_ScreenManager = GetComponent<FistBumpScreenManager>() ?? gameObject.AddComponent<FistBumpScreenManager>();
        m_AudioManager = GetComponent<FistBumpAudioManager>() ?? gameObject.AddComponent<FistBumpAudioManager>();
        m_MissionManager = GetComponent<FistBumpMissionManager>() ?? gameObject.AddComponent<FistBumpMissionManager>();
        m_PlayerManager = GetComponent<FistBumpPlayerManager>() ?? gameObject.AddComponent<FistBumpPlayerManager>();
        m_ControlSchemeManager = GetComponent<FistBumpControlSchemeManager>() ?? gameObject.AddComponent<FistBumpControlSchemeManager>();
        m_StatisticManager = GetComponent<FistBumpStatisticManager>() ?? gameObject.AddComponent<FistBumpStatisticManager>();
    }
    

    private void InitializeGameServices()
    {
        if (Application.isWebPlayer)
        {
            // Facebook
            // try initialization

            // Kongregate
            m_GameService = GetComponent<KongregateGameService>() ?? gameObject.AddComponent<KongregateGameService>();
            m_GameService.Initialize();

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            // OpenFeint
            // try initialization
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Steam
            // try initialization
        }
        else if (Application.platform == RuntimePlatform.XBOX360)
        {
            // XBLA
            // try initialization
        }
        else if (Application.platform == RuntimePlatform.PS3)
        {
            // PSN
            // try initialization
        }
    }
 
    #endregion

    #region GameServices methods

    #endregion
}

