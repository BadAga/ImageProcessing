using ImageProsessingApp.Model.Mulithraeding;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Multithreading
{
    public class MultithreadingManager
    {
        private ConcurrentQueue<MyThread> threads = new ConcurrentQueue<MyThread>();
        private ConcurrentQueue<PixelChange> pixelChanges = new ConcurrentQueue<PixelChange>();

        private int threadCount = 0;
        public int ThreadCount
        {
            get { return threadCount; }
            set { threadCount = value; }
        }
        private static MultithreadingManager s_instance = null;
        private static object s_lock = new object();
        public static MultithreadingManager Instance
        {
            get
            {
                lock (s_lock)
                {
                    if (s_instance == null)
                    {
                        s_instance = new MultithreadingManager(0);
                    }
                    return s_instance;
                }
            }
        }
        private MultithreadingManager(int threadCount)
        {
            UpdateThreadCount(threadCount);
        }

        private MultithreadingManager(int threadCount, ConcurrentQueue<PixelChange> _pixelChanges)
        {
            UpdateThreadCount(threadCount);
        }
        ~MultithreadingManager()
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
        public void AddThread()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cThread = new MyThread(() =>
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    PixelChange pixelChange;
                    if (pixelChanges.TryDequeue(out pixelChange))
                    {
                        pixelChange.Action(pixelChange.Coordinates);

                        ((AutoResetEvent)pixelChange.WaitHandle).Set();
                    }
                }
                Thread.Sleep(20);
            }, cancellationTokenSource);
            threads.Enqueue(cThread);
            cThread.Thread.Start();
        }

        private void RemoveThread()
        {
            var threadToRemove = threads.TryDequeue(out MyThread t);
        }
    }
}
