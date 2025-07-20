using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Models;

namespace Command
{
    public interface ICommand
    {
        string CommandType { get; }
        DateTime Timestamp { get; }
        string CommandId { get; }

        bool Validate();
        UniTask<bool> Execute();
        //string Serialize();
        //string GetDescription();
    }

    public interface IGameFlowCommand : ICommand
    {

    }

    public interface IPlayerActionCommand : ICommand
    {
        string PlayerId { get; }
    }

    public interface IUICommand : ICommand
    {
    }

    public abstract class BaseCommand : ICommand
    {
        public abstract string CommandType { get; }
        public DateTime Timestamp { get; set; }
        public string CommandId { get; set; }

        private readonly GameModel gameModel;

        public BaseCommand(GameModel gameModel)
        {
            Timestamp = DateTime.UtcNow;
            CommandId = Guid.NewGuid().ToString();
        }

        public abstract bool Validate();
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
        // Todo: Remove playerId from command
        public string PlayerId { get; }

        public BasePlayerActionCommand(string playerId, GameModel gameModel) : base(gameModel)
        {
            PlayerId = playerId;
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