namespace BashSoft.Exeptions
{
    using System;

    public class InvalidCommandException : Exception
    {
        public const string InvalidCommandMessage = "The command '{0}' is invalid";

        public InvalidCommandException(string command)
            : base(string.Format(InvalidCommandMessage, command))
        {
        }
    }
}