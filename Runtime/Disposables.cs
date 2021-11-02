using System;
using System.Collections.Generic;

namespace Abg
{
    public struct Disposables : IDisposable
    {
        private List<IDisposable> disposables;

        public void Dispose()
        {
            if (disposables == null) return;
            foreach (var d in disposables)
            {
                d.Dispose();
            }
            disposables = null;
        }

        public static Disposables operator +(Disposables disposables, IDisposable d)
        {
            if (disposables.disposables == null) disposables.disposables = new List<IDisposable>();
            disposables.disposables.Add(d);
            return disposables;
        }
    }
}