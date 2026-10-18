using Godot;
using System;

public partial class SettingsScreen : Control
{
	[Export] private CheckBox distortionCheckBox;
	[Export] private HSlider masterSlider;
	[Export] private HSlider sfxSlider;
	[Export] private HSlider musicSlider;

	// Bus Indices
	private int masterBusIndex;
	private int sfxBusIndex;
	private int musicBusIndex;

	public override void _Ready()
	{
		// Get bus indices by exact name match
		masterBusIndex = AudioServer.GetBusIndex("Master");
		sfxBusIndex = AudioServer.GetBusIndex("SFX");
		musicBusIndex = AudioServer.GetBusIndex("Music");

		// Initialize slider values based on current audio bus volumes
		if (masterSlider != null)
			masterSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(masterBusIndex));
		if (sfxSlider != null)
			sfxSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(sfxBusIndex));
		if (musicSlider != null)
			musicSlider.Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(musicBusIndex));
	}

	/// <summary>
	/// Receiver method for Distortion CheckBox "toggled(bool toggled_on)" signal.
	/// </summary>
	public void _on_distortion_check_box_toggled(bool toggledOn)
	{
		// Distortion effect logic to be implemented here later
	}

	/// <summary>
	/// Receiver method for Master HSlider "value_changed(float value)" signal.
	/// </summary>
	public void _on_master_slider_value_changed(float value)
	{
		SetBusVolume(masterBusIndex, value);
	}

	/// <summary>
	/// Receiver method for SFX HSlider "value_changed(float value)" signal.
	/// </summary>
	public void _on_sfx_slider_value_changed(float value)
	{
		SetBusVolume(sfxBusIndex, value);
	}

	/// <summary>
	/// Receiver method for Music HSlider "value_changed(float value)" signal.
	/// </summary>
	public void _on_music_slider_value_changed(float value)
	{
		SetBusVolume(musicBusIndex, value);
		
	}

	private void SetBusVolume(int busIndex, float linearValue)
	{
		// Convert 0.0001 - 1.0 linear slider value to decibels
		float dbValue = Mathf.LinearToDb(linearValue);
		AudioServer.SetBusVolumeDb(busIndex, dbValue);

		string busName = AudioServer.GetBusName(busIndex);
		GD.Print($"[Audio Test] Bus: '{busName}' | Slider: {linearValue:F2} | Decibels: {dbValue:F2} dB | Muted: {linearValue <= 0.0001f}");

		// Mute bus completely if slider is moved to absolute minimum
		AudioServer.SetBusMute(busIndex, linearValue <= 0.0001f);
	}
	public void _on_back_button_pressed()
{
	GetTree().ChangeSceneToFile("res://title_screen.tscn");
}
}
