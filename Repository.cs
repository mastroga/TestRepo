using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Interview
{
    public class Repository<T> :
        IRepository<T>, IDisposable
        where T : IStoreable
    {
        private readonly ICollection<T> _repoData;
        private bool _disposed;

        protected ICollection<T> RepoData { get => _repoData; }

        public Repository(ICollection<T> repositoryCollection)
        {
            if (repositoryCollection is null)
            {
                throw new ArgumentNullException(nameof(repositoryCollection));
            }
            _repoData = repositoryCollection;
        }

        public virtual IEnumerable<T> All()
        {
            return RepoData;
        }

        public virtual void Delete(IComparable id)
        {
            var match = FindById(id);
            RepoData.Remove(match);
        }

        public virtual T FindById(IComparable id)
        {
            return RepoData.FirstOrDefault(m => m.Id == id);
        }

        public virtual void Save(T item)
        {
            if (item != null)
            {
                Delete(item.Id);
                RepoData.Add(item);
            }
        }

        ~Repository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    GC.SuppressFinalize(this);
                }
                _disposed = true;
            }
        }

        //private void CheckDisposed()
        //{
        //    if (_disposed)
        //    {
        //        throw new ObjectDisposedException(GetType().FullName);
        //    }
        //}

    }

}


