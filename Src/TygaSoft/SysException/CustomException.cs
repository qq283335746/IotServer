using System;
using System.Runtime.Serialization;

namespace TygaSoft.SysException
{
    public class CustomException : Exception
    {
        public int ResCode { get; set; }
        public CustomException() { }

        public CustomException(string message) : base(message)
        {

        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

    }
}
