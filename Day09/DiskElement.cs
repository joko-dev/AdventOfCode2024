using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day09
{
    internal class DiskElement
    {
        public int Length {  get; internal set; }
        public int ID { get; internal set; }
        public bool IsFile { get; internal set; }

        public DiskElement(int length, int id, bool isFile)
        {
            this.Length = length;
            this.ID = id;
            this.IsFile = isFile;
        }
    }
}
