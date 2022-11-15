using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Mulithraeding
{
    public class PixelChange
    {
        public WaitHandle WaitHandle { get; set; }
        public Action< int,int> Action { get; set; }
      //  public int Coordinates { get; }

        public int Prev { get; }
        public int Width { get; }
        public PixelChange(Action<int,int> action, int width, int prev)
        {
            Action = action;
            Width = width;
            Prev = prev;
        }

    }
}
