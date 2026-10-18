using Godot;

public partial class CrtOverlay : CanvasLayer
{
    [Export] private ColorRect crtEffect;

    // Track whether distortion is allowed by player settings
    private bool isDistortionAllowed = true;
    
    // Track conditional gameplay states (e.g. low health, specific zone, insanity meter)
    private bool isConditionActive = false;

    public override void _Ready()
    {
        // Force the overlay to process during pauses if needed
        ProcessMode = ProcessModeEnum.Always;
        
        if (crtEffect == null)
        {
            crtEffect = GetNode<ColorRect>("CrtEffect");
        }

        UpdateEffectState();

        GameEvents.OnCrtConditionChanged += SetConditionActive;
        GameEvents.OnDistortionSettingToggled += SetDistortionAllowed;
    }

    /// <summary>
    /// Call this from SettingsScreen when the user toggles "Disable Distortion".
    /// </summary>
    public void SetDistortionAllowed(bool allowed)
    {
        isDistortionAllowed = allowed;
        UpdateEffectState();
    }

    /// <summary>
    /// Call this from gameplay logic when specific game conditions are met.
    /// </summary>
    public void SetConditionActive(bool active)
    {
        isConditionActive = active;
        UpdateEffectState();
    }

    /// <summary>
    /// Evaluates both player preference and gameplay state to toggle the screen shader.
    /// </summary>
    private void UpdateEffectState()
    {
        if (crtEffect != null)
        {
            crtEffect.Visible = isDistortionAllowed && isConditionActive;
        }
    }
    public override void _ExitTree()
    {
        GameEvents.OnCrtConditionChanged -= SetConditionActive;
        GameEvents.OnDistortionSettingToggled -= SetDistortionAllowed;
    }
}

