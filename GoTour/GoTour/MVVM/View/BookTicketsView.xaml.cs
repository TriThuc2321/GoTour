using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GoTour.MVVM.ViewModel;

namespace GoTour.MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookTicketsView : Shell
    {
        public BookTicketsView()
        {
            InitializeComponent();
            this.BindingContext = new BookTicketsViewModel (Navigation);
        }
    }
}