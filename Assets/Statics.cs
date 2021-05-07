using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class Statics
{
    public static void PrintHelp() 
    {
        Debug.Log($"{Application.productName} v{Application.version}. \r\nUsage:\r\n" +
        " -batchmode      Unity argument to run without a window (headless)\r\n" +
        " -url            URL to download\r\n" +
        " -id             ID of the file\r\n" +
        " -ver            file version\r\n" +
        " -path           cache path\r\n" +
        " -uncompressed   save bundles uncompressed");
        Application.Quit();
    }
    public static string GetArg(string name)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals(name, StringComparison.OrdinalIgnoreCase) && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
    public static bool GetFlag(string name)
    {
        var args = Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            if(arg.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
    public static string GetHashFromString(string str)
    {
        return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str)).ToHex(true).Substring(0, 16);
    }
    public static string ToHex(this byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

        return result.ToString();
    }
}