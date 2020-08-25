using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Editure.Backend.ViewModels;

namespace Editure.Backend.Doer
{
    public abstract class Doer<TViewModel, TSrc> where TViewModel : ViewModelPauseable
    {
        protected readonly TViewModel viewModel;
        private readonly Queue<Task> tasks;

        protected Doer(TViewModel viewModel)
        {
            this.viewModel = viewModel;
            tasks = new Queue<Task>();
        }

        public void Begin()
        {
            Task.Factory.StartNew(BeginAsync);
        }

        private async Task BeginAsync()
        {
            TSrc[] src = Initialize();

            viewModel.BeginCount(src.Length);
            viewModel.IsDoing = true;

            tasks.Clear();
            Parallel.ForEach(src, DoPrepare);

            while (tasks.Count > 0) await tasks.Dequeue();

            viewModel.IsDoing = false;
        }

        protected abstract TSrc[] Initialize();

        public void Pause()
        {
            viewModel.IsPause = true;

            lock (this)
            {
                Monitor.PulseAll(this);
            }
        }

        public void Resume()
        {
            viewModel.IsPause = false;

            lock (this)
            {
                Monitor.PulseAll(this);
            }
        }

        public void Stop()
        {
            viewModel.IsDoing = false;

            lock (this)
            {
                Monitor.PulseAll(this);
            }
        }

        private async void DoPrepare(TSrc obj)
        {
            lock (this)
            {
                while (viewModel.IsPause) Monitor.Wait(this);
            }

            if (!viewModel.IsDoing) return;

            Task task = Do(obj);
            
            if (!task.IsCompleted)
            {
                lock (tasks) tasks.Enqueue(task);

                await task;
            }

            viewModel.IncreaseCurrentCount();
        }

        protected abstract Task Do(TSrc obj);
    }
}