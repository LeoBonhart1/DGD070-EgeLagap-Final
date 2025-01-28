using Entitas;
using UnityEngine;

public class PadTriggerSystem : IExecuteSystem
{
    private readonly GameContext _context;
    private readonly IGroup<GameEntity> _players;
    private readonly IGroup<GameEntity> _pads;

    public PadTriggerSystem(Contexts contexts)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position));
        _pads = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Pad, GameMatcher.Position));
    }

    public void Execute()
    {
        var player = _players.GetSingleEntity();
        if (player == null) return;

        foreach (var pad in _pads)
        {
            if (!pad.pad.isActivated && Vector3.Distance(player.position.value, pad.position.value) < 1f)
            {
                pad.isTrigger = true;
            }
        }
    }
}