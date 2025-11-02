using UnityEngine;
using System.Collections.Generic;

public class DebugDisplay : MonoBehaviour
{
    private static List<string> logs = new List<string>();
    private static int maxLogs = 10;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logs.Add(logString);
        if (logs.Count > maxLogs)
            logs.RemoveAt(0);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        foreach (string log in logs)
        {
            GUILayout.Label(log);
        }
        GUILayout.EndArea();
    }
}