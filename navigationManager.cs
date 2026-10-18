using Godot;
using System.Collections.Generic;

public static class NavigationManager
{
    private static readonly Stack<string> SceneHistory = new Stack<string>();

    /// <summary>
    /// Changes to a new scene while saving the current scene path to history.
    /// </summary>
    public static void ChangeScene(SceneTree tree, string newScenePath)
    {
        if (tree.CurrentScene != null)
        {
            SceneHistory.Push(tree.CurrentScene.SceneFilePath);
        }

        Error result = tree.ChangeSceneToFile(newScenePath);
        if (result != Error.Ok)
        {
            GD.PrintErr($"[NavigationManager] Failed to load scene at: {newScenePath}");
        }
    }

    /// <summary>
    /// Returns to the previous scene stored in history stack.
    /// </summary>
    public static void GoBack(SceneTree tree)
    {
        if (SceneHistory.Count > 0)
        {
            string previousScenePath = SceneHistory.Pop();
            Error result = tree.ChangeSceneToFile(previousScenePath);
            
            if (result != Error.Ok)
            {
                GD.PrintErr($"[NavigationManager] Failed to return to previous scene at: {previousScenePath}");
            }
        }
        else
        {
            GD.PrintErr("[NavigationManager] No previous scene found in history stack!");
        }
    }

    /// <summary>
    /// Clears scene history (useful when restarting or quitting to main menu).
    /// </summary>
    public static void ClearHistory()
    {
        SceneHistory.Clear();
    }
}