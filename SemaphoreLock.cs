using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ultranaco.Threading
{
  public class SemaphoreLock : IDisposable
  {
    public static ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

    private SemaphoreSlim _semaphore;
    private string _name;
    private int _concurrency;
    private int _timeout;

    public SemaphoreLock(string semaphoreName, int timeout = 1000, int concurrency = 2, bool open = false)
    {
      this._name = semaphoreName;
      this._concurrency = concurrency;
      this._timeout = timeout;

      if (open)
        this.Open();
    }

    public void Open(Action action)
    {
      Exception exception = null;

      try
      {
        this.Open();
        action();
      }
      catch (Exception e)
      {
        exception = e;
      }
      finally
      {
        this.Dispose();
        if (exception != null)
          throw exception;
      }
    }

    public async Task OpenAsync(Func<Task> action)
    {
      Exception exception = null;

      try
      {
        this.Open();
        await action();
      }
      catch (Exception e)
      {
        exception = e;
      }
      finally
      {
        this.Dispose();
        if (exception != null)
          throw exception;
      }
    }

    public void Open()
    {
      this._semaphore = _semaphores.GetOrAdd(this._name, (key) =>
      {
        return new SemaphoreSlim(this._concurrency, this._concurrency);
      });

      _semaphore.Wait(this._timeout);
    }

    public void Dispose()
    {
      _semaphore.Release();
    }
  }
}
