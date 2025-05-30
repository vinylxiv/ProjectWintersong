using UnityEditor;
using UnityEngine;

namespace Thry.ThryEditor.Decorators
{
    public class ThrySeperatorDecorator : MaterialPropertyDrawer
    {
        Color _color = Colors.foreground;

        public ThrySeperatorDecorator() { }
        public ThrySeperatorDecorator(string c)
        {
            ColorUtility.TryParseHtmlString(c, out _color);
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            ShaderProperty.RegisterDecorator(this);
            return 1;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            position = EditorGUI.IndentedRect(position);
            EditorGUI.DrawRect(position, _color);
        }
    }

}