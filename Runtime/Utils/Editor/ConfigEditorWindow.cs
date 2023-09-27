
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace RazerCore.Utils.Editor
{
    public class ConfigEditorWindow : OdinMenuEditorWindow
    {
        private static string dataAssetsPath => "Windosia/Resources";

        [MenuItem("Windosia/Config")]
        protected static void OpenWindow()
        {
            GetWindow<ConfigEditorWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree
            {
                DefaultMenuStyle =
                {
                    IconSize = 20f
                }
            };

            tree.AddAllAssetsAtPath("Config", dataAssetsPath, typeof(ScriptableObject)).AddThumbnailIcons().SortMenuItemsByName();

            tree.Config.DrawSearchToolbar = true;

            return tree;
        }
    }
}
#endif
