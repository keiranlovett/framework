#region Using statements

using FistBump.Framework;
using UnityEngine;

#endregion

public abstract class FistBumpGame : MonoBehaviour
{
    #region Private Variables

    public static string Version { get { return "0.1.0 Alpha"; } }
    protected static FistBumpGame s_Instance;
    
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

    protected void Start()
    {

    }

    protected void Update()
    {

    }
    
    protected void OnGUI()
    {
        GUIContent versionText = new GUIContent( Version );
        GUI.Label(new Rect(0,0,160, 150), versionText);
    }
    
    /// <summary>
    /// Sent to all game objects before the application is quit.
    /// </summary>
    protected void OnApplicationQuit()
    {
        Debug.Log("Session play time: " + Time.time);
        s_Instance = null;
    }
    
    #endregion

    #region Private Methods
    private void InitializeManagers()
    {
    }
    

    private void InitializeGameServices()
    {
        if (Application.isWebPlayer)
        {
            // Facebook
            // try initialization

            // Kongregate
            //m_GameService = GetComponent<Kongregate>() ?? gameObject.AddComponent<Kongregate>();
            //m_GameService.Initialize();

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

