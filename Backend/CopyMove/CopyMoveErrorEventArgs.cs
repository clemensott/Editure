using System;

namespace Editure.Backend.CopyMove
{
    public class CopyMoveErrorEventArgs : EventArgs
    {
        public CopyMoveFilePair Pair { get; }

        public Exception Exception { get; }

        public CopyMoveErrorEventArgs(CopyMoveFilePair pair, Exception exception = null)
        {
            Pair = pair;
            Exception = exception;
        }
    }
}