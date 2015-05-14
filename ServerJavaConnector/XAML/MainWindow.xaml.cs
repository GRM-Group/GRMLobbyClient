﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerJavaConnector.Core;
using ServerJavaConnector.Core.Commands;
using ServerJavaConnector.Core.Connection;
using ServerJavaConnector.Pages;
using ServerJavaConnector.XAML.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerJavaConnector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow instance;
        private CommandManager _CM;

        public MainWindow()
        {
            this.Conn = new Connection();
            instance = this;
            InitializeComponent();
            PageManager pM = new PageManager(getFrames());
            pM.initSetup();
            CommandManager = new CommandManager(this);
        }

        private Dictionary<FrameType, CFrame> getFrames()
        {
            Dictionary<FrameType, CFrame> frames = new Dictionary<FrameType, CFrame>();
            frames.Add(FrameType.MainFrame, MainFrame);
            frames.Add(FrameType.TopFrame, TopFrame);
            frames.Add(FrameType.BottomFrame, BottomFrame);
            return frames;
        }

        public static void CloseApp()
        {
            if (instance != null)
            {
                instance.Conn.Disconnect();
                Application.Current.Shutdown();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            CDialogManager.ShowClosingDialog();
        }

        public Connection Conn { get; private set; }

        public CommandManager CommandManager
        {
            get { return _CM; }
            private set { _CM = value; }
        }
    }
}
