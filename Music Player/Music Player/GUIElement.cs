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

        /// <summary>
        /// Returns if element is visible.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Hides the element.
        /// </summary>
        public void Hide()
        {
            foreach(FrameworkElement E in myElements)
            {
                E.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Shows the element.
        /// </summary>
        public void Show()
        {
            foreach(FrameworkElement E in myElements)
            {
                E.Visibility = Visibility.Visible;
            }
        }
    }
}
