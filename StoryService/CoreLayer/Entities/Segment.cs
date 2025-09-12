using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class Segment
    {
        public int StartChar { get; set; }  // 0-based, inclusive
        public int EndChar { get; set; }    // 0-based, exclusive
    }
}
