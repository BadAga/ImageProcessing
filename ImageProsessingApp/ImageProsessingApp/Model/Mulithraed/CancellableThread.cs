using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Mulithraed
{
    public class CancellableThread
    {
        private CancellationTokenSource CancellationTS { get; }
        public Thread Thread {get;}

        public CancellableThread(ThreadStart threadStart)
        {
            CancellationTS = new CancellationTokenSource();
            Thread = new Thread(threadStart);
        }

        public CancellableThread(ThreadStart threadStart, CancellationTokenSource cancellationTS)
        {
            CancellationTS = cancellationTS;
            Thread = new Thread(threadStart);
            Thread.IsBackground = true;
        }
    }
}
