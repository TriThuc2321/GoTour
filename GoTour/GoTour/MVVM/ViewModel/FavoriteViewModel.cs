using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using GoTour.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    public class FavoriteViewModel : ObservableObject
    {
        INavigation navigation;
        Page page;

        public FavoriteViewModel() { }
        public FavoriteViewModel(INavigation navigation, Page page)
        {
            this.page = page;
            this.navigation = navigation;

            Favourites = new ObservableCollection<FavouriteTour>();
            foreach (var favourites in DataManager.Ins.ListFavouriteTours)
            {
                if (favourites.email == DataManager.Ins.CurrentUser.email)
                    Favourites.Add(favourites);
            }
        }

      

        private ObservableCollection<FavouriteTour> _favourites;
        public ObservableCollection<FavouriteTour> Favourites
        {
            get { return _favourites; }
            set
            {
                _favourites = value;
                OnPropertyChanged("Favourites");
            }
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

        public ICommand SelectedCommand => new Command<object>((obj) =>
        {
            FavouriteTour result = obj as FavouriteTour;
            if (result != null)
            {
                DataManager.Ins.currentTour = result.tour;
                navigation.PushAsync(new DetailTourView());
                SelectedTour = null;
            }
        });

        public ICommand UnlovedCommand => new Command<object>((obj) =>
        {
            FavouriteTour result = obj as FavouriteTour;

            if (result != null)
            {
                //var decision = page.DisplayAlert("Delete Tour",
                //    "Are you sure to delete this tour?",
                //    "Delete", "Cancel");

                //if (!decision.Result) return;

                int index = Favourites.IndexOf(result);

              //  Favourites[index].love = "love_white.png";

                DataManager.Ins.FavoritesServices.DeleteFavoriteTour(result.id);
                DataManager.Ins.ListFavouriteTours.Remove(result);
                Favourites.Remove(result);
            }
        });



        //private string _love;
        //public string Love
        //{
        //    get { return _love; }
        //    set
        //    {
        //        _love = value;
        //        OnPropertyChanged("Love");
        //    }
        //}

        //private void unlovedPlace(object obj)
        //{
        //    _love = "love_white.png";
        //}

        //void init()
        //{
        //    Tours = new ObservableCollection<FavouriteTour>();

        //   Tours.Add( new FavouriteTour()
        //    {
        //        name = "Dạo chơi Sóc Trăng",
        //        imgSource = "https://cdn3.ivivu.com/2019/09/chua-som-rong-soc-trang-ivivu-3.jpg",
        //        startTime = DateTime.Now.Date,
        //        days = "2 ngày 1 đêm",
        //        passengerNumber = 20
        //    });

        //    Tours.Add(new FavouriteTour()
        //    {
        //        name = "Lang thang Cần Thơ",
        //        imgSource = "https://thamhiemmekong.com/wp-content/uploads/2020/02/dulichcantho.jpg",
        //        startTime = DateTime.Now.Date,
        //        days = "3 ngày",
        //        passengerNumber = 25
        //    });

        //    Tours.Add(new FavouriteTour()
        //    {
        //        name = "DU LỊCH CỦ CHI - ĐẤT THÉP THÀNH ĐỒNG [KHU DI TÍCH ĐỊA ĐẠO CỦ CHI – ĐỀN BẾN DƯỢC]",
        //        imgSource = "https://saigontourist.net/uploads/images/Page/SGT-CuChi/242834363_10160271427402952_5307975930879584325_n.jpg",
        //        startTime = DateTime.Now.Date,
        //        days = "3 ngày 2 đêm",
        //        passengerNumber = 30
        //    });

        //    Tours.Add(new FavouriteTour()
        //    {
        //        name = "DU LỊCH PHÚ QUỐC [KHỞI HÀNH TỪ HẢI PHÒNG]",
        //        imgSource = "https://saigontourist.net/uploads/destination/TrongNuoc/Phuquoc/phu-quoc-beach_788218267.jpg",
        //        startTime = DateTime.Now.Date,
        //        days = "3 ngày 2 đêm",
        //        passengerNumber = 10
        //    });
        //}
    }
}
