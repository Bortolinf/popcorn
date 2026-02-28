using Popcorn.UI.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace Popcorn.UI.Views;

public partial class CronometragemView : UserControl
{
    public CronometragemView() => InitializeComponent();

    private void NumeroBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is CronometragemViewModel vm)
            if (vm.RegistrarPorNumeroCommand.CanExecute(null))
                vm.RegistrarPorNumeroCommand.Execute(null);
    }

    private void TagBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is CronometragemViewModel vm)
            if (vm.RegistrarPorTagCommand.CanExecute(null))
                vm.RegistrarPorTagCommand.Execute(null);
    }
}
