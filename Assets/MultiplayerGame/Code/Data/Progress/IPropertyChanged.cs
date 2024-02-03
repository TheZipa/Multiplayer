using System;

namespace MultiplayerGame.Code.Data.Progress
{
    public interface IPropertyChanged
    {
        event Action OnPropertyChanged;
    }
}