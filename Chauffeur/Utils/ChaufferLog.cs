using System;
using System.IO;
using System.Threading;
using BepInEx.Logging;

namespace Chauffer.Utils;

public class ChaufferLog : ILogListener, IDisposable
{
    public LogLevel DisplayedLogLevel;
    public TextWriter LogWriter;
    public Timer FlushTimer;

    public ChaufferLog(string path, LogLevel logLevel)
    {
        DisplayedLogLevel = logLevel;
        FileStream fileStream;
        if (!BepInEx.Utility.TryOpenFileStream(path, FileMode.Create, out fileStream, FileAccess.Write))
        {
            Plugin.ChaufferLogger.LogError($"unable to open {path}");
            return;
        }

        LogWriter = TextWriter.Synchronized(new StreamWriter(fileStream, BepInEx.Utility.UTF8NoBom));
        FlushTimer = new Timer(_ => LogWriter?.Flush(), null, 2000, 2000);
    }

    public void LogEvent(object sender, LogEventArgs eventArgs)
    {
        if (eventArgs.Source is UnityLogSource || (eventArgs.Level & DisplayedLogLevel) == LogLevel.None)
            return;
        LogWriter.WriteLine(eventArgs.ToString());
    }

    public void Dispose()
    {
        FlushTimer?.Dispose();
        LogWriter?.Flush();
        LogWriter?.Dispose();
    }

    ~ChaufferLog() => Dispose();
}