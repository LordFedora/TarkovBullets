using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarkovBullets
{
    class Body
    {
        private int HeadHP = 35;
        private int ThoraxHP = 85;
        private int RArmHP = 60;
        private int LArmHP = 60;
        private int StomachHP = 70;
        private int RLegHP = 65;
        private int LLegHP = 65;

        private static float HeadRatio = 35f / 440f;
        private static float ThoraxRatio = 85f / 440f;
        private static float RArmRatio = 60f / 440f;
        private static float LArmRatio = 60f / 440f;
        private static float StomachRatio = 70f / 440f;
        private static float RLegRatio = 65f / 440f;
        private static float LLegRatio = 65f / 440f;

        private static float ARM_MULTIPLIER = 0.7f;
        private static float STOMACH_MULTIPLIER = 1.5f;
        private static float LEG_MULTIPLIER = 1.0f;

        private Armor EquipedBodyArmor;
        private Armor EquipedHelmet;

        public enum BodyRegions
        {
            Head,Thorax,RArm,LArm,Stomach,RLeg,LLeg,Miss
        }

        public Body(Armor Helmet, Armor BodyArmor)
        {
            EquipedBodyArmor = BodyArmor.resetArmor();
            EquipedHelmet = Helmet.resetArmor();
        }

        public Boolean isDead()
        {
            return (HeadHP <= 0) || (ThoraxHP <= 0);
        }
        public int damageTaken()
        {
            return 440 - (HeadHP + ThoraxHP + RArmHP + LArmHP + RLegHP + LLegHP + StomachHP); //super nieve
        }

        public Armor getCoveredArmor(BodyRegions hitRegion)
        {
            if (EquipedBodyArmor.CoveredRegions.Contains(hitRegion)) return EquipedBodyArmor;
            else if (EquipedHelmet.CoveredRegions.Contains(hitRegion)) return EquipedHelmet;
            return null;

        }

        public void takeDamage(Bullet bullet, BodyRegions hitRegion, Random rand)
        {

            int damage = bullet.Damage;
            Armor blockingArmor = getCoveredArmor(hitRegion);

            if (!(blockingArmor is null)) damage = blockingArmor.defend(bullet,rand);


            switch (hitRegion)
            {
                case BodyRegions.Head:
                    HeadHP -= damage;
                    damage = 0; //who cares about overkill on head and thorax
                    break;
                case BodyRegions.Thorax:
                    ThoraxHP -= damage;
                    damage = 0; //don't want to black the limb because we're done here
                    break;
                case BodyRegions.LArm:
                    if (LArmHP > damage)
                    {
                        LArmHP -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= LArmHP;
                        LArmHP = 0;
                        damage = (int)(damage * ARM_MULTIPLIER);
                    }
                    break;
                case BodyRegions.RArm:
                    if (RArmHP > damage)
                    {
                        RArmHP -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= RArmHP;
                        RArmHP = 0;
                        damage = (int)(damage * ARM_MULTIPLIER);
                    }
                    break;
                case BodyRegions.LLeg:
                    if (LLegHP > damage)
                    {
                        LLegHP -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= LLegHP;
                        LLegHP = 0;
                        damage = (int)(damage * LEG_MULTIPLIER);
                    }
                    break;
                case BodyRegions.RLeg:
                    if (RLegHP > damage)
                    {
                        RLegHP -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= RLegHP;
                        RLegHP = 0;
                        damage = (int)(damage * LEG_MULTIPLIER);
                    }
                    break;
                case BodyRegions.Stomach:
                    if (StomachHP > damage)
                    {
                        StomachHP -= damage;
                        damage = 0;
                    }
                    else
                    {
                        damage -= StomachHP;
                        StomachHP = 0;
                        damage = (int)(damage * STOMACH_MULTIPLIER);
                    }
                    break;
                case BodyRegions.Miss:
                    damage = 0;
                    break;
            }

            if (damage <= 0) return; //we didn't splash

            //we did splash

            //Always splash to Head
            HeadHP = Math.Max(HeadHP - (int)(damage * HeadRatio),0);
            //Always splash to Thorax too
            ThoraxHP = Math.Max(ThoraxHP - (int)(damage * ThoraxRatio),0);
            if (hitRegion != BodyRegions.LArm) //LArm splash
            {
                LArmHP = Math.Max(LArmHP - (int)(damage * LArmRatio), 0);//don't over damage just for convience tbh
            }
            if (hitRegion != BodyRegions.RArm)
            {
                RArmHP = Math.Max(RArmHP - (int)(damage * RArmRatio), 0);//don't over damage just for convience tbh
            }
            if (hitRegion != BodyRegions.LLeg) //LLeg splash
            {
                LLegHP = Math.Max(LLegHP - (int)(damage * LLegRatio), 0);//don't over damage just for convience tbh
            }
            if (hitRegion != BodyRegions.RLeg)
            {
                RLegHP = Math.Max(RLegHP - (int)(damage * RLegRatio), 0);//don't over damage just for convience tbh
            }
            if(hitRegion != BodyRegions.Stomach)
            {
                StomachHP = Math.Max(StomachHP - (int)(damage * StomachRatio), 0);//don't over damage just for convience tbh
            }
        }
    }
}
