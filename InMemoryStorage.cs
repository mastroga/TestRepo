
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Interview
{
    public class InMemoryStorage
    {
        public static IRepository<IStoreable> GetInMemoryDefaultStorageObj()
        {
            return new Repository<IStoreable>(new Collection<IStoreable>());
        }

        // Sample implementation using an alternative type
        public static IRepository<IStoreable> GetInMemoryHashSetStorageObj()
        {
            return new Repository<IStoreable>(new HashSet<IStoreable>());
        }

    }
}
