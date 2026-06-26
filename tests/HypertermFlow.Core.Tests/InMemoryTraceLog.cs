using System;
using System.Collections.Generic;
using HypertermFlow.Core.Logging;

namespace HypertermFlow.Core.Tests
{
    /// <summary>
    /// Log en memoria para tests: guarda cada entrada en una lista (para
    /// aserciones) y la imprime en consola (para evidencia visual de la corrida).
    /// </summary>
    public sealed class InMemoryTraceLog : ITraceLog
    {
        public readonly List<string> Entries = new List<string>();

        public void Write(string category, string message)
        {
            string line = category + " | " + message;
            Entries.Add(line);
            Console.WriteLine("    " + line);
        }

        /// <summary>Coordenadas de cada ClickLeft, en orden (ej. "(255,191)").</summary>
        public List<string> ClickCoordinates()
        {
            List<string> coords = new List<string>();
            foreach (string e in Entries)
            {
                int i = e.IndexOf("ClickLeft (");
                if (i >= 0)
                {
                    int start = e.IndexOf('(', i);
                    int end = e.IndexOf(')', start);
                    if (start >= 0 && end > start)
                        coords.Add(e.Substring(start, end - start + 1));
                }
            }
            return coords;
        }

        public int Count(string fragment)
        {
            int n = 0;
            foreach (string e in Entries)
                if (e.IndexOf(fragment) >= 0) n++;
            return n;
        }
    }
}
