using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{
    public class Index2D
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Index2D(int x = 0, int y = 0) { X = x; Y = y; }
        public Index2D(Index2D a) { X = a.X; Y = a.Y; }

        public static Index2D operator +(Index2D a, Index2D other) => new Index2D() { X = a.X + other.X, Y = a.Y + other.Y };
        public static Index2D operator -(Index2D a, Index2D other) => new Index2D() { X = a.X - other.X, Y = a.Y - other.Y };
        public static Index2D operator *(Index2D a, int other) => new Index2D() { X = a.X * other, Y = a.Y * other };
        public static Index2D operator /(Index2D a, int other) => new Index2D() { X = a.X / other, Y = a.Y / other };
        //public static bool operator ==(Index2D a, Index2D other) => a.X == other.X && a.Y == other.Y;
        //public static bool operator !=(Index2D a, Index2D other) => a.X != other.X || a.Y != other.Y;

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Index2D other:
                    return X == other.X && Y == other.Y;
                default:
                    return false;
            }
        }

        public class Range : IEnumerable<Index2D>, IEnumerable
        {
            public Index2D Min { get; }
            public Index2D Max { get; }

            private Range() { }

            public Range(int w, int h)
            {
                Min = new Index2D(0, 0);
                Max = new Index2D(w, h);
            }

            public IEnumerator<Index2D> GetEnumerator() => new Enumerator(this);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            class Enumerator : IEnumerator<Index2D>, IEnumerator
            {
                public Index2D Current { get; private set; } = null;
                object IEnumerator.Current => Current;

                private Range range = null;
                public Enumerator(Range range)
                {
                    this.range = range;
                }

                public void Dispose() { }

                public bool MoveNext()
                {
                    if(Current == null)
                    {
                        Current = new Index2D(this.range.Min);
                        return true;
                    }
                    if (Current.X >= this.range.Max.X - 1)
                    {
                        Current = new Index2D(this.range.Min.X, Current.Y + 1);
                        if (Current.Y >= this.range.Max.Y)
                        {
                            Reset();
                            return false;
                        }
                    }
                    else
                    {
                        Current = new Index2D(Current.X + 1, Current.Y);
                    }
                    return true;
                }

                public void Reset()
                {
                    Current = null;
                }
            }
        }
    }
}
