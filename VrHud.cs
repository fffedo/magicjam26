using Godot;

public partial class pauseButton : Button
{
	public void _on_pause_button_pressed()
	{
		TriggerInputAction("pause");
	}

	private void TriggerInputAction(string actionName)
	{
		var actionPress = new InputEventAction
		{
			Action = actionName,
			Pressed = true
		};

		var actionRelease = new InputEventAction
		{
			Action = actionName,
			Pressed = false
		};

		Input.ParseInputEvent(actionPress);
		Input.ParseInputEvent(actionRelease);
	}
}
