// ----------------------------------------------------------------------------
// MIT License
// Copyright (c) 2023  Vladimir Karyagin <tripolis777@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TripolisInc.EcsCore.Editor
{
    internal class EcsComponentsSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private HashSet<Type> _excludedTypes;
        
        public event Action<EcsEditorUtils.EcsComponentEditorInfo> onSelectEntryCallback;
        
        public void SetExcludes(HashSet<Type> excludedTypes) => _excludedTypes = excludedTypes;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchTree = new List<SearchTreeEntry>();
            searchTree.Add(new SearchTreeGroupEntry(new GUIContent("Ecs Components")));
            PopulateSearchTree(searchTree);
                
            return searchTree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            onSelectEntryCallback?.Invoke((EcsEditorUtils.EcsComponentEditorInfo) searchTreeEntry.userData);
            return true;
        }

        private void PopulateSearchTree(List<SearchTreeEntry> searchTree)
        {
            var componentsInfo = EcsEditorUtils.GetEcsEditorComponents().Where(x => !_excludedTypes.Contains(x.type))
                .ToList();
            componentsInfo.Sort(ComponentsInfoComparer);

            var groups = new HashSet<string>();
            foreach (var componentInfo in componentsInfo)
            {
                var path = componentInfo.attribute.path.Split('/');
                var groupName = "";
                for (var i = 0; i < path.Length - 1; i++)
                {
                    groupName += path[i];
                    if (!groups.Contains(groupName))
                    {
                        searchTree.Add(new SearchTreeGroupEntry(new GUIContent(path[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                var entry = new SearchTreeEntry(new GUIContent(path[^1]));
                entry.level = path.Length;
                entry.userData = componentInfo;
                searchTree.Add(entry);
            }
        }

        private int ComponentsInfoComparer(EcsEditorUtils.EcsComponentEditorInfo a,
            EcsEditorUtils.EcsComponentEditorInfo b)
        {
            var splitA = a.attribute.path.Split('/');
            var splitB = b.attribute.path.Split('/');
            for (var i = 0; i < splitA.Length; i++)
            {
                if (i >= splitB.Length)
                    return 1;

                var value = splitA[i].CompareTo(splitB[i]);
                if (value != 0)
                {
                    if (splitA.Length != splitB.Length && (i == splitA.Length - 1 || i == splitB.Length - 1))
                        return splitA.Length < splitB.Length ? 1 : -1;
                    return value;
                }
            }

            return 0;
        }
    }
}