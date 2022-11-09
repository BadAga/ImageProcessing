using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Multithreading
{
    public class MyThread
    {
        private CancellationTokenSource CancellationTS { get; }
        public Thread Thread { get; }

        public MyThread(ThreadStart threadStart, CancellationTokenSource cancellationTS)
        {
            CancellationTS = cancellationTS;
            Thread = new Thread(threadStart);
            Thread.IsBackground = true;
        }
    }
}
