namespace Klinkby.CleanFn.Core.Models;

/// <summary>
///     Simple thread-safe counter for tracking success rate.
/// </summary>
internal record SuccessCounter : ISuccessSubscriber
{
    private const uint MaxValue = 1024;
    private const int Shift = 4;

    private readonly ReaderWriterLockSlim _lock = new();
    private uint _failures;
    private uint _successes;

    public float SuccessRatio
    {
        get
        {
            _lock.EnterReadLock();
            var successes = _successes;
            var failures = _failures;
            try
            {
                return successes + failures == 0 ? 1f : (float)successes / (successes + failures);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    public void IncrementSuccess()
    {
        var count = Interlocked.Increment(ref _successes);
        if (count < MaxValue)
        {
            return;
        }

        Reduce(ref _successes);
    }

    public void IncrementFailure()
    {
        var count = Interlocked.Increment(ref _failures);
        if (count < MaxValue)
        {
            return;
        }

        Reduce(ref _failures);
    }

    private void Reduce(ref uint value)
    {
        var success = _lock.TryEnterWriteLock(0); // immediately return if lock is taken
        if (!success)
        {
            return;
        }

        try
        {
            if (value < MaxValue)
            {
                return;
            }

            _successes >>= Shift;
            _failures >>= Shift;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
