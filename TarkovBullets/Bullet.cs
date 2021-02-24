using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarkovBullets
{
    class Bullet
    {
        private static Dictionary<String, Bullet> allBullets;

        public int Damage;
        public int Penetration;
        public int ArmorDamage; //integer percent (value*100)
        public int Fragmentation;//integer percent (value*100)
        public String Name;

        static Bullet()
        {

            allBullets = new Dictionary<string, Bullet>();
            List<Bullet> bullets = getAllBullets();

            foreach(Bullet bullet in bullets)
            {
                allBullets.Add(bullet.Name, bullet);
            }
        }

        public Bullet(String name, int damage, int penetration, int armorDamage, int fragmentation)
        {
            Damage = damage;
            Penetration = penetration;
            ArmorDamage = armorDamage;
            Fragmentation = fragmentation;
            Name = name;
        }

        public static List<Bullet> getAllBullets()
        {

            List<Bullet> bullets = new List<Bullet>();

            bullets.Add(new Bullet("12x70 5.25mm Buckshot", 37, 1, 15, 0));
            bullets.Add(new Bullet("12x70 8.5 mm Magnum Buckshot", 50, 2, 26, 0));
            bullets.Add(new Bullet("12x70 6.5 mm Express Buckshot", 35, 3, 26, 0));
            bullets.Add(new Bullet("12x70 7mm Buckshot", 39, 3, 26, 0));
            bullets.Add(new Bullet("12x70 Flechette", 25, 31, 26, 0));
            bullets.Add(new Bullet("12x70 RIP", 265, 0, 11, 100));
            bullets.Add(new Bullet("12x70 HP Slug SuperFormance", 220, 2, 12, 39));
            bullets.Add(new Bullet("12x70 Grizzly 40 Slug", 190, 12, 48, 12));
            bullets.Add(new Bullet("12x70 HP Slug Copper Sabot Premier", 206, 14, 46, 38));
            bullets.Add(new Bullet("12x70 Led slug", 167, 15, 55, 20));
            bullets.Add(new Bullet("12x70 Poleva-3 Slug", 140, 17, 40, 20));
            bullets.Add(new Bullet("12x70 Dual Sabot Slug", 85, 17, 65, 10));
            bullets.Add(new Bullet("12x70 FTX Custom LIte Slug", 183, 20, 50, 10));
            bullets.Add(new Bullet("12x70 Poleva-6u Slug", 150, 20, 50, 15));
            bullets.Add(new Bullet("12x70 shell with .50 BMG bullet", 197, 26, 57, 5));
            bullets.Add(new Bullet("12x70 AP-20 Slug", 164, 37, 65, 3));
            bullets.Add(new Bullet("20x70 5.6mm Buckshot", 26, 1, 12, 0));
            bullets.Add(new Bullet("20x70 6.2mm Buckshot", 22, 2, 13, 0));
            bullets.Add(new Bullet("20x70 7.5mm Buckshot", 25, 3, 14, 0));
            bullets.Add(new Bullet("20x70 7.3mm Buckshot", 23, 3, 13, 0));
            bullets.Add(new Bullet("20x70 Devastator Slug", 198, 5, 13, 100));
            bullets.Add(new Bullet("20x70 Slug Poleva-3", 120, 14, 35, 20));
            bullets.Add(new Bullet("20x70 Star Slug", 154, 16, 42, 10));
            bullets.Add(new Bullet("20x70 Slug Poleva-6u", 135, 17, 40, 15));
            bullets.Add(new Bullet("23x75mm Shrapnel-25", 78, 10, 20, 0));
            bullets.Add(new Bullet("23x75mm Shrapnel 10", 87, 11, 20, 0));
            bullets.Add(new Bullet("23x75mm Barricade", 192, 39, 75, 20));
            bullets.Add(new Bullet("9x18 mm PM SP8 gzh", 67, 1, 2, 60));
            bullets.Add(new Bullet("9x18 mm PM SP7 gzh", 77, 2, 5, 2));
            bullets.Add(new Bullet("9x18 mm PM PSV", 69, 3, 5, 40));
            bullets.Add(new Bullet("9x18 mm PM 9 P gzh", 50, 5, 16, 25));
            bullets.Add(new Bullet("9x18 mm PM PSO gzh", 54, 5, 13, 35));
            bullets.Add(new Bullet("9x18 mm PM PS gs PPO", 55, 6, 16, 25));
            bullets.Add(new Bullet("9x18 mm PM PRS gs", 58, 6, 16, 30));
            bullets.Add(new Bullet("9x18 mm PM PPe gzh", 61, 7, 15, 35));
            bullets.Add(new Bullet("9x18 mm PM PPT gzh", 59, 8, 22, 17));
            bullets.Add(new Bullet("9x18 mm PM Pst gzh", 50, 12, 26, 20));
            bullets.Add(new Bullet("9x18 PM mm RG028 gzh", 65, 13, 26, 2));
            bullets.Add(new Bullet("9x18 mm PM 9 BZT gzh", 53, 18, 28, 17));
            bullets.Add(new Bullet("9x18 mm PM PMM", 58, 24, 33, 17));
            bullets.Add(new Bullet("9x18 mm PM PBM", 40, 28, 30, 16));
            bullets.Add(new Bullet("7.62x25mm TT LRNPC", 66, 7, 27, 35));
            bullets.Add(new Bullet("7.62x25mm TT LRN", 64, 8, 28, 35));
            bullets.Add(new Bullet("7.62x25mm TT FMJ43", 60, 11, 29, 25));
            bullets.Add(new Bullet("7.62x25mm TT AKB", 58, 12, 32, 25));
            bullets.Add(new Bullet("7.62x25mm TT P gl", 58, 14, 32, 25));
            bullets.Add(new Bullet("7.62x25mm TT PT gzh", 55, 18, 34, 17));
            bullets.Add(new Bullet("7.62x25mm TT Pst gzh", 50, 25, 36, 20));
            bullets.Add(new Bullet("9x19 mm RIP", 102, 2, 11, 100));
            bullets.Add(new Bullet("9x19 mm QuakeMaker", 85, 8, 22, 10));
            bullets.Add(new Bullet("9x19 mm PSO gzh", 59, 10, 32, 25));
            bullets.Add(new Bullet("9x19 mm Luger CCI", 70, 10, 38, 25));
            bullets.Add(new Bullet("9x19 mm Green Tracer", 58, 14, 33, 15));
            bullets.Add(new Bullet("9x19 mm Pst gzh", 54, 20, 33, 15));
            bullets.Add(new Bullet("9x19 mm AP 6.3", 52, 30, 48, 5));
            bullets.Add(new Bullet("9x19 mm 7N31", 52, 39, 53, 5));
            bullets.Add(new Bullet(".45 RIP", 127, 3, 12, 100));
            bullets.Add(new Bullet(".45 ACP Hydra-Shok", 95, 13, 30, 50));
            bullets.Add(new Bullet(".45 ACP Lasermatch FMJ", 74, 19, 37, 1));
            bullets.Add(new Bullet(".45 ACP FMJ", 69, 23, 36, 1));
            bullets.Add(new Bullet(".45 ACP AP", 70, 36, 43, 1));
            bullets.Add(new Bullet("9x21 mm SP12", 80, 15, 63, 35));
            bullets.Add(new Bullet("9x21 mm SP11", 65, 18, 44, 30));
            bullets.Add(new Bullet("9x21 mm SP10", 49, 35, 46, 20));
            bullets.Add(new Bullet("9x21 mm SP13", 63, 39, 47, 20));
            bullets.Add(new Bullet("5.7x28 mm R37.F", 98, 8, 7, 100));
            bullets.Add(new Bullet("5.7x28 mm SS198LF", 74, 10, 15, 80));
            bullets.Add(new Bullet("5.7x28 mm R37.X", 81, 11, 14, 70));
            bullets.Add(new Bullet("5.7x28 mm SS197SR", 62, 20, 22, 50));
            bullets.Add(new Bullet("5.7x28 mm L191", 58, 33, 41, 20));
            bullets.Add(new Bullet("5.7x28 mm SB193", 54, 35, 37, 20));
            bullets.Add(new Bullet("5.7x28 mm SS190", 49, 37, 43, 20));
            bullets.Add(new Bullet("4.6x30mm Action SX", 65, 18, 39, 50));
            bullets.Add(new Bullet("4.6x30mm Subsonic SX", 45, 36, 46, 20));
            bullets.Add(new Bullet("4.6x30mm FMJ SX", 43, 40, 41, 20));
            bullets.Add(new Bullet("4.6x30mm AP SX", 35, 53, 46, 10));
            bullets.Add(new Bullet("9x39 mm SP-5", 68, 38, 52, 20));
            bullets.Add(new Bullet("9x39 mm SP-6", 58, 46, 60, 10));
            bullets.Add(new Bullet("9x39 mm 7N9 SPP", 64, 50, 56, 20));
            bullets.Add(new Bullet("9x39 mm 7N12 BP", 60, 55, 68, 10));
            bullets.Add(new Bullet(".366 TKM Geksa", 110, 14, 38, 45));
            bullets.Add(new Bullet(".366 TKM FMJ", 98, 23, 48, 25));
            bullets.Add(new Bullet(".366 TKM EKO", 73, 30, 40, 20));
            bullets.Add(new Bullet(".366 AP", 90, 42, 60, 1));
            bullets.Add(new Bullet("5.45x39 mm SP", 68, 11, 34, 45));
            bullets.Add(new Bullet("5.45x39 mm HP", 74, 11, 20, 35));
            bullets.Add(new Bullet("5.45x39 mm PRS", 60, 14, 28, 30));
            bullets.Add(new Bullet("5.45x39 mm US", 65, 15, 34, 10));
            bullets.Add(new Bullet("5.45x39 mm FMJ", 54, 20, 30, 25));
            bullets.Add(new Bullet("5.45x39 mm T", 57, 20, 38, 16));
            bullets.Add(new Bullet("5.45x39 mm PS", 50, 25, 35, 40));
            bullets.Add(new Bullet("5.45x39 mm PP", 46, 30, 32, 17));
            bullets.Add(new Bullet("5.45x39 mm BP", 48, 32, 41, 16));
            bullets.Add(new Bullet("5.45x39 mm BT", 44, 37, 49, 16));
            bullets.Add(new Bullet("5.45x39 mm BS", 40, 51, 57, 17));
            bullets.Add(new Bullet("5.45x39 mm 7N39 Igolnik", 37, 62, 60, 2));
            bullets.Add(new Bullet("5.56x45 mm Warmage", 85, 3, 14, 90));
            bullets.Add(new Bullet("5.56x45 mm 55 HP", 75, 9, 22, 70));
            bullets.Add(new Bullet("5.56x45 mm Mk 255 Mod 0", 60, 17, 32, 3));
            bullets.Add(new Bullet("5.56x45 mm M856", 55, 23, 34, 33));
            bullets.Add(new Bullet("5.56x45 mm 55 FMJ", 52, 24, 33, 50));
            bullets.Add(new Bullet("5.56x45 mm M855", 50, 30, 37, 40));
            bullets.Add(new Bullet("5.56x45 mm M856A1", 51, 37, 52, 33));
            bullets.Add(new Bullet("5.56x45 mm M855A1", 45, 43, 52, 34));
            bullets.Add(new Bullet("5.56x45 mm M995", 40, 53, 58, 32));
            bullets.Add(new Bullet(".300 BPZ AAC Blackout", 60, 28, 36, 30));
            bullets.Add(new Bullet(".300 AAC Blackout AP", 55, 44, 60, 30));
            bullets.Add(new Bullet("7.62x39 mm HP", 87, 15, 35, 26));
            bullets.Add(new Bullet("7.62x39 mm US", 56, 29, 42, 8));
            bullets.Add(new Bullet("7.62x39 mm T45M", 62, 30, 46, 12));
            bullets.Add(new Bullet("7.62x39 mm PS", 57, 32, 52, 25));
            bullets.Add(new Bullet("7.62x39 mm BP", 58, 47, 63, 12));
            bullets.Add(new Bullet("7.62x51 mm Ultra Nosler", 107, 15, 20, 70));
            bullets.Add(new Bullet("7.62x51 mm BPZ FMJ", 88, 31, 33, 25));
            bullets.Add(new Bullet("7.62x51 mm TPZ SP", 60, 36, 40, 20));
            bullets.Add(new Bullet("7.62x51 mm M80", 80, 41, 66, 17));
            bullets.Add(new Bullet("7.62x51 mm M62", 79, 44, 75, 14));
            bullets.Add(new Bullet("7.62x51 mm M61", 70, 64, 83, 13));
            bullets.Add(new Bullet("7.62x51 mm M993", 67, 70, 85, 13));
            bullets.Add(new Bullet("7.62x54R T-46M", 82, 41, 83, 18));
            bullets.Add(new Bullet("7.62x54R LPS Gzh", 81, 42, 78, 18));
            bullets.Add(new Bullet("7.62x54R 7N1 Sniper cartridge", 86, 45, 84, 8));
            bullets.Add(new Bullet("7.62x54R 7BT1", 78, 59, 87, 8));
            bullets.Add(new Bullet("7.62x54R SNB", 75, 62, 87, 8));
            bullets.Add(new Bullet("7.62x54R 7N37", 72, 70, 88, 8));
            bullets.Add(new Bullet(".338 Lapua Magnum TAC-X", 196, 18, 55, 50));
            bullets.Add(new Bullet(".338 UPZ Lapua Magnum", 142, 32, 70, 60));
            bullets.Add(new Bullet(".338 Lapua Magnum FMJ", 122, 47, 83, 20));
            bullets.Add(new Bullet(".338 Lapua Magnum AP", 115, 79, 89, 13));
            bullets.Add(new Bullet("12.7x55 mm PS12A", 165, 10, 22, 70));
            bullets.Add(new Bullet("12.7x55 mm PS12S", 115, 28, 60, 30));
            bullets.Add(new Bullet("12.7x55 mm PS12B", 102, 46, 57, 30));

            return bullets;
        }

        public static Bullet getBullet(string name)
        {
            try
            {
                return allBullets[name];
                
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentOutOfRangeException("Bullet " + name + " not found");
            }
        }


    }
}
