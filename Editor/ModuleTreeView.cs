using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CharacterGenerator.Editor
{
    public class ModuleTreeView : TreeView
    {
        private TreeViewItem _root;

        private IList<ModuleTreeViewItem> _modules = new List<ModuleTreeViewItem>();
        
        public ModuleTreeView(TreeViewState state, IList<ModuleTreeViewItem> modules) : base(state)
        {
            _modules = modules;
            showBorder = true;
            Reload();
        }

        public ModuleTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            Reload();
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override TreeViewItem BuildRoot()
        {
            _root = new TreeViewItem(0, -1, "root")
            {
                children = new List<TreeViewItem>()
            };
            
            SetupParentsAndChildrenFromDepths(_root, _modules.Select(c => c as TreeViewItem).ToList());

            return _root;
        }

        public ModuleTreeViewItem GetSelectedModule()
        {
            var selectedId = GetSelection().FirstOrDefault();

            if (selectedId < 0)
            {
                return null;
            }

            return _modules.FirstOrDefault(c => c.id == selectedId);
        }
    }
}