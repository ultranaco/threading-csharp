using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Falcon.Threading
{
  public class SemaphoreLock : IDisposable
  {
    public static ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

    private SemaphoreSlim _semaphore;
    private bool _raiseException;

    public SemaphoreLock(string semaphoreName, int timeOut = 1000, int concurrency = 2, bool raiseException = false)
    {
      _raiseException = raiseException;
      _semaphore = _semaphores.GetOrAdd(semaphoreName, (key) => { return new SemaphoreSlim(concurrency, concurrency); });
      _semaphore.Wait(timeOut);
    }

    public void Dispose()
    {
      try
      {
        _semaphore.Release();
      }
      catch(Exception e)
      {
        if (_raiseException)
          throw e;
      }
    }
  }
}
