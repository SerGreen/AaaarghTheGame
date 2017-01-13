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
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Net.Sockets;
using MultiplayerLib.GameClasses;
using MultiplayerLib.Particles;
using MultiplayerLib.Projectiles;

namespace Client
{
    public partial class GameWindow : Form
    {
        private static class PressedKeys
        {
            public static bool Down { get; set; }
            public static bool Up { get; set; }
        }

        private Random rnd;
        private List<Bitmap> sprites;
        //private Bitmap[] tileset;
        private Tilemap tilemap;
        private string loadedMapName;
        private GameController controller;
        private Player player;
        private string name;
        private bool joined;
        private Thread thread;
        private const int FPS = 45;
        private int frameTime;
        private Rectangle camera;
        private Sprite drake;
        private Sprite glasses;
        private Sprite crown;
        private Bitmap teammateArrow;
        private Bitmap meArrow;
        private Bitmap background;
        private Bitmap backgroundDead;
        private Bitmap blackFade;
        private Bitmap redFade;
        private bool showScoreboard;
        private bool showMenu;
        private List<MenuItem> menuClassItems;
        private List<MenuItem> menuTeamItems;
        private List<Color> teamColors;
        private Point mouseLoc;
        private int myPlayerID;
        private int playerTeam;

        private Brush brushBlack;
        private Brush brushBlackA;
        private Brush brushWhite;
        private Brush brushGreen;
        private Brush brushYellow;
        private Brush brushYellowA;
        private Brush brushOrange;
        private Brush brushCyan;
        private Brush brushBlue;
        private Brush brushBlueA;
        private Brush brushGreenA;
        private Brush brushRed;
        private Brush brushGrayA;
        private Pen penBlackA;
        private Pen penRed;
        private Font font10;
        private Font font14;
        private Font font16;
        private Font font20;
        private Font font32;

        public GameWindow()
        {
            InitializeComponent();
            teamBox.SelectedIndex = 0;
            classBox.SelectedIndex = 0;
            ipTextBox.Text = loadLastIP();
            rnd = new Random();
            string currentLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            frameTime = 1000 / FPS;
            joined = false;
            myPlayerID = -1;
            playerTeam = -1;
            name = "player_" + new Random().Next(0, 1000);
            //loadTileset((Bitmap)Bitmap.FromFile(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../res/tileset.png"), 16);
            loadSprites();
            loadedMapName = "//NONE//";
            //loadTilemap();

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);

            camera = new Rectangle(-427, -240, 854, 480);
            drake = new Sprite(0, 0, (Bitmap)Bitmap.FromFile(currentLoc + "/../res/drake.png"), 50, 2);
            glasses = new Sprite(0, 0, (Bitmap)Bitmap.FromFile(currentLoc + "/../res/glasses.png"), 50, 2);
            crown = new Sprite(0, 0, SpriteIndex.Crown, (Bitmap)Bitmap.FromFile(currentLoc + "/../res/crown.png"), 15, 2);
            showScoreboard = false;
            initTeamColors();
            initMenu(currentLoc);

            brushBlack = new SolidBrush(Color.Black);
            brushBlackA = new SolidBrush(Color.FromArgb(128, Color.Black));
            brushWhite = new SolidBrush(Color.White);
            brushGreen = new SolidBrush(Color.Lime);
            brushYellow = new SolidBrush(Color.Yellow);
            brushYellowA = new SolidBrush(Color.FromArgb(128, Color.Yellow));
            brushOrange = new SolidBrush(Color.Orange);
            brushBlueA = new SolidBrush(Color.FromArgb(128, Color.Blue));
            brushBlue = new SolidBrush(Color.LightYellow);
            brushCyan = new SolidBrush(Color.Cyan);
            brushGreenA = new SolidBrush(Color.FromArgb(128, Color.Green));
            brushRed = new SolidBrush(Color.Red);
            brushGrayA = new SolidBrush(Color.FromArgb(128, Color.DarkGray));
            penBlackA = new Pen(Color.FromArgb(128, Color.Black));
            penRed = new Pen(Color.Red);
            font10 = new Font(new FontFamily("Calibri"), 10, FontStyle.Regular);
            font14 = new Font(new FontFamily("Calibri"), 14, FontStyle.Regular);
            font16 = new Font(new FontFamily("Calibri"), 16, FontStyle.Bold);
            font20 = new Font(new FontFamily("Calibri"), 20, FontStyle.Bold);
            font32 = new Font(new FontFamily("Calibri"), 32, FontStyle.Bold);
        }

        private void initTeamColors()
        {
            teamColors = new List<Color>();
            teamColors.Add(Color.RoyalBlue);
            teamColors.Add(Color.DarkOrange);
            teamColors.Add(Color.LimeGreen);
            teamColors.Add(Color.Purple);
        }

        private void initMenu(string currentLoc)
        {
            showMenu = false;
            menuClassItems = new List<MenuItem>();
            menuClassItems.Add(new MenuItem(277, 64, 300, 50, "Warrior", sprites[(int)SpriteIndex.war_Stand]));
            menuClassItems.Add(new MenuItem(277, 128, 300, 50, "Berserker", sprites[(int)SpriteIndex.ber_Stand]));
            menuClassItems.Add(new MenuItem(277, 192, 300, 50, "Bowman", sprites[(int)SpriteIndex.bow_Stand]));
            menuClassItems.Add(new MenuItem(277, 256, 300, 50, "Arbalester", sprites[(int)SpriteIndex.arb_Stand]));
            menuClassItems.Add(new MenuItem(277, 320, 300, 50, "Knight", sprites[(int)SpriteIndex.kni_Stand]));
            menuClassItems.Add(new MenuItem(277, 384, 300, 50, "Thief", sprites[(int)SpriteIndex.thi_Stand]));

            menuTeamItems = new List<MenuItem>();
            menuTeamItems.Add(new MenuItem(camera.Width - 140, 64, 140, 50, "Team 1"));
            menuTeamItems.Add(new MenuItem(camera.Width - 140, 128, 140, 50, "Team 2"));
            menuTeamItems.Add(new MenuItem(camera.Width - 140, 192, 140, 50, "Team 3"));
            menuTeamItems.Add(new MenuItem(camera.Width - 140, 256, 140, 50, "Team 4"));
            for (int i = 0; i < menuTeamItems.Count; i++)
                menuTeamItems[i].selectedColor = teamColors[i];
        }

        private void loadSprites()
        {
            sprites = new List<Bitmap>();
            string currentLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arrow.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/slash.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/slash_axe.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/slash_dagger.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bolt.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/melon.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/melon_crack.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knife.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalest_smash.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/fireball.png"));
            
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/dust.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/blood_arrow.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/blood_slash.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/+1.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/+2.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/-1.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/fireball_explosion.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/explosion.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/fire.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/domination_plates.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/flag.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/flag_place.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/crown.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/skulls.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/skulls_front.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/statue.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/tombstone.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/tombstone_crush.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/warrior_run_swing.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/berserker_run_swing.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_run_swing.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_run_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_reload.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_smack.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_run_smack.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/arbalester_jump_smack.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/knight_run_swing.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_run_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_throw.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_jump_throw.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/thief_run_throw.png"));

            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_standing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_run.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_jump.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_jump_swing.png"));
            sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/mage_run_swing.png"));

            //sprites.Add((Bitmap)Bitmap.FromFile(currentLoc + "/../res/bowman_charge.png"));

            teammateArrow = (Bitmap)Bitmap.FromFile(currentLoc + "/../res/teammate.png");
            meArrow = (Bitmap)Bitmap.FromFile(currentLoc + "/../res/me.png");
            //backgroundDead = (Bitmap)Bitmap.FromFile(currentLoc + "/../res/background_dead.png");
            blackFade = (Bitmap)Bitmap.FromFile(currentLoc + "/../res/black_fade.png");
            redFade = (Bitmap)Bitmap.FromFile(currentLoc + "/../res/red_fade_blood.png");
        }

        private void loadBackground(int index, string assemblyFolderPath)
        {
            background = (Bitmap)Bitmap.FromFile(assemblyFolderPath + "/../res/background_" + index + ".png");
            createDeadBackground();
            screen.BackColor = background.GetPixel(0, 0);
        }

        private void createDeadBackground()
        {
            backgroundDead = new Bitmap(background.Width, background.Height);
            for (int i = 0; i < background.Width; i++)
                for (int j = 0; j < background.Height; j++)
                {
                    Color pixel = background.GetPixel(i, j);
                    int colorTone = (int)(((pixel.R + pixel.G + pixel.B) / 6) * 1.5f);
                    backgroundDead.SetPixel(i, j, Color.FromArgb(colorTone, colorTone, colorTone));
                }
        }

        /*
        private void loadTileset(Bitmap bmp, int tileWidth)
        {
            tileset = new Bitmap[bmp.Width / tileWidth];

            for (int i = 0; i < tileset.Length; i++)
            {
                tileset[i] = bmp.Clone(new Rectangle(i * tileWidth, 0, tileWidth, bmp.Height), bmp.PixelFormat);
            }
        }
        */

        private void loadTilemap(string mapName)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (File.Exists(assemblyFolder + "/../res/maps/" + mapName + ".tiles"))
            {
                //load client data
                StreamReader sr = new StreamReader(assemblyFolder + "/../res/maps/" + mapName + ".tiles");
                string file = sr.ReadToEnd();
                sr.Close();

                string[] tileDat = file.Split('#');

                //load tilemap width and height
                int tileX = int.Parse(tileDat[0].Split(':')[0]);
                int tileY = int.Parse(tileDat[0].Split(':')[1]);

                //load tiles data
                tilemap = new Tilemap(tileX, tileY, 16, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/tileset.png"));
                string[] tileRows = tileDat[1].Split('\n');
                for (int j = 0; j < tileRows.Length; j++)
                {
                    string[] tileCols = tileRows[j].Split(' ');
                    for (int i = 0; i < tileCols.Length; i++)
                    {
                        tilemap.setTile(i, j, short.Parse(tileCols[i]));
                    }
                }

                tilemap.shiftY = -10;

                int backgroundNumber = 0;
                if (tileDat.Length > 2 && tileDat[2].Length > 0)
                    backgroundNumber = int.Parse(tileDat[2]);

                loadBackground(backgroundNumber, assemblyFolder);
            }
        }

        private string loadLastIP()
        {
            string lastIpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/lastip";

            if (File.Exists(lastIpPath))
            {
                StreamReader sr = new StreamReader(lastIpPath);
                string ip = sr.ReadToEnd();
                sr.Close();

                return ip;
            }
            else
            {
                StreamWriter sw = new StreamWriter(lastIpPath);
                sw.Write("localhost");
                sw.Close();

                return "localhost";
            }
        }

        private void saveLastIP(string ip)
        {
            string lastIpPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/lastip";

            if (File.Exists(lastIpPath))
                File.Delete(lastIpPath);

            StreamWriter sw = new StreamWriter(lastIpPath);
            sw.Write(ip);
            sw.Close();
        }

        private void startGame()
        {
            thread = new Thread(new ThreadStart(gameLoop));
            thread.Start();
        }

        private void gameLoop()
        {
            Stopwatch timer = new Stopwatch();

            try
            {
                while (joined)
                {
                    timer.Reset();
                    timer.Start();
                    //***********************

                    tick();
                    render();

                    //***********************
                    timer.Stop();
                    int timeToSleep = frameTime - (int)timer.ElapsedMilliseconds;
                    if (timeToSleep > 0)
                        Thread.Sleep(timeToSleep);
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message + "\nDAMN!!! LETS GET OUTTA HERE!!!11", "AAAAAAAARRRGGGHHHH!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                joined = false;
                Application.Exit();
            }
        }

        private void tick()
        {
            if (showMenu)
            {
                foreach (MenuItem mi in menuClassItems)
                    mi.tick(mouseLoc.X, mouseLoc.Y);

                foreach (MenuItem mi in menuTeamItems)
                    mi.tick(mouseLoc.X, mouseLoc.Y);
            }

            if (joined)
            {
                player = controller.getPlayerForClient(myPlayerID);
                string serverMapName = controller.getMapName();
                if (loadedMapName.CompareTo(serverMapName) != 0)
                {
                    loadedMapName = serverMapName;
                    loadTilemap(loadedMapName);
                }

                if (playerTeam != player.Team)
                {
                    playerTeam = player.Team;
                    updateMenuColors();
                }
            }
        }

        private void updateMenuColors()
        {
            foreach (MenuItem mi in menuClassItems)
                mi.selectedColor = teamColors[playerTeam];
        }

        private void render()
        {
            Bitmap bmp = new Bitmap(camera.Width, camera.Height);   //surface to render on
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;   //without this scaling will be blurry

            paintBackground(g);            
            
            g.Dispose();
            g = Graphics.FromImage(bmp);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;                   //without this scaling will be blurry
            g.SetClip(new RectangleF(player.X + camera.X, player.Y + camera.Y, camera.Width, camera.Height));   //not sure if neccessary, but probably cuts render area
            g.TranslateClip(-player.X - camera.X, -player.Y - camera.Y);                                        //needed with upper line
            g.TranslateTransform((int) (-player.X - camera.X - 50), (int) (-player.Y - camera.Y - 30));         //move surface to follow player with camera

            /*
            foreach (SolidObject so in controller.getSolidObjects())
                paintSolidObject(g, so);
            */

            foreach (Projectile pr in controller.getProjectiles())
            {
                if (pr != null)                                         //pretty stupid thing, but it may crash without this
                {
                    //if (!(pr is Fireball) || player.Dead || player is Mage)
                        paintProjectiles(g, pr);
                }
            }

            //paintTileMap(g, controller.getTiles());
            //tilemap.paint(g, (int)(player.X + player.Width / 2 + camera.X), (int)(player.Y + player.Height / 2 + camera.Y), camera.Width, camera.Height);
            tilemap.paint(g);

            GameMode gameMode = controller.GetGameMode;

            if (gameMode == GameMode.Domination)
            {
                foreach (DominationPlate dp in controller.getDominationPlates())
                    if (dp != null)
                        paintSprite(g, dp.getSprite, 1, 1);
            }
            else if (gameMode == GameMode.Harvester)
            {
                paintSprite(g, controller.getStatueHarvester().getSprite, 1, 1);
            }

            foreach (Tombstone ts in controller.getTombstones())
            {
                if (ts != null)
                    paintTombstone(g, ts);
            }

            foreach (Player p in controller.getPlayers())
            {
                if (p != null)                                          //same shit here
                    if (player.Dead || !p.Dead)
                        paintPlayer(g, p);
            }

            foreach (Skull skull in controller.getSkulls())
            {
                if (skull != null)
                    paintSkull(g, skull);
            }

            if (gameMode == GameMode.CaptureFlag || gameMode == GameMode.Harvester)
            {
                foreach (FlagPlace fp in controller.getFlagPlaces())
                {
                    paintFlag(g, fp);
                }
            }

            //paint particles
            foreach (Particle part in controller.getParticles())
            {
                if (part != null)                                       //and one more over here
                {
                    if (!((part is FireballExplosion) || (part is Explosion)) || player.Dead || player is Mage)
                        paintSprite(g, part.getSprite(), part.face, 1);
                }
            }
            
            g.Dispose();
            g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;   //without this scaling will be blurry

            if (player.Dead)
            {
                g.DrawImage(blackFade, 0, 0, 854, 480);
                paintRespawnBar(g);
            }
            else if (player.Health / player.healthMax <= 0.35)
            {
                g.DrawImage(redFade.SetOpacity((1 - player.Health / (float)(player.healthMax * 0.35f))), 0, 0, 854, 480);
            }

            if (controller.TimeLimit > 0)
                paintTimeLimit(g);

            if (controller.SpawnsLimit > 0)
                paintSpawnsLimit(g);

            if (controller.PointsLimit > 0)
                paintTeamPoints(g);

            if (controller.GameOver)
                paintRoundOver(g, "GAME");
            else if (controller.RoundOver)
                paintRoundOver(g, "ROUND");

            if(showScoreboard)
                paintScoreboard(g);

            if (showMenu)
                paintMenu(g);

            g.Dispose();
            screen.Image = bmp;
        }

        private void paintSkull(Graphics g, Skull skull)
        {
            float scale = skull.captured ? 0.6f : 1;
            paintSprite(g, skull.GetSprite, skull.face * scale, scale);
            //g.DrawString(skull.vx.ToString(), font10, brushRed, skull.X, skull.Y - 8);
        }

        private void paintTombstone(Graphics g, Tombstone tombstone)
        {
            paintSprite(g, tombstone.GetSprite, 1, 1);
        }

        private void paintFlag(Graphics g, FlagPlace fp)
        {
            if (fp != null && controller.getPlayersInTeam(fp.team) > 0)
            {
                paintSprite(g, fp.GetSprite, 1, 1);
                paintSprite(g, fp.Flag.GetSprite, 1, 1);
                //g.DrawString(fp.FlagOnPlace.ToString(), font14, brushRed, fp.GetSprite.x, fp.GetSprite.y - 14);
            }
        }

        private void paintSpawnsLimit(Graphics g)
        {
            int teamsAmount = controller.getTeamsAmount();
            int spawnsLimit = controller.SpawnsLimit;

            int h = 32 + 16 * teamsAmount;
            g.FillRectangle(brushBlackA, camera.Width - 80, camera.Height - h - 10, 70, h);
            g.DrawString("Spawns", font14, brushWhite, camera.Width - 75, camera.Height - 10 - h);
            for (int i = 0; i < teamsAmount; i++)
            {
                int teamSpawnsLeft = controller.getTeamSpawnsLeft(i);
                float spawnsPercent = teamSpawnsLeft / (float)spawnsLimit;
                g.FillRectangle(new SolidBrush(teamColors[i]), camera.Width - 75, camera.Height + 16 - h + i * 16, 60 * spawnsPercent, 14);
                g.DrawRectangle(teamSpawnsLeft == 0 ? penRed : penBlackA, camera.Width - 75, camera.Height + 16 - h + i * 16, 60, 14);
            }
        }

        private void paintTeamPoints(Graphics g)
        {
            int teamsAmount = controller.getTeamsAmount();
            int pointsLimit = controller.PointsLimit;
            GameMode gm = controller.GetGameMode;

            int shiftX = controller.TimeLimit > 0 ? 25 : 5;
            int h = 32 + 16 * teamsAmount;
            int w = 100;
            float pointSize = (w - 10) / (float)pointsLimit;

            g.FillRectangle(brushBlackA, shiftX, camera.Height - h - 10, w, h);
            g.DrawString("Points", font14, brushWhite, shiftX + 5, camera.Height - 10 - h);
            for (int i = 0; i < teamsAmount; i++)
            {
                int teamPoints = controller.getTeamPoints(i);
                if (gm == GameMode.Domination)
                    teamPoints = pointsLimit - teamPoints;

                float pointsPercent = teamPoints / (float)pointsLimit;
                g.FillRectangle(new SolidBrush(teamColors[i]), shiftX + 5, camera.Height + 16 - h + i * 16, (w - 10) * pointsPercent, 14);
                g.DrawRectangle(penBlackA, shiftX + 5, camera.Height + 16 - h + i * 16, w - 10, 14);

                if (pointSize >= 2)
                {
                    for (float j = shiftX + 5 + pointSize; j <= shiftX + w - 5; j += pointSize)
                    {
                        int y = camera.Height + 16 - h + i * 16;
                        g.DrawLine(penBlackA, j, y, j, y + 14);
                    }
                }
            }
        }

        private void paintTimeLimit(Graphics g)
        {
            int timeLimit = controller.TimeLimit;
            int timeLeft = controller.TimeLeft;
            float timePercent = timeLeft / (float)timeLimit;

            g.FillRectangle(brushWhite, 5, camera.Height - 10 - 100 * timePercent, 15, 100 * timePercent);
            g.DrawRectangle(penBlackA, 5, camera.Height - 110, 15, 100);
        }

        private void paintRoundOver(Graphics g, string param)
        {
            int winner = controller.WinnerTeam;
            Brush teamBrush = brushYellowA;


            int shiftX = 247;
            int shiftY = 170;
            if (winner >= 0)
                teamBrush = new SolidBrush(Color.FromArgb(196, teamColors[winner]));

            g.FillRectangle(teamBrush, shiftX, shiftY, 430, 100);
            if (winner >= 0)
                g.DrawString((winner + 1) + " TEAM WINS " + param + "!", font32, brushWhite, shiftX + 20, shiftY + 20);
            else
                g.DrawString(param + " IS A DRAW...", font32, brushBlack, shiftX + 40, shiftY + 20);

            g.FillRectangle(teamBrush, camera.Width / 2 - controller.RestartRoundTime / 2, camera.Height - 12, controller.RestartRoundTime, 6);
        }

        private void paintRespawnBar(Graphics g)
        {
            g.FillRectangle(brushWhite, camera.Width / 2 - player.TimeToRespawn / 2, camera.Height - 12, player.TimeToRespawn, 6);
            //g.DrawRectangle(penBlackA, camera.Width / 2 - player.TimeToRespawn / 2, camera.Height - 12, player.TimeToRespawn, 6);
        }

        private void paintSolidObject(Graphics g, SolidObject so)
        {
            if(so.semiSolid == false)
                g.FillRectangle(new SolidBrush(Color.Black), so.box);
            else
                g.FillRectangle(new SolidBrush(Color.DarkGray), so.box);
        }

        /*
        private void paintTileMap(Graphics g, Tilemap tiles)
        {
            for (int i = 0; i < tiles.map.GetLength(0); i++)
                for (int j = 0; j < tiles.map.GetLength(1); j++)
                {
                    short index = tiles.map[i, j];
                    if (index >= 0)
                        g.DrawImage(tileset[index], i * tiles.tileWidth + tiles.shiftX, j * tiles.tileHeight + tiles.shiftY, tiles.tileWidth, tiles.tileHeight);
                }
        }
        */

        private void paintProjectiles(Graphics g, Projectile pr)
        {
            paintSprite(g, pr.getSprite, pr.face, 1);
        }

        private void paintBackground(Graphics g)
        {
            screen.BackColor = player.Dead ? backgroundDead.GetPixel(0, 0) : background.GetPixel(0, 0);

            int maxParallY = tilemap.map.GetLength(1) * 2;

            float scaleY = (camera.Height + maxParallY) / (float)background.Height;
            float scaledWidth = background.Width * scaleY;

            int parallY = maxParallY - (int)(tilemap.map.GetLength(1) * 16 - player.Y) / 8;
            int parallX = (int)player.X / 5;
            int repeatX = (int)(camera.Width / scaledWidth + 1);
            while (repeatX * scaledWidth - parallX < camera.Width)
                repeatX++;

            if (!player.Dead)
            {
                for (int i = 0; i < repeatX; i++)
                    g.DrawImage(background, (int)(i * (scaledWidth - 2) - parallX), -parallY, (int)scaledWidth, (int)(background.Height * scaleY));
            }
            else
            {
                for (int i = 0; i < repeatX; i++)
                    g.DrawImage(backgroundDead, (int)(i * (scaledWidth - 2) - parallX), -parallY, (int)scaledWidth, (int)(background.Height * scaleY));
            }
        }

        private void paintPlayer(Graphics g, Player p)
        {
            //taking player coords, without that they may change during drawing and pixel-gaps appear
            float pX = p.X;
            float pY = p.Y;

            paintSprite(g, p.getSprite(), p.face, 1, p.Dead ? 0.4f : 1);

            if (p.Name == "pifpaf")
            {
                drake.x = pX;
                drake.y = pY + 10;
                if (p.getSprite().spriteIndex != (int)SpriteIndex.war_Stand)
                    drake.x += 6 * p.face;
                drake.paint(g, p.face, 1);
            }
            if (p.Name == "SerGreen")
            {
                glasses.x = pX;
                glasses.y = pY + 10;
                if (p.getSprite().spriteIndex != (int)SpriteIndex.war_Stand)
                    glasses.x += 4 * p.face;
                glasses.paint(g, p.face, 1);
            }

            if (p.king)
            {
                int shiftX = p.face == 1 ? -17 : -13;
                crown.x = pX + p.Width * p.getSprite().Scale / 2 + shiftX;
                crown.y = pY + (p is Knight ? 6 : 0);
                paintSprite(g, crown, p.face, 1, p.Dead ? 0.4f : 1);
            }

            Brush brush = brushBlackA;
            if (p.Team == player.Team)
            {
                if (p == player)
                    brush = brushGreenA;
                else
                    brush = brushBlueA;
            }

            g.FillRectangle(brush, (int)pX + 15, (int)pY - 18, 75, 14);
            g.DrawString(p.Name, font10, brushWhite, (int)pX + 20, (int)pY - 19);

            float healthPercent = Math.Max(0, p.Health / (p.healthMax * (p.king ? 1.5f : 1.0f)));
            Brush healthColor = null;
            if (p.poison > 0)
            {
                int colorTone = rnd.Next(0, 150);
                healthColor = new SolidBrush(Color.FromArgb(colorTone, colorTone, colorTone));
            }
            else
                healthColor = new SolidBrush(Color.FromArgb((int)(Math.Min(255, 350 * (1 - healthPercent))), (int)Math.Min(255, 350 * healthPercent), 0));

            if (p == player)
            {
                g.FillRectangle(healthColor, (int)pX + 15, (int)pY - 4, 75 * healthPercent, 3);
                g.DrawRectangle(penBlackA, (int)pX + 15, (int)pY - 4, 75, 3);
            }
            else
            {
                if (p is Berserker && ((Berserker)p).InRage)
                {
                    int colorTone = rnd.Next(0, 255);
                    healthColor = new SolidBrush(Color.FromArgb(255, colorTone, colorTone));
                }

                //g.FillRectangle(healthColor, (int)pX + 40, (int)pY - 4, 30, 3);
                //g.DrawRectangle(penBlackA, (int)pX + 40, (int)pY - 4, 30, 3);

                g.FillEllipse(healthColor, (int)pX + 45, (int)pY - 4, 7, 7);
                g.DrawEllipse(penBlackA, (int)pX + 45, (int)pY - 4, 7, 7);
            }

            if (p == player)
            {
                if (player is Berserker)
                {
                    float chargePercent = (player.weaponCharge + (int)((Berserker)player).Rage) / (float)player.weaponChargeLimit;
                    float ragePercent = ((Berserker)player).Rage / (float)player.weaponChargeLimit;

                    Brush rageColor = new SolidBrush(Color.Red);
                    if (!((Berserker)player).InRage)
                        g.FillRectangle(brushBlue, (int)pX + 15, (int)pY - 1, 75 * chargePercent, 3);
                    else
                    {
                        int colorTone = rnd.Next(0, 255);
                        rageColor = new SolidBrush(Color.FromArgb(255, colorTone, colorTone));
                    }

                    g.FillRectangle(rageColor, (int)pX + 15, (int)pY - 1, 75 * ragePercent, 3);
                    g.DrawRectangle(penBlackA, (int)pX + 15, (int)pY - 1, 75, 3);
                }
                else
                {
                    float chargePercent = player.weaponCharge / (float)player.weaponChargeLimit;
                    g.FillRectangle(brushBlue, (int)pX + 15, (int)pY - 1, 75 * chargePercent, 3);
                    g.DrawRectangle(penBlackA, (int)pX + 15, (int)pY - 1, 75, 3);
                }


                if (player is Thief)
                {
                    for (int i = 0; i < ((Thief)player).knivesLeft; i++)
                    {
                        g.FillEllipse(brushBlue, (int)pX + 95, (int)pY - 4 + i * 5, 3, 3);
                        g.DrawEllipse(penBlackA, (int)pX + 95, (int)pY - 4 + i * 5, 3, 3);
                    }
                }
                else if (player is Bowman)
                {
                    for (int i = 0; i < ((Bowman)player).arrowsLeft; i++)
                    {
                        g.FillEllipse(brushBlue, (int)pX + 95, (int)pY - 4 + i * 5, 3, 3);
                        g.DrawEllipse(penBlackA, (int)pX + 95, (int)pY - 4 + i * 5, 3, 3);
                    }
                }
                else if (p is Warrior)
                {
                    float dashPercent = (int)((Warrior)player).Dash / (float)((Warrior)player).DashMax;
                    Brush dashBrush = dashPercent == 1 ? brushYellow : brushOrange;

                    g.FillRectangle(dashBrush, (int)pX + 92, (int)(pY + 1 - 20 * dashPercent), 3, 20 * dashPercent);
                    g.DrawRectangle(penBlackA, (int)pX + 92, (int)(pY - 19), 3, 20);
                }
                else if (p is Mage)
                {
                    float manaPercent = (int)((Mage)player).Mana/ (float)((Mage)player).ManaMax;

                    g.FillRectangle(brushCyan, (int)pX + 92, (int)(pY + 1 - 20 * manaPercent), 3, 20 * manaPercent);
                    g.DrawRectangle(penBlackA, (int)pX + 92, (int)(pY - 19), 3, 20);
                }
            }

            /*
            if(p == player)
                g.DrawImage(meArrow, pX + 46, pY - 25);
            else if (p.team == player.team)
                g.DrawImage(teammateArrow, pX + 46, pY - 25);
            */
        }

        private void paintSprite(Graphics g, Sprite s, float scaleX, float scaleY, float opacity)
        {
            Bitmap bmp = sprites[s.spriteIndex];

            Bitmap frame = bmp.Clone(new Rectangle(s.currentFrame * s.frameWidth, 0, s.frameWidth, bmp.Height), bmp.PixelFormat);
              
            int shiftX = 0;
            int shiftY = 0;
            if (scaleX < 0)
                shiftX = (int)(frame.Width * s.getScale() * scaleX);
            if (scaleY < 0)
                shiftY = (int)(frame.Height * s.getScale() * scaleY);

            g.DrawImage(opacity < 1 ? frame.SetOpacity(0.5f) : frame, s.x + s.getOrigin().X * scaleX * s.getScale() - shiftX, s.y + s.getOrigin().Y * scaleY * s.getScale() - shiftY, frame.Width * scaleX * s.getScale(), frame.Height * scaleY * s.getScale());
        }

        private void paintSprite(Graphics g, Sprite s, float scaleX, float scaleY)
        {
            paintSprite(g, s, scaleX, scaleY, 1);
        }

        private void paintScoreboard(Graphics g)
        {
            ArrayList players = controller.getPlayers();
            int teamsAmount = controller.getTeamsAmount();
            List<Player>[] playersInTeams = new List<Player>[teamsAmount];
            for (int i = 0; i < controller.getTeamsAmount(); i++)
                playersInTeams[i] = new List<Player>();

            int boxWidth = 200;
            int portX = (camera.Width - teamsAmount * boxWidth - (teamsAmount - 1) * 10) / 2; //12;
            int portY = 0;

            foreach (Player p in players)
                playersInTeams[p.Team].Add(p);

            for (int i = 0; i < teamsAmount; i++)
            {
                Brush teamColor = brushWhite;
                if (i == player.Team)
                    teamColor = brushGreen;

                int score = controller.getTeamScore(i);

                g.FillRectangle(brushBlackA, portX + i * (boxWidth + 10), portY, boxWidth, 32 + 16 * playersInTeams[i].Count);
                g.FillRectangle(brushBlackA, portX + i * (boxWidth + 10), portY, boxWidth, 24);
                g.DrawString("[T" + (i + 1) + "]:", font16, teamColor, portX + i * (boxWidth + 10), portY);
                g.DrawString(score.ToString(), font16, brushWhite, portX + i * (boxWidth + 10) + 50, portY);
                g.DrawString("K", font16, brushWhite, portX + i * (boxWidth + 10) + 110, portY);
                g.DrawString("A", font16, brushWhite, portX + i * (boxWidth + 10) + 140, portY);
                g.DrawString("D", font16, brushWhite, portX + i * (boxWidth + 10) + 170, portY);

                for (int j = 0; j < playersInTeams[i].Count; j++)
                {
                    Brush playerColor = brushWhite;
                    if (playersInTeams[i][j] == player)
                        playerColor = brushGreen;

                    g.DrawString(playersInTeams[i][j].Name, font14, playerColor, portX + i * (boxWidth + 10), portY + 24 + 16 * j);
                    g.DrawString(playersInTeams[i][j].Frags.ToString(), font14, playerColor, portX + i * (boxWidth + 10) + 110, portY + 24 + 16 * j);
                    g.DrawString(playersInTeams[i][j].Assists.ToString(), font14, playerColor, portX + i * (boxWidth + 10) + 140, portY + 24 + 16 * j);
                    g.DrawString(playersInTeams[i][j].Deaths.ToString(), font14, playerColor, portX + i * (boxWidth + 10) + 170, portY + 24 + 16 * j);
                }
            }
        }

        private void paintMenu(Graphics g)
        {
            g.FillRectangle(brushBlackA, 227, 0, 400, menuClassItems.Count * 64 + 64);
            g.FillRectangle(brushBlackA, 227, 0, 400, 64);
            g.DrawString("Select class:", font20, brushWhite, 300, 10);

            foreach (MenuItem mi in menuClassItems)
                mi.paint(g);

            int teamsAmount = controller.getTeamsAmount();
            g.FillRectangle(brushBlackA, camera.Width - 150, 0, 150, teamsAmount * 64 + 64);
            g.FillRectangle(brushBlackA, camera.Width - 150, 0, 150, 64);
            g.DrawString("Select team:", font20, brushWhite, camera.Width - 150, 10);

            for (int i = 0; i < teamsAmount; i++)
                menuTeamItems[i].paint(g);
        }

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (joined)
            {
                controller.kickPlayer(player.ID);
                joined = false;
            }
        }

        private void GameWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (joined)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        controller.doActionForPlayer(player.ID, "left", true);
                        break;

                    case Keys.Right:
                        controller.doActionForPlayer(player.ID, "right", true);
                        break;

                    case Keys.Down:
                        PressedKeys.Down = true;
                        controller.doActionForPlayer(player.ID, "down", true);
                        break;

                    case Keys.Up:
                        PressedKeys.Up = true;
                        break;

                    case Keys.Z:
                        if (PressedKeys.Down)
                            controller.doActionForPlayer(player.ID, "ZDown", true);
                        else
                            controller.doActionForPlayer(player.ID, "Z", true);
                        break;

                    case Keys.X:
                        if(PressedKeys.Up)
                            controller.doActionForPlayer(player.ID, "XUp", true);
                        else if(PressedKeys.Down)
                            controller.doActionForPlayer(player.ID, "XDown", true);
                        else 
                            controller.doActionForPlayer(player.ID, "X", true);
                        break;

                    case Keys.C:
                        controller.doActionForPlayer(player.ID, "C", true);
                        break;

                    case Keys.Tab:
                        showScoreboard = true;
                        break;

                    case Keys.Escape:
                        if (showMenu)
                            showMenu = false;
                        else
                            showMenu = true;
                        break;
                }
            }
        }

        private void GameWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (joined)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        controller.doActionForPlayer(player.ID, "left", false);
                        break;

                    case Keys.Right:
                        controller.doActionForPlayer(player.ID, "right", false);
                        break;

                    case Keys.Down:
                        PressedKeys.Down = false;
                        controller.doActionForPlayer(player.ID, "down", false);
                        break;

                    case Keys.Up:
                        PressedKeys.Up = false;
                        break;

                    case Keys.Z:
                        controller.doActionForPlayer(player.ID, "Z", false);
                        break;

                    case Keys.X:
                        controller.doActionForPlayer(player.ID, "X", false);
                        break;

                    case Keys.C:
                        controller.doActionForPlayer(player.ID, "C", false);
                        break;

                    case Keys.Tab:
                        showScoreboard = false;
                        break;
                }
            }
        }

        private void JoinBtn_Click(object sender, EventArgs e)
        {
            join();
        }

        private Player tryToJoin(string ip)
        {
            Player p = null;

            try
            {
                controller = (GameController)Activator.GetObject(typeof(GameController), "tcp://" + ip + ":31337/Controller");

                p = controller.addPlayer(name, teamBox.SelectedIndex, (string)classBox.SelectedItem);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nGOD DAMN IT!!!", "WRAAAAAAAGGGHHHH!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return p;
        }

        private void join()
        {
            if (nameTextBox.Text.Length > 0)
                name = nameTextBox.Text;

            if (ipTextBox.Text.Length > 0)
            {
                string ip = ipTextBox.Text;

                player = tryToJoin(ip);

                if (player != null)
                {
                    myPlayerID = player.ID;                     //order of these rows DAMMIT MATTERS!
                    loadedMapName = controller.getMapName();    //DO
                    loadTilemap(loadedMapName);                 //NOT
                    joined = true;                              //EVER
                    startGame();                                //SWAP THEM!!!!

                    saveLastIP(ip);
                    loginBox.Visible = false;
                    titleBackground.Visible = false;

                    screen.Visible = true;
                    Focus();
                }
            }
        }

        private void join_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                join();
            }
        }

        private void GameWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (showMenu)
            {
                float scale = screen.Height / (float)screen.Width < camera.Height / (float)camera.Width
                    ? screen.Height / (float)camera.Height
                    : screen.Width / (float)camera.Width;

                mouseLoc = new Point((int)(e.X / scale), (int)(e.Y / scale));
            }
        }

        private void screen_MouseClick(object sender, MouseEventArgs e)
        {
            if (showMenu)
            {
                string seleceted = null;

                foreach (MenuItem mi in menuClassItems)
                    if (mi.hovered)
                    {
                        seleceted = mi.Label;
                        break;
                    }

                if (seleceted != null)
                {
                    player.classAfterRespawn = seleceted;
                    showMenu = false;
                }
                else
                {
                    int index = -1;
                    for (int i = 0; i < controller.getTeamsAmount();i++ )
                        if (menuTeamItems[i].hovered)
                        {
                            index = i;
                            break;
                        }

                    if (index != -1)
                    {
                        player.teamAfterRespawn = index;
                        showMenu = false;
                    }
                }
            }
        }
    }
}
