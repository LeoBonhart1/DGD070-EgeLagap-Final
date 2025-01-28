using Entitas;
using UnityEngine;
using TMPro;

public class PadSystem : IExecuteSystem
{
    private readonly GameContext _context;
    private readonly IGroup<GameEntity> _players;
    private readonly IGroup<GameEntity> _pads;
    private readonly IGroup<GameEntity> _gameStates;
    private readonly TextMeshProUGUI _winText;

    public PadSystem(Contexts contexts, TextMeshProUGUI winText)
    {
        _context = contexts.game;
        _players = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position));
        _pads = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Pad, GameMatcher.Position));
        _gameStates = _context.GetGroup(GameMatcher.GameState);
        _winText = winText;
    }

    public void Execute()
    {
        var player = _players.GetSingleEntity();
        if (player == null) return;

        var gameStateEntity = _gameStates.GetSingleEntity();
        if (gameStateEntity == null) return;

        // Don't process pad activation if game is already won
        if (gameStateEntity.gameState.isGameWon) return;

        foreach (var pad in _pads)
        {
            if (!pad.pad.isActivated && Vector3.Distance(player.position.value, pad.position.value) < 1f)
            {
                pad.pad.isActivated = true;
                pad.view.gameObject.GetComponent<Renderer>().material.color = Color.green;

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

                    player.isDestroy = true;
                }
            }
        }
    }
}