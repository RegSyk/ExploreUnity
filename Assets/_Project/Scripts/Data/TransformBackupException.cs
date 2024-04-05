using System;
namespace Explore
{
    public class TransformBackupException : Exception
    {
        public TransformBackupException() { }

        public TransformBackupException(string message)
            : base(message) { }

        public TransformBackupException(string message, Exception inner)
            : base(message, inner) { }
    }
}
