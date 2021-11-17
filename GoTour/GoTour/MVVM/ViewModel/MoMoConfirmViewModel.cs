using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class MoMoConfirmViewModel : ObservableObject
    {
        INavigation navigation;
        string Money;
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
            ConfirmText = "Paying later by cash";

            ImageLink = "";
            ImageVisible = false;
        }


        async void confirm(object obj)
        {
            // Nếu cash trước và trả MoMo
           // bool changeMethod = false;
            bool payLater = true;
            //if (DataManager.Ins.CurrentInvoice.method == "Cash")
            //    changeMethod = true;

            

         //   if (changeMethod == false)
           // {
                if (await checkRemaining() == false)
                {
                    return;
                }

                if (await checkDiscount() == false) return;
            //}
            
            if (ImageVisible && currentPhoto != null)
            {
                string url = await DataManager.Ins.InvoicesServices.saveMoMoImage(
                    currentPhoto.GetStream(),
                    DataManager.Ins.CurrentBookedTicket.invoice.id
                    );

                DataManager.Ins.CurrentInvoice.isPaid = true;
                DataManager.Ins.CurrentInvoice.payingTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                DataManager.Ins.CurrentInvoice.photoMomo = url;

                payLater = false;
            }
            else
            {
                DataManager.Ins.CurrentInvoice.isPaid = false;
                DataManager.Ins.CurrentInvoice.payingTime = "";
                payLater = true;
            }

            if (!payLater)
            {
                DataManager.Ins.CurrentInvoice.method = "MoMo";
                DataManager.Ins.CurrentInvoice.momoVnd = Money;
            }else {
                DataManager.Ins.CurrentInvoice.method = "Cash";

            }

          //  if (changeMethod == false)
            //{
                DataManager.Ins.CurrentBookedTicket.bookTime = DateTime.Now.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            //}

            //if (!changeMethod)
            //{
                await DataManager.Ins.InvoicesServices.AddInvoice(DataManager.Ins.CurrentInvoice);
                await DataManager.Ins.BookedTicketsServices.AddBookedTicket(DataManager.Ins.CurrentBookedTicket);
            //}
          //  else {
           //     await DataManager.Ins.InvoicesServices.UpdateInvoice(DataManager.Ins.CurrentInvoice);
             //   await DataManager.Ins.BookedTicketsServices.UpdateBookedTicket(DataManager.Ins.CurrentBookedTicket);
            //}

            if (DataManager.Ins.CurrentDiscount != null)
            {
                int isUsed = int.Parse(DataManager.Ins.CurrentDiscount.isUsed);
                isUsed++;
                DataManager.Ins.CurrentDiscount.isUsed = isUsed.ToString();

                await DataManager .Ins.DiscountsServices.UpdateDiscount(DataManager.Ins.CurrentDiscount);

            }

            if (DataManager.Ins.currentTour != null 
                //&& !changeMethod
                )
            {
                int remaining = int.Parse(DataManager.Ins.currentTour.remaining);
                remaining = remaining - int.Parse(DataManager.Ins.CurrentInvoice.amount);
                DataManager.Ins.currentTour.remaining = remaining.ToString();

                await DataManager.Ins.TourServices.UpdateTour(DataManager.Ins.currentTour);

            }

            //   updateManager(changeMethod);
            updateManager();
            
            await navigation.PushAsync(new BookedTicketDetailView());
        }

        #region money
        private string _strMoney;
        public string StrMoney
        {
            get { return _strMoney; }
            set
            {
                _strMoney = value;
                OnPropertyChanged("StrMoney");
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
            string[] currency = DataManager.Ins.USDCurrency.Split(',');
            string usd = currency[0] + currency[1];
            int money = int.Parse(DataManager.Ins.CurrentInvoice.total) * int.Parse(usd);
            Money = money.ToString();
            Regulation = "This is our regulation: ";
            ConfirmText = "Paying later by cash";
            UploadImageText = "Upload photo";

            ImageLink = "";
            ImageVisible = false;
            RemovePhotoVisible = false;

            money = int.Parse(DataManager.Ins.InvoicesServices.RoundMoney(money));
            StrMoney = String.Format("{0:#,##0.##}", money);
            SelectedTour = DataManager.Ins.currentTour;
        }

        async Task<bool> checkRemaining()
        {
            Tour temp = await DataManager.Ins.TourServices.FindTourById(DataManager.Ins.currentTour.id);
            if (int.Parse(DataManager.Ins.CurrentInvoice.amount) <= int.Parse(temp.remaining))
                return true;
            else
                DependencyService.Get<IToast>().ShortToast("This tour has no turn!");

            return false;
        }

        async Task<bool> checkDiscount()
        {
            if (DataManager.Ins.CurrentDiscount == null) return true;
            if (DataManager.Ins.CurrentDiscount != null)
            {
                Discount temp = await DataManager.Ins.DiscountsServices.FindDiscountById(DataManager.Ins.CurrentDiscount.id);
                if (int.Parse(temp.isUsed) < int.Parse(temp.total))
                    return true;
                else
                    DependencyService.Get<IToast>().ShortToast("Your discount has no turn!");
            };

            return false;
        }

        void updateManager()
        {
            
                DataManager.Ins.ListBookedTickets.Add(DataManager.Ins.CurrentBookedTicket);
                DataManager.Ins.ListInvoice.Add(DataManager.Ins.CurrentInvoice);

                DataManager.Ins.CurrentBookedTicket.invoice = DataManager.Ins.CurrentInvoice;

                if (DataManager.Ins.CurrentDiscount != null)
                {
                    DataManager.Ins.CurrentBookedTicket.invoice.discount = DataManager.Ins.CurrentDiscount;

                    for (int i = 0; i < DataManager.Ins.ListDiscount.Count - 1; i++)
                    {
                        if (DataManager.Ins.ListDiscount[i].id == DataManager.Ins.CurrentDiscount.id)
                        {
                            DataManager.Ins.ListDiscount[i] = DataManager.Ins.CurrentDiscount;
                            break;
                        }
                    }
                }
            //}
            //else
            //{
            //    for (int i = 0; i < DataManager.Ins.ListBookedTickets.Count - 1; i++)
            //    {
            //        if (DataManager.Ins.ListBookedTickets[i].id == DataManager.Ins.CurrentBookedTicket.id)
            //        {
            //            DataManager.Ins.ListBookedTickets[i] = DataManager.Ins.CurrentBookedTicket;
            //            break;
            //        }
            //    }

            //    for (int i = 0; i < DataManager.Ins.ListInvoice.Count - 1; i++)
            //    {
            //        if (DataManager.Ins.ListInvoice[i].id == DataManager.Ins.CurrentInvoice.id)
            //        {
            //            DataManager.Ins.ListInvoice[i] = DataManager.Ins.CurrentInvoice;
            //            break;
            //        }
            //    }
           // }
        }
    }
}
