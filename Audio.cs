#region Using statements

using UnityEngine;

#endregion

namespace FistBump.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>FistBump.ca - Copyright (C)</remarks>
    public static class Audio
    {
        #region Public Methods

        /// <summary>
        /// Plays a sound by creating an empty game object with an AudioSource
        /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="emitter"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static AudioSource Play(AudioClip clip, Transform emitter, float volume = 1.0f, float pitch = 1.0f)
        {
            //Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = emitter.position;
            go.transform.parent = emitter;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
            Object.Destroy(go, clip.length);
            return source;
        }

        /// <summary>
        /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
        /// in that place and destroys it after it finished playing.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="point"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static AudioSource Play(AudioClip clip, Vector3 point, float volume = 1.0f, float pitch = 1.0f)
        {
            //Create an empty game object
            GameObject go = new GameObject("Audio: " + clip.name);
            go.transform.position = point;

            //Create the source
            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
            Object.Destroy(go, clip.length);
            return source;
        }

        #endregion
    }
}