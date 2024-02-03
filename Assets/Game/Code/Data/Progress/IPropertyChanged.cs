using System;

namespace Game.Code.Data.Progress
{
    public interface IPropertyChanged
    {
        event Action OnPropertyChanged;
    }
}