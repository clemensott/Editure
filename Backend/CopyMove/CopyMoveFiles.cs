using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Editure.Backend.CopyMove
{
    public class CopyMoveFiles
    {
        private static CopyMoveFiles instance;

        public static CopyMoveFiles Current
        {
            get
            {
                if (instance == null) instance = new CopyMoveFiles();

                return instance;
            }
        }

        private readonly Queue<CopyMoveFilePair> queue;

        public event EventHandler<CopyMoveErrorEventArgs> Error;

        public bool Stopped { get; private set; }

        private CopyMoveFiles()
        {
            queue = new Queue<CopyMoveFilePair>();
            Task.Run((Action)Handle);
        }

        public Task<string> ToFolder(FileInfo src, string destFolderPath, bool copy, CollisionType collision = CollisionType.Error)
        {
            string destFilePath = Path.Combine(destFolderPath, src.Name);
            return ToFile(src, destFilePath, copy, collision);
        }

        public Task<string> ToFile(FileInfo src, string destFilePath, bool copy, CollisionType collision = CollisionType.Error)
        {
            return Enqueue(new CopyMoveFilePair(src, destFilePath, copy, collision));
        }

        public Task<string> EnqueueAgain(CopyMoveFilePair pair, CollisionType? overrideCollision = null)
        {
            return Enqueue(new CopyMoveFilePair(pair, overrideCollision));
        }

        private Task<string> Enqueue(CopyMoveFilePair pair)
        {
            lock (queue)
            {
                queue.Enqueue(pair);
                Monitor.Pulse(queue);
            }

            return pair.Task;
        }

        private void Handle()
        {
            while (!Stopped)
            {
                CopyMoveFilePair pair;
                lock (queue)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(queue);
                        if (Stopped) return;
                    }

                    pair = queue.Dequeue();
                }

                try
                {
                    if (pair.Collision == CollisionType.Error && File.Exists(pair.DestFilePath))
                    {
                        Error?.Invoke(this, new CopyMoveErrorEventArgs(pair));
                        continue;
                    }

                    CreateFolder(Path.GetDirectoryName(pair.DestFilePath));

                    bool force = pair.Collision == CollisionType.Override;
                    string destFilePath = pair.Collision == CollisionType.Unique ?
                        Utils.GetNotExistingFilePath(pair.DestFilePath) : pair.DestFilePath;

                    if (pair.Copy) pair.Source.CopyTo(destFilePath, force);
                    else
                    {
                        if (force && File.Exists(destFilePath)) File.Delete(destFilePath);
                        pair.Source.MoveTo(destFilePath);
                    }

                    pair.Complete();

                }
                catch (Exception e)
                {
                    Error?.Invoke(this, new CopyMoveErrorEventArgs(pair, e));
                }
            }
        }

        private static void CreateFolder(string path)
        {
            Stack<string> missingFolders = new Stack<string>();
            string parent = path.TrimEnd('\\');

            while (true)
            {
                if (Directory.Exists(parent)) break;
                if (parent == Path.GetPathRoot(parent)) throw new Exception("Drive does not exists.");

                missingFolders.Push(parent);

                parent = Path.GetDirectoryName(parent);
            }

            while (missingFolders.Count > 0)
            {
                string folder = missingFolders.Pop();
                Directory.CreateDirectory(folder);
            }
        }

        public void Stop()
        {
            lock (queue)
            {
                Stopped = true;
                Monitor.Pulse(queue);
            }
        }
    }
}