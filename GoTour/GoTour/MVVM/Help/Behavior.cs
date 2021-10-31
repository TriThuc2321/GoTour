using System;
using System.Collections.Generic;
using System.Text;
using GoTour.MVVM.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace GoTour.Help
{
    public class Behavior : Behavior<ContentPage>
    {
        #region Fields
        SfListView ListView;
        SearchBar SearchBar;
        #endregion

        #region overides
        protected override void OnAttachedTo(ContentPage bindable)
        {
            ListView = bindable.FindByName<SfListView>("listView");
            SearchBar = bindable.FindByName<SearchBar>("filterText");
            SearchBar.TextChanged += SearchBar_TextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            SearchBar.TextChanged -= SearchBar_TextChanged;
            SearchBar = null;
            ListView = null;
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region Methods

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListView.DataSource != null)
            {
                ListView.DataSource.Filter = FilterContacts;
                ListView.DataSource.RefreshFilter();
            }
            ListView.RefreshView();
        }

        private bool FilterContacts(object obj)
        {
            if (SearchBar == null || SearchBar.Text == null)
                return true;

            var taskInfo = obj as TaskInfo;
            return (taskInfo.Title.ToLower().Contains(SearchBar.Text.ToLower())
                || taskInfo.Description.ToLower().Contains(SearchBar.Text.ToLower()));
        }
        #endregion

    }
}
