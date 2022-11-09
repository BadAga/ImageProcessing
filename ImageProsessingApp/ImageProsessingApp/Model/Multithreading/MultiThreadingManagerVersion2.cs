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
    public class MultiThreadingManagerVersion2
    {
        private ConcurrentQueue<MyThread> threads = new ConcurrentQueue<MyThread>();
        private ConcurrentQueue<PixelChange> pixelChanges = new ConcurrentQueue<PixelChange>();

        public MultiThreadingManagerVersion2(int numberofThreads, ConcurrentQueue<PixelChange> pixelChanges)
        {
            this.pixelChanges = pixelChanges;
            AddThread(numberofThreads);
        }

        private void AddThread(int quantity)
        {
            for(int i=0;i<quantity;i++)
            {
                var newCancellationTokenSource = new CancellationTokenSource();
                var newThread = new MyThread(()=>WorkerMethod(newCancellationTokenSource),
                                                            newCancellationTokenSource);
                this.threads.Enqueue(newThread);
            }
        }
        private void WorkerMethod(CancellationTokenSource cancellationTokenSource)
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
        }
        public void CompleteTasks()
        {
            foreach (var thread in this.threads)
            {
                thread.Thread.Start();
            }
            while(this.pixelChanges.Count!=0)
            {
                
            }
        }
        ~MultiThreadingManagerVersion2()
        {
            foreach (var thread in this.threads)
            {
                var threadToRemove = threads.TryDequeue(out MyThread t);
            }
        }
    }
}
