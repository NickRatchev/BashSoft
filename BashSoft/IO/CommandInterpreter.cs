namespace BashSoft
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BashSoft.Attributes;
    using BashSoft.Exeptions;
    using BashSoft.IO.Commands;
    using Contracts;

    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            string[] data = input.Split(' ');
            string commandName = data[0].ToLower();

            try
            {
                IExecutable command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private IExecutable ParseCommand(string input, string[] data, string command)
        {
            try
            {
                object[] parametersForConstruction = new object[] { input, data };

                Type typeOfCommand =
                    Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .First(type => type.GetCustomAttributes(typeof(AliasAttribute))
                                           .Where(atr => atr.Equals(command))
                                           .ToArray()
                                           .Length > 0);

                Type typeOfInterpreter = typeof(CommandInterpreter);

                Command exe = (Command)Activator.CreateInstance(typeOfCommand, parametersForConstruction);
                FieldInfo[] fieldsOfComand =
                    typeOfCommand.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo[] fieldsOfInterpreter =
                    typeOfInterpreter.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo fieldOfCommand in fieldsOfComand)
                {
                    Attribute atr = fieldOfCommand.GetCustomAttribute(typeof(InjectAttribute));
                    if (atr != null)
                    {
                        if (fieldsOfInterpreter.Any(f => f.FieldType == fieldOfCommand.FieldType))
                        {
                            fieldOfCommand
                                .SetValue(exe, fieldsOfInterpreter.First(f => f.FieldType == fieldOfCommand.FieldType)
                                .GetValue(this));
                        }
                    }
                }

                return exe;
            }
            catch (Exception)
            {
                throw new InvalidCommandException(input);
            }
        }

        private void TryDownloadFile(string input, string[] data)
        {
            throw new NotImplementedException();
        }

        private void TryDownloadFileAsynch(string input, string[] data)
        {
            throw new NotImplementedException();
        }
    }
}