using Godot;
using System;

public partial class pauseMenu : Control
{
	[Export(PropertyHint.File, "*.tscn")]
	private string titleScreenScenePath = "res://title_screen.tscn";

	public override void _Ready()
	{
		// Ensure menu starts hidden and unpaused
		Visible = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Listen for pause input action (e.g. Esc or Start button on controller)
		if (@event.IsActionPressed("pause"))
		{
			if (GetTree().Paused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}

			// Mark input as handled so it doesn't trigger other actions in the game
			GetViewport().SetInputAsHandled();
		}
	}

	public void PauseGame()
	{
		Visible = true;
		GetTree().Paused = true;
	}

	public void ResumeGame()
	{
		Visible = false;
		GetTree().Paused = false;
	}

	// --- Signal Receiver Methods ---

	/// <summary>
	/// Receiver for ResumeButton "pressed" signal
	/// </summary>
	public void _on_continue_button_pressed()
	{
		ResumeGame();
	}

	/// <summary>
	/// Receiver for QuitButton "pressed" signal
	/// </summary>
	public void _on_quit_button_pressed()
	{
		// Unpause before changing scene so the target scene isn't stuck paused!
		GetTree().Paused = false;
		
		Error result = GetTree().ChangeSceneToFile(titleScreenScenePath);
		if (result != Error.Ok)
		{
			GD.PrintErr($"Failed to load title screen at: {titleScreenScenePath}");
		}
	}
	public void _on_settings_button_pressed()
	{
		GD.PrintErr($"opening settings");
		NavigationManager.ChangeScene(GetTree(), "res://settings_screen.tscn");
	}
}
