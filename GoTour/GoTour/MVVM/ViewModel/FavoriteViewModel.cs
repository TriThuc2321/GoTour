using GoTour.Core;
using GoTour.Database;
using GoTour.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GoTour.MVVM.ViewModel
{
    class FavoriteViewModel : ObservableObject
    {
        INavigation navigation;

        public Command UnlovedCommand { get; }


        public FavoriteViewModel()
        {
        }
        public FavoriteViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            // Tours = DataManager.Ins.ListPlace;

            //UnlovedCommand = new Command(unlovedPlace);

            //Love = "love_red.png";

            init();
        }

        void init()
        {
            Tours = new ObservableCollection<FavouriteTour>();

           Tours.Add( new FavouriteTour()
            {
                name = "Dạo chơi Sóc Trăng",
                imgSource = "https://cdn3.ivivu.com/2019/09/chua-som-rong-soc-trang-ivivu-3.jpg",
                startTime = DateTime.Now.Date,
                days = "2 ngày 1 đêm",
                passengerNumber = 20
            });

            Tours.Add(new FavouriteTour()
            {
                name = "Lang thang Cần Thơ",
                imgSource = "https://thamhiemmekong.com/wp-content/uploads/2020/02/dulichcantho.jpg",
                startTime = DateTime.Now.Date,
                days = "3 ngày",
                passengerNumber = 25
            });

            Tours.Add(new FavouriteTour()
            {
                name = "DU LỊCH CỦ CHI - ĐẤT THÉP THÀNH ĐỒNG [KHU DI TÍCH ĐỊA ĐẠO CỦ CHI – ĐỀN BẾN DƯỢC]",
                imgSource = "https://saigontourist.net/uploads/images/Page/SGT-CuChi/242834363_10160271427402952_5307975930879584325_n.jpg",
                startTime = DateTime.Now.Date,
                days = "3 ngày 2 đêm",
                passengerNumber = 30
            });

            Tours.Add(new FavouriteTour()
            {
                name = "DU LỊCH PHÚ QUỐC [KHỞI HÀNH TỪ HẢI PHÒNG]",
                imgSource = "https://saigontourist.net/uploads/destination/TrongNuoc/Phuquoc/phu-quoc-beach_788218267.jpg",
                startTime = DateTime.Now.Date,
                days = "3 ngày 2 đêm",
                passengerNumber = 10
            });
        }

        private ObservableCollection<FavouriteTour> _tour;
        public ObservableCollection<FavouriteTour> Tours
        {
            get { return _tour; }
            set
            {
                _tour = value;
                OnPropertyChanged("Tours");
            }
        }

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

    }
}
