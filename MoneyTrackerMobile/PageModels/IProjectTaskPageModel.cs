using CommunityToolkit.Mvvm.Input;
using MoneyTrackerMobile.Models;

namespace MoneyTrackerMobile.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}