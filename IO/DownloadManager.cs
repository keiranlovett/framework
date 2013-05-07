using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace FistBump.Framework
{
    public class DownloadManager : SingletonMonoBehaviour<DownloadManager>
    {
        private class Downloadable
        {
            public string Url { get; set; }
            public DownloadCallback Callback { get; set; }

            public Downloadable(string url, DownloadCallback fn)
            {
                Url = url;
                Callback = fn;
            }
        }

        private WWW m_Downloader;
        public delegate void DownloadCallback(WWW downloader);
        private static readonly Queue<Downloadable> s_Queue = new Queue<Downloadable>();

        void StartNextDownload()
        {
            if (s_Queue.Count == 0 || (m_Downloader != null && m_Downloader.isDone == false))
            {
                return;
            }
            StartCoroutine("StartDownload");
        }

        private IEnumerator OnDownload(Downloadable toDownload)
        {
            m_Downloader = new WWW(toDownload.Url);
            yield return m_Downloader;
        }

        private IEnumerator StartDownload()
        {
            Downloadable toDownload = s_Queue.Dequeue();
            yield return StartCoroutine("OnDownload", toDownload);
            Debug.Log("downloaded: " + toDownload.Url);
            toDownload.Callback(m_Downloader);
            StartNextDownload();
        }

        public void Download(string url, DownloadCallback callback)
        {
            s_Queue.Enqueue(new Downloadable(url, callback));
            StartNextDownload();
        }
    }
}