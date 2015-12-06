using RobertIagar.Podcasts.UWP.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace RobertIagar.Podcasts.UWP.Infrastructure.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public async Task<IUICommand> ShowDialogAsync(string content, string title, IEnumerable<UICommand> commands, uint defaultCommandIndex)
        {
            var showDialog = new MessageDialog(content, title);
            commands.ForEach(c => showDialog.Commands.Add(c));
            showDialog.DefaultCommandIndex = defaultCommandIndex;

            return await showDialog.ShowAsync();
        }
    }

    public interface IMessageDialogService
    {
        Task<IUICommand> ShowDialogAsync(string content, string title, IEnumerable<UICommand> commands, uint defaultCommandIndex);
    }
}
