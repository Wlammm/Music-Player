using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Music_Player
{
    class GUIElement
    {
        FrameworkElement[] myElements;

        public GUIElement(params FrameworkElement[] someElements)
        {
            myElements = someElements;
        }

        public bool IsVisible()
        {
            foreach(FrameworkElement E in myElements)
            {
                if(E.Visibility != Visibility.Visible)
                {
                    return false;
                }
            }
            return true;
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
