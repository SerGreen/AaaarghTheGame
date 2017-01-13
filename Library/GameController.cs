using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using MultiplayerLib;
using MultiplayerLib.GameClasses;
using MultiplayerLib.Projectiles;
using MultiplayerLib.Particles;
using MultiplayerLib.FunctionalObjects;

namespace MultiplayerLib
{
    public enum GameMode { DeathMatch, Domination, CaptureFlag, ProtectKing, Harvester }

    public class GameController : MarshalByRefObject
    {
        int playerLimit { get; set; }

        ArrayList players;
        ArrayList solidObjects;
        ArrayList harmfulObjects;
        ArrayList projectiles;
        ArrayList particles;
        ArrayList domminationPlates;
        ArrayList flagPlaces;
        ArrayList skulls;
        ArrayList tombstones;
        StatueHarvester statue;
        string mapName;
        //Tilemap tiles;
        int playersConnected = 0;
        Thread thread;
        bool running;
        const int FPS = 45;
        int frameTime;
        List<Point> spawnPoints;
        int respawnTime;
        int restartTime;
        int teamsAmount;

        int[] teamScore;
        int[] teamPoints;
        int[] teamSpawnsLeft;
        int timeLimit;
        int timeLeft;
        int pointsLimit;
        int spawnsLimit;
        int winnerTeam;
        bool roundOver;
        bool gameOver;
        GameMode gameMode;

        public GameController()
        {
            frameTime = 1000 / FPS;
            respawnTime = 0;
            teamsAmount = 4;
            playerLimit = 8;
            players = new ArrayList();
            harmfulObjects = new ArrayList();
            projectiles = new ArrayList();
            particles = new ArrayList();
            domminationPlates = new ArrayList();
            flagPlaces = new ArrayList();
            skulls = new ArrayList();
            tombstones = new ArrayList();
            
            gameMode = GameMode.DeathMatch;
            mapName = "map_1";
            restartTime = -1;
            roundOver = false;
            gameOver = false;
            winnerTeam = -1;
            timeLimit = -1;
            timeLeft = -1;
            pointsLimit = -1;
            spawnsLimit = -1;

            teamScore = new int[teamsAmount];
            teamPoints = new int[teamsAmount];
            teamSpawnsLeft = new int[teamsAmount];

            loadMap(mapName);

            thread = new Thread(new ThreadStart(serverLoop));
            thread.Start();
        }

        public void setRespawnTime(int time)
        {
            if (time >= 0)
            {
                if (respawnTime < 0)
                {
                    restartTime = -1;
                    roundOver = false;
                }
                respawnTime = time;
            }
            else
                respawnTime = -1;
        }

        public void setTimeLimit(int limit)
        {
            timeLimit = limit;
            timeLeft = timeLimit;
        }

        public void setPointsLimit(int limit)
        { pointsLimit = limit; }

        public void setSpawnsLimit(int limit)
        {
            spawnsLimit = limit;
            for (int i = 0; i < teamsAmount; i++)
                teamSpawnsLeft[i] = spawnsLimit;
        }

        public void setGameMode(GameMode gm)
        { gameMode = gm; }

        public void setTeamsAmount(int amount)
        {
            if (teamsAmount != amount)
            {
                teamsAmount = amount;
                resetPlayersByTeams();

                teamScore = new int[teamsAmount];
            }

            for (int i = 0; i < domminationPlates.Count; i++)
                ((DominationPlate)domminationPlates[i]).updateTeamsAmount(teamsAmount);
        }

        private void resetPlayersByTeams()
        {
            if (teamsAmount == 4)
            {
                Random rnd = new Random();
                for (int i = 0; i < players.Count; i++)
                {
                    ((Player)players[i]).teamAfterRespawn = rnd.Next(0, 4);
                }
            }
            else if (teamsAmount == 2)
            {
                Random rnd = new Random();
                for (int i = 0; i < players.Count; i++)
                {
                    if (((Player)players[i]).Team > 1)
                        ((Player)players[i]).teamAfterRespawn = rnd.Next(0, 2);
                }
            }
        }

        public void changePlayerTeam(int ID, int team)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (((Player)players[i]).ID == ID)
                {
                    ((Player)players[i]).Team = team;
                    ((Player)players[i]).teamAfterRespawn = team;
                    return;
                }
            }
        }

        private void serverLoop()
        {
            running = true;
            Stopwatch timer = new Stopwatch();

            while (running)
            {
                timer.Reset();
                timer.Start();
                //***********************

                tick();

                //***********************
                timer.Stop();
                int timeToSleep = frameTime - (int)timer.ElapsedMilliseconds;
                if (timeToSleep > 0)
                    Thread.Sleep(timeToSleep);
            }
        }

        private void tick()
        {
            tickPlayers();
            tickProjectiles();
            tickParticles();
            tickSkulls();
            tickTombstones();

            if (!roundOver && !gameOver)
            {
                if (gameMode == GameMode.Domination)
                    tickDominationPlates();
                else if (gameMode == GameMode.CaptureFlag)
                    tickFlags();
                else if (gameMode == GameMode.Harvester)
                    tickHarvesterLogic();
            }

            if (timeLimit > 0)
                if (timeLeft > 0)
                    timeLeft--;

            if (restartTime < 0)
                restartTick();
            else
            {
                restartTime--;

                if (restartTime == 0)
                {
                    if (roundOver)
                    {
                        restartRound();
                    }
                    else if (gameOver)
                    {
                        restart(mapName);
                    }
                }
            }

        }

        private void restartRound()
        {
            restartTime = -1;
            roundOver = false;
            winnerTeam = -1;
            for (int i = 0; i < players.Count; i++)
                respawnPlayer(((Player)players[i]).ID);

            tombstones = new ArrayList();

            if (gameMode == GameMode.ProtectKing)
                spawnKings();
        }

        private void tickHarvesterLogic()
        {
            for (int i = flagPlaces.Count - 1; i >= 0; i--)
            {
                FlagPlace fp = (FlagPlace)flagPlaces[i];
                if (getPlayersInTeam(fp.team) > 0)
                {
                    for (int j = 0; j < players.Count; j++)
                    {
                        Player p = (Player)players[j];
                        if (!p.Dead)
                        {
                            if (p.Team != fp.team && p.getSprite().checkCollision(fp.GetSprite))
                            {
                                int points = p.removeMySkulls(fp.team);
                                teamPoints[p.Team] += points;
                            }
                        }
                    }
                }
            }
        }

        private void tickSkulls()
        {
            for (int i = skulls.Count - 1; i >= 0; i--)
            {
                Skull sk = (Skull)skulls[i];
                sk.tick(solidObjects, ref particles);
                
                foreach (Player p in players)
                {
                    if (p.getSprite().checkCollision(sk.GetSprite))
                    {
                        if (!sk.captured && !p.Dead)
                        {
                            if (sk.team == p.Team)
                            {
                                sk.TimeToDisappear = true;
                            }
                            else
                            {
                                p.addSkull(sk);
                                sk.captured = true;
                                sk.disappearTimeout = -1;
                            }
                        }
                    }
                }

                if (sk.TimeToDisappear)
                    skulls.RemoveAt(i);
            }
        }

        private void tickTombstones()
        {
            for (int i = tombstones.Count - 1; i >= 0; i--)
            {
                Tombstone ts = (Tombstone)tombstones[i];
                ts.tick(solidObjects, ref particles);

                if (ts.TimeToDisappear)
                {
                    particles.Add(new TombstoneCrush(ts.X, ts.Y));
                    tombstones.RemoveAt(i);
                }
            }
        }

        private void restartTick()
        {
            if (pointsLimit > 0)
            {
                for (int i = 0; i < teamPoints.Length; i++)
                    if (teamPoints[i] >= pointsLimit)
                    {
                        restartTime = 300;
                        winnerTeam = i;
                        gameOver = true;
                    }
            }

            if (timeLimit > 0)
            {
                if (timeLeft <= 0)
                {
                    bool draw = false;
                    int bestTeam = 0;

                    if (pointsLimit > 0)
                    {
                        for (int i = 1; i < teamsAmount; i++)
                        {
                            if (teamPoints[i] == teamPoints[bestTeam])
                                draw = true;
                            else if (teamPoints[i] > teamPoints[bestTeam])
                            {
                                bestTeam = i;
                                draw = false;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < teamsAmount; i++)
                        {
                            if (teamScore[i] == teamScore[bestTeam])
                                draw = true;
                            else if (teamScore[i] > teamScore[bestTeam])
                            {
                                bestTeam = i;
                                draw = false;
                            }
                        }
                    }


                    if (draw)
                        winnerTeam = -1;
                    else
                        winnerTeam = bestTeam;

                    restartTime = 300;
                    gameOver = true;
                }
            }

            if (respawnTime < 0 || spawnsLimit > 0)
            {
                int[] aliveInTeams = new int[teamsAmount];
                foreach (Player pl in players)
                    if (!pl.Dead)
                        aliveInTeams[pl.Team]++;

                int teamsAlive = 0;
                int aliveTeamID = -1;
                for (int i = 0; i < aliveInTeams.Length; i++)
                    if (aliveInTeams[i] > 0)
                    {
                        aliveTeamID = i;
                        teamsAlive++;
                    }

                if (teamsAlive <= 1)
                {
                    if (spawnsLimit > 0)
                    {
                        bool teamsHaveNoSpawns = true;
                        for (int i = 0; i < teamsAmount; i++)
                            if (i != aliveTeamID && teamSpawnsLeft[i] > 0 && getPlayersInTeam(i) > 0)
                            {
                                teamsHaveNoSpawns = false;
                                break;
                            }

                        if (teamsHaveNoSpawns)
                        {
                            restartTime = 300;
                            winnerTeam = aliveTeamID;
                            gameOver = true;
                        }
                    }

                    if (respawnTime < 0 && !gameOver)
                    {
                        restartTime = 150;
                        winnerTeam = aliveTeamID;
                        roundOver = true;
                    }
                }
            }

            if (gameMode == GameMode.ProtectKing)
            {
                bool[] aliveKings = new bool[teamsAmount];
                foreach (Player pl in players)
                    if (pl.king && !pl.Dead)
                        aliveKings[pl.Team] = true;

                int kingsAlive = 0;
                int aliveKingTeamID = -1;
                for (int i = 0; i < aliveKings.Length; i++)
                    if (aliveKings[i] == true)
                    {
                        aliveKingTeamID = i;
                        kingsAlive++;
                    }

                if (kingsAlive <= 1 && !gameOver)
                {
                    restartTime = 150;
                    winnerTeam = aliveKingTeamID;
                    roundOver = true;
                }
            }
        }

        private void tickFlags()
        {
            lock (flagPlaces)
            {
                for (int i = flagPlaces.Count - 1; i >= 0; i--)
                {
                    FlagPlace fp = (FlagPlace)flagPlaces[i];
                    if (getPlayersInTeam(fp.team) > 0)
                    {
                        fp.tick(solidObjects, ref particles);

                        for (int j = 0; j < players.Count; j++)
                        {
                            Player p = (Player)players[j];
                            if (!p.Dead)
                            {
                                if (fp.FlagOnPlace)
                                {
                                    if (p.flag == null)
                                    {
                                        if (p.Team != fp.team && p.getSprite().checkCollision(fp.GetSprite))
                                        {
                                            p.flag = fp.flag;
                                            p.flag.captured = true;
                                            fp.FlagOnPlace = false;
                                        }
                                    }
                                    else
                                    {
                                        if (p.Team == fp.team && p.getSprite().checkCollision(fp.GetSprite))
                                        {
                                            if (pointsLimit > 0)
                                                teamPoints[p.Team]++;

                                            p.flag.timeToReturn = true;
                                            p.flag = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!fp.flag.captured && p.getSprite().checkCollision(fp.flag.GetSprite))
                                    {
                                        if (p.Team == fp.team)
                                        {
                                            fp.flag.timeToReturn = true;
                                        }
                                        else if (p.flag == null)
                                        {
                                            p.flag = fp.flag;
                                            p.flag.captured = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void tickDominationPlates()
        {
            lock (domminationPlates)
            {
                for (int i = domminationPlates.Count - 1; i >= 0; i--)
                {
                    DominationPlate dp = (DominationPlate)domminationPlates[i];
                    dp.tick(players, teamsAmount);

                    if (pointsLimit > 0)
                    {
                        if (dp.team >= 0)
                            teamPoints[dp.team]++;
                    }
                }
            }
        }

        private void tickParticles()
        {
            lock (particles)
            {
                for (int i = particles.Count - 1; i >= 0; i--)
                {
                    Particle part = (Particle)particles[i];
                    part.tick();
                    if(part.timeToDie)
                        particles.RemoveAt(i);
                }
            }
        }

        private void tickProjectiles()
        {
            lock (projectiles)
            {
                for (int i = projectiles.Count - 1; i >= 0; i--)
                {
                    Projectile pr = (Projectile)projectiles[i];
                    
                    if (pr.activated)
                    {
                        pr.tick(solidObjects, projectiles, ref particles);
                        if (((Projectile)projectiles[i]).TimeToDie)
                            projectiles.RemoveAt(i);
                    }
                    else
                        pr.activated = true;
                }
            }
        }

        private void tickPlayers()
        {
            lock (players)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    Player p = (Player)players[i];

                    if (p.weaponReadyToAction)
                    {
                        if (p.weaponStrikeDelay == 0)
                        {
                            Projectile pr = p.spawnProjectile();
                            if (pr != null)
                                projectiles.Add(pr);
                        }
                    }

                    p.tick(solidObjects, projectiles, ref particles, harmfulObjects);

                    if (!p.Dead)
                    {
                        if (p.Health <= 0)
                            killPlayer(p);
                    }
                    else
                    {
                        if (p.TimeToRespawn == 0)
                        {
                            bool kingAlive = false;
                            if(gameMode== GameMode.ProtectKing)
                            {
                                foreach(Player pl in players)
                                    if(pl.king && pl.Team == p.Team && !pl.Dead)
                                    {
                                        kingAlive = true;
                                        break;
                                    }
                            }
                            else
                                kingAlive = true;

                            if (respawnTime >= 0 && !RoundOver && !gameOver && kingAlive &&
                                (spawnsLimit <= 0 || teamSpawnsLeft[p.Team] > 0))
                            {
                                respawnPlayer(p.ID);
                                teamSpawnsLeft[p.Team]--;
                            }
                        }
                    }
                }
            }
        }

        private void killPlayer(Player victim)
        {
            int assistantID = victim.DamageStats.isKillAssisted(victim.lastHitByPlayer);
            Player assistant = null;
            Player killer = null;

            foreach (Player p in players)
            {
                if (p.ID == victim.lastHitByPlayer)
                    killer = p;

                if (p.ID == assistantID)
                    assistant = p;
            }

            if (killer != null)
            {
                teamScore[killer.Team]++;
                killer.Frags++;

                if (gameMode == GameMode.DeathMatch)
                    teamPoints[killer.Team]++;

                if (killer is Berserker)
                {
                    float coef = ((Berserker)killer).InRage ? 23 : 14;
                    ((Berserker)killer).addRage(victim.DamageStats.getDamageMadeByPlayer(killer.ID) / coef);
                }

                if (victim.king && !GameOver)
                {
                    teamPoints[killer.Team]++;
                }
            }
            else
            {
                teamScore[victim.Team]--;
                particles.Add(new ScoreUpParticle(-1, victim.X+victim.Width/2, victim.Y, 30));

                if (victim.king && !GameOver && teamPoints[victim.Team] > 0)
                {
                    teamPoints[victim.Team]--;
                }
            }

            if (assistant != null)
            {
                if (assistant != killer)
                {
                    assistant.Assists++;
                    particles.Add(new ScoreUpParticle(1, assistant.X + assistant.Width / 2, assistant.Y, 30));
                    if (killer != null)
                        particles.Add(new ScoreUpParticle(1, killer.X + killer.Width / 2, killer.Y, 30));
                }
                else
                {
                    particles.Add(new ScoreUpParticle(2, killer.X + killer.Width / 2, killer.Y, 30));
                }

                teamScore[assistant.Team]++;
            }

            if (victim.flag != null)
            {
                victim.flag.returnTimeout = victim.flag.maxReturnTimeout;
                victim.flag.captured = false;
                victim.flag = null;
            }

            if (gameMode == GameMode.Harvester)
            {
                if (statue != null)
                    skulls.Add(statue.spawnSkull(victim.Team, ref particles));
            }

            tombstones.Add(new Tombstone(victim.X + 35, victim.Y, victim.Team, respawnTime > 0 ? 750 : -1));

            victim.Deaths++;
            victim.Dead = true;
            victim.removeMyArrows();
            victim.removeMySkulls(-1);
            victim.TimeToRespawn = respawnTime <= 0 ? 0 : respawnTime;
            //respawnPlayer(victim.ID);
        }

        public void stop()
        {
            running = false;
        }

        private void spawnKings()
        {
            Random rnd = new Random();
            for (int i = 0; i < teamsAmount; i++)
            {
                var team = (from p in players.ToArray()
                            where ((Player)p).Team == i
                            select p).ToList();

                if (team.Count > 0)
                {
                    int king = rnd.Next(0, team.Count());
                    ((Player)team[king]).king = true;
                }
            }
        }

        public void restart(string newMapName)
        {
            if (mapName.CompareTo(newMapName) != 0)
            {
                mapName = newMapName;
                loadMap(mapName);
            }

            for (int i = 0; i < players.Count;i++ )
            {
                ((Player)players[i]).Frags = 0;
                ((Player)players[i]).Deaths = 0;
                ((Player)players[i]).Assists = 0;
                respawnPlayer(((Player)players[i]).ID);
            }

            if (gameMode == GameMode.ProtectKing)
                spawnKings();

            for (int i = 0; i < teamsAmount; i++)
            {
                teamScore[i] = 0;
                teamPoints[i] = 0;
            }

            for (int i = 0; i < domminationPlates.Count; i++)
                ((DominationPlate)domminationPlates[i]).reset();

            skulls = new ArrayList();
            tombstones = new ArrayList();

            roundOver = false;
            gameOver = false;

            if (timeLimit > 0)
                setTimeLimit(timeLimit);

            if (spawnsLimit > 0)
                setSpawnsLimit(spawnsLimit);
        }

        public bool kickPlayer(int ID)
        {
            lock (players)
            {
                for (int i = players.Count - 1; i >= 0; i--)
                {
                    if (((Player)players[i]).ID == ID)
                    {
                        if (((Player)players[i]).flag != null)
                        {
                            ((Player)players[i]).flag.returnTimeout = ((Player)players[i]).flag.maxReturnTimeout;
                            ((Player)players[i]).flag.captured = false;
                            ((Player)players[i]).flag = null;
                        }
                        players.RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool respawnPlayer(int ID)
        {
            lock (players)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (((Player)players[i]).ID == ID)
                    {
                        Random rnd = new Random();
                        ((Player)players[i]).teamAfterRespawn %= teamsAmount;
                        int teamToSpawn = ((Player)players[i]).teamAfterRespawn;
                        int spawnX = rnd.Next(-20, 20) + spawnPoints[teamToSpawn].X;
                        int spawnY = spawnPoints[teamToSpawn].Y;

                        if (((Player)players[i]).classAfterRespawn == ((Player)players[i]).className)
                        {
                            ((Player)players[i]).respawn(spawnX, spawnY, ref projectiles);
                        }
                        else
                        {
                            ((Player)players[i]).respawn(spawnX, spawnY, ref projectiles);

                            int team = ((Player)players[i]).teamAfterRespawn;
                            int id = ((Player)players[i]).ID;
                            int frags = ((Player)players[i]).Frags;
                            int deaths = ((Player)players[i]).Deaths;
                            int assists= ((Player)players[i]).Assists;
                            string name = ((Player)players[i]).Name;
                            string afterResp = ((Player)players[i]).classAfterRespawn;

                            if (afterResp == "Warrior")
                            {
                                players[i] = new Warrior(spawnX, spawnY, team, 1, name, id);
                            }
                            else if (afterResp == "Berserker")
                            {
                                players[i] = new Berserker(spawnX, spawnY, team, 1, name, id);
                            }
                            else if (afterResp == "Bowman")
                            {
                                players[i] = new Bowman(spawnX, spawnY, team, 1, name, id);
                            }
                            else if (afterResp == "Arbalester")
                            {
                                players[i] = new Arbalester(spawnX, spawnY, team, 1, name, id);
                            }
                            else if (afterResp == "Knight")
                            {
                                players[i] = new Knight(spawnX, spawnY, team, 1, name, id);
                            }
                            else if (afterResp == "Thief")
                            {
                                players[i] = new Thief(spawnX, spawnY, team, 1, name, id);
                            }

                            ((Player)players[i]).Frags = frags;
                            ((Player)players[i]).Deaths = deaths;
                            ((Player)players[i]).Assists = assists;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public Player getPlayerForClient(int ID)
        {
            for (int i = 0; i < players.Count; i++)
                if (((Player)players[i]).ID == ID)
                    return (Player) players[i];

            return null;
        }

        public Player addPlayer(string name, int team, string gameClass)
        {
            Player p = null;
            Random rnd = new Random();

            if (players.Count < playerLimit)
            {
                team %= teamsAmount;
                int spawnX = rnd.Next(-20, 20) + spawnPoints[team].X;
                int spawnY = spawnPoints[team].Y;

                if (name.ToLower().CompareTo("mage") == 0)
                {
                    p = new Mage(spawnX, spawnY, team, 1, name, playersConnected);
                }
                else switch (gameClass)
                {
                    case "Warrior":
                        p = new Warrior(spawnX, spawnY, team, 1, name, playersConnected);
                        break;

                    case "Berserker":
                        p = new Berserker(spawnX, spawnY, team, 1, name, playersConnected);
                        break;

                    case "Bowman":
                        p = new Bowman(spawnX, spawnY, team, 1, name, playersConnected);
                        break;

                    case "Arbalester":
                        p = new Arbalester(spawnX, spawnY, team, 1, name, playersConnected);
                        break;

                    case "Knight":
                        p = new Knight(spawnX, spawnY, team, 1, name, playersConnected);
                        break;
                    case "Thief":
                        p = new Thief(spawnX, spawnY, team, 1, name, playersConnected);
                        break;
                }

                players.Add(p);
                playersConnected++;
            }

            return p;
        }

        public ArrayList getPlayers()
        { return players; }

        public ArrayList getSolidObjects()
        { return solidObjects; }

        public ArrayList getProjectiles()
        { return projectiles; }

        public ArrayList getParticles()
        { return particles; }

        public ArrayList getDominationPlates()
        { return domminationPlates; }

        public ArrayList getFlagPlaces()
        { return flagPlaces; }

        public ArrayList getSkulls()
        { return skulls; }

        public ArrayList getTombstones()
        { return tombstones; }

        public StatueHarvester getStatueHarvester()
        { return statue; }

        public string getMapName()
        { return mapName; }

        public int getTeamScore(int teamIndex)
        {
            if (teamIndex >= 0 && teamIndex < teamScore.Length)
            {
                return teamScore[teamIndex];
            }
            else
                return -1;
        }

        public int getPlayersInTeam(int teamIndex)
        {
            var playersInTeam = from p in players.ToArray() where ((Player)p).Team == teamIndex select p;
            /*
            int count = 0;
            foreach (Player p in players)
                if (p.Team == teamIndex)
                    count++;
            */
            return playersInTeam.Count();
        }

        public bool[] getTeamsInGame()
        {
            bool[] teams = new bool[teamsAmount];
            foreach (Player p in players)
                teams[p.Team] = true;

            return teams;
        }

        public int getTeamsAmount()
        { return teamsAmount; }

        public int WinnerTeam
        { get { return winnerTeam; } }

        public bool RoundOver
        { get { return roundOver; } }

        public bool GameOver
        { get { return gameOver; } }

        public int RestartRoundTime
        { get { return restartTime; } }

        public GameMode GetGameMode
        { get { return gameMode; } }

        public int TimeLimit
        { get { return timeLimit; } }

        public int TimeLeft
        { get { return timeLeft; } }

        public int SpawnsLimit
        { get { return spawnsLimit; } }

        public int getTeamSpawnsLeft(int team)
        { return teamSpawnsLeft[team]; }

        public int PointsLimit
        { get { return pointsLimit; } }

        public int getTeamPoints(int team)
        { return teamPoints[team]; }

        public void doTickForPlayer(int ID)
        {
            foreach (Player p in players)
                if (p.ID == ID)
                    p.tick(solidObjects, projectiles, ref particles, harmfulObjects);
        }

        public void doActionForPlayer(int ID, string action, bool isKeyDown)
        {
            foreach (Player p in players)
                if (p.ID == ID)
                {
                    if (action == "left")
                    {
                        if (isKeyDown)
                        {
                            if (p is Arbalester)
                                ((Arbalester)p).breakReload();

                            p.face = -1;
                            p.vx = -p.movingSpeedX;
                            p.running = true;
                            if (!p.inAir)
                            {
                                if (p.weaponReadyToAction)
                                    p.setSprite("runSwing");
                                else
                                    p.setSprite("run");
                            }
                        }
                        else
                        {
                            if (p.face == -1)
                            {
                                p.vx = 0;
                                p.running = false;
                                if (!p.inAir)
                                {
                                    if (p.weaponReadyToAction)
                                    {
                                        p.setSprite("swing");
                                        p.getSprite().setFrame(0);
                                    }
                                    else
                                        p.setSprite("stand");
                                }
                            }
                        }
                    }
                    else if (action == "right")
                    {
                        if (isKeyDown)
                        {
                            if (p is Arbalester)
                                ((Arbalester)p).breakReload();

                            p.face = 1;
                            p.vx = p.movingSpeedX;
                            p.running = true;
                            if (!p.inAir)
                            {
                                if (p.weaponReadyToAction)
                                    p.setSprite("runSwing");
                                else
                                    p.setSprite("run");
                            }
                        }
                        else
                        {
                            if (p.face == 1)
                            {
                                p.vx = 0;
                                p.running = false;
                                if (!p.inAir)
                                {
                                    if (p.weaponReadyToAction)
                                    {
                                        p.setSprite("swing");
                                        p.getSprite().setFrame(0);
                                    }
                                    else
                                        p.setSprite("stand");
                                }
                            }
                        }
                    }
                    else if (action == "down")
                    {
                        if (isKeyDown)
                        {
                            p.fallThroughSemiSolid = true;
                        }
                        else
                        {
                            p.fallThroughSemiSolid = false;
                        }
                    }
                    else if (action == "Z")
                    {
                        if (isKeyDown)
                        {
                            if (!p.inAir)
                            {
                                if (p is Arbalester)
                                    ((Arbalester)p).breakReload();

                                p.doJump();
                            }
                            else if (p is Thief)
                            {
                                ((Thief)p).doWallJumpIfCan(solidObjects);
                            }
                        }
                    }
                    else if (action == "ZDown")
                    {
                        if (isKeyDown)
                        {
                            if (!p.inAir)
                            {
                                if (p is Arbalester)
                                    ((Arbalester)p).breakReload();

                                bool onSemiSolid = false;
                                bool onSolid = false;
                                foreach (SolidObject so in solidObjects)
                                    if (p.getSprite().checkCollision(so.box, 0, 1))
                                    {
                                        if (so.semiSolid)
                                            onSemiSolid = true;
                                        else
                                        {
                                            onSolid = true;
                                            break;
                                        }
                                    }

                                if (!onSolid && onSemiSolid)
                                    p.doJumpDown();
                            }
                        }
                    }
                    else if (action == "X")
                    {
                        if (!p.Dead)
                        {
                            if (isKeyDown)
                            {
                                if (p.weaponCooldown == 0 && (!(p is Arbalester) || !((Arbalester)p).weaponReloading))
                                {
                                    p.weaponInAction = false;
                                    p.weaponStrikeDelay = -1;
                                    p.weaponReadyToAction = true;

                                    if (p.inAir)
                                    {
                                        p.setSprite("jumpSwing");
                                    }
                                    else
                                    {
                                        if (p.vx != 0)
                                            p.setSprite("runSwing");
                                        else
                                        {
                                            p.setSprite("swing");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                p.startSwing();
                            }
                        }
                    }
                    else if (action == "XUp")
                    {
                        if (!p.Dead)
                        {
                            if (isKeyDown)
                            {
                                if (p.Name.ToLower() == "sergreen")
                                {
                                    Projectile pr = new Melon(p.X + 25, p.Y, p.face, 160, p.Team, p.ID, 300, 6 * p.face, -8, 0.5f);
                                    projectiles.Add(pr);
                                }
                            }
                        }
                    }
                    else if (action == "XDown")
                    {
                        if (!p.Dead)
                        {
                            if (isKeyDown)
                            {
                                if (p.Name.ToLower() == "pif")
                                {
                                    foreach (Projectile pr in projectiles)
                                    {
                                        if (pr is Melon && p.getSprite().checkCollision(pr.getSprite) && p.Team == pr.team)
                                        {
                                            pr.disarmed = false;
                                            ((Melon)pr).addSpeed(-4 * pr.face, 10);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (action == "C")
                    {
                        if (!p.Dead)
                        {
                            if (isKeyDown)
                            {
                                if (p is Arbalester)
                                {
                                    if (p.weaponCharge < p.weaponChargeLimit && !p.weaponReadyToAction)
                                    {
                                        ((Arbalester)p).startReload();
                                    }
                                }
                                else if (p is Bowman)
                                {
                                    if (p.weaponCooldown == 0 && ((Bowman)p).arrowsLeft > 0 && !p.weaponReadyToAction)
                                    {
                                        ((Bowman)p).poisonedShoot = true;
                                        p.weaponInAction = false;
                                        p.weaponStrikeDelay = -1;
                                        p.weaponReadyToAction = true;

                                        if (p.inAir)
                                        {
                                            p.setSprite("jumpSwing");
                                            p.getSprite().setFrame(0);
                                        }
                                        else
                                        {
                                            if (p.vx != 0)
                                                p.setSprite("runSwing");
                                            else
                                            {
                                                p.setSprite("swing");
                                                p.getSprite().setFrame(0);
                                            }
                                        }
                                    }
                                }
                                else if (p is Thief)
                                {
                                    if (p.weaponCooldown == 0)
                                    {
                                        if (((Thief)p).knivesLeft > 0)
                                        {
                                            ((Thief)p).IsThrowing = true;
                                            p.weaponInAction = false;
                                            p.weaponStrikeDelay = -1;
                                            p.weaponReadyToAction = true;

                                            if (p.inAir)
                                            {
                                                p.setSprite("jumpSwing");
                                                p.getSprite().setFrame(0);
                                            }
                                            else
                                            {
                                                if (p.vx != 0)
                                                    p.setSprite("runSwing");
                                                else
                                                {
                                                    p.setSprite("swing");
                                                    p.getSprite().setFrame(0);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (p is Warrior)
                                {
                                    if (((Warrior)p).Dash == ((Warrior)p).DashMax)
                                        ((Warrior)p).InDash = true;
                                }
                                else if (p is Mage)
                                {
                                    Particle part = ((Mage)p).suicide();
                                    projectiles.Add(new ExplosionDamageable(p.X + p.Width * p.getSprite().Scale / 2, p.Y + p.Height * p.getSprite().Scale, p.Team, p.ID));
                                    particles.Add(part);
                                }
                            }
                            else
                            {
                                if (p is Arbalester)
                                {
                                    if (((Arbalester)p).weaponReloading && p.weaponCharge < p.weaponChargeLimit)
                                    {
                                        ((Arbalester)p).breakReload();
                                    }
                                }
                                else if (p is Bowman)
                                {
                                    p.startSwing();
                                }
                                else if (p is Thief)
                                {
                                    p.startSwing();
                                }
                                else if (p is Warrior)
                                {
                                    ((Warrior)p).stopDash();
                                }
                            }
                        }
                    }
                }
        }

        private void loadMap(string mapName)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (File.Exists(assemblyFolder + "/res/maps/" + mapName + ".map"))
            {
                //load server data
                StreamReader sr = new StreamReader(assemblyFolder + "/res/maps/" + mapName + ".map");
                string file = sr.ReadToEnd();
                sr.Close();

                string[] mapParts = file.Split('#');

                //load spawn points
                string[] spawnData = mapParts[0].Split('\n');
                spawnPoints = new List<Point>();
                for (int i = 0; i < spawnData.Length; i++)
                {
                    string[] spwnP = spawnData[i].Split(':');
                    int x = int.Parse(spwnP[0]);
                    int y = int.Parse(spwnP[1]);

                    spawnPoints.Add(new Point(x, y));
                }

                //load solid objects
                solidObjects = new ArrayList();
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
                harmfulObjects = new ArrayList();
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
                domminationPlates = new ArrayList();
                if (mapParts.Length > 3 && mapParts[3].Length > 0)
                {
                    string[] domPlates = mapParts[3].Split('\n');
                    for (int i = 0; i < domPlates.Length; i++)
                    {
                        string[] obj = domPlates[i].Split(':');
                        int x = int.Parse(obj[0]);
                        int y = int.Parse(obj[1]);

                        domminationPlates.Add(new DominationPlate(x, y, teamsAmount));
                    }
                }

                //load flag places
                flagPlaces = new ArrayList();
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
        }
    }
}
