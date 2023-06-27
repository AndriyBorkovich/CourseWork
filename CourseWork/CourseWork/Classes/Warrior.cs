namespace CourseWork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    //перелічення зі званнями в батальйоні
    public enum Ranks
    {
        Рекрут = 1,
        Солдат = 2,
        Сержант = 3,
        Лейтенант = 4,
        Капітан = 5,
        Майор = 6,
        Підполковник = 7
    }

    //перелічення з групами крові (разом з резус-факторами)
    public enum BloodGroups
    {
        // 1 група
        [Display(Name = "O-")] OMinus = -1,
        [Display(Name = "O+")] OPlus = +1,
        
        // 2 група
        [Display(Name = "A-")] AMinus = -2,
        [Display(Name = "A+")] APlus = +2,
        
        // 3 група
        [Display(Name = "B-")] BMinus = -3,
        [Display(Name = "B+")] BPlus = +3,
        
        // 4 група
        [Display(Name = "AB-")] AbMinus = -4,
        [Display(Name = "AB+")] AbPlus = +4
    }
    //клас Боєць
    class Warrior
    {
        public string Name { get; set; } //ім'я бійця
        public string Surname { get; set; } //прізвище бійця
        public int Age { get; set; } //вік бійця
        public BloodGroups BloodGroup { get; set; } //група крові
        public List<string> AmmunitionList { get; set; } //боєкомплект (список зброї)
        public Ranks Rank { get; set; } //звання бійця в батальйоні
        public string AmmunitionListAsString { get; set; } //боєкомплект у вигляді стрічки (для коректного відображення у таблиці)
        public string BloodGroupAsString { get; set; } //група крові у вигляді стрічки (для коректного відображення у таблиці)

        /// <summary>
        /// Initializes a new instance of the <see cref="Warrior"/> class.
        /// </summary>
        public Warrior()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Age = 18;
            BloodGroup = BloodGroups.OPlus;
            AmmunitionList = new List<string>();
            Rank = Ranks.Рекрут;
        }
        //конструктор з параметрами
        public Warrior(string name, string surname, int age,
            BloodGroups bloodGroup, List<string> ammunitionList, Ranks rank)
        {
            Name = name;
            Surname = surname;
            Age = age;
            BloodGroup = bloodGroup;
            var bloodGroupAttribute = BloodGroup.GetDisplayAttributesFrom(typeof(BloodGroups));
            BloodGroupAsString = bloodGroupAttribute.Name;
            AmmunitionList = ammunitionList;
            SetAmmunitionListForTable();
            Rank = rank;
        }

        /// <summary>
        /// Copy constructor that initializes a new instance of the <see cref="Warrior"/> class.
        /// </summary>
        /// <param name="warrior">
        /// The warrior.
        /// </param>
        public Warrior(ref Warrior warrior)
        {
            this.Name = warrior.Name;
            this.Surname = warrior.Surname;
            this.Age = warrior.Age;
            this.BloodGroup = warrior.BloodGroup;
            var bloodGroupAttribute = this.BloodGroup.GetDisplayAttributesFrom(typeof(BloodGroups));
            this.BloodGroupAsString = bloodGroupAttribute.Name;
            this.AmmunitionList = warrior.AmmunitionList;
            this.SetAmmunitionListForTable();
            this.Rank = warrior.Rank;
        }

        //формуємо стрічку з боєкомлекту
        public void SetAmmunitionListForTable()
        {
            this.AmmunitionListAsString = string.Join(", ", AmmunitionList.ToArray());//розділяємо кожен вид зброї з списку боєкомлекту комою
        }
       
        //перевірка чи боєць має більше 1 одиниці зброї в своєму боєкомплекті
        public bool MoreThanOneWeapon()
        {
            return AmmunitionList.Count > 1;
        }

        //перевірка чи боєць не має взагалі зброї в своєму боєкомплекті
        public bool NoWeapons()
        {
            return AmmunitionList.Count == 0;
        }

        //перевірка наявності у бійця будь-якого автомата
        public bool HaveAssaultRifle()
        {
            //масив можливих автоматів
            string[] rifles =
            {
                "АК-47", "АКМ", "РПК", "АК-74",
                "АК-101", "АК-103", "АК-105", "ПКМ", "ПКТ", "М16", "М4"
            };
            bool isRifle = false;
            foreach (var _ in from rifle in rifles
                              where this.AmmunitionList.Contains(rifle)
                              select new { })
            {
                isRifle = true;
            }

            return isRifle;
        }

        //перевірка наявності у бійця пістолета
        public bool HaveGun()
        {
            return AmmunitionList.Contains("Пістолет");
        }

        //перевірка наявності у бійця і АК-47 і гранатомета
        public bool HaveAk47AndGrenadeGun()
        {
            return AmmunitionList.Contains("АК-47") && AmmunitionList.Contains("Гранатомет");
        }

        //перевантаження оператора для виводу у файл 1 бійця
        public static bool operator +(Warrior warrior, string filePath)
        {
            string warriorLine = string.Empty;//стрічка (рядок), яка зберігатиме дані про бійця
            warriorLine += $"{warrior.Surname,14}\t {warrior.Name,12}\t {warrior.Age,2}\t{warrior.BloodGroupAsString,4}\t{warrior.Rank,16}\t{warrior.AmmunitionListAsString,25}\n";
            File.AppendAllText(filePath, warriorLine);
            return true;
        }
        //перевантаження оператора для зчитування з файлу списку бійців
        public static List<Warrior> operator -(Warrior warrior, string filePath)
        {
            List<Warrior> warriors = new();
            List<string> lines = File.ReadAllLines(filePath).ToList();//формуємо список зчитаних рядків
            int lineCounter = 0;
            if (lines.Count == 0)
            {
                throw new FileLoadException("Файл пустий!\nВиберіть інший файл!");
            }

            foreach (var line in lines)
            {
                warrior = new Warrior();
                string[] entries = line.Split(' '); //формуємо масив стрічок в рядку, які розділені пробілом 
                if (!entries[0].All(char.IsLetter) && !entries[0].Contains("'"))
                {
                    throw new FormatException(
                        $"Surname in row {lineCounter + 1} contains banned symbols!\nPlease, edit this file or choose other");
                }

                if ((!entries[1].All(char.IsLetter)) && !entries[1].Contains("'"))
                {
                    throw new FormatException(
                        $"Name in row {lineCounter + 1} contains banned symbols!\nPlease, edit this file or choose other");
                }

                warrior.Surname = entries[0];
                warrior.Name = entries[1];
                warrior.Age = Convert.ToInt32(entries[2]);
                if (warrior.Age < 18 || warrior.Age > 65)
                {
                    throw new FormatException(
                        $"Age of warrior {warrior.Surname} {warrior.Name} is not acceptable for army!");
                }

                warrior.BloodGroup = (BloodGroups)Enum.Parse(typeof(BloodGroups), entries[3]);
                
                // доступаємось до атрибуту Name для поточної групи крові
                DisplayAttribute bloodGroup = warrior.BloodGroup.GetDisplayAttributesFrom(typeof(BloodGroups));
                warrior.BloodGroupAsString = bloodGroup.Name;
                warrior.Rank = (Ranks)Enum.Parse(typeof(Ranks), entries[4]);
                
                if (entries[5] == string.Empty)
                {
                    warrior.AmmunitionList.Capacity = 0;
                }
                else
                {
                    warrior.AmmunitionList = entries[5].Split(',').ToList();
                    warrior.SetAmmunitionListForTable();
                }

                warriors.Add(new Warrior(ref warrior));
                lineCounter++;
            }

            return warriors;
        }
    }
}
