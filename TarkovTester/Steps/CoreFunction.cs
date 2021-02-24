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

        public CoreFunction(ScenarioContext scenarioContext)
        {
            rand = new Random();
            helmet = new Armor();
            armor = new Armor();
            bullet = null;
            iterations = 1000;
            regionOdds = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            _scenarioContext = scenarioContext;
        }

        [Then(@"The Bullet should be blocked (.*)% of the time")]
        public void ThenTheBulletShouldBeBlockedOfTheTime(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"You are shooting (.*)")]
        public void GivenYouAreShooting(string p0)
        {
            List<Bullet> bullets = Bullet.getAllBullets();

            foreach(Bullet b in bullets)
            {
                if (b.Name.Equals(p0))
                {
                    bullet = b;
                    break;
                }
            }
        }

        [Given(@"The target is unarmored")]
        public void GivenTheTargetIsUnarmored()
        {
            //Nop, we start without the target wearing armor
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
                default:
                    Assert.Fail("Hit in region not programmed [" + p0 + "]");
                    break;
            }
        }


        [Then(@"The target should take (.*) damage")]
        public void ThenTheTargetShouldTakeDamage(int p0)
        {
            int totalDamage = 0;
            for(int i = 0; i < iterations; i++)
            {
                Body testBody = new Body(helmet, armor);
                testBody.takeDamage(bullet, Program.randomRegion(regionOdds, rand), rand);

                totalDamage += testBody.damageTaken();
            }

            Assert.AreEqual(p0, totalDamage / iterations, "Damage taken was not equal to the expected");
        }

    }
}
