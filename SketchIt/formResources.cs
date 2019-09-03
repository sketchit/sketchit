using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SketchIt
{
    public partial class ResourcesForm : BaseForm
    {
        List<ProjectFileReference> _resources;

        public ResourcesForm()
        {
            InitializeComponent();
        }

        public static void ShowResources(List<ProjectFileReference> resources)
        {
            using (ResourcesForm f = new ResourcesForm())
            {
                f._resources = resources;
                f.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.Title = "Add Resource File Reference";
                d.Filter = "Image Files|*.jpg;*.png;*.gif;*.bmp|All Files|*.*";
                d.Multiselect = true;

                if (d.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in d.FileNames)
                    {
                        _resources.Add(new ProjectFileReference() { Name = file });
                    }

                    PopulateList();
                }
            }
        }

        private void ResourcesForm_Load(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            lvwFiles.Items.Clear();

            foreach (ProjectFileReference resource in _resources)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = resource;
                lvi.Text = resource.Name;
                lvwFiles.Items.Add(lvi);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lvwFiles.SelectedItems.Count > 0)
            {
                _resources.Remove(lvwFiles.SelectedItems[0].Tag as ProjectFileReference);
                lvwFiles.Items.Remove(lvwFiles.SelectedItems[0]);
            }
        }
    }
}
