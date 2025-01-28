using Entitas;
using UnityEngine;

[Game]
public sealed class PositionComponent : IComponent
{
    public Vector3 value;
}

[Game]
public sealed class MovementSpeedComponent : IComponent
{
    public float value;
}

[Game]
public sealed class PlayerComponent : IComponent { }

[Game]
public sealed class BoundaryComponent : IComponent
{
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
}

[Game]
public sealed class PadComponent : IComponent
{
    public bool isActivated;
}

[Game]
public sealed class ViewComponent : IComponent
{
    public GameObject gameObject;
}

[Game]
public sealed class GameStateComponent : IComponent
{
    public bool isGameWon;
    public int activatedPadsCount;
}

[Game]
public sealed class DestroyComponent : IComponent { }