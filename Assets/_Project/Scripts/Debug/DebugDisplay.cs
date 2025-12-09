using UnityEngine;
using System.Collections.Generic;

public class DebugDisplay : MonoBehaviour
{
    private static readonly List<string> logs = new();
    private static readonly int maxLogs = 25;
    private bool _showDebug = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
            _showDebug = !_showDebug;
    }

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
        if (!_showDebug) return;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.normal.textColor = Color.white;

        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        foreach (string log in logs)
        {
            GUILayout.Label(log, style);
        }
        GUILayout.EndArea();
    }
}