using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CharacterGenerator.Editor
{
    public class CategoryTreeView : TreeView
    {
        private TreeViewItem _root;

        private IList<CategoryTreeViewItem> _categories = new List<CategoryTreeViewItem>();
        
        public CategoryTreeView(TreeViewState state, IList<CategoryTreeViewItem> categories) : base(state)
        {
            _categories = categories;
            showBorder = true;
            Reload();
        }

        public CategoryTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
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
            
            SetupParentsAndChildrenFromDepths(_root, _categories.Select(c => c as TreeViewItem).ToList());

            return _root;
        }

        public CategoryTreeViewItem GetCurrentCategory()
        {
            var selectedId = GetSelection().FirstOrDefault();

            if (selectedId < 0)
            {
                return null;
            }

            return _categories.FirstOrDefault(c => c.id == selectedId);
        }
    }
}