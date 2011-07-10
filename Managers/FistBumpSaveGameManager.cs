#region Using statements

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public class FistBumpSaveGameManager : MonoBehaviour
{
    #region Sealed Class

    private sealed class VersionDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
            {
                Type typeToDeserialize = null;

                assemblyName = Assembly.GetExecutingAssembly().FullName;

                // The following line of code returns the type. 
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

                return typeToDeserialize;
            }

            return null;
        }
    }
    #endregion

    #region Private Fields
    private bool m_EncryptSave = false;
    private const string m_EncryptionKey = "BumpIt";
    #endregion

    #region Public Properties

    #endregion

    #region Implementation of MonoBehaviour

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // Update is called once per frame
    }

    #endregion

    #region Public Methods
    public void Save()
    {
        SaveData savedata = new SaveData();

        Stream saveGameStream = File.Open("MySavedGame.game", FileMode.Create);
        BinaryFormatter binaryformatter = new BinaryFormatter { Binder = new VersionDeserializationBinder() };
        if (m_EncryptSave)
        {
            try
            {
                Debug.Log("Writing Save Information (Encrypted)");
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {Key = Encoding.ASCII.GetBytes(m_EncryptionKey), IV = Encoding.ASCII.GetBytes(m_EncryptionKey)};
                CryptoStream cryptoStream = new CryptoStream(saveGameStream, desProvider.CreateEncryptor(), CryptoStreamMode.Write);

                binaryformatter.Serialize(cryptoStream, savedata);

                cryptoStream.Close();
            }
            catch (CryptographicException e)
            {
                Debug.Log(string.Format("A Cryptographic error occurred: {0}", e.Message));
                Debug.Break();
            }
        }
        else
        {
            Debug.Log("Writing Save Information");
            binaryformatter.Serialize(saveGameStream, savedata);
        }
        
        saveGameStream.Close();
    }

    public void Load()
    {
        SaveData savedata = new SaveData();
        savedata.levelReached = 0;
        Stream saveGameStream = File.Open("MySavedGame.game", FileMode.Open);
        BinaryFormatter binaryformatter = new BinaryFormatter { Binder = new VersionDeserializationBinder() };
        if(m_EncryptSave)
        {
            try
            {
                Debug.Log("Reading Save Information (Encrypted)");
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {Key = Encoding.ASCII.GetBytes(m_EncryptionKey), IV = Encoding.ASCII.GetBytes(m_EncryptionKey)};
                CryptoStream cryptoStream = new CryptoStream(saveGameStream, desProvider.CreateDecryptor(), CryptoStreamMode.Read);
                savedata = (SaveData) binaryformatter.Deserialize(cryptoStream);

                cryptoStream.Close();
            }
            catch (CryptographicException e)
            {
                Debug.Log(string.Format("A Cryptographic error occurred: {0}", e.Message));
                Debug.Break();
            }
        }
        else
        {
            Debug.Log("Reading Save Information");
            savedata = (SaveData)binaryformatter.Deserialize(saveGameStream);
        }
        
        saveGameStream.Close();
    }
    #endregion

    #region Private Methods

    #endregion
}
