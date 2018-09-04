using System;
using System.Threading;
using System.Threading.Tasks;

namespace MainProgram
{
    public abstract class Doer<TViewModel, TSrc> where TViewModel : ViewModelPauseable
    {
        protected TViewModel viewModel;

        public Doer(TViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Begin()
        {
            Task.Factory.StartNew(new Action(BeginAsync));
        }

        private void BeginAsync()
        {
            TSrc[] Src = Initialize();

            viewModel.BeginCount(Src.Length);
            viewModel.IsDoing = true;

            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 10,
            };

            Parallel.ForEach(Src, po, new Action<TSrc>(DoPrepare));

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

        private void DoPrepare(TSrc obj)
        {
            lock (this)
            {
                while (viewModel.IsPause) Monitor.Wait(this);
            }

            if (!viewModel.IsDoing) return;

            Do(obj);

            viewModel.IncreaseCurrentCount();
        }

        protected abstract void Do(TSrc obj);
    }
}
