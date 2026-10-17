using Godot;

public partial class MainScene : Node2D
{
	private CurrentWorld CurrentWorld { get; set; } = CurrentWorld.Real;
	
	[Export]
	private Node2D realWorld;
	[Export]
	private Node2D virtualWorld;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("switch_world"))
		{
			CurrentWorld = CurrentWorld == CurrentWorld.Real ? CurrentWorld.Virtual : CurrentWorld.Real;
			GD.Print("Current World: " + CurrentWorld);
		}
	}

	public bool IsCurrentWorld(Node2D world)
	{
		return (CurrentWorld == CurrentWorld.Real && world == realWorld) || (CurrentWorld == CurrentWorld.Virtual && world == virtualWorld);
	}
}
