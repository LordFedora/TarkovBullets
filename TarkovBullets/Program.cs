using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarkovBullets
{
    class Program
    {
        private static int HEAD_ODDS = 0;
        private static int THORAX_ODDS = 2;
        private static int RARM_ODDS = 0;
        private static int LARM_ODDS = 1;
        private static int RLEG_ODDS = 2;
        private static int LLEG_ODDS = 1;
        private static int STOMACH_ODDS = 7;
        private static int MISS_ODDS = 7;
        private static List<int[]> ODDS_ARRAYS = new List<int[]>();

        static void Main(string[] args)
        {
            ODDS_ARRAYS.Add(new int[] { HEAD_ODDS, THORAX_ODDS, RARM_ODDS, LARM_ODDS, RLEG_ODDS, LLEG_ODDS, STOMACH_ODDS, MISS_ODDS }); //Wayne's Accuracy
            ODDS_ARRAYS.Add(new int[] { 1, 0, 0, 0, 0, 0, 0, 0 });//Always Headshots
            ODDS_ARRAYS.Add(new int[] { 0, 1, 0, 0, 0, 0, 0, 0 });//Always Thorax
            int[] ODDS_ARRAY = ODDS_ARRAYS[0];

            vsAllInClass("9x19 mm Pst gzh",4, ODDS_ARRAY);
            //singleSim("9x19 mm Pst gzh", "BNTI Gzhel-K armor");
            //testAllCases(ODDS_ARRAY);
        }




        public static Body.BodyRegions randomRegion(int[] regionOdds, Random rand)
        {

            if (regionOdds.Count() != 8) return Body.BodyRegions.Miss; //closest to an error state, will cause most "until death" loops to hang forever

            int headOdds = regionOdds[0];
            int thoraxOdds = regionOdds[1];
            int rArmOdds = regionOdds[2];
            int lArmOdds = regionOdds[3];
            int rLegOdds = regionOdds[4];
            int lLegOdds = regionOdds[5];
            int stomachOdds = regionOdds[6];
            int missOdds = regionOdds[7];


            int totalOdds = headOdds + thoraxOdds + rArmOdds + rLegOdds + lArmOdds + lLegOdds + stomachOdds + missOdds; 

            if (totalOdds == 1)
            {
                if (headOdds != 0) return Body.BodyRegions.Head;
                if (thoraxOdds != 0) return Body.BodyRegions.Thorax;
                if (rArmOdds != 0) return Body.BodyRegions.RArm;
                if (lArmOdds != 0) return Body.BodyRegions.LArm;
                if (rLegOdds != 0) return Body.BodyRegions.RLeg;
                if (lLegOdds != 0) return Body.BodyRegions.LLeg;
                if (stomachOdds != 0) return Body.BodyRegions.Stomach;
                return Body.BodyRegions.Miss; //only way we could be here
            }


            int randomNumber = rand.Next(totalOdds);

            if (headOdds != 0)
            {
                randomNumber -= headOdds; //did we hit the region?
                if (randomNumber < 0) return Body.BodyRegions.Head; //yes!
                                                                    //repeat for all regions :/
            }


            if (headOdds != 0)
            {
                randomNumber -= thoraxOdds;
                if (randomNumber < 0) return Body.BodyRegions.Thorax;
            }

            if (rArmOdds != 0)
            {
                randomNumber -= rArmOdds;
                if (randomNumber < 0) return Body.BodyRegions.RArm;
            }

            if (lArmOdds != 0)
            {
                randomNumber -= lArmOdds;
                if (randomNumber < 0) return Body.BodyRegions.LArm;
            }

            if (stomachOdds != 0)
            {
                randomNumber -= stomachOdds;
                if (randomNumber < 0) return Body.BodyRegions.Stomach;
            }

            if (rLegOdds != 0)
            {
                randomNumber -= rLegOdds;
                if (randomNumber < 0) return Body.BodyRegions.RLeg;
            }

            if (lLegOdds != 0)
            {
                randomNumber -= lLegOdds;
                if (randomNumber < 0) return Body.BodyRegions.LLeg;
            }

            return Body.BodyRegions.Miss; //can't be anthing else
        }

        public static void singleSim(string bulletName, string armorName)
        {
            Random rand = new Random();

            String docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Output.txt")))
            {
                Bullet bullet = Bullet.getBullet(bulletName);
                Armor armor = Armor.getBodyArmor(armorName);

                int iters = 1000000;

                for (int cycle = 0; cycle < 10000; cycle++)
                {
                    int count = 0;
                    for (int iteration = 0; iteration < iters; iteration++)
                    {
                        armor.resetArmor();
                        if (armor.simulateBlock(bullet, rand))
                        {
                            count++;
                        }
                    }
                    if (cycle % 100 == 0) Console.WriteLine(cycle / 100);
                    outputFile.WriteLine(count/(iters/100));
                }
            }

        }

        public static void vsAllInClass(string bulletName, int armorClass, int[] hitRegions)
        {
            Random rand = new Random();

            String docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Output.txt")))
            {
                Bullet bullet = Bullet.getBullet(bulletName);
                List<Armor> armors = Armor.getBodyArmor(armorClass);
                foreach (Armor armor in armors)
                {

                    double count = 0;
                    for (int i = 0; i < 1000; i++)
                    {

                        Body body = new Body(new Armor(), armor);


                        while (!body.isDead())
                        {
                            body.takeDamage(bullet, randomRegion(hitRegions, rand), rand);
                            count++;
                        }
                    }
                    outputFile.Write(armor.Name + " | ");
                    outputFile.WriteLine(count / 1000);
                }
            }
        }

        public static void testAllCases(int[] ODDS_ARRAY)
        {
            Random rand = new Random();

            String docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Output.txt")))
            {

                List<Armor> helmets = new List<Armor>();
                helmets.Add(new Armor()); //nothing
                helmets.Add(Armor.getHelmet("Kolpak-1S"));
                helmets.Add(Armor.getHelmet("6B47 Ratnik"));
                helmets.Add(Armor.getHelmet("Fast MT SUPER HIGH CUT"));
                helmets.Add(Armor.getHelmet("Altyn helmet"));
                helmets.Add(Armor.getVisor("Altyn face shield"));
                helmets.Add(Armor.getHelmet("Vulkan-5"));
                helmets.Add(Armor.getVisor("Maska 1Sch face shield"));



                List<Armor> bodyArmors = new List<Armor>();
                bodyArmors.Add(new Armor()); //also nothing
                bodyArmors.Add(Armor.getBodyArmor("PACA"));
                bodyArmors.Add(Armor.getBodyArmor("UNTAR"));
                bodyArmors.Add(Armor.getBodyArmor("Kirasa"));
                bodyArmors.Add(Armor.getBodyArmor("Trooper"));
                bodyArmors.Add(Armor.getBodyArmor("6B23-2"));
                bodyArmors.Add(Armor.getBodyArmor("Korund"));
                bodyArmors.Add(Armor.getBodyArmor("Redut-T5"));
                bodyArmors.Add(Armor.getBodyArmor("Hexgrid"));
                bodyArmors.Add(Armor.getBodyArmor("Zabralo-Sh"));


                List<Bullet> bullets = Bullet.getAllBullets();


                //TODO, use json to pull all these numbers out into factory class

                foreach (Bullet bullet in bullets)
                {
                    foreach (Armor helmet in helmets)
                    {
                        foreach (Armor bodyArmor in bodyArmors)
                        {

                            int count = 0;
                            for (int i = 0; i < 1000; i++)
                            {

                                Body body = new Body(helmet, bodyArmor);


                                while (!body.isDead())
                                {
                                    body.takeDamage(bullet, randomRegion(ODDS_ARRAY, rand), rand);
                                    count++;
                                }
                            }


                            Console.Write(bullet.Name);
                            outputFile.Write(bullet.Name);
                            Console.Write("|");
                            outputFile.Write("|");
                            Console.Write(helmet.Name);
                            outputFile.Write(helmet.Name);
                            Console.Write("|");
                            outputFile.Write("|");
                            Console.Write(bodyArmor.Name);
                            outputFile.Write(bodyArmor.Name);
                            Console.Write("|");
                            outputFile.Write("|");
                            Console.WriteLine(count / 1000f);
                            outputFile.WriteLine(count / 1000f);
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}