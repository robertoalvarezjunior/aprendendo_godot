using Godot;

public partial class Camera : Camera2D
{

    Node2D target;

    public override void _Ready()
    {
        var players = GetTree().GetNodesInGroup("Player");
        if (players.Count > 0)
        {
            target = (Node2D)players[0];
        }
        else
        {
            GD.PrintErr("No player found in the scene.");
        }
    }

    public override void _Process(double delta)
    {
        if (target != null)
        {
            Position = target.Position;
        }
    }
}