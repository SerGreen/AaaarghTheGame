using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MultiplayerLib;
using System.IO;
using MultiplayerLib.FunctionalObjects;
using System.Reflection;

namespace LevelEditor
{
    public partial class EditorForm : Form
    {
        Tilemap tiles;
        short currentTile;    //tiles
        int currentTool;    //0=solidObject, 1=tile, 2=semiSolid, 3=spawnTool, 4=harmfulObject, 5=dominationPlate, 6=flagTool, 7=harvesterTool
        bool mousePressed = false;
        Point startP;
        Point endP;
        Point shift;
        int totalMapsAmount;
        Bitmap background;

        List<SolidObject> solidObjects;
        List<HarmfulObject> harmfulObjects;
        List<DominationPlate> dominationPlates;
        List<FlagPlace> flagPlaces;
        //FlagPlace[] flagPlaces;
        StatueHarvester statue;
        Point[] spawnPoints;
        short currentTeam;
        short currentFlagTeam;

        public EditorForm()
        {
            InitializeComponent();
            totalMapsAmount = scanMaps();
            scanBackgrounds();
            loadBackground(backgroundBox.SelectedIndex);

            //tiles = new Tilemap(120, 51, 16, Properties.Resources.tileset);
            currentTile = 0;
            currentTool = 0;
            currentTeam = 0;
            currentFlagTeam = 0;
            tiles = null;
            solidObjects = null;
            harmfulObjects = null;
            dominationPlates = null;
            flagPlaces = null;
            statue = null;
            //solidObjects = new List<SolidObject>();
            //harmfulObjects = new List<HarmfulObject>();
            //dominationPlates = new List<DominationPlate>();
            //flagPlaces = new FlagPlace[] { new FlagPlace(48, 48, 0), new FlagPlace(48, 48, 1), new FlagPlace(48, 48, 2), new FlagPlace(48, 48, 3) };
            //flagPlaces = new List<FlagPlace>();
            //statue = new StatueHarvester(32, 48);
            spawnPoints = new Point[] { new Point(32, 32), new Point(32, 32), new Point(32, 32), new Point(32, 32) };
            shift = new Point(0, 0);
            
            loadMap(1);
        }

        private void EditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                    currentTool = 0;
                    Text = "Editor : SolidObject tool";
                    break;

                case Keys.NumPad2:
                    currentTool = 2;
                    Text = "Editor : SemiSolidObject tool";
                    break;

                case Keys.NumPad3:
                    currentTool = 4;
                    Text = "Editor : HarmfulObject tool";
                    break;

                case Keys.NumPad0:
                    currentTool = 5;
                    Text = "Editor : DominationPlate tool";
                    break;

                case Keys.R:
                    currentTool = 7;
                    Text = "Editor : HarvesterStatue tool";
                    break;

                case Keys.NumPad6:
                    currentTool = 1;
                    Text = "Editor : Tile tool : [" + currentTile.ToString() + "]";
                    break;

                case Keys.NumPad4:
                    if (currentTool == 1 && currentTile > -1)
                    {
                        currentTile--;
                        Text = "Editor : Tile tool : [" + currentTile.ToString() + "]";
                    }
                    break;

                case Keys.NumPad5:
                    if (currentTool == 1 && currentTile < tiles.tileset.Length - 1)
                    {
                        currentTile++;
                        Text = "Editor : Tile tool : [" + currentTile.ToString() + "]";
                    }
                    break;

                case Keys.NumPad9:
                    currentTool = 3;
                    Text = "Editor : TeamSpawn tool : [T" + (currentTeam + 1).ToString() + "]";
                    break;

                case Keys.NumPad7:
                    if (currentTool == 3 && currentTeam > 0)
                    {
                        currentTeam--;
                        Text = "Editor : TeamSpawn tool : [T" + (currentTeam + 1).ToString() + "]";
                    }
                    break;

                case Keys.NumPad8:
                    if (currentTool == 3 && currentTeam < 3)
                    {
                        currentTeam++;
                        Text = "Editor : TeamSpawn tool : [T" + (currentTeam + 1).ToString() + "]";
                    }
                    break;

                case Keys.E:
                    currentTool = 6;
                    Text = "Editor : Flag tool : [T" + (currentFlagTeam + 1).ToString() + "]";
                    break;

                case Keys.Q:
                    if (currentTool == 6 && currentFlagTeam > 0)
                    {
                        currentFlagTeam--;
                        Text = "Editor : Flag tool : [T" + (currentFlagTeam + 1).ToString() + "]";
                    }
                    break;

                case Keys.W:
                    if (currentTool == 6 && currentFlagTeam < 3)
                    {
                        currentFlagTeam++;
                        Text = "Editor : Flag tool : [T" + (currentFlagTeam + 1).ToString() + "]";
                    }
                    break;

                case Keys.Enter:
                    if (e.Shift)
                        loadMap(mapListBox.SelectedIndex);
                    else
                        writeMap(mapListBox.SelectedIndex);
                    break;
            }
        }

        private void screen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePressed = true;

                int mouseX = e.X - shift.X;
                int mouseY = e.Y - shift.Y;

                int x = mouseX / 16 * 16;
                int y = mouseY / 16 * 16;
                startP = new Point(x, y);

            }
        }

        private void screen_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePressed = false;

                int mouseX = e.X - shift.X;
                int mouseY = e.Y - shift.Y;

                if (currentTool == 0)
                {
                    int x = mouseX / 16 * 16 + 16;
                    int y = mouseY / 16 * 16 + 16;
                    endP = new Point(x, y);

                    solidObjects.Add(new SolidObject(startP.X, startP.Y, endP.X - startP.X, endP.Y - startP.Y, false));
                }
                else if (currentTool == 2)
                {
                    int x = mouseX / 16 * 16 + 16;
                    int y = mouseY / 16 * 16 + 16;
                    endP = new Point(x, y);

                    Point sP = new Point(Math.Min(startP.X, endP.X), Math.Min(startP.Y, endP.Y));
                    Point eP = new Point(Math.Max(startP.X, endP.X), Math.Max(startP.Y, endP.Y));

                    solidObjects.Add(new SolidObject(sP.X, sP.Y, eP.X - sP.X, eP.Y - sP.Y, true));
                }
                else if (currentTool == 4)
                {
                    int x = mouseX / 16 * 16 + 16;
                    int y = mouseY / 16 * 16 + 16;
                    endP = new Point(x, y);

                    Point sP = new Point(Math.Min(startP.X, endP.X), Math.Min(startP.Y, endP.Y));
                    Point eP = new Point(Math.Max(startP.X, endP.X), Math.Max(startP.Y, endP.Y));

                    harmfulObjects.Add(new HarmfulObject(sP.X, sP.Y, eP.X - sP.X, eP.Y - sP.Y, 10));
                }
                else if (currentTool == 1)
                {
                    int x = mouseX / 16 * 16 + 16;
                    int y = mouseY / 16 * 16 + 16;
                    endP = new Point(x, y);

                    Point sP = new Point(Math.Min(startP.X, endP.X), Math.Min(startP.Y, endP.Y));
                    Point eP = new Point(Math.Max(startP.X, endP.X), Math.Max(startP.Y, endP.Y));

                    for (int i = sP.X / 16; i < eP.X / 16; i++)
                    {
                        for (int j = sP.Y / 16; j < eP.Y / 16; j++)
                        {
                            tiles.setTile(i, j, currentTile);
                        }
                    }
                }
                else if (currentTool == 3)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    spawnPoints[currentTeam] = new Point(x, y);
                }
                else if (currentTool == 7)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    statue = new StatueHarvester(x, y);
                }
                else if (currentTool == 5)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    dominationPlates.Add(new DominationPlate(x, y, 4));
                }
                else if (currentTool == 6)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    //flagPlaces[currentFlagTeam] = new FlagPlace(x, y, currentFlagTeam);
                    flagPlaces.Add(new FlagPlace(x, y, currentFlagTeam));
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                int mouseX = e.X - shift.X;
                int mouseY = e.Y - shift.Y;

                if (currentTool == 0 || currentTool == 2)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    for (int i = solidObjects.Count - 1; i >= 0; i--)
                    {
                        if (x >= solidObjects[i].box.X && x < solidObjects[i].box.X + solidObjects[i].box.Width &&
                            y >= solidObjects[i].box.Y && y < solidObjects[i].box.Y + solidObjects[i].box.Height)
                        {
                            solidObjects.RemoveAt(i);
                        }
                    }
                }
                else if (currentTool == 4)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    for (int i = harmfulObjects.Count - 1; i >= 0; i--)
                    {
                        if (x >= harmfulObjects[i].box.X && x < harmfulObjects[i].box.X + harmfulObjects[i].box.Width &&
                            y >= harmfulObjects[i].box.Y && y < harmfulObjects[i].box.Y + harmfulObjects[i].box.Height)
                        {
                            harmfulObjects.RemoveAt(i);
                        }
                    }
                }
                else if (currentTool == 1)
                {
                    int x = mouseX / 16;
                    int y = mouseY / 16;

                    tiles.setTile(x, y, -1);
                }
                else if (currentTool == 5)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    for (int i = dominationPlates.Count - 1; i >= 0; i--)
                        if (dominationPlates[i].getSprite.checkCollision(new Rectangle(x, y, 16, 16)))
                            dominationPlates.RemoveAt(i);
                }
                else if (currentTool == 6)
                {
                    int x = mouseX / 16 * 16;
                    int y = mouseY / 16 * 16;

                    for (int i = flagPlaces.Count - 1; i >= 0; i--)
                        if (flagPlaces[i].GetSprite.checkCollision(new Rectangle(x, y, 16, 16)))
                            flagPlaces.RemoveAt(i);
                }
            }
        }

        private void renderTimer(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(screen.Width, screen.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TranslateTransform(shift.X, shift.Y);

            paintBackground(g);

            foreach (SolidObject so in solidObjects)
            {
                so.paint(g);
            }

            foreach (HarmfulObject ho in harmfulObjects)
            {
                ho.paint(g);
            }

            tiles.paint(g);

            if(statue != null)
                g.DrawRectangle(new Pen(Color.Yellow), statue.getSprite.x, statue.getSprite.y, 32, 55);

            foreach (DominationPlate dp in dominationPlates)
                g.DrawRectangle(new Pen(Color.DarkViolet), dp.getSprite.x, dp.getSprite.y, 64, 16);

            //paint spawn points
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Color tColor = Color.Blue;
                if (i == 1)
                    tColor = Color.Violet;
                else if (i == 2)
                    tColor = Color.Cyan;
                else if (i == 3)
                    tColor = Color.Green;

                g.DrawRectangle(new Pen(tColor), new Rectangle(spawnPoints[i].X, spawnPoints[i].Y, 16, 16));
            }

            //paint flag places
            for (int i = 0; i < flagPlaces.Count; i++)
            {
                Color tColor = Color.Blue;
                if (flagPlaces[i].team == 1)
                    tColor = Color.Violet;
                else if (flagPlaces[i].team == 2)
                    tColor = Color.Cyan;
                else if (flagPlaces[i].team == 3)
                    tColor = Color.Green;

                g.DrawRectangle(new Pen(tColor), new Rectangle((int)flagPlaces[i].GetSprite.x, (int)flagPlaces[i].GetSprite.y, 16, 16));
                g.DrawString("F", new Font("calibri", 16), new SolidBrush(tColor), flagPlaces[i].GetSprite.x, flagPlaces[i].GetSprite.y);
            }

            if (mousePressed)
            {
                g.DrawRectangle(new Pen(Color.Red), new Rectangle(startP.X, startP.Y, endP.X - startP.X, endP.Y - startP.Y));
            }
            else
            {
                g.DrawRectangle(new Pen(Color.OrangeRed), new Rectangle(endP.X, endP.Y, 16, 16));
            }

            if (currentTool == 1 && currentTile >= 0)
            {
                g.DrawImage(tiles.tileset[currentTile], 0, 0, tiles.tileWidth, tiles.tileHeight);
            }

            g.DrawRectangle(new Pen(Color.Lime), 0, 0, tiles.map.GetLength(0) * tiles.tileWidth, tiles.map.GetLength(1) * tiles.tileHeight);

            g.Dispose();
            screen.Image = bmp;
        }

        private void screen_MouseMove(object sender, MouseEventArgs e)
        {
            int mouseX = e.X - shift.X;
            int mouseY = e.Y - shift.Y;

            int x = mouseX / 16 * 16;
            int y = mouseY / 16 * 16;

            if (mousePressed)
            {
                x += 16;
                y += 16;
            }

            endP = new Point(x, y);
        }

        /* All map data separated in two files:
         * First file is for SERVER: contains data about spawn points and all solid and semiSolid objects
         * Second file is for CLIENT: contains data about tile map - it's width, height and tilemap itself
         */
        private void writeMap(int index)
        {
            if (File.Exists("./map_" + index + ".map"))
                File.Delete("./map_" + index + ".map");

            if (File.Exists("./tiles_" + index + ".map"))
                File.Delete("./tiles_" + index + ".map");

            StreamWriter sr = null;

            //preparing data for server file
            string file = "";

            //Write spawn points in format 'X:Y', each team in new line
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (i > 0)
                    file = file + "\n";

                file = file + spawnPoints[i].X + ":" + spawnPoints[i].Y;
            }
            file = file + "#";  //separate spawn info from other map with '#'

            //Write all Solid objects in format 'X:Y:Width:Heaight:isSemiSolid', each object in new line
            for (int i = 0; i < solidObjects.Count; i++)
            {
                if (i > 0)
                    file = file + "\n";

                file = file + solidObjects[i].box.X + ":" + solidObjects[i].box.Y + ":" + solidObjects[i].box.Width + ":" + solidObjects[i].box.Height + ":" + solidObjects[i].semiSolid;
            }

            //write harmful objects
            file = file + "#";
            for (int i = 0; i < harmfulObjects.Count; i++)
            {
                if (i > 0)
                    file = file + "\n";

                file = file + harmfulObjects[i].box.X + ":" + harmfulObjects[i].box.Y + ":" + harmfulObjects[i].box.Width + ":" + harmfulObjects[i].box.Height + ":" + harmfulObjects[i].Damage;
            }

            //write domination plates
            file = file + "#";
            for (int i = 0; i < dominationPlates.Count; i++)
            {
                if (i > 0)
                    file = file + "\n";

                file = file + dominationPlates[i].getSprite.x + ":" + dominationPlates[i].getSprite.y;
            }

            //write flag places
            file = file + "#";
            for (int i = 0; i < flagPlaces.Count; i++)
            {
                if (i > 0)
                    file = file + "\n";

                file = file + flagPlaces[i].GetSprite.x + ":" + flagPlaces[i].GetSprite.y + ":" + flagPlaces[i].team;
            }

            //write harvester statue
            file = file + "#";
            file = file + statue.getSprite.x + ":" + statue.getSprite.y;
            
            //save first file, for server
            sr = new StreamWriter("./map_" + index + ".map");
            sr.Write(file);
            sr.Close();


            //prepare data for client file
            file = "";

            //info about height and width of tilemap (in tiles count) in format 'Width:Height', separated from other info with '#'
            file = file + tiles.map.GetLength(0) + ":" + tiles.map.GetLength(1) + "#";  

            //write tilemap data in format each line is tiles row, each tile data in a row separated with space
            for (int j = 0; j < tiles.map.GetLength(1); j++)
            {
                if(j > 0)
                    file = file + "\n";

                for (int i = 0; i < tiles.map.GetLength(0); i++)
                {
                    if (i > 0)
                        file = file + " ";
                    file = file + tiles.map[i, j];
                }
            }

            file = file + "#" + (backgroundBox.SelectedIndex - 1);

            sr = new StreamWriter("./map_" + index + ".tiles");
            sr.Write(file);
            sr.Close();
        }

        private void loadMap(int index)
        {
            StreamReader sr = null;
            string file = null;

            if (File.Exists("./map_" + index + ".map"))
            {
                //load server data
                sr = new StreamReader("./map_" + index + ".map");
                file = sr.ReadToEnd();
                sr.Close();

                string[] mapParts = file.Split('#');

                //load spawn points
                string[] spawnData = mapParts[0].Split('\n');
                for (int i = 0; i < spawnData.Length; i++)
                {
                    string[] spwnP = spawnData[i].Split(':');
                    int x = int.Parse(spwnP[0]);
                    int y = int.Parse(spwnP[1]);

                    spawnPoints[i] = new Point(x, y);
                }

                //load solid objects
                solidObjects = new List<SolidObject>();
                string[] solidObjs = mapParts[1].Split('\n');
                for (int i = 0; i < solidObjs.Length; i++)
                {
                    string[] obj = solidObjs[i].Split(':');
                    int x = int.Parse(obj[0]);
                    int y = int.Parse(obj[1]);
                    int w = int.Parse(obj[2]);
                    int h = int.Parse(obj[3]);
                    bool ss = bool.Parse(obj[4]);

                    solidObjects.Add(new SolidObject(x, y, w, h, ss));
                }

                //load harmful objects
                harmfulObjects = new List<HarmfulObject>();
                if (mapParts.Length > 2 && mapParts[2].Length > 0)
                {
                    string[] harmObjs = mapParts[2].Split('\n');
                    for (int i = 0; i < harmObjs.Length; i++)
                    {
                        string[] obj = harmObjs[i].Split(':');
                        int x = int.Parse(obj[0]);
                        int y = int.Parse(obj[1]);
                        int w = int.Parse(obj[2]);
                        int h = int.Parse(obj[3]);
                        int d = int.Parse(obj[4]);

                        harmfulObjects.Add(new HarmfulObject(x, y, w, h, d));
                    }
                }

                //load domination plates
                dominationPlates = new List<DominationPlate>();
                if (mapParts.Length > 3 && mapParts[3].Length > 0)
                {
                    string[] domPlates = mapParts[3].Split('\n');
                    for (int i = 0; i < domPlates.Length; i++)
                    {
                        string[] obj = domPlates[i].Split(':');
                        int x = int.Parse(obj[0]);
                        int y = int.Parse(obj[1]);

                        dominationPlates.Add(new DominationPlate(x, y, 4));
                    }
                }

                //load flag places
                flagPlaces = new List<FlagPlace>();
                if (mapParts.Length > 4 && mapParts[4].Length > 0)
                {
                    string[] flgPlces = mapParts[4].Split('\n');
                    for (int i = 0; i < flgPlces.Length; i++)
                    {
                        string[] obj = flgPlces[i].Split(':');
                        int x = int.Parse(obj[0]);
                        int y = int.Parse(obj[1]);
                        int team = int.Parse(obj[2]);

                        flagPlaces.Add(new FlagPlace(x, y, team));
                    }
                }

                //load harvester statue
                if (mapParts.Length > 5 && mapParts[5].Length > 0)
                {
                    string[] stat = mapParts[5].Split(':');
                    int x = int.Parse(stat[0]);
                    int y = int.Parse(stat[1]);

                    statue = new StatueHarvester(x, y);
                }
            }

            if (File.Exists("./map_" + index + ".tiles"))
            {
                //load client data
                sr = new StreamReader("./map_" + index + ".tiles");
                file = sr.ReadToEnd();
                sr.Close();

                string[] tileDat = file.Split('#');

                //load tilemap width and height
                int tileX = int.Parse(tileDat[0].Split(':')[0]);
                int tileY = int.Parse(tileDat[0].Split(':')[1]);

                numericWidth.Value = tileX;
                numericHeight.Value= tileY;

                //load tiles data
                tiles = new Tilemap(tileX, tileY, 16, Properties.Resources.tileset);
                string[] tileRows = tileDat[1].Split('\n');
                for (int j = 0; j < tileRows.Length; j++)
                {
                    string[] tileCols = tileRows[j].Split(' ');
                    for (int i = 0; i < tileCols.Length; i++)
                    {
                        tiles.setTile(i, j, short.Parse(tileCols[i]));
                    }
                }

                if (tileDat.Length > 2 && tileDat[2].Length > 0)
                {
                    int bckgrNumber = int.Parse(tileDat[2]);
                    backgroundBox.SelectedIndex = bckgrNumber + 1;
                }
            }
        }

        private void loadBackground(int index)
        {
            if (index == 0)
                background = new Bitmap(1, 1);
            else
            {
                background = (Bitmap)Bitmap.FromFile("./res/background_" + (index - 1) + ".png");
            }
        }

        private void paintBackground(Graphics g)
        {
            screen.BackColor = background.GetPixel(0, 0);

            int maxParallY = tiles.map.GetLength(1) * 2;

            float scaleY = (tiles.map.GetLength(1) * 16) / (float)background.Height;
            float scaledWidth = background.Width * scaleY;
            int repeatX = (int)((tiles.map.GetLength(0) * 16) / scaledWidth + 1);

            while (repeatX * scaledWidth < (tiles.map.GetLength(0) * 16))
                repeatX++;

            for (int i = 0; i < repeatX; i++)
                g.DrawImage(background, (int)(i * (scaledWidth - 2)), 0, (int)scaledWidth, (int)(background.Height * scaleY));
        }

        private int scanMaps()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int currentMapToCheck = 1;
            mapListBox.Items.Clear();

            while (File.Exists(assemblyFolder + "/map_" + currentMapToCheck + ".map") && File.Exists(assemblyFolder + "/map_" + currentMapToCheck + ".tiles"))
            {
                mapListBox.Items.Add("map_" + currentMapToCheck);
                currentMapToCheck++;
            }

            mapListBox.SelectedIndex = 0;

            return currentMapToCheck - 1;
        }

        private int scanBackgrounds()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int currentBckgrToCheck = 0;
            backgroundBox.Items.Clear();

            backgroundBox.Items.Add("no background");

            while (File.Exists(assemblyFolder + "/res/background_" + currentBckgrToCheck + ".png"))
            {
                backgroundBox.Items.Add("background_" + currentBckgrToCheck);
                currentBckgrToCheck++;
            }

            backgroundBox.SelectedIndex = 0;

            return currentBckgrToCheck;
        }

        private void left_Click(object sender, EventArgs e)
        {
            shift.X += 32;
        }

        private void right_Click(object sender, EventArgs e)
        {
            shift.X -= 32;
        }

        private void up_Click(object sender, EventArgs e)
        {
            shift.Y += 32;
        }

        private void down_Click(object sender, EventArgs e)
        {
            shift.Y -= 32;
        }

        private void newMap_Click(object sender, EventArgs e)
        {
            tiles = new Tilemap((int)numericWidth.Value, (int)numericHeight.Value, 16, Properties.Resources.tileset);
            solidObjects = new List<SolidObject>();
            harmfulObjects = new List<HarmfulObject>();
            dominationPlates = new List<DominationPlate>();
            flagPlaces = new List<FlagPlace>();
            //flagPlaces = new FlagPlace[] { new FlagPlace(48, 48, 0), new FlagPlace(48, 48, 1), new FlagPlace(48, 48, 2), new FlagPlace(48, 48, 3) };
            statue = new StatueHarvester(32, 48);
            spawnPoints = new Point[] { new Point(32, 32), new Point(32, 32), new Point(32, 32), new Point(32, 32) };

            writeMap(totalMapsAmount + 1);
            totalMapsAmount = scanMaps();
            mapListBox.SelectedIndex = totalMapsAmount - 1;
        }

        private void save_Click(object sender, EventArgs e)
        {
            writeMap(mapListBox.SelectedIndex + 1);
        }

        private void load_Click(object sender, EventArgs e)
        {
            loadMap(mapListBox.SelectedIndex + 1);
        }

        private void toolsFocus_KeyDown(object sender, KeyEventArgs e)
        {
            EditorForm_KeyDown(sender, e);
        }

        private void fixBridge_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();

            for (int j = 0; j < tiles.map.GetLength(1); j++)
            {
                for (int i = 0; i < tiles.map.GetLength(0); i++)
                {
                    if (tiles.map[i, j] == 16)
                        tiles.setTile(i, j, (short)rnd.Next(16, 19));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (SolidObject so in solidObjects)
                so.box = new Rectangle(so.box.X, so.box.Y + 16, so.box.Width, so.box.Height);

            foreach (HarmfulObject ho in harmfulObjects)
                ho.box = new Rectangle(ho.box.X, ho.box.Y + 16, ho.box.Width, ho.box.Height);

            foreach (DominationPlate dp in dominationPlates)
                dp.getSprite.y += 16;

            foreach (FlagPlace fp in flagPlaces)
                fp.GetSprite.y += 16;

            statue.getSprite.y += 16;

            for (int i = 0; i < spawnPoints.Length; i++)
                spawnPoints[i] = new Point(spawnPoints[i].X, spawnPoints[i].Y + 16);
        }

        private void backgroundBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadBackground(backgroundBox.SelectedIndex);
        }
    }
}
