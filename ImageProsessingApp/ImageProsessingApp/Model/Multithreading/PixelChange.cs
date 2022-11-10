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
        public Action< int> Action { get; set; }
        public int Coordinates { get; }

        public PixelChange(Action<int> action, int coordinates)
        {
            Action = action;
            Coordinates = coordinates;
        }

    }
}
