using System;

public static class GameEvents
{
    // Fired when game conditions change (e.g. entered retro zone, taken damage)
    public static event Action<bool> OnCrtConditionChanged;

    // Fired when settings checkbox is toggled
    public static event Action<bool> OnDistortionSettingToggled;

    public static void TriggerCrtCondition(bool active)
    {
        OnCrtConditionChanged?.Invoke(active);
    }

    public static void ToggleDistortionSetting(bool allowed)
    {
        OnDistortionSettingToggled?.Invoke(allowed);
    }
}