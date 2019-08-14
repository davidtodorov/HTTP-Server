using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Excceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string InternalServerErrorExceptionDefaultMessage = "The Server has encountered an error.";
        public InternalServerErrorException() : this(InternalServerErrorExceptionDefaultMessage)
        {

        }

        public InternalServerErrorException(string message) : base(message)
        { 

        }
    }
}
