using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using MultiplayerLib;
using System.Collections;
using System.IO;
using System.Reflection;

namespace ServerGUI
{
    public partial class ServerWindow : Form
    {
        GameController controller;

        public ServerWindow()
        {
            InitializeComponent();
            scanMaps();

            TcpChannel channel = new TcpChannel(31337);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(GameController), "Controller", WellKnownObjectMode.Singleton);
            controller = (GameController)Activator.GetObject(typeof(GameController), "tcp://localhost:31337/Controller");

            respawnTimeBox.SelectedIndex = 2;
            spawnsLimitBox.SelectedIndex = 0;
            TeamsBox.SelectedIndex = 1;
            gameModeBox.SelectedIndex = 0;
            timeBox.SelectedIndex = 0;
            initTeamScoreList();
        }

        private void setRespawnTimeOnServer()
        {
            int time = respawnTimeBox.SelectedIndex == 0 ? -1 : int.Parse((string)respawnTimeBox.SelectedItem) * 45;
            controller.setRespawnTime(time);
        }

        private void setTeamsAmountOnServer()
        {
            int teams = int.Parse((string)TeamsBox.SelectedItem);
            controller.setTeamsAmount(teams);
        }

        private void setTimeLimitOnServer()
        {
            int time = timeBox.SelectedIndex == 0 ? -1 :  int.Parse((string)timeBox.SelectedItem) * 2700;
            controller.setTimeLimit(time);
        }

        private void setSpawnsLimitOnServer()
        {
            int spawns = spawnsLimitBox.SelectedIndex == 0 ? -1 : int.Parse((string)spawnsLimitBox.SelectedItem);
            controller.setSpawnsLimit(spawns);
        }

        private void setPointsLimitOnServer()
        {
            int points = pointsLimitBox.SelectedIndex == 0 ? -1 : int.Parse((string)pointsLimitBox.SelectedItem);
            controller.setPointsLimit(points);
        }

        private void setGameModeOnServer()
        {
            GameMode mode = (GameMode) gameModeBox.SelectedIndex;
            controller.setGameMode(mode);
        }

        private void initTeamScoreList()
        {
            teamScoreList.Items.Clear();
            for (int i = 0; i < 4; i++)
            {
                teamScoreList.Items.Add("[T" + (i + 1) + "]");
                teamScoreList.Items[i].SubItems.Add(controller.getTeamScore(i).ToString());
                teamScoreList.Items[i].SubItems.Add(controller.getPlayersInTeam(i).ToString());
            }
        }

        private void ServerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopServer();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            stopServer();
            Close();
        }

        private void stopServer()
        {
            controller.stop();
        }
        
        private void bResart_Click(object sender, EventArgs e)
        {
            setTeamsAmountOnServer();
            setGameModeOnServer();
            setPointsLimitOnServer();
            setTimeLimitOnServer();
            setSpawnsLimitOnServer();
            string mapName = (string) mapsListBox.SelectedItem;
            controller.restart(mapName);
        }
        
        private void bRespawn_Click(object sender, EventArgs e)
        {
            int ID = (int) selectedPlayerID.Value;
            if (!controller.respawnPlayer(ID))
                MessageBox.Show("No player with given ID", "Successfully failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
        }

        private void bKick_Click(object sender, EventArgs e)
        {
            int ID = (int)selectedPlayerID.Value;
            if (!controller.kickPlayer(ID))
                MessageBox.Show("No player with given ID", "Successfully failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
        }
        
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            ArrayList players = controller.getPlayers();
            playersList.Items.Clear();

            for (int i = 0; i < players.Count; i++)
            {
                Player p = (Player)players[i];

                playersList.Items.Add(p.ID.ToString());
                playersList.Items[i].SubItems.Add(p.Name);
                playersList.Items[i].SubItems.Add((p.Team + 1).ToString());
                playersList.Items[i].SubItems.Add(p.Health.ToString());
                playersList.Items[i].SubItems.Add(p.Frags.ToString());
                playersList.Items[i].SubItems.Add(p.Assists.ToString());
                playersList.Items[i].SubItems.Add(p.Deaths.ToString());
                playersList.Items[i].SubItems.Add((p.Frags / (float)p.Deaths).ToString());
                playersList.Items[i].SubItems.Add(p.className);
            }
            playersList.Refresh();

            for (int i = 0; i < 4; i++)
            {
                teamScoreList.Items[i].SubItems[1].Text = controller.getTeamScore(i).ToString();
                teamScoreList.Items[i].SubItems[2].Text = controller.getPlayersInTeam(i).ToString();
            }
            teamScoreList.Refresh();
        }
        
        private void scanMaps()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int currentMapToCheck = 1;

            while(File.Exists(assemblyFolder + "/res/maps/map_" + currentMapToCheck + ".map"))
            {
                mapsListBox.Items.Add("map_" + currentMapToCheck);
                currentMapToCheck++;
            }

            if (mapsListBox.Items.Count == 0)
                MessageBox.Show("Server did not found any *.map files in ~/res/maps/");
            else
                mapsListBox.SelectedIndex = 0;
        }

        private void respawnTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setRespawnTimeOnServer();
        }

        private void gameModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Deathmatch
            //Domination
            //Capture the flag
            //Protect the king
            //Harvester
            pointsLimitBox.Items.Clear();

            switch (gameModeBox.SelectedIndex)
            {
                case 0: //DeathMatch
                    pointLimitLabel.Text = "Frags";
                    pointsLimitBox.Items.Add("0");
                    pointsLimitBox.Items.Add("5");
                    pointsLimitBox.Items.Add("10");
                    pointsLimitBox.Items.Add("20");
                    pointsLimitBox.Items.Add("30");
                    pointsLimitBox.Items.Add("40");
                    pointsLimitBox.Items.Add("50");
                    pointsLimitBox.SelectedIndex = 3;
                    break;

                case 1: //Domination
                    pointLimitLabel.Text = "Control time";
                    pointsLimitBox.Items.Add("0");
                    pointsLimitBox.Items.Add("3000");
                    pointsLimitBox.Items.Add("5000");
                    pointsLimitBox.Items.Add("10000");
                    pointsLimitBox.Items.Add("15000");
                    pointsLimitBox.Items.Add("20000");
                    pointsLimitBox.Items.Add("30000");
                    pointsLimitBox.SelectedIndex = 3;
                    break;

                case 2: //Capture the flag
                    pointLimitLabel.Text = "Captures";
                    pointsLimitBox.Items.Add("0");
                    pointsLimitBox.Items.Add("3");
                    pointsLimitBox.Items.Add("5");
                    pointsLimitBox.Items.Add("10");
                    pointsLimitBox.SelectedIndex = 2;
                    break;

                case 3: //Protect the king
                    pointLimitLabel.Text = "Assassiations";
                    pointsLimitBox.Items.Add("0");
                    pointsLimitBox.Items.Add("3");
                    pointsLimitBox.Items.Add("5");
                    pointsLimitBox.Items.Add("10");
                    pointsLimitBox.Items.Add("15");
                    pointsLimitBox.SelectedIndex = 3;
                    break;

                case 4: //Harvester
                    pointLimitLabel.Text = "Skulls";
                    pointsLimitBox.Items.Add("0");
                    pointsLimitBox.Items.Add("5");
                    pointsLimitBox.Items.Add("10");
                    pointsLimitBox.Items.Add("15");
                    pointsLimitBox.Items.Add("20");
                    pointsLimitBox.Items.Add("30");
                    pointsLimitBox.SelectedIndex = 3;
                    break;
            }
        }
    }
}
