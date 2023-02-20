// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using TripolisInc.EcsCore.GameComponent;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
#if UNITY_2021_3_OR_NEWER
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

namespace TripolisInc.EcsCore.Editor
{
    [CustomEditor(typeof(EcsEntityComponent))]
    public class EcsEntityComponentEditor : UnityEditor.Editor
    {
        private static readonly string CreateButtonText = "Add Ecs Component";
        private static readonly string DeleteButtonText = "Del";
        
        private EcsComponentsSearchProvider _searchProvider;
        private HashSet<Type> _excludedComponets;
        private Dictionary<Type, string> _ecsComponentsNames;
        private SerializedProperty _ecsComponentsProperty;
        private EcsEntityComponent _targetComponent;

        private void Awake()
        {
            _searchProvider = CreateInstance<EcsComponentsSearchProvider>();   
        }

        private void OnEnable()
        {
            _targetComponent = (EcsEntityComponent) target;
            
            _excludedComponets = new HashSet<Type>();
            _ecsComponentsNames = EcsEditorUtils.GetEcsEditorComponents()
                .ToDictionary(x => x.type, x => x.attribute.displayedName);
            _ecsComponentsProperty = serializedObject.FindProperty(EcsEntityComponent.ECS_COMPONENTS_PROP_NAME);
            var componentsSize = _ecsComponentsProperty.arraySize;
            for (var i = 0; i < componentsSize; i++)
            {
                var componentContainer = _targetComponent.GetEcsComponent(i);
                if (componentContainer.component == default)
                    continue;
                
                _excludedComponets.Add(componentContainer.component.GetType());
            }
            
            _searchProvider.onSelectEntryCallback += OnAddNewComponent;
        }

#if UNITY_2021_3_OR_NEWER
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            container.style.paddingLeft = 15;
            container.Bind(serializedObject);
            
            var button = new Button(() =>
            {
                _searchProvider.SetExcludes(_excludedComponets);
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    _searchProvider);
            });
            button.text = CreateButtonText;

            var listContainer = new VisualElement();
            SetupComponentList(listContainer);

            var property = serializedObject.GetIterator();
            var isFirst = true;
            while (property.NextVisible(isFirst))
            {
                if (SerializedProperty.EqualContents(property, _ecsComponentsProperty))
                    continue;

                var field = new PropertyField(property);
                field.BindProperty(property);
                field.SetEnabled(!isFirst);
                
                container.Add(field);
                isFirst = false;
            }

            var componentsProperty = _ecsComponentsProperty.Copy();
            componentsProperty.NextVisible(true);
            container.TrackPropertyValue(componentsProperty, prop => SetupComponentList(listContainer));
            container.Add(listContainer);
            container.Add(button);
            
            return container;
        }

        private void SetupComponentList(VisualElement container)
        {
            container.Clear();
            var property = _ecsComponentsProperty.Copy();
            var endProperty = property.GetEndProperty();
            property.NextVisible(true);
            
            var childIndex = 0;
            do
            {
                if (SerializedProperty.EqualContents(property, endProperty))
                    break;

                if (property.propertyType == SerializedPropertyType.ArraySize)
                    continue;
                
                if (childIndex >= container.childCount)
                {
                    var element = new VisualElement();
                    var componentProperty = property.FindPropertyRelative(EcsEntityComponent.ComponentContainer.COMPONENT_FIELD_NAME);
                    SetupComponent(element, componentProperty);
                    container.Add(element);
                }

                ++childIndex;
            } while (property.NextVisible(false));
        }

        private void SetupComponent(VisualElement container, SerializedProperty componentProperty)
        {
            var value = componentProperty.managedReferenceValue;
            if (value == null)
                return;

            var compType = value.GetType();
            if (!_ecsComponentsNames.TryGetValue(compType, out var compName))
                compName = compType.Name;

            var foldout = new Foldout();
            foldout.value = componentProperty.isExpanded;
            foldout.text = compName;
            EcsEditorStyleUtils.SetupComponentStyle(foldout, compType);
            container.Add(foldout);
            container.Add(CreateDeleteButton(compType));

            var copyProperty = componentProperty.Copy();
            var endProperty = componentProperty.GetEndProperty();
            foldout.RegisterValueChangedCallback(value => copyProperty.isExpanded = value.newValue);

            var firstNext = true;
            while (componentProperty.NextVisible(firstNext) &&
                   componentProperty.propertyPath != endProperty.propertyPath)
            {
                var property = new PropertyField(componentProperty);
                property.BindProperty(componentProperty);
                foldout.Add(property);
                firstNext = false;
            }
        }

        private Button CreateDeleteButton(Type componentType)
        {
            var delButton = new Button(() => OnDeleteComponent(componentType));
            delButton.style.position = Position.Absolute;
            delButton.style.right = 0;
            delButton.style.top = 1f;
            delButton.text = DeleteButtonText;

            return delButton;
        }
#else
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            DrawComponentsList();
            
            if (GUILayout.Button(CreateButtonText))
            {
                _searchProvider.SetExcludes(_excludedComponets);
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    _searchProvider);
            }
        
            serializedObject.ApplyModifiedProperties();
        }
        
        
        private void DrawComponentsList()
        {
            EditorGUILayout.BeginVertical();
            var componentsCount = _ecsComponentsProperty.arraySize;
            
            for (var i = 0; i < componentsCount; i++)
            {
                var componentContainerProperty = _ecsComponentsProperty.GetArrayElementAtIndex(i);
                var componentProperty =
                    componentContainerProperty.FindPropertyRelative(EcsEntityComponent.ComponentContainer
                        .COMPONENT_FIELD_NAME);
                DrawComponent(componentProperty);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawComponent(SerializedProperty componentProperty)
        {
            var value = componentProperty.managedReferenceValue;
            if (value == null)
                return;

            var compType = value.GetType();
            if (!_ecsComponentsNames.TryGetValue(compType, out var compName))
                compName = compType.Name;
            
            EditorGUI.indentLevel++;
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var isExpanded = componentProperty.isExpanded = EditorGUILayout.Foldout(componentProperty.isExpanded, new GUIContent(compName));
            var lastRect = GUILayoutUtility.GetLastRect();
            lastRect.position = new Vector2(lastRect.width - 10f, lastRect.position.y);
            lastRect.width = 30f;
            if (GUI.Button(lastRect, DeleteButtonText))
            {
                OnDeleteComponent(compType);
                Repaint();
                return;
            }
            
            if (isExpanded)
            {
                EditorGUI.indentLevel++;
                
                var endProperty = componentProperty.GetEndProperty();
                while (componentProperty.NextVisible(true) &&
                       componentProperty.propertyPath != endProperty.propertyPath)
                    EditorGUILayout.PropertyField(componentProperty, true);
                
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
#endif

        private void OnAddNewComponent(EcsEditorUtils.EcsComponentEditorInfo componentInfo)
        {
            using (EditorUtils.ChangeSerializeObject(serializedObject))
            {
                _excludedComponets.Add(componentInfo.type);
                _targetComponent.AddEcsComponent(componentInfo.type);
            }
        }

        private void OnDeleteComponent(Type componentType)
        {
            using (EditorUtils.ChangeSerializeObject(serializedObject))
            {
                _targetComponent.DeleteEcsComponent(componentType);
                _excludedComponets.Remove(componentType);
            }
        }
    }
}