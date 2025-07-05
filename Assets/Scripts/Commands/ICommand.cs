using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Models;
using Services;
using Unity.VisualScripting;
using UnityEngine.InputSystem.LowLevel;

namespace Command
{
    public interface ICommand
    {
        string CommandType { get; }
        string PlayerId { get; }
        DateTime Timestamp { get; }
        string CommandId { get; }

        bool Validate();
        bool Execute();
        //string Serialize();
        //string GetDescription();
    }

    public abstract class BaseCommand : ICommand
    {
        public abstract string CommandType { get; }
        public string PlayerId { get; }
        public DateTime Timestamp { get; set; }
        public string CommandId { get; set; }

        public BaseCommand(string playerId)
        {
            PlayerId = playerId;
            Timestamp = DateTime.UtcNow;
            CommandId = Guid.NewGuid().ToString();
        }

        public abstract bool Validate();
        public abstract bool Execute();
        //public abstract string Serialize();
        //public abstract string GetDescription();
    }

    public class CommandValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }
        public List<string> Warnings { get; }
        public CommandValidationResult(bool isValid, string errorMessage = null, List<string> warnings = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            Warnings = warnings ?? new List<string>();
        }

        public static CommandValidationResult Success(List<string> warnings = null) => new CommandValidationResult(true, null, warnings);
        public static CommandValidationResult Failure(string errorMessage, List<string> warnings = null) => new CommandValidationResult(false, errorMessage, warnings);
    }

    public class CommandExecutionResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        // public List<IGameEvent> GenerateEvents { get; }
        public Dictionary<string, object> ResultData { get; }
        public CommandExecutionResult(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        internal static UniTask<CommandExecutionResult> Failure(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}