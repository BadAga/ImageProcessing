using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Mulithraed
{
    public class ThreadWorker
    {
        private Queue<CancellableThread> threads=new Queue<CancellableThread>();
        private ConcurrentQueue<PixelChange> pixelChanges=new ConcurrentQueue<PixelChange>();

        private int threadCount = 0;
        public int ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }

        public ThreadWorker(int threadCount)
        {
            UpdateThreadCount(threadCount);
        }

        public ThreadWorker(int threadCount, ConcurrentQueue<PixelChange> _pixelChanges)
        {
            UpdateThreadCount(threadCount);
        }

        ~ThreadWorker()
        {
            UpdateThreadCount(0);
        }

        public void UpdateThreadCount(int threadCount)
        {
            if (ThreadCount != threadCount)
            {
                if (threadCount > ThreadCount)
                {
                    for (int i = 0; i < threadCount - ThreadCount; ++i)
                    {
                        AddThread();
                    }
                }
                else
                {
                    for (int i = 0; i < ThreadCount - threadCount; ++i)
                    {
                        RemoveThread();
                    }
                }
                ThreadCount = threadCount;
            }
        }

        public WaitHandle AddPixelChange(PixelChange pixelChange)
        {
            pixelChange.WaitHandle = new AutoResetEvent(false);
            this.pixelChanges.Enqueue(pixelChange);
            return pixelChange.WaitHandle;
        }
        private void AddThread()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cThread = new CancellableThread(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    PixelChange pixelChange;
                    if (pixelChanges.TryDequeue(out pixelChange))
                    {
                        pixelChange.Action(pixelChange.Range,pixelChange.Coordinates);

                        ((AutoResetEvent)pixelChange.WaitHandle).Set();
                    }
                }
            }, cancellationTokenSource);
            threads.Enqueue(cThread);
            cThread.Thread.Start();
        }

        private void RemoveThread()
        {
            var threadToRemove = threads.Dequeue();
        }

    }
}
