using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Ultranaco.Threading
{
  public class SemaphoreLock : IDisposable
  {
    public static ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
    private SemaphoreSlim _semaphore;
    private string Name;

    /// <summary> SemaphoreLock lock execution by tag name and concurrent degree</summary>
    /// <param name="semaphoreName">Used to tag many instances of semaphores</param>
    /// <param name="concurrency">Number of executions allowed by semaphoreName</param>
    public SemaphoreLock(string semaphoreName, int concurrency = 1, int timeOut = 5000)
    {
      this.Name = semaphoreName;
      this._semaphore = _semaphores.GetOrAdd(semaphoreName, (key) => { return new SemaphoreSlim(concurrency, concurrency); });
      _semaphore.Wait(timeOut);
    } 

    public void Dispose()
    {
      try
      {
          _semaphore.Release();
      }
      catch
      {
        Console.WriteLine("Semaphore Counts: {0}", _semaphore.CurrentCount);
      }
    }
  }
}
