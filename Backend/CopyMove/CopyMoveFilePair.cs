using System;
using System.IO;
using System.Threading.Tasks;
using StdOttStandard.AsyncResult;

namespace Editure.Backend.CopyMove
{
    public class CopyMoveFilePair
    {
        private readonly AsyncResult<string> asyncResult;
        
        public FileInfo Source { get; }

        public string DestFilePath { get; }

        public bool Copy { get; }

        public CollisionType Collision { get; }

        public Task<string> Task => asyncResult.Task;

        public CopyMoveFilePair(FileInfo source, string destFileFilePath, bool copy, CollisionType collision)
        {
            Source = source;
            DestFilePath = destFileFilePath;
            Copy = copy;
            Collision = collision;
            asyncResult = new AsyncResult<string>();
        }

        public CopyMoveFilePair(CopyMoveFilePair pair, CollisionType? overrideCollision = null)
        {
            Source = pair.Source;
            DestFilePath = pair.DestFilePath;
            Copy = pair.Copy;
            Collision = overrideCollision ?? pair.Collision;
            asyncResult = pair.asyncResult;
        }

        public void Complete()
        {
            asyncResult.SetValue(DestFilePath);
        }

        public void Cancel()
        {
            asyncResult.SetValue(null);
        }
    }
}