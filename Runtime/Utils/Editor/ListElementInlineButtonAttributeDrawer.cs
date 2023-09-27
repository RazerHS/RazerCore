
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using RazerCore.Utils.Attributes;

#if UNITY_EDITOR
namespace RazerCore.Utils.Editor
{
    public class ListElementInlineButtonAttributeDrawer : OdinAttributeDrawer<ListElementInlineButtonAttribute>
    {
		private ValueResolver<string> labelGetter;
		private ActionResolver clickAction;
		private ValueResolver<bool> showIfGetter;

		private bool show = true;
		private string tooltip;

		protected override bool CanDrawAttributeProperty(InspectorProperty property)
		{
			bool parentIsCollection = property.ParentValueProperty.ChildResolver is ICollectionResolver;
			return parentIsCollection;
		}

		protected override void Initialize()
		{
			InspectorProperty parentProperty = this.Property.ParentValueProperty;
			var collectionResolver = (ICollectionResolver)this.Property.ParentValueProperty.ChildResolver;

			Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue[] actionNamedValues = new Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue[2]
			{
				new Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue("index", typeof(int), this.Property.Index),
				new Sirenix.OdinInspector.Editor.ActionResolvers.NamedValue("element", collectionResolver.ElementType)
			};

			Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue[] valueNamedValues = new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue[2]
			{
				new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("index", typeof(int), this.Property.Index),
				new Sirenix.OdinInspector.Editor.ValueResolvers.NamedValue("element", collectionResolver.ElementType)
			};

			if (this.Attribute.Label != null)
			{
				this.labelGetter = ValueResolver.GetForString(parentProperty, this.Attribute.Label, valueNamedValues);
			}
			else
			{
				this.labelGetter = ValueResolver.Get<string>(parentProperty, null, this.Attribute.Action.SplitPascalCase(), valueNamedValues);
			}

			this.clickAction = ActionResolver.Get(parentProperty, this.Attribute.Action, actionNamedValues);

			this.showIfGetter = ValueResolver.Get(parentProperty, this.Attribute.ShowIf, true, valueNamedValues);
			this.showIfGetter.Context.NamedValues.Set("element", this.Property.ValueEntry.WeakSmartValue);
			this.show = this.showIfGetter.GetValue();

			this.tooltip = this.Property.GetAttribute<PropertyTooltipAttribute>()?.Tooltip
				?? this.Property.GetAttribute<TooltipAttribute>()?.tooltip;
		}

		protected override void DrawPropertyLayout(GUIContent label)
		{
			if (this.labelGetter.HasError || this.clickAction.HasError)
			{
				this.labelGetter.DrawError();
				this.clickAction.DrawError();
				this.CallNextDrawer(label);
			}
			else
			{
				if (Event.current.type == EventType.Layout)
				{
					this.showIfGetter.Context.NamedValues.Set("element", this.Property.ValueEntry.WeakSmartValue);
					this.show = this.showIfGetter.GetValue();
				}

				if (this.show)
				{
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.BeginVertical();
					this.CallNextDrawer(label);
					EditorGUILayout.EndVertical();

					this.labelGetter.Context.NamedValues.Set("element", this.Property.ValueEntry.WeakSmartValue);
					var btnLabel = new GUIContent(this.labelGetter.GetValue(), this.tooltip);
					SirenixEditorGUI.CalculateMinimumSDFIconButtonWidth(btnLabel.text, null, this.Attribute.Icon != SdfIconType.None, EditorGUIUtility.singleLineHeight, out _, out _, out _, out var btnWidth);

					var btnRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, GUILayout.MaxWidth(btnWidth));
					if (SirenixEditorGUI.SDFIconButton(btnRect, btnLabel, this.Attribute.Icon, this.Attribute.IconAlignment))
					{
						this.InvokeButton(btnLabel);
					}

					EditorGUILayout.EndHorizontal();
				}
				else
				{
					this.CallNextDrawer(label);
				}
			}
		}

		private void InvokeButton(GUIContent buttonLabel)
		{
			this.Property.RecordForUndo("Click " + buttonLabel);
			this.clickAction.Context.NamedValues.Set("element", this.Property.ValueEntry.WeakSmartValue);
			this.clickAction.DoActionForAllSelectionIndices();
		}
    }
}
#endif
#endif
