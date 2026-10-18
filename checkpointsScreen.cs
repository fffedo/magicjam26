using Godot;
using System;

public partial class checkpointsScreen : Control
{
	// Path to your main game scene configurable from the Inspector
	[Export(PropertyHint.File, "*.tscn")]
	private string onePath = "res://mainScene.tscn";

	/// <summary>
	/// Receiver method for the StartButton's "pressed" signal.
	/// </summary>
	public void _on_1_pressed()
	{
		GD.PrintErr($"Starting game");
		Error result = GetTree().ChangeSceneToFile(onePath);
		
		if (result != Error.Ok)
		{
			GD.PrintErr($"Failed to load scene at path: {onePath}");
		}
	}
}
