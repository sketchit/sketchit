using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SketchIt.Api.Internal
{
    public class ThreadLocker : IDisposable
    {
        private static object _defaultLock = new object();
        private static List<object> _lockedObjects = new List<object>();
        private static List<object> _blockedObjects = new List<object>();

        public static ThreadLocker AttemptLock(object target) { return AttemptLock(target, 100); }
        public static ThreadLocker AttemptLock(object target, int timeout)
        {
            return new ThreadLocker(target, timeout);
        }

        public static ThreadLocker Lock() { return Lock(null, 100); }
        public static ThreadLocker Lock(object target) { return Lock(null, 100); }
        public static ThreadLocker Lock(object target, int timeout)
        {
            return new ThreadLocker(target, timeout);
        }

        public static object[] GetLockedObjects()
        {
            lock (_lockedObjects)
                return _lockedObjects.ToArray().Clone() as object[];
        }

        public static object[] GetBlockedObjects()
        {
            lock (_blockedObjects)
                return _blockedObjects.ToArray().Clone() as object[];
        }

        public object LockedObject;
        public string ObjectName;
        public string Trace;
        public bool IsLocked;

        private ThreadLocker(object target, int timeout)
        {
            IsLocked = false;
            LockedObject = target ?? _defaultLock;
            ObjectName = LockedObject.ToString();
            Trace = new StackTrace().ToString();

            lock (_blockedObjects)
                _blockedObjects.Add(this);

            if (Monitor.TryEnter(LockedObject, timeout == -1 ? 10000 : timeout))
            {
                lock (_lockedObjects)
                    _lockedObjects.Add(this);

                IsLocked = true;
            }

            lock (_blockedObjects)
                _blockedObjects.Remove(this);
        }

        public void Dispose()
        {
            if (IsLocked)
            {
                Monitor.Exit(LockedObject);

                lock (_lockedObjects)
                    _lockedObjects.Remove(this);

                IsLocked = false;
            }

            LockedObject = null;
        }

        ~ThreadLocker()
        {
            //this.Dispose();
        }
    }

    //public class ThreadLocker : IDisposable
    //{
    //    private static readonly object _locker = new object();

    //    private ThreadLocker(int timeout)
    //    {
    //        IsLocked = Monitor.TryEnter(_locker, timeout);
    //    }

    //    public static ThreadLocker Lock(int timeout)
    //    {
    //        return new ThreadLocker(timeout);
    //    }

    //    public static ThreadLocker Lock()
    //    {
    //        return Lock(50);
    //    }

    //    public bool IsLocked
    //    {
    //        get;
    //        private set;
    //    }

    //    public void Dispose()
    //    {
    //        try
    //        {
    //            if (IsLocked)
    //            {
    //                Monitor.Exit(_locker);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ex.ToString();
    //        }
    //    }
    //}
}
