using System;
using Godot;

public partial class LevelEnd : Area2D
{

	[Export]
	String nextLevelScene = "";

	[Signal]
	public delegate void LevelCompletedEventHandler();

	public override void _Ready()
	{
		LevelCompleted += () => GD.Print("Level Completed!");
		BodyEntered += OnBodyEntered2;
	}

	// private void OnBodyEntered(Node body)
	// {
	// 	if (body.IsInGroup("Player"))
	// 	{
	// 		EmitSignal(SignalName.LevelCompleted);
	// 		CallDeferred("LoadNextLevel");
	// 	}
	// }
	private void OnBodyEntered2(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			EmitSignal(SignalName.LevelCompleted);
			CallDeferred("LoadNextLevel");
		}
	}

	private void LoadNextLevel()
	{
		GetTree().ChangeSceneToFile($"res://scene/{nextLevelScene}.tscn");
	}
}
