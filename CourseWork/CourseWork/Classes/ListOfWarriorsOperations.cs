namespace CourseWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The static class to perform operations with the list of warriors.
    /// </summary>
    internal static class ListOfWarriorsOperations
    {
        // обчислення середнього віку серед бійців
        public static double CalculateAverageAge(List<Warrior> warriors)
        {
            double averageAge = 0;
            foreach (var warrior in warriors)
            {
                averageAge += warrior.Age;
            }

            return Math.Round(averageAge / warriors.Count, 2);
        }
        //обчислення кількості озброєння для бійців
        public static int CountAmmunition(List<Warrior> warriors)
        {
            int amountOfAmmunition = 0;
            foreach (var warrior in warriors) amountOfAmmunition += warrior.AmmunitionList.Count;
            return amountOfAmmunition;
        }
        //посортувати бійців за званням
        public static void SortByRank(List<Warrior> warriors)
        {
            int n = warriors.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (warriors[j].Rank < warriors[minIndex].Rank)
                    {
                        minIndex = j;
                    }
                }

                (warriors[minIndex], warriors[i]) = (warriors[i], warriors[minIndex]);
            }
        }

        //отримати список рекрутів
        public static List<Warrior> GetRecruits(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Рекрут)).ToList();
        }
        //отримати список солдатів
        public static List<Warrior> GetSoldiers(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Солдат)).ToList();
        }
        //отримати список сержантів
        public static List<Warrior> GetSergeants(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Сержант)).ToList();
        }
        //отримати список лейтинантів
        public static List<Warrior> GetLieutenants(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Лейтенант)).ToList();
        }
        //отримати список капітанів
        public static List<Warrior> GetCaptains(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Капітан)).ToList();
        }
        //отримати список майорів
        public static List<Warrior> GetMayors(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Майор)).ToList();
        }
        //отримати список підполковників
        public static List<Warrior> GetLieutenantColonels(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Rank.Equals(Ranks.Підполковник)).ToList();
        }
        //визначення бійців, які мають більше однієї одиниці зброї
        public static List<Warrior> GetWithMoreThanOneWeapon(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.MoreThanOneWeapon()).ToList();
        }
        //визначення бійців, які мають АК-47 і гранатомет
        public static List<Warrior> GetWithAk47AndGrenadeGun(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.HaveAk47AndGrenadeGun()).ToList();
        }
        //визначення бійців, які мають пістолет і будь-який автомат
        public static List<Warrior> GetWithGunAndAnyRifle(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.HaveAssaultRifle() && warrior.HaveGun()).ToList();
        }
        //визначення солдат, які не мають озброєння віком до 20 років
        public static List<Warrior> GetSoldiersWithNoWeaponsAndYoungerThan20Years(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Age < 20 && warrior.NoWeapons() && warrior.Rank.Equals(Ranks.Солдат)).ToList();
        }
        //визначення бійців, які мають першу групу крові резус мінус і четверту групу резус плюс віком до 25 років.
        public static List<Warrior> GetWithSpecificBloodGroupAndAge(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.Age < 25 && (warrior.BloodGroup.Equals(BloodGroups.OMinus) || warrior.BloodGroup.Equals(BloodGroups.AbPlus))).ToList();
        }
        //визначити донорів для 1 групи крові
        public static List<Warrior> GetDonorsForFirstBloodGroup(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.BloodGroup.Equals(BloodGroups.OMinus) || warrior.BloodGroup.Equals(BloodGroups.OPlus)).ToList();
        }
        //визначити донорів для 2 групи крові
        public static List<Warrior> GetDonorsForSecondBloodGroup(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.BloodGroup.Equals(BloodGroups.AMinus) || warrior.BloodGroup.Equals(BloodGroups.APlus)).ToList();
        }
        //визначити донорів для 3 групи крові
        public static List<Warrior> GetDonorsForThirdBloodGroup(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.BloodGroup.Equals(BloodGroups.BMinus) || warrior.BloodGroup.Equals(BloodGroups.BPlus)).ToList();
        }
        //визначити донорів для 4 групи крові
        public static List<Warrior> GetDonorsForFourthBloodGroup(List<Warrior> warriors)
        {
            return warriors.Where(warrior => warrior.BloodGroup.Equals(BloodGroups.AbMinus) || warrior.BloodGroup.Equals(BloodGroups.AbPlus)).ToList();
        }
    }
}
