using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageProccesor.ViewModel
{
    
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;
        public bool IsNotBusy => !IsBusy;
        public BaseViewModel()
        {
            _isBusy= false;
        }
    }
}
