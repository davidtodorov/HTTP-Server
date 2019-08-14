﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Excceptions
{
    public class BadRequestException : Exception
    {
        private const string BadRequestExceptionDefaultMessage = "The Request was malformed or contains unsupported elements.";
        public BadRequestException() : this(BadRequestExceptionDefaultMessage)
        {

        }
        public BadRequestException(string message) : base(message)
        {

        }
    }
}
