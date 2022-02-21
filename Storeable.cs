using System;

namespace Interview
{

    public class Storeable : IStoreable, IDisposable
    {
        public IComparable Id { get; set; }

        public void Dispose()
        {
        }

        public override int GetHashCode()
        {
            int hash = 37;
            hash = hash * 23 + base.GetHashCode();
            hash = hash * 23 + Id.GetHashCode();
            return hash;
        }
    }
}



