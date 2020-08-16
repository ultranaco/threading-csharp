using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Falcon.Threading
{
  public class SemaphoreLock : IDisposable
  {
    public static ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

    private SemaphoreSlim _semaphore;

    public SemaphoreLock(string semaphoreName, int timeOut = 5000, int concurrency = 2)
    {
      _semaphore = _semaphores.GetOrAdd(semaphoreName, (key) => { return new SemaphoreSlim(0, concurrency); });
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
        throw e;
      }
    }
  }
}
