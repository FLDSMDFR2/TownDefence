using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;
using Unity.UIElements.Runtime;

public class ShopManagerUIBuilder : MonoBehaviour
{
    //private VisualElement _visualTree;
    //private VisualTreeAsset _assetTree;

    public PanelRenderer GameScreen;

    public void OnEnable()
    {
        GameScreen.postUxmlReload = BindGameScreen;
    }

    private IEnumerable<Object> BindGameScreen()
    {
        var root = GameScreen.visualTree;
        Button b = root.Q<Button>("TestButton");
        b.clickable.clicked += ButtonClicked;


       //_assetTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Button.uxml");

        //_visualTree = _assetTree.CloneTree();

        //Button b = _visualTree.Q<Button>("Button");

        //b.clickable.clicked += ButtonClicked;

        return null;
    }

    private void ButtonClicked()
    {
        TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "Button Clicked");
    }
}
