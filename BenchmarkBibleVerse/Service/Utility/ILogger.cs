﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkBibleVerse.Service.Utility
{
    public interface ILogger
    {
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
