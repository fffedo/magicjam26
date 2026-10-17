using Godot;
using System;

public partial class titleScreen : Control
{
	// Path to your main game scene configurable from the Inspector
	[Export(PropertyHint.File, "*.tscn")]
	private string mainGameScenePath = "res://MainScene.tscn";

	/// <summary>
	/// Receiver method for the StartButton's "pressed" signal.
	/// </summary>
	public void _on_start_button_pressed()
	{
		GD.PrintErr($"Starting game");
		Error result = GetTree().ChangeSceneToFile(mainGameScenePath);
		
		if (result != Error.Ok)
		{
			GD.PrintErr($"Failed to load scene at path: {mainGameScenePath}");
		}
	}

	/// <summary>
	/// Receiver method for the QuitButton's "pressed" signal.
	/// </summary>
	public void _on_quit_button_pressed()
	{
		GD.PrintErr($"Quitting game");
		GetTree().Quit();
	}
}
