using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;


namespace TarkovBullets
{
    
    [Binding]
    public sealed class CoreFunction
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private Random rand;
        private Armor helmet;
        private Armor armor;
        private Bullet bullet;
        private int iterations;
        private int[] regionOdds;
        private double damageError;
        private double bulletError;

        public CoreFunction(ScenarioContext scenarioContext)
        {
            rand = new Random();
            helmet = new Armor();
            armor = new Armor();
            bullet = null;
            iterations = 1000;
            regionOdds = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            damageError = 0;
            bulletError = 0;
            _scenarioContext = scenarioContext;
        }

        [Given(@"An error of (.*)% of a bullet")]
        public void GivenAnErrorOfOfABullet(double p0)
        {
            damageError = p0/100;
        }

        [Given(@"An error of (.*) bullets")]
        public void GivenAnErrorOfBullets(double p0)
        {
            bulletError = p0;
        }


        [Given(@"You are shooting (.*)")]
        public void GivenYouAreShooting(string p0)
        {
            bullet = Bullet.getBullet(p0);
        }

        [Given(@"The target is unarmored")]
        public void GivenTheTargetIsUnarmored()
        {
            //Nop, we start without the target wearing armor
        }

        [Given(@"Running the simulation (.*) times")]
        public void GivenRunningTheSimulationTimes(int p0)
        {
            iterations = p0;
        }


        [Given(@"The target is hit in the (.*)")]
        public void GivenTheTargetIsHitIn(string p0)
        {

            Assert.IsNotNull(bullet, "Bullet not Defined");
            switch (p0)
            {
                case "Thorax":
                    regionOdds = new int[] { 0, 1, 0, 0, 0, 0, 0, 0 };
                    break;
                case "Head":
                    regionOdds = new int[] { 1, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                default:
                    Assert.Fail("Hit in region not programmed [" + p0 + "]");
                    break;
            }
        }

        [Given(@"the target is wearing (.*)")]
        [Given(@"The target is wearing (.*)")]
        public void GivenTheTargetIsWearing(string p0)
        {
            //Helmet?
            Armor toWear = Armor.getHelmet(p0);
            if (!(toWear is null))
            {
                helmet = toWear;
            }
            else
            {
                toWear = Armor.getBodyArmor(p0);
                if (!(toWear is null))
                {
                    armor = toWear;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Armor " + p0 + " not found");
                }
            }
        }



        [Then(@"The target should take (.*) damage")]
        public void ThenTheTargetShouldTakeDamage(double p0)
        {
            double totalDamage = 0;
            for (int i = 0; i < iterations; i++)
            {
                Body testBody = new Body(helmet, armor);
                testBody.takeDamage(bullet, Program.randomRegion(regionOdds, rand), rand);

                totalDamage += testBody.damageTaken();
            }

            Assert.AreEqual(p0, totalDamage / iterations,damageError*bullet.Damage, "Damage taken ["+totalDamage/iterations+"] was not equal to the expected ["+p0+"]");
        }


        [Then(@"The Bullet should be unblocked (.*)% of the time")]
        public void ThenTheBulletShouldBeUnlockedOfTheTime(double p0)
        {
            ThenTheBulletShouldBeBlockedOfTheTime(100 - p0);
        }

        [Then(@"The Bullet should be blocked (.*)% of the time")]
        public void ThenTheBulletShouldBeBlockedOfTheTime(double p0)
        {
            int totalBlocks = 0;
            for(int i = 0; i < iterations; i++)
            {
                Body testBody = new Body(helmet, armor);
                Armor hitArmor = testBody.getCoveredArmor(Program.randomRegion(regionOdds, rand));
                if (!(hitArmor is null) && hitArmor.simulateBlock(bullet, rand))
                {
                    totalBlocks++;
                }
            }

            double percentBlocks = (100f * totalBlocks) / iterations;

            Assert.AreEqual(p0, percentBlocks,0.125, "Blocked shots ["+percentBlocks+"] not equal to the expected ["+p0+"]");
        }

    }
}
