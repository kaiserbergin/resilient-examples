using Prometheus;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

    Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken);
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private ConcurrentQueue<Func<CancellationToken, Task>> _workItems =
        new ConcurrentQueue<Func<CancellationToken, Task>>();
    private SemaphoreSlim _signal = new SemaphoreSlim(0);
    private readonly IGauge _queuedItemsGauge;

    public BackgroundTaskQueue(IGauge queuedItemsGauge)
    {
        _queuedItemsGauge = queuedItemsGauge ?? throw new ArgumentNullException(nameof(queuedItemsGauge));
    }

    public void QueueBackgroundWorkItem(
        Func<CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _workItems.Enqueue(workItem);
        _queuedItemsGauge.Inc();
        _signal.Release();
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _workItems.TryDequeue(out var workItem);
        _queuedItemsGauge.Dec();

        return workItem;
    }
}