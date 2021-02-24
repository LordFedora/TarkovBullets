using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TarkovBullets
{
    class Armor
    {
        public List<Body.BodyRegions> CoveredRegions;
        public int CurrentDurability;
        public int MaximumDurability;
        public ArmorMaterial Material;
        public int ArmorClass;
        public int Resistance;
        public float BluntThroughput;

        public String Name;

        public Armor()
        {
            CoveredRegions = new List<Body.BodyRegions>();//empty we don't cover anything
            CurrentDurability = 0;
            MaximumDurability = 0;//hopefully this doesn't break anything, spoilers lol
            Material = ArmorMaterial.Glass; //need to have *a* material
            ArmorClass = 0;
            BluntThroughput = 1;
            Resistance = 1;

            Name = "Nothing";
        }

        //Body Armor
        public Armor(String name, int armorClass, int Durability, ArmorMaterial material, Boolean Stomach, Boolean Arms = false, float blunt = 1)
        {
            CoveredRegions = new List<Body.BodyRegions>();
            CoveredRegions.Add(Body.BodyRegions.Thorax);//everybody covers this one
            if (Stomach) CoveredRegions.Add(Body.BodyRegions.Stomach);
            if (Arms)
            {
                CoveredRegions.Add(Body.BodyRegions.LArm);
                CoveredRegions.Add(Body.BodyRegions.RArm);//Both arms
            }
            CurrentDurability = Durability;
            MaximumDurability = Durability;
            Material = material;
            ArmorClass = armorClass;
            Resistance = armorClass * 10;
            BluntThroughput = blunt;

            Name = name;
        }

        //Helmet
        public Armor(String name, int armorClass, int Durability, ArmorMaterial material, float blunt = 1)
        {
            CoveredRegions = new List<Body.BodyRegions>();
            CoveredRegions.Add(Body.BodyRegions.Head);
            CurrentDurability = Durability;
            MaximumDurability = Durability;
            Material = material;
            ArmorClass = armorClass;
            Resistance = armorClass * 10;
            BluntThroughput = blunt;

            Name = name;
        }

        public static float getDestructibility(ArmorMaterial material)
        {
            switch (material)
            {
                case ArmorMaterial.Aramid:
                    return 0.25f;
                case ArmorMaterial.Combined:
                    return 0.5f;
                case ArmorMaterial.UHMWPE:
                    return 0.45f;
                case ArmorMaterial.Titan:
                    return 0.55f;
                case ArmorMaterial.Aluminium:
                    return 0.6f;
                case ArmorMaterial.Steel:
                    return 0.7f;
                case ArmorMaterial.Ceramic:
                    return 0.8f;
                case ArmorMaterial.Glass:
                    return 0.8f;

                default:
                    return 0;
            }
        }

        public Armor resetArmor()
        {
            CurrentDurability = MaximumDurability; //this is a convience method so in case we need to do more work we can put it here
            return this;
        }

        //returns the left over damage
        public int defend(Bullet bullet, Random rand)
        {
            //Stolen from https://github.com/tarkovarmory/tarkovarmory/blob/5ad21f145f2477418094c1929b1ecb17d299d914/src/simulations.ts#L158

            int damageToArmor = 0;
            int passDamage = bullet.Damage; //Setup both for empty armor, and for fragmentation

            if ((bullet.Penetration > 20) && (rand.Next(100) < bullet.Fragmentation))
            {
                passDamage = passDamage + passDamage / 2;
            }


            if (CurrentDurability > 0)
            {
                float num = (100.0f * CurrentDurability) / MaximumDurability;
                float num3 = (121.0f - (5000.0f / (45.0f + num * 2.0f))) * Resistance * 0.01f;

                if (simulateBlock(bullet, rand))
                {
                    damageToArmor = (int)(bullet.Penetration * bullet.ArmorDamage * Math.Min(Math.Max(1.0f * bullet.Penetration / Resistance, 0.6), 1.1) * getDestructibility(Material));
                    passDamage = (int)(passDamage * (BluntThroughput * Math.Min(Math.Max(1.0f - 0.03f * (num3 - bullet.Penetration), 0.2), 1.0)));
                }
                else
                {
                    damageToArmor = (int)(bullet.Penetration * bullet.ArmorDamage * Math.Min(Math.Max(1.0f * bullet.Penetration / Resistance, 0.6), 1.0));
                    passDamage = (int)(passDamage * Math.Min(Math.Max(bullet.Penetration / (num3 + 12.0f), 0.6), 1.0));
                }

                damageToArmor = Math.Max(1, damageToArmor);
                CurrentDurability -= damageToArmor;
                CurrentDurability = Math.Max(0, CurrentDurability); //prevent underflow

            }

            return passDamage;
        }

        public Boolean simulateBlock(Bullet bullet, Random rand)
        {
            //Stolen from https://github.com/tarkovarmory/tarkovarmory/blob/5ad21f145f2477418094c1929b1ecb17d299d914/src/simulations.ts#L158
            if (CurrentDurability <= 0) return false;

            float num = (100.0f * CurrentDurability) / MaximumDurability;
            float num3 = (121.0f - (5000.0f / (45.0f + num * 2.0f))) * Resistance * 0.01f;
            float num4 = (num3 >= bullet.Penetration + 15) ? 0.0f : ((num3 <= bullet.Penetration) ? (100.0f + bullet.Penetration / (0.9f * num3 - bullet.Penetration)) : (0.4f * (num3 - bullet.Penetration - 15.0f) * (num3 - bullet.Penetration - 15.0f)));
            //what. the. fuck?
            //TODO, unscramble lol
            return (num4 < (rand.NextDouble() * 100.0f));

        }

        public static List<Armor> getHelmets()
        {
            List<Armor> helmets = new List<Armor>();

            helmets.Add(new Armor("Tac-Kek Fast MT Helmet (non-ballistic replica)", 1, 40, ArmorMaterial.Combined, 0.5f));
            helmets.Add(new Armor("Soft tank crew helmet TSH-4M-L", 1, 100, ArmorMaterial.Aramid, 0.1f));
            helmets.Add(new Armor("Kolpak-1S riot helmet", 2, 25, ArmorMaterial.Aramid, 0.16f));
            helmets.Add(new Armor("SHPM Firefighter's helmet", 2, 40, ArmorMaterial.Aramid, 0.19f));
            helmets.Add(new Armor("PSH-97 'Djeta' helmet", 2, 65, ArmorMaterial.Aramid, 0.19f));
            helmets.Add(new Armor("6B47 Ratnik-BSh Helmet", 3, 25, ArmorMaterial.Aluminium, 0.17f));
            helmets.Add(new Armor("UNTAR helmet", 3, 25, ArmorMaterial.Aramid, 0.18f));
            helmets.Add(new Armor("SSh-68 helmet (1968 steel helmet)", 3, 30, ArmorMaterial.Steel, 0.2f));
            helmets.Add(new Armor("LZSh light helmet", 3, 30, ArmorMaterial.Aramid, 0.15f));
            helmets.Add(new Armor("Kiver-M Helmet", 3, 35, ArmorMaterial.UHMWPE, 0.15f));
            helmets.Add(new Armor("SSSh-95 Sfera-S (Sphere-S)", 3, 100, ArmorMaterial.Steel, 0.1f));
            helmets.Add(new Armor("MSA ACH TC-2001 MICH Series Helmet", 4, 25, ArmorMaterial.Combined, 0.17f));
            helmets.Add(new Armor("MSA ACH TC-2002 MICH Series Helmet", 4, 27, ArmorMaterial.Combined, 0.17f));
            helmets.Add(new Armor("MSA Gallet TC 800 High Cut combat helmet", 4, 30, ArmorMaterial.Combined, 0.16f));
            helmets.Add(new Armor("Highcom Striker ACHHC IIIA helmet", 4, 30, ArmorMaterial.Combined, 0.13f));
            helmets.Add(new Armor("ZSh-1-2M helmet", 4, 30, ArmorMaterial.Combined, 0.15f));
            helmets.Add(new Armor("Diamond Age Bastion Helmet", 4, 30, ArmorMaterial.Combined, 0.11f));
            helmets.Add(new Armor("Highcom Striker ULACH IIIA Helmet", 4, 38, ArmorMaterial.UHMWPE, 0.13f));
            helmets.Add(new Armor("Ops-Core Fast MT SUPER HIGH CUT Helmet", 4, 40, ArmorMaterial.Combined, 0.14f));
            helmets.Add(new Armor("Crye Precision Airframe", 4, 40, ArmorMaterial.Combined, 0.11f));
            helmets.Add(new Armor("Team Wendy EXFIL Ballistic Helmet", 4, 45, ArmorMaterial.Combined, 0.16f));
            helmets.Add(new Armor("Galvion Caiman Ballistic Helmet", 4, 50, ArmorMaterial.Combined, 0.12f));
            helmets.Add(new Armor("BNTI LSHZ-2DTM Helmet", 4, 55, ArmorMaterial.Combined, 0.12f));
            helmets.Add(new Armor("Maska 1Sch helmet", 4, 60, ArmorMaterial.Steel, 0.14f));
            helmets.Add(new Armor("Altyn helmet", 5, 45, ArmorMaterial.Steel, 0.14f));
            helmets.Add(new Armor("Rys-T helmet", 5, 50, ArmorMaterial.Titan, 0.12f));
            helmets.Add(new Armor("Vulkan-5 (LShZ-5) heavy helmet", 6, 55, ArmorMaterial.Combined, 0.11f));

            return helmets;
        }

        public static Armor getHelmet(string name)
        {
            List<Armor> helmets = getHelmets();
            name = name.ToLower();

            foreach (Armor helmet in helmets)
            {
                if (helmet.Name.ToLower().Contains(name)) return helmet;
            }

            return null;
        }
        public static List<Armor> getHelmet(int ac)
        {
            List<Armor> bodyArmors = getHelmets();

            bodyArmors.RemoveAll(x => x.ArmorClass != ac);

            return bodyArmors;
        }

        public static List<Armor> getVisors()
        {
            List<Armor> visors = new List<Armor>();

            visors.Add(new Armor("Ops-Core FAST Visor", 2, 20, ArmorMaterial.Glass, 0.15f));
            visors.Add(new Armor("Caiman Fixed Arm Visor", 2, 20, ArmorMaterial.Glass, 0.15f));
            visors.Add(new Armor("K1S Visor", 2, 30, ArmorMaterial.Glass, 0.1f));
            visors.Add(new Armor("Kiver face shield", 3, 40, ArmorMaterial.Glass, 0.15f));
            visors.Add(new Armor("TW EXFIL Ballistic helmet face shield", 3, 45, ArmorMaterial.Glass, 0.12f));
            visors.Add(new Armor("Multi-hit ballistic face shield-visor for Ops-Core FAST helmet", 3, 40, ArmorMaterial.Glass, 0.15f));
            visors.Add(new Armor("ZSh-1-2M face shield", 3, 50, ArmorMaterial.Glass, 0.1f));
            visors.Add(new Armor("LSHZ-2DTM face shield", 4, 50, ArmorMaterial.Glass, 0.07f));
            visors.Add(new Armor("Vulkan-5 face shield", 4, 85, ArmorMaterial.Glass, 0.08f));
            visors.Add(new Armor("Altyn face shield", 5, 50, ArmorMaterial.Combined, 0.08f));
            visors.Add(new Armor("Rys-T face shield", 5, 55, ArmorMaterial.Combined, 0.08f));
            visors.Add(new Armor("Maska 1Sch face shield", 6, 50, ArmorMaterial.Steel, 0.08f));

            return visors;
        }

        public static Armor getVisor(string name)
        {
            List<Armor> visors = getVisors();
            name = name.ToLower();

            foreach (Armor visor in visors)
            {
                if (visor.Name.ToLower().Contains(name)) return visor;
            }

            return null;
        }
        public static List<Armor> getVisor(int ac)
        {
            List<Armor> bodyArmors = getVisors();

            bodyArmors.RemoveAll(x => x.ArmorClass != ac);

            return bodyArmors;
        }

        public static List<Armor> getHelmetArmors()
        {
            List<Armor> armors = new List<Armor>();

            armors.Add(new Armor("Ops-Core Fast GUNSIGHT Mandible", 2, 20, ArmorMaterial.Aramid, 0.2f));
            armors.Add(new Armor("Caiman Ballistic Guard Mandible", 2, 20, ArmorMaterial.Aramid, 0.2f));
            armors.Add(new Armor("Tac-Kek Heavy Trooper mask", 2, 45, ArmorMaterial.Aramid, 0.1f));
            armors.Add(new Armor("Crye Airframe Ears", 3, 27, ArmorMaterial.Aluminium, 0.15f));
            armors.Add(new Armor("Crye Airframe Chops", 3, 30, ArmorMaterial.Aluminium, 0.15f));
            armors.Add(new Armor("TW EXFIL Ear Covers", 3, 30, ArmorMaterial.Aluminium, 0.13f));
            armors.Add(new Armor("Ops-Core Fast Side Armor", 3, 25, ArmorMaterial.Aluminium, 0.15f));
            armors.Add(new Armor("Caiman Hybrid Ballistic Applique", 4, 40, ArmorMaterial.UHMWPE, 0.08f));
            armors.Add(new Armor("SLAAP armor Plate", 5, 30, ArmorMaterial.UHMWPE, 0.08f));
            armors.Add(new Armor("LSHZ-2DTM Aventail", 5, 35, ArmorMaterial.Combined, 0.04f));
            armors.Add(new Armor("Additional armor for the Bastion helmet", 6, 40, ArmorMaterial.Ceramic, 0.05f));


            return armors;
        }

        public static Armor getHelmetArmor(String name)
        {
            List<Armor> armors = getHelmetArmors();
            name = name.ToLower();

            foreach (Armor armor in armors)
            {
                if (armor.Name.ToLower().Contains(name)) return armor;
            }

            return null;
        }
        public static List<Armor> getHelmetArmor(int ac)
        {
            List<Armor> bodyArmors = getHelmetArmors();

            bodyArmors.RemoveAll(x => x.ArmorClass != ac);

            return bodyArmors;
        }

        public static List<Armor> getBodyArmors()
        {

            List<Armor> bodyArmors = new List<Armor>();

            bodyArmors.Add(new Armor("Module-3M bodyarmor", 2, 40, ArmorMaterial.Aramid, true, false, 0.35f));
            bodyArmors.Add(new Armor("PACA Soft Armor", 2, 50, ArmorMaterial.Aramid, true, false, 0.3f));
            bodyArmors.Add(new Armor("6B2 armor (flora)", 2, 80, ArmorMaterial.Titan, true, false, 0.24f));
            bodyArmors.Add(new Armor("MF-UNTAR armor vest", 3, 50, ArmorMaterial.Aluminium, true, false, 0.2f));
            bodyArmors.Add(new Armor("Zhuk-3 Press armor", 3, 50, ArmorMaterial.UHMWPE, true, false, 0.2f));
            bodyArmors.Add(new Armor("6B23-1 armor (digital flora pattern)", 3, 60, ArmorMaterial.Steel, true, false, 0.17f));
            bodyArmors.Add(new Armor("BNTI Kirasa-N armor", 3, 70, ArmorMaterial.Combined, true, false, 0.24f));
            bodyArmors.Add(new Armor("Highcom Trooper TFO armor (multicam)", 4, 85, ArmorMaterial.UHMWPE, false, false, 0.18f));
            bodyArmors.Add(new Armor("6B13 assault armor", 4, 47, ArmorMaterial.Ceramic, true, false, 0.17f));
            bodyArmors.Add(new Armor("6B23-2 armor (mountain flora pattern)", 4, 55, ArmorMaterial.Ceramic, true, false, 0.17f));
            bodyArmors.Add(new Armor("BNTI Korund-VM armor", 5, 45, ArmorMaterial.Steel, true, false, 0.24f));
            bodyArmors.Add(new Armor("FORT Redut-M body armor", 5, 60, ArmorMaterial.Combined, true, false, 0.2f));
            bodyArmors.Add(new Armor("6B13 M assault armor", 5, 60, ArmorMaterial.UHMWPE, true, false, 0.2f));
            bodyArmors.Add(new Armor("BNTI Gzhel-K armor", 5, 65, ArmorMaterial.Ceramic, true, false, 0.12f));
            bodyArmors.Add(new Armor("IOTV Gen4 armor (high mobility kit)", 5, 65, ArmorMaterial.Titan, true, false, 0.22f));
            bodyArmors.Add(new Armor("FORT Defender-2 body armor", 5, 70, ArmorMaterial.Steel, true, false, 0.2f));
            bodyArmors.Add(new Armor("IOTV Gen4 armor (assault kit)", 5, 75, ArmorMaterial.Titan, true, true, 0.22f));
            bodyArmors.Add(new Armor("IOTV Gen4 armor (full protection)", 5, 95, ArmorMaterial.Titan, true, true, 0.22f));
            bodyArmors.Add(new Armor("FORT Redut-T5 body armor", 5, 100, ArmorMaterial.Combined, true, true, 0.2f));
            bodyArmors.Add(new Armor("5.11 Hexgrid plate carrier", 6, 50, ArmorMaterial.UHMWPE, false, false, 0.2f));
            bodyArmors.Add(new Armor("LBT 6094A Slick Plate Carrier", 6, 80, ArmorMaterial.Steel, false, false, 0.19f));
            bodyArmors.Add(new Armor("Zhuk-6a heavy armor", 6, 75, ArmorMaterial.Ceramic, true, false, 0.21f));
            bodyArmors.Add(new Armor("6B43 Zabralo-Sh 6A Armor", 6, 85, ArmorMaterial.Combined, true, true, 0.25f));

            return bodyArmors;
        }

        public static Armor getBodyArmor(string name)
        {
            List<Armor> bodyArmors = getBodyArmors();
            name = name.ToLower();

            foreach (Armor bodyArmor in bodyArmors)
            {
                if (bodyArmor.Name.ToLower().Contains(name)) return bodyArmor;
            }

            return null;
        }

        public static List<Armor> getBodyArmor(int ac)
        {
            List<Armor> bodyArmors = getBodyArmors();

            bodyArmors.RemoveAll(x => x.ArmorClass != ac);

            return bodyArmors;
        }

        public static List<Armor> getArmoredRigs()
        {

            List<Armor> bodyArmors = new List<Armor>();

            bodyArmors.Add(new Armor("6B5-16 Zh -86 'Uley' armored rig", 3, 80, ArmorMaterial.Titan, true, false, 0.16f));
            bodyArmors.Add(new Armor("6B3TM-01M armored rig", 4, 40, ArmorMaterial.Titan, true, false, 0.24f));
            bodyArmors.Add(new Armor("6B5-15 Zh -86 'Uley' armored rig", 4, 50, ArmorMaterial.Ceramic, true, false, 0.17f));
            bodyArmors.Add(new Armor("ANA Tactical M2 armored rig", 4, 60, ArmorMaterial.Titan, true, false, 0.15f));
            bodyArmors.Add(new Armor("ANA Tactical M1 armored rig", 4, 65, ArmorMaterial.Steel, true, false, 0.15f));
            bodyArmors.Add(new Armor("Crye Precision AVS platecarrier", 4, 70, ArmorMaterial.Combined, true, false, 0.15f));
            bodyArmors.Add(new Armor("Ars Arma A18 Skanda plate carrier", 4, 80, ArmorMaterial.Combined, false, false, 0.13f));
            bodyArmors.Add(new Armor("Wartech TV-110 plate carrier", 4, 85, ArmorMaterial.Steel, false, false, 0.16f));
            bodyArmors.Add(new Armor("5.11 Tactec plate carrier", 5, 50, ArmorMaterial.UHMWPE, false, false, 0.13f));
            bodyArmors.Add(new Armor("Ars Arma CPC MOD.2 plate carrier", 5, 60, ArmorMaterial.UHMWPE, false, false, 0.11f));

            return bodyArmors;
        }

        public static Armor getArmoredRig(string name)
        {
            List<Armor> bodyArmors = getArmoredRigs();
            name = name.ToLower();

            foreach (Armor bodyArmor in bodyArmors)
            {
                if (bodyArmor.Name.ToLower().Contains(name)) return bodyArmor;
            }

            return null;
        }

        public static List<Armor> getArmoredRig(int ac)
        {
            List<Armor> bodyArmors = getArmoredRigs();

            bodyArmors.RemoveAll(x => x.ArmorClass != ac);

            return bodyArmors;
        }
    

    }

    public enum ArmorMaterial
    {
        Aramid, Combined,UHMWPE,Titan,Aluminium,Steel,Ceramic,Glass
    }

    
}
