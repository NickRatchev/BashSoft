namespace BashSoft.Exeptions
{
    using System;

    internal class InvalidTakeQuantityException : Exception
    {
        public const string InvalidTakeQuantityParameter = "The take command expected does not match the format wanted!";

        public InvalidTakeQuantityException()
            : base(InvalidTakeQuantityParameter)
        {
        }

        public InvalidTakeQuantityException(string message)
            : base(message)
        {
        }
    }
}