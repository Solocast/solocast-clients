using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Solocast.UWP.Infrastructure.StyleSelectors
{
    public class AlternatingColorListViewStyleSelector : StyleSelector
    {
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            Style st = new Style();
            st.TargetType = typeof(ListViewItem);
            Setter backGroundSetter = new Setter();
            backGroundSetter.Property = ListViewItem.BackgroundProperty;
            ListView listView = ItemsControl.ItemsControlFromItemContainer(container) as ListView;
            int index = listView.IndexFromContainer(container);

            if (index % 2 == 0)
            {
                backGroundSetter.Value = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            }
            else
            {
                backGroundSetter.Value = new SolidColorBrush(Color.FromArgb(75,255,255,255));
            }
            st.Setters.Add(backGroundSetter);
            return st;
        }
    }
}
