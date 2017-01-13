using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;

namespace MultiplayerLib
{
    public class DominationPlate : MarshalByRefObject
    {
        private Sprite sprite;
        public int team;
        private int maxCaptureTime;
        private int[] captureTime;

        public DominationPlate(float x, float y, int teamsAmount)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.DominPlate, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/domination_plates.png"), 64, 1);
            team = -1;
            maxCaptureTime = 50;
            captureTime = new int[teamsAmount];
        }

        public Sprite getSprite
        { get { return sprite; } }

        public void updateTeamsAmount(int teamsAmount)
        {
            int[] newCaptureTime = new int[teamsAmount];

            for (int i = 0; i < Math.Min(newCaptureTime.Length, captureTime.Length); i++)
                newCaptureTime[i] = captureTime[i];

            captureTime = newCaptureTime;

            if (team > teamsAmount - 1)
                team = -1;
        }

        public void reset()
        {
            team = -1;
            captureTime = new int[captureTime.Length];
        }

        public void tick(ArrayList players, int teamsAmount)
        {
            int[] teamStanding = new int[teamsAmount];
            for (int i = 0; i < players.Count; i++)
                if (!((Player)players[i]).Dead && sprite.checkCollision(((Player)players[i]).getSprite()))
                    teamStanding[((Player)players[i]).Team]++;

            int countAllPlayersStanding = teamStanding.Sum();
            for (int i = 0; i < teamStanding.Length; i++)
            {
                if (teamStanding[i] == countAllPlayersStanding)     //that means that all players standing from one team
                {
                    captureTime[i] += teamStanding[i];
                }
                else if (teamStanding[i] == 0)
                {
                    if (captureTime[i] > 0)
                        captureTime[i]--;
                }
                else
                    break;      //this means that more than one team standing on plate
            }

            for (int i = 0; i < captureTime.Length; i++)
            {
                if (captureTime[i] >= maxCaptureTime && i != team)
                {
                    team = i;
                    for (int j = 0; j < captureTime.Length; j++)
                        captureTime[j] = 0;
                    break;
                }
            }

            sprite.setFrame(team + 1);
        }
    }
}
