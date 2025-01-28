using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class DestroySystem : IExecuteSystem
{
    private readonly GameContext _context;
    private readonly IGroup<GameEntity> _destroyEntities;
    private readonly List<GameEntity> _buffer;

    public DestroySystem(Contexts contexts)
    {
        _context = contexts.game;
        _destroyEntities = _context.GetGroup(GameMatcher.Destroy);
        _buffer = new List<GameEntity>();
    }

    public void Execute()
    {
        // Clear and fill buffer
        _buffer.Clear();
        foreach (var entity in _destroyEntities)
        {
            _buffer.Add(entity);
        }

        // Process buffer
        foreach (var entity in _buffer)
        {
            if (entity.hasView)
            {
                Object.Destroy(entity.view.gameObject);
            }
            entity.Destroy();
        }
    }
}