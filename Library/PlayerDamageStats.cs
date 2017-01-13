using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MultiplayerLib
{
    public class PlayerDamageStats
    {
        private List<int> damagersID;
        private List<float> damageAmount;

        public PlayerDamageStats()
        {
            damagersID = new List<int>();
            damageAmount = new List<float>();
        }

        public void addDamage(int ID, float damage)
        {
            int index = -1;
            for (int i = 0; i < damagersID.Count; i++)
                if (damagersID[i] == ID)
                {
                    index = i;
                    break;
                }

            if (index >= 0)
                damageAmount[index] += damage;
            else
            {
                damagersID.Add(ID);
                damageAmount.Add(damage);
            }
        }

        public float getDamageMadeByPlayer(int ID)
        {
            float damage = 0;
            for(int i=0;i<damagersID.Count;i++)
                if (damagersID[i] == ID)
                {
                    damage = damageAmount[i];
                    break;
                }

            return damage;
        }

        public int isKillAssisted(int kilerID)  //returns assistant ID or killerID if not assisted
        {
            int assistantID = kilerID;
            float killerDamage = 0;
            int maxDamageIndex = 0;

            for (int i = 0; i < damagersID.Count; i++)
            {
                if (damagersID[i] == kilerID)
                    killerDamage = damageAmount[i];

                if (damageAmount[i] > damageAmount[maxDamageIndex])
                    maxDamageIndex = i;
            }

            if (damageAmount[maxDamageIndex] > 0)
            {
                if (killerDamage / damageAmount[maxDamageIndex] < 0.67)
                    assistantID = damagersID[maxDamageIndex];
            }

            return assistantID;
        }
    }
}
