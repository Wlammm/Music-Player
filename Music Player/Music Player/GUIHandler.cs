using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Music_Player
{
    public class GUIHandler
    {
        public enum GUITab { AllSong, Youtube, Local, Download, Playlist, Setting, PlaylistName, PlaylistShowSongs, PlaylistsList }
        private GUITab activeTab = GUITab.AllSong;

        private Dictionary<GUITab, GUIElement> myTabs;

        private MainWindow myMainWindow;

        public GUIHandler(MainWindow aMainWindow)
        {
            myMainWindow = aMainWindow;

            // Add all elements to different GUI categories. 
            myTabs = new Dictionary<GUITab, GUIElement>
            {
                {GUITab.AllSong, new GUIElement(myMainWindow.AllSongsGrid) },
                {GUITab.Youtube, new GUIElement(myMainWindow.YoutubeGrid) },
                {GUITab.Local, new GUIElement(myMainWindow.LocalGrid, myMainWindow.btnAddSongsLocal) },
                {GUITab.Download, new GUIElement(myMainWindow.DownloadsGrid) },
                {GUITab.Playlist, new GUIElement(myMainWindow.PlaylistsGrid, myMainWindow.btnAddPlaylist) },
                {GUITab.Setting, new GUIElement(myMainWindow.SettingsGrid) },
                {GUITab.PlaylistName, new GUIElement(myMainWindow.PlaylistNameMenu, myMainWindow.FadeBackground) },
                {GUITab.PlaylistShowSongs, new GUIElement(myMainWindow.PlaylistShowSongs) },
                {GUITab.PlaylistsList, new GUIElement(myMainWindow.ListPlaylists, myMainWindow.FadeBackground) }
            };

            SwapTab(GUITab.AllSong);
        }

        /// <summary>
        /// Swaps from current GUI element to new element.
        /// Hiding current active element and showing new.
        /// </summary>
        /// <param name="aGUITab">New element to be visible</param>
        public void SwapTab(GUITab aGUITab)
        {
            myTabs[activeTab].Hide();
            myTabs[aGUITab].Show();

            activeTab = aGUITab;
        }

        /// <summary>
        /// Shows a specific element.
        /// </summary>
        /// <param name="aGUIElement"></param>
        public void ShowElement(GUITab aGUIElement)
        {
            myTabs[aGUIElement].Show();
        }

        /// <summary>
        /// Hides a specific element.
        /// </summary>
        /// <param name="aGUIElement"></param>
        public void HideElement(GUITab aGUIElement)
        {
            myTabs[aGUIElement].Hide();
        }
    }
}
