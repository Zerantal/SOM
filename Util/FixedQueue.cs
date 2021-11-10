using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class FixedQueue<T> : Queue<T>
    {
        private int size = -1;

        public int FixedSize
        {
            get { return size; }
            set { size = value; }
        }

        public FixedQueue(int fixedSize)
            : base(fixedSize)
        {
            this.FixedSize = fixedSize;
        }

        public new void Enqueue(T item)
        {
            while (this.Count >= this.FixedSize)
            {
                this.Dequeue();
            }
            base.Enqueue(item);
        }
    }
}
