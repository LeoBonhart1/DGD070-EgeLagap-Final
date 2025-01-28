using UnityEngine;
using Entitas;
using TMPro;

public class GameController : MonoBehaviour
{
    private Systems _systems;
    private Contexts _contexts;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject[] pads;  

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _systems = CreateSystems(_contexts);

        InitializeGame();

        _systems.Initialize();
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    private Systems CreateSystems(Contexts contexts)
    {
        return new Feature("Game")
            .Add(new MovementSystem(contexts))
            .Add(new PadTriggerSystem(contexts))
            .Add(new PadSystem(contexts, winText))
            .Add(new ViewSystem(contexts))
            .Add(new DestroySystem(contexts));
    }

    private void InitializeGame()
    {
        var playerEntity = _contexts.game.CreateEntity();
        playerEntity.isPlayer = true;
        playerEntity.AddPosition(player.transform.position);
        playerEntity.AddMovementSpeed(5f);
        playerEntity.AddView(player);
        playerEntity.AddBoundary(-8f, 8f, -4f, 4f);

        foreach (var padObject in pads)
        {
            var padEntity = _contexts.game.CreateEntity();
            padEntity.AddPad(false);
            padEntity.AddPosition(padObject.transform.position);
            padEntity.AddView(padObject);
        }

        var gameState = _contexts.game.CreateEntity();
        gameState.AddGameState(false, 0);
    }
}