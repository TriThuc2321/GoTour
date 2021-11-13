using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class EditTourViewModel: ObservableObject
    {
        INavigation navigation;
        Shell currentShell;

        public Command OpenDetailTour { get; }
        public Command BookTourCommand { get; }
        public Command OpenDetailTour1 { get; }
        public Command NavigationBack { get; }
        public Command DescriptionBtn { get; }
        public Command ReviewBtn { get; }

        public EditTourViewModel() { }
        public EditTourViewModel(INavigation navigation, Shell currentShell)
        {
            this.navigation = navigation;
            this.currentShell = currentShell;

            this.navigation = navigation;
            SelectedTour = DataManager.Ins.currentTour;

            DurationProcess();
            NavigationBack = new Command(() => navigation.PopAsync());
            DescriptionBtn = new Command(() => {
                DescriptionInfo = "True";
                ReviewInfo = "False";
            });
            ReviewBtn = new Command(() =>
            {
                DescriptionInfo = "False";
                ReviewInfo = "True";
            });
            OpenDetailTour = new Command(OpenDetailTourHandler);
            OpenDetailTour1 = new Command(OpenDetailTourHandler1);

            SourceIcon = "editIcon.png";
        }
        public void OpenDetailTourHandler()
        {
            navigation.PushAsync(new DetailTourView2());
        }

        public void OpenBookTourView(object obj)
        {
            navigation.PushAsync(new BookTourView());
        }

        public void OpenDetailTourHandler1()
        {
            navigation.PushAsync(new TourScheduleView());
        }
        private Tour selectedTour;
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                selectedTour = value;
                OnPropertyChanged("SelectedTour");
            }
        }

        private string processedDuration;
        public string ProcessedDuration
        {
            get { return processedDuration; }
            set
            {
                processedDuration = value;
                OnPropertyChanged("ProcessedDuration");
            }
        }
        private string descriptionInfo = "True";
        public string DescriptionInfo
        {
            get { return descriptionInfo; }
            set
            {
                descriptionInfo = value;
                OnPropertyChanged("DescriptionInfo");
            }
        }
        private string reviewInfo = "False";
        public string ReviewInfo
        {
            get { return reviewInfo; }
            set
            {
                reviewInfo = value;
                OnPropertyChanged("ReviewInfo");
            }
        }
        private void DurationProcess()
        {
            if (DataManager.Ins.currentTour.duration == null) return;
            string[] _ProcessedDuration = DataManager.Ins.currentTour.duration.Split('/');
            string result = _ProcessedDuration[0] + " Ngày " + _ProcessedDuration[1] + " Đêm";
            ProcessedDuration = result;
        }
        private string sourceIcon;
        public string SourceIcon
        {
            get { return sourceIcon; }
            set
            {
                sourceIcon = value;
                OnPropertyChanged("SourceIcon");

            }
        }
    }
}
