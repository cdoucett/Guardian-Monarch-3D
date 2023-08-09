using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


//Used to fix 2022.3.4f verison's canvas not saving correctly when closing project
//Please delete this file if any troubles comes compiling
//
//Got from: https://forum.unity.com/threads/canvas-broken-after-loading-project-on-2022-3-3f1.1453069/
//by: c0nd3v
/*public static class CanvasFixer
{
    [InitializeOnLoadMethod]
    public static void InitializeOnLoad()
    {
        EditorApplication.update += Update1;
    }

    public static void Update1()
    {
        // 1. Open game view
        var gameView = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));

        EditorApplication.update -= Update1;
        EditorApplication.update += Update2;
    }

    public static void Update2()
    {
        // 2. Open scene view
        var sceneView = EditorWindow.GetWindow(typeof(SceneView));

        // 3. Reload scene
        var scene = SceneManager.GetActiveScene();
        EditorSceneManager.OpenScene(scene.path);

        EditorApplication.update -= Update2;
    }
}*/


