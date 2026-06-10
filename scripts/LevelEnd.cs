using System;
using Godot;

public partial class LevelEnd : Area2D
{

    [Export]
    String nextLevelScene = "";

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            CallDeferred("loadNextLevel");
        }
    }

    private void loadNextLevel()
    {
        GetTree().ChangeSceneToFile($"res://scene/{nextLevelScene}.tscn");
    }
}