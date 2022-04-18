using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleBuggyWindow : EditorWindow
{
    [MenuItem("bla/SimpleBuggyWindow")]
    public static void ShowExample()
    {
        SimpleBuggyWindow wnd = GetWindow<SimpleBuggyWindow>();
        wnd.titleContent = new GUIContent("SimpleBuggyWindow");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Button.uxml");
        var treeElement = tree.CloneTree();

        // Do not use .Query() for a single element.
        //Button button = treeElement.Query<Button>();
        // Use .Q():
        var button = treeElement.Q<Button>();

        button.clickable.clicked += () => Debug.Log("Clicked");

        root.Add(button);
    }
}