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
        public Action<double, int> Action { get; set; }

        public double Range { get; }
        public int Coordinates { get; }

        public PixelChange(Action<double, int> action, double range, int coordinates)
        {
            Action = action;
            Range = range;
            Coordinates = coordinates;
        }
    }
}
