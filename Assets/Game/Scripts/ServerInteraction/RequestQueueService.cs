using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Requests
{
    public class RequestQueueService : IInitializable, IDisposable
    {
        private readonly Queue<QueuedRequest> queue = new();
        private readonly ReactiveCommand onRequestCompleted = new();
        private bool isProcessing;
        private CancellationTokenSource currentCts;
        private string currentTag;

        public Observable<Unit> OnRequestCompleted => onRequestCompleted;

        public void Initialize()
        {
        }

        public async UniTask<T> EnqueueAsync<T>(Func<CancellationToken, UniTask<T>> requestFactory, string tag = null)
        {
            var tcs = new UniTaskCompletionSource<T>();
            var queued = new QueuedRequest
            {
                Tag = tag,
                Execute = async ct =>
                {
                    try
                    {
                        var result = await requestFactory(ct);
                        tcs.TrySetResult(result);
                    }
                    catch (OperationCanceledException)
                    {
                        tcs.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            };

            queue.Enqueue(queued);
            ProcessQueue();

            return await tcs.Task;
        }

        public void CancelByTag(string tag)
        {
            if (currentCts != null && currentTag == tag)
            {
                currentCts.Cancel();
            }

            var newQueue = new Queue<QueuedRequest>();
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item.Tag != tag)
                    newQueue.Enqueue(item);
            }
            queue.Clear();
            while (newQueue.Count > 0) queue.Enqueue(newQueue.Dequeue());
        }

        private async void ProcessQueue()
        {
            if (isProcessing) return;
            isProcessing = true;

            while (queue.Count > 0)
            {
                var request = queue.Dequeue();
                currentTag = request.Tag;
                currentCts = new CancellationTokenSource();

                try
                {
                    await request.Execute(currentCts.Token);
                    currentTag = string.Empty;
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request failed: {ex}");
                }
                finally
                {
                    currentCts.Dispose();
                    currentCts = null;
                }
                onRequestCompleted.Execute(Unit.Default);
            }

            isProcessing = false;
        }

        public void Dispose()
        {
            currentCts?.Cancel();
            currentCts?.Dispose();
            onRequestCompleted.Dispose();
        }
    }

    public class QueuedRequest
    {
        public string Tag { get; set; }
        public Func<CancellationToken, UniTask> Execute { get; set; }
    }
}