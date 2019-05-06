using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Music_Player
{
    class TabPanel
    {
        FrameworkElement[] myElements;

        public TabPanel(params FrameworkElement[] someElements)
        {
            myElements = someElements;
        }

        public void Hide()
        {
            foreach(FrameworkElement E in myElements)
            {
                E.Visibility = Visibility.Collapsed;
            }
        }

        public void Show()
        {
            foreach(FrameworkElement E in myElements)
            {
                E.Visibility = Visibility.Visible;
            }
        }
    }
}
