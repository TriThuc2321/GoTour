using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class MoMoConfirmViewModel : ObservableObject
    {
        INavigation navigation;
        public MoMoConfirmViewModel() { }
        public Command NavigationBack { get; }
        public Command UploadPhoto { get; }
        public Command RemovePhoto { get; }

        public Command Confirm { get; }
        public MoMoConfirmViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            NavigationBack = new Command(() => navigation.PopAsync());
            UploadPhoto = new Command(uploadPhoto);
            Confirm = new Command(confirm);
            RemovePhoto = new Command(removePhoto);


            SetInformation();
        }

        Plugin.Media.Abstractions.MediaFile currentPhoto;
        async void uploadPhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            var imgData = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());
            currentPhoto = imgData;

            if (imgData != null)
            {
                ImageLink = ImageSource.FromStream(imgData.GetStream);
                ConfirmText = "DONE";
                UploadImageText = "Change this photo";
                ImageVisible = true;
                RemovePhotoVisible = true;
            }
        }

        void removePhoto(object obj)
        {
            currentPhoto = null;
            RemovePhotoVisible = false;
            UploadImageText = "Upload photo";
            ConfirmText = "Paying later";

            ImageLink = "";
            ImageVisible = false;
        }


        async void confirm(object obj)
        {
            if (ImageVisible && currentPhoto != null)
            {
                string url = await DataManager.Ins.InvoicesServices.saveMoMoImage(
                    currentPhoto.GetStream(),
                    DataManager.Ins.CurrentBookedTicket.invoice.id
                    );


                DataManager.Ins.CurrentInvoice.isPaid = true;
                DataManager.Ins.CurrentInvoice.payingTime = DateTime.Now.ToString();
                DataManager.Ins.CurrentInvoice.photoMomo = url;

                navigation.PushAsync(new BookedTicketDetailView());
            }
            else
            {
                DataManager.Ins.CurrentInvoice.isPaid = false;
                DataManager.Ins.CurrentInvoice.payingTime = "";
            }

            DataManager.Ins.InvoicesServices.AddInvoice(DataManager.Ins.CurrentInvoice);
            DataManager.Ins.BookedTicketsServices.AddBookedTicket(DataManager.Ins.CurrentBookedTicket);

            if (DataManager.Ins.CurrentDiscount != null)
            {
                int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                isUsed++;
                DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                DataManager.Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

            }

            if (DataManager.Ins.currentTour != null)
            {
                int remaining = int.Parse(DataManager.Ins.currentTour.remaining);
                remaining = remaining - int.Parse(DataManager.Ins.CurrentInvoice.amount);
                DataManager.Ins.currentTour.remaining = remaining.ToString();

                await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);

            }
        }

        #region money
        private string _money;
        public string Money
        {
            get { return _money; }
            set
            {
                _money = value;
                OnPropertyChanged("Money");
            }
        }
        #endregion

        #region image
        private ImageSource _imageLink;
        public ImageSource ImageLink
        {
            get { return _imageLink; }
            set
            {
                _imageLink = value;
                OnPropertyChanged("ImageLink");
            }
        }

        private bool _imageVisible;
        public bool ImageVisible
        {
            get { return _imageVisible; }
            set
            {
                _imageVisible = value;
                OnPropertyChanged("ImageVisible");
            }
        }
        #endregion

        #region confirm Text
        private string _confirmText;
        public string ConfirmText
        {
            get { return _confirmText; }
            set
            {
                _confirmText = value;
                OnPropertyChanged("ConfirmText");
            }
        }
        #endregion

        private string _uploadImageText;
        public string UploadImageText
        {
            get { return _uploadImageText; }
            set
            {
                _uploadImageText = value;
                OnPropertyChanged("UploadImageText");
            }
        }

        #region regulation 
        private string _regulation;
        public string Regulation
        {
            get { return _regulation; }
            set
            {
                _regulation = value;
                OnPropertyChanged("Regulation");
            }
        }

        #endregion

        #region RemovePhoto 
        private bool _removePhotoVisible;
        public bool RemovePhotoVisible
        {
            get { return _removePhotoVisible; }
            set
            {
                _removePhotoVisible = value;
                OnPropertyChanged("RemovePhotoVisible");
            }
        }
        #endregion

        void SetInformation()
        {
            Money = DataManager.Ins.CurrentBookedTicket.invoice.total;
            Regulation = "This is our regulation: ";
            ConfirmText = "Paying later";
            UploadImageText = "Upload photo";

            ImageLink = "";
            ImageVisible = false;
            RemovePhotoVisible = false;
        }
    }
}
