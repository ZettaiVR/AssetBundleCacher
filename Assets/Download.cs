using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Download : MonoBehaviour
{
    public IEnumerator DownloadBundle() 
    {
        if (Statics.GetFlag("-?"))  
        {
            Statics.PrintHelp();
            yield break;
        }
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        string url = Statics.GetArg("-url");
        string id = Statics.GetArg("-id");
        string versionString = Statics.GetArg("-ver");
        string path = Statics.GetArg("-path");
        bool compressed = !Statics.GetFlag("-uncompressed");
        uint version = 0;
        if (!string.IsNullOrEmpty(versionString)) 
        {
            uint.TryParse(versionString, out version); 
        }
        if (string.IsNullOrEmpty(path))
        {
            path = Application.persistentDataPath + Path.DirectorySeparatorChar + "cache";
        }
        StringBuilder sb = new StringBuilder();
        sb.Append("path: ");
        sb.AppendLine(path);
        sb.Append("id: ");
        sb.AppendLine(id);
        sb.Append("version: ");
        sb.Append(version);
        sb.AppendLine();
        sb.Append("url: ");
        sb.AppendLine(url);
        sb.Append("compression: ");
        sb.AppendLine(compressed ? "LZ4 compressed" : "uncompressed");
        sb.AppendLine("Command line arguments:");
        foreach (var item in Environment.GetCommandLineArgs())
        {
            sb.AppendLine(item);
        }
        Debug.Log(sb.ToString());
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(id) && version > 0)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Hash128 hash = new Hash128(0, 0, 0, version);
            Cache newCache = Caching.AddCache(path);
            if (newCache.valid)
            {
                Caching.compressionEnabled = compressed;
                Caching.currentCacheForWriting = newCache;
            }
            string folder = Statics.GetHashFromString(id);
            CachedAssetBundle cachedAssetBundle = new CachedAssetBundle(folder, hash);
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url, cachedAssetBundle);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                Console.WriteLine("Error! " + request.error);
            }
            stopwatch.Stop();
            Debug.Log("Elapsed ticks: " + stopwatch.ElapsedTicks);
            Application.Quit(1);
        }
        Application.Quit();
    }
    private void Start()
    {
        StartCoroutine(DownloadBundle());
    }
}