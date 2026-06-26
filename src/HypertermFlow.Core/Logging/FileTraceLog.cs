using System;
using System.IO;
using System.Text;
using HypertermFlow.Core.Abstractions;

namespace HypertermFlow.Core.Logging
{
    /// <summary>
    /// Log append-only a archivo: "timestamp | categoria | mensaje".
    /// Thread-safe (la FSM puede invocar desde callbacks de timer).
    /// </summary>
    public sealed class FileTraceLog : ITraceLog
    {
        private readonly object _gate = new object();
        private readonly string _path;
        private readonly IClock _clock;

        public FileTraceLog(string path, IClock clock)
        {
            _path = path;
            _clock = clock;
            string dir = Path.GetDirectoryName(Path.GetFullPath(path));
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public void Write(string category, string message)
        {
            string line = _clock.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                          + " | " + category + " | " + message;
            lock (_gate)
            {
                File.AppendAllText(_path, line + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}
