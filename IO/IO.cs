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

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    public static class IO
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

        /// http://www.codeproject.com/Articles/32407/A-Generic-Method-for-Deep-Cloning-in-C-3-0
        /// <summary>
        /// This class offers the ability to save the fields
        /// of types that don't have the <c ref="System.SerializableAttribute">SerializableAttribute</c>.
        /// </summary>
        private sealed class NonSerialiazableTypeSurrogateSelector : System.Runtime.Serialization.ISerializationSurrogate, System.Runtime.Serialization.ISurrogateSelector
        {
            #region ISerializationSurrogate Members

            public void GetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            {
                FieldInfo[] fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var fi in fieldInfos)
                {
                    if (IsKnownType(fi.FieldType))
                    {
                        info.AddValue(fi.Name, fi.GetValue(obj));
                    }
                    else if (fi.FieldType.IsClass)
                    {
                        info.AddValue(fi.Name, fi.GetValue(obj));
                    }
                }
            }

            public object SetObjectData(object obj, System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context, System.Runtime.Serialization.ISurrogateSelector selector)
            {
                FieldInfo[] fieldInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var fi in fieldInfos)
                {
                    if (IsKnownType(fi.FieldType))
                    {
                        if (IsNullableType(fi.FieldType))
                        {
                            // Nullable<argumentValue>
                            Type argumentValueForTheNullableType = GetFirstArgumentOfGenericType(fi.FieldType);
                            fi.SetValue(obj, info.GetValue(fi.Name, argumentValueForTheNullableType));
                        }
                        else
                        {
                            fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType));
                        }
                    }
                    else
                        if (fi.FieldType.IsClass)
                        {
                            fi.SetValue(obj, info.GetValue(fi.Name, fi.FieldType));
                        }
                }

                return obj;
            }

            private Type GetFirstArgumentOfGenericType(Type type)
            {
                return type.GetGenericArguments()[0];
            }

            private bool IsNullableType(Type type)
            {
                if (type.IsGenericType)
                    return type.GetGenericTypeDefinition() == typeof(Nullable<>);
                return false;
            }

            private bool IsKnownType(Type type)
            {
                return type == typeof(string) || type.IsPrimitive || type.IsSerializable;
            }

            #endregion

            #region ISurrogateSelector Members

            System.Runtime.Serialization.ISurrogateSelector _nextSelector;

            public void ChainSelector(System.Runtime.Serialization.ISurrogateSelector selector)
            {
                this._nextSelector = selector;
            }

            public System.Runtime.Serialization.ISurrogateSelector GetNextSelector()
            {
                return _nextSelector;
            }

            public System.Runtime.Serialization.ISerializationSurrogate GetSurrogate(Type type, System.Runtime.Serialization.StreamingContext context, out System.Runtime.Serialization.ISurrogateSelector selector)
            {
                if (IsKnownType(type))
                {
                    selector = null;
                    return null;
                }
                else if (type.IsClass || type.IsValueType)
                {
                    selector = this;
                    return this;
                }
                else
                {
                    selector = null;
                    return null;
                }
            }

            #endregion
        }

        #endregion

        #region Private fields

        private const string DEFAULT_ENCRYPTION_KEY = "12345678";
        private static string s_EncryptionKey = DEFAULT_ENCRYPTION_KEY;
        private static readonly BinaryFormatter s_Binaryformatter = new BinaryFormatter { Binder = new VersionDeserializationBinder(), SurrogateSelector = new NonSerialiazableTypeSurrogateSelector() };
        private static DESCryptoServiceProvider s_DESProvider = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(s_EncryptionKey), IV = Encoding.ASCII.GetBytes(s_EncryptionKey) };
        private static bool s_IsVerbose = true;
        
        #endregion

        #region Public Accessor

        public static string EncryptionKey
        {
            set
            {
                s_EncryptionKey = value;
                if (s_EncryptionKey.Length != 8) Debug.LogError("[IO] ERROR: Encryption key length need to be 8");
                s_DESProvider = new DESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(s_EncryptionKey), IV = Encoding.ASCII.GetBytes(s_EncryptionKey)};
            }
        }
        public static bool IsVerbose
        {
            get { return s_IsVerbose; }
            set { s_IsVerbose = value; }
        }
        
        #endregion

        #region Path

        public static string ToDataPath(string filename)
        {
            return Path.Combine(Application.dataPath, filename);
        }

        public static string ToPersistentDataPath(string filename)
        {
            return Path.Combine(Application.persistentDataPath, filename);
        }

        public static string ToStreamingAssetsPath(string filename)
        {
            return Path.Combine(Application.streamingAssetsPath, filename);
        }

        #endregion

        #region Secure IO

        public static void WriteSecure<T>(string filename, T data)
        {
            if (s_EncryptionKey == DEFAULT_ENCRYPTION_KEY) Debug.LogWarning("[IO] WARNING: Encrypting file with default, publicly known key");

            FileStream writeStream = null;
            try
            {
                writeStream = File.Open(filename, FileMode.Create);
                try
                {
                    if(IsVerbose)
                        Debug.Log(string.Format("[IO] Writing Encrypted Binary File {0}", filename));
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
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
            }
            finally
            {
                if (writeStream != null)
                {
                    writeStream.Close();
                }
            }
        }

        public static T ReadSecure<T>(string filename)
        {
            T data = default(T);
            FileStream readStream = null;
            try
            {
                readStream = File.Open(filename, FileMode.Open);
                try
                {
                    if(IsVerbose)
                        Debug.Log(string.Format("[IO] Reading Encrypted Binary File {0}", filename));
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
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
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

        #region Binary IO

        public static void WriteBinary<T>(string filename, T data)
        {
            FileStream writeStream = null;
            try
            {
                writeStream = File.Open(filename, FileMode.Create);
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Writing Binary File {0}", filename));
                s_Binaryformatter.Serialize(writeStream, data);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
            }
            finally
            {
                if (writeStream != null)
                {
                    writeStream.Close();
                }
            }
        }

        public static T ReadBinary<T>(string filename)
        {
            T data = default(T);
            FileStream readStream = null;
            try
            {
                readStream = File.Open(filename, FileMode.Open);
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Reading Binary File {0}", filename));
                data = (T)s_Binaryformatter.Deserialize(readStream);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
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

        #region XML IO

        public static void WriteXML<T>(string filename, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream writeStream = null;
            try
            {
                writeStream = File.Open(filename, FileMode.Create);
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Writing XML File {0}", filename));
                serializer.Serialize(writeStream, data);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
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
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Reading XML File {0}", filename));
                data = (T)serializer.Deserialize(readStream);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
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

        #region PlayerPrefs IO

        public static void WritePlayerPref<T>(string filename, T data)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Writing In Player Pref File {0}", filename));
                s_Binaryformatter.Serialize(memoryStream, data);
                PlayerPrefs.SetString(filename, System.Convert.ToBase64String(memoryStream.ToArray()));
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
            }
            finally
            {
                memoryStream.Close();
            }
        }

        public static T ReadPlayerPref<T>(string filename)
        {
            T data = default(T);
            if (PlayerPrefs.HasKey(filename))
                return data;

            MemoryStream dataStream = null;
            try
            {
                if(IsVerbose)
                    Debug.Log(string.Format("[IO] Reading In Player Pref File {0}", filename));
                string serializedData = PlayerPrefs.GetString(filename);
                dataStream = new MemoryStream(Convert.FromBase64String(serializedData));
                data = (T) s_Binaryformatter.Deserialize(dataStream);
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("[IO] {0}", ex));
            }
            finally
            {
                if (dataStream != null)
                {
                    dataStream.Close();
                }
            }

            return data;
        }

        #endregion
    }
}