using System;
using System.Diagnostics;
using Command;
using Cysharp.Threading.Tasks;
using Models;
using Services;
using UnityEngine;

namespace Manager
{
    public interface ICommandManager
    {
        bool ExecuteCommand(ICommand command);
    }

    /*

    public class CommandManager : ICommandManager
    {
        private readonly IGameService gameService;
        private readonly IGameModelReader gameModelReader;

        public CommandManager(IGameService gameService, IGameModelReader gameModel)
        {
            this.gameService = gameService;
            this.gameModelReader = gameModel;
        }

        public bool ExecuteCommand(ICommand command)
        {
            try {
                var validationResult = command.Validate();
                if (!validationResult) {
                    UnityEngine.Debug.LogWarning($"[CommandManager] Command validation failed");
                    return validationResult;
                }

                var executionResult = command.Execute();
                if (!executionResult) {
                    UnityEngine.Debug.LogWarning($"[CommandManager] Command execution failed");
                    return validationResult;
                }

                return executionResult;
            }
            catch (Exception e) {
                UnityEngine.Debug.LogError($"[CommandManager] Error executing command: {e.Message}");
                return false;
            }
        }
    }
    */
}