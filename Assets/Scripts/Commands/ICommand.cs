using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Models;
using Unity.VisualScripting;

namespace Command
{
    public interface ICommand
    {
        string CommandType { get; }
        DateTime Timestamp { get; }
        string CommandId { get; }

        UniTask<bool> Validate();
        UniTask<bool> Execute();
        //string Serialize();
        //string GetDescription();
    }

    public interface IGameFlowCommand : ICommand
    {

    }

    public interface IPlayerActionCommand : ICommand
    {
    }

    public interface IUICommand : ICommand
    {
    }

    public abstract class BaseCommand : ICommand
    {
        public abstract string CommandType { get; }
        public DateTime Timestamp { get; set; }
        public string CommandId { get; set; }

        protected readonly GameModel gameModel;

        public BaseCommand(GameModel gameModel)
        {
            this.gameModel = gameModel;
            Timestamp = DateTime.UtcNow;
            CommandId = Guid.NewGuid().ToString();
        }

        public abstract UniTask<bool> Validate();
        public abstract UniTask<bool> Execute();
        //public abstract string Serialize();
        //public abstract string GetDescription();
    }

    public abstract class BaseGameFlowCommand : BaseCommand, IGameFlowCommand
    {
        public BaseGameFlowCommand(GameModel gameModel) : base(gameModel)
        {

        }
    }

    public abstract class BasePlayerActionCommand : BaseCommand, IPlayerActionCommand
    {
        public BasePlayerActionCommand(GameModel gameModel) : base(gameModel)
        {
        }
    }

    public abstract class BaseUICommand : BaseCommand, IUICommand
    {
        public BaseUICommand(GameModel gameModel) : base(gameModel)
        {

        }
    }

    /*

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
    } */
}