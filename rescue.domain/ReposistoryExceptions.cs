using System;

namespace rescue.domain
{
    public class NotFoundException : Exception
    {
        //public NotFoundException() : base("No resource found")
        //{
        //}

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
