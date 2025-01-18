
using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandProcessor : ICommandProcessor
{
    private readonly Dictionary<Type, object> handlersMap = new();
    private readonly IGameStateProvider _gameStateProvider;

    public CommandProcessor(IGameStateProvider gameStateProvider)
    {
        this._gameStateProvider = gameStateProvider;
    }
    public bool Process<TCommand>(TCommand command) where TCommand : ICommand
    {
        if (handlersMap.TryGetValue(typeof(TCommand), out var handler))
        {
            var typedHandler = (ICommandHandler<TCommand>)handler;

            var result = typedHandler.Handle(command);

            // Будет сохранять состояние игры при каждом успешном выполнении команд
            // Возможно, но не обязательно, может ухудшать производительность
            if (result)
            {
                _gameStateProvider.SaveGameState();
            }

            return result;
        }

        Debug.LogError($"Handler for {typeof(TCommand)} not found");
        return false;
    }

    public void RegisterHandler<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand
    {
        handlersMap[typeof(TCommand)] = commandHandler;
    }
}