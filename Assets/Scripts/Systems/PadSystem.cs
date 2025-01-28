using Entitas;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PadSystem : ReactiveSystem<GameEntity>
{
    private readonly GameContext _context;
    private readonly TextMeshProUGUI _winText;

    public PadSystem(Contexts contexts, TextMeshProUGUI winText) : base(contexts.game)
    {
        _context = contexts.game;
        _winText = winText;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Trigger);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPad && entity.isTrigger;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var gameStateEntity = _context.GetGroup(GameMatcher.GameState).GetSingleEntity();
        if (gameStateEntity == null) return;

        foreach (var entity in entities)
        {
            if (!entity.pad.isActivated)
            {
                entity.pad.isActivated = true;
                entity.view.gameObject.GetComponent<Renderer>().material.color = Color.green;

                gameStateEntity.ReplaceGameState(
                    gameStateEntity.gameState.isGameWon,
                    gameStateEntity.gameState.activatedPadsCount + 1
                );

                if (gameStateEntity.gameState.activatedPadsCount >= 4)
                {
                    gameStateEntity.ReplaceGameState(true, gameStateEntity.gameState.activatedPadsCount);

                    if (_winText != null)
                    {
                        _winText.gameObject.SetActive(true);
                    }

                    var player = _context.GetGroup(GameMatcher.Player).GetSingleEntity();
                    if (player != null)
                    {
                        player.isDestroy = true;
                    }
                }
            }

            entity.isTrigger = false;
        }
    }
}