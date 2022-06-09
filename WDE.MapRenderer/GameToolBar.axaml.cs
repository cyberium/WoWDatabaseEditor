using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace WDE.MapRenderer
{
    public class GameToolBar : UserControl
    {
        public GameToolBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void GoCoordKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = (GameViewModel)DataContext;
                if (viewModel.OnGoToClickCommand.CanExecute(null))
                    viewModel.OnGoToClickCommand.Execute(null);
            }
        }
    }
}