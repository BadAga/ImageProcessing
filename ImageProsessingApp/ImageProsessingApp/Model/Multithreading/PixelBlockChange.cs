using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model.Mulithraeding
{
    public class PixelBlockChange
    {
        public WaitHandle WaitHandle { get; set; }
        public Action Action { get; set; }
        public PixelBlockChange(Action action)
        {
            Action = action;
        }
    }
}
