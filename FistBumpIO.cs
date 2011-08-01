#region Using statements

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

#endregion

/// <summary>
/// 
/// </summary>
/// <remarks>FistBump.ca - Copyright (C)</remarks>
public static class FistBumpIO
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

    #region Private fields

    private static bool s_EncryptFiles = false;
    private static string s_EncryptionKey = "12345678";
    private static readonly BinaryFormatter s_Binaryformatter = new BinaryFormatter { Binder = new VersionDeserializationBinder() };
    private static DESCryptoServiceProvider s_DESProvider = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(s_EncryptionKey), IV = Encoding.ASCII.GetBytes(s_EncryptionKey) };
    #endregion

    #region Public Accessor

    public static string EncryptionKey
    {
        get { return s_EncryptionKey; }
        set
        {
            s_EncryptionKey = value;
            s_DESProvider = new DESCryptoServiceProvider {Key = Encoding.ASCII.GetBytes(s_EncryptionKey), IV = Encoding.ASCII.GetBytes(s_EncryptionKey)};
        }
    }

    public static bool EncryptFiles
    {
        get { return s_EncryptFiles; }
        set
        {
            s_EncryptFiles = value;
            s_DESProvider = new DESCryptoServiceProvider {Key = Encoding.ASCII.GetBytes(s_EncryptionKey), IV = Encoding.ASCII.GetBytes(s_EncryptionKey)};
        }
    }

    #endregion

    #region Private Methods

    public static void Write<T>(string filename, T data)
    {
        if (s_EncryptFiles && s_EncryptionKey == "12345678") Debug.LogWarning("[IO] WARNING: Encrypting file with default, publicly known key");

        FileStream writeStream = null;
        try
        {
            writeStream = File.Open(filename, FileMode.Create);
	        if (s_EncryptFiles)
	        {
	            try
	            {
	                Debug.Log("[IO] Writing Save Information (Encrypted)");
	                
	                CryptoStream cryptoStream = new CryptoStream(writeStream, s_DESProvider.CreateEncryptor(), CryptoStreamMode.Write);
	
	                s_Binaryformatter.Serialize(cryptoStream, data);
	
	                cryptoStream.Close();
	            }
	            catch (CryptographicException e)
	            {
	                Debug.Log(string.Format("[IO] A Cryptographic error occurred: {0}", e.Message));
	                Debug.Break();
	            }
	        }
	        else
	        {
	            Debug.Log("[IO] Writing Save Information");
	            s_Binaryformatter.Serialize(writeStream, data);
	        }
        }
        catch (Exception ex)
        {
            Debug.Log("[IO] " + ex.ToString());
        }
        finally
        {
            if (writeStream != null)
            {
                writeStream.Close();
            }
        }

        
    }

    public static T Read<T>(string filename)
    {
        T data = default(T);
        FileStream readStream = null;
        try
        {
	        readStream = File.Open(filename, FileMode.Open);
	        if (s_EncryptFiles)
	        {
	            try
	            {
	                Debug.Log("[IO] Reading Save Information (Encrypted)");
	                CryptoStream cryptoStream = new CryptoStream(readStream, s_DESProvider.CreateDecryptor(), CryptoStreamMode.Read);
	                data = (T)s_Binaryformatter.Deserialize(cryptoStream);
	
	                cryptoStream.Close();
	            }
	            catch (CryptographicException e)
	            {
	                Debug.Log(string.Format("[IO] A Cryptographic error occurred: {0}", e.Message));
	                Debug.Break();
	            }
	        }
	        else
	        {
	            Debug.Log("[IO] Reading Save Information");
	            data = (T)s_Binaryformatter.Deserialize(readStream);
	        }
        }
        catch (System.Exception ex)
        {
            Debug.Log("[IO] " + ex.ToString());
        }
        finally
        {
            if (readStream != null)
            {
                readStream.Close();
            }
        }

        return data;
    }

    public static void WriteXML<T>(string filename, T data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream writeStream = null;
        try
        {
            writeStream = File.Open(filename, FileMode.Create);
            serializer.Serialize(writeStream, data);
        }
        catch (System.Exception ex)
        {
            Debug.Log("[IO] " + ex.ToString());
        }
        finally
        {
            if (writeStream != null)
            {
                writeStream.Close();
            }
        }
    }

    public static T ReadXML<T>(string filename)
    {
        T data = default(T);
        
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream readStream = null;
        try
        {
            readStream = File.Open(filename, FileMode.Open);
	        data = (T)serializer.Deserialize(readStream);
        }
        catch (System.Exception ex)
        {
            Debug.Log("[IO] " + ex.ToString());
        }
        finally
        {
            if (readStream != null)
            {
                readStream.Close();
            }
        }

        return data;
    }

    #endregion
}
