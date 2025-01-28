using Entitas;

[Game]
public sealed class GameStateComponent : IComponent
{
    public bool isGameWon;
    public int activatedPadsCount;
} 