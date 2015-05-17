﻿using ServerJavaConnector.Core.Commands;
using ServerJavaConnector.XAML;
using ServerJavaConnector.XAML.Dialogs;
using ServerJavaConnector.XAML.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Connection
{
    public class WebMessageListener
    {
        private Thread listenerThread;
        private Boolean listening;

        public WebMessageListener(Connection conn)
        {
            this.Conn = conn;
        }

        public void run()
        {
            MainWindow MWindow = MainWindow.instance;
            try
            {
                while (MWindow.Conn.Connected && listening)
                {
                    String msg = PacketParser.receivePacket(Conn.ClientSocket);
                    if (!msg.Equals(""))
                    {
                        if (MWindow.CommandManager.executeCommand(msg, Conn, true))
                        {
                            Console.WriteLine("ServerSide Command executed");
                        }
                        else
                        {
                            Console.WriteLine("ServerSide Command not executed");
                        }
                    }
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        CDialogManager.ShowExceptionDialog(ex, null);
                    }
                }
            }
            catch (IOException e)
            {
                CDialogManager.ShowExceptionDialog(e, null);
                Conn.Disconnect();
            }
        }

        public void startListening()
        {
            if (listenerThread == null && !listening)
            {
                listening = true;
                listenerThread = new Thread(() => run());
                listenerThread.Start();
            }
        }

        public void stopListening()
        {
            if (listenerThread != null && listening)
            {
                listening = false;
                listenerThread.Abort();
                listenerThread = null;
            }
        }

        public Connection Conn { get; private set; }
    }
}
