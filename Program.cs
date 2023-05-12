using OOP__lab10;
using lab13_malts;
using System.Diagnostics;
using lab12;

namespace lab14_malts
{
    public static class Program
    {
        public static List<Stack<Engine>> collection = new List<Stack<Engine>>();
        static void Main(string[] args)
        {
            FillStack(5);
            PrintCollection();

            Request1Linq();
            Request1Ext();
             
            Request2Linq();
            Request2Ext();

            Request3Linq();
            Request3Ext();

            Request4Linq();
            Request4Ext();

            Request5Linq();
            Request5Ext();

            SecondTask();
        }

        public static void AddStack(int count)
        {
            var stck = new Stack<Engine>();
            for (int i = 0; i < count; ++i)
            {
                Engine toAdd;
                switch ((AddMenu)new Random().Next(1, 5))
                {
                    case AddMenu.Engine:
                        {
                            toAdd =  new Engine().RandomInit();
                            stck.Push(toAdd);
                            break;
                        }
                    case AddMenu.InternalCombustionEngine:
                        {
                            toAdd = new InternalCombustionEngine().RandomInit();
                            stck.Push(toAdd);
                            break;
                        }
                    case AddMenu.DiselEngine:
                        {
                            toAdd = new DiselEngine().RandomInit();
                            stck.Push(toAdd);
                            break;
                        }
                    case AddMenu.TurbojetEngine:
                        {
                            toAdd = new TurbojetEngine().RandomInit();
                            stck.Push(toAdd);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            collection.Add(stck);
        }

        public static void FillStack(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                AddStack(count);
            }
        }

        public static void PrintCollection()
        {
            Console.WriteLine("{");
            foreach (Stack<Engine> stck in collection)
            {
                Console.WriteLine("\t[");
                foreach (Engine engine in stck)
                {
                    Console.WriteLine("\t\t" + engine.ToString());
                }
                Console.WriteLine("\t]\n");
            }
            Console.WriteLine("}");
            DrawLine();
        }
        public static void DrawLine()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------");
        }

        public static void Request1Linq()
        {
            Console.WriteLine("Все двигатели весом больше 5000 кг\t\tLinq");
            var ans = from stck in collection from engine in stck where engine.Weight > 5000 select engine;
            foreach (Engine engine in ans)
            {
                Console.WriteLine(engine.ToString());
            }
            DrawLine();
        }
        public static void Request1Ext()
        {
            Console.WriteLine("Все двигатели весом больше 5000 кг\t\tExtenssion");
            var ans = collection.SelectMany(stck => stck).Select(engine => engine).Where(engine => engine.Weight > 5000);
            foreach (var engine in ans)
            {
                Console.WriteLine(engine.ToString());
            }
            DrawLine();
        }
        public static void Request2Linq()
        {
            Console.WriteLine("Получение количества дизельных двигателей\t\tLinq");
            var ans = (from stck in collection from engine in stck where engine is DiselEngine select engine).Count();
            Console.WriteLine(ans);
            DrawLine();
        }
        public static void Request2Ext()
        {
            Console.WriteLine("Получение количества дизельных двигателей\t\tExtenssion");
            var ans = collection.SelectMany(stck => stck).Select(engine => engine).Where(engine => engine is DiselEngine).Count();
            Console.WriteLine(ans);
            DrawLine();
        }
        public static void Request3Linq()
        {
            Console.WriteLine("Пересечение двигателей с весом больше 3333 и меньше 6666\t\tLinq");
            var ans = (from stck in collection from engine in stck where engine.Weight > 3333 select engine).Intersect
                (from stck in collection from engine in stck where engine.Weight < 6666 select engine);
            foreach (var item in ans)
            {
                Console.WriteLine(item.ToString());
            }
            DrawLine();
        }
        public static void Request3Ext()
        {
            Console.WriteLine("Пересечение двигателей с весом больше 3333 и меньше 6666\t\tExtenssion");
            var ans = collection.SelectMany(stck => stck).Select(engine => engine).Where(engine => engine.Weight > 3333).Intersect
                (collection.SelectMany(stck => stck).Select(engine => engine).Where(engine => engine.Weight < 6666));
            foreach (var item in ans)
            {
                Console.WriteLine(item.ToString());
            }
            DrawLine();
        }
        public static void Request4Linq()
        {
            Console.WriteLine("Средний вес двигателй\t\tLinq");
            var ans = (from stck in collection 
                       from engine in stck 
                       select engine.Weight)
                       .Average();
            Console.WriteLine(ans);
            DrawLine();
        }
        public static void Request4Ext()
        {
            Console.WriteLine("Средний вес двигателй\t\tExtenssion");
            var ans = collection.SelectMany(stck => stck)
                .Select(engine => engine.Weight)
                .Average();
            Console.WriteLine(ans);
            DrawLine();
        }
        public static void Request5Linq()
        {
            Console.WriteLine("Группировка дизельных двигателей по тиу топлива\t\tLinq");
            var ans = from stck in collection from engine in stck where engine is DiselEngine group engine by ((DiselEngine)engine).fuel;
            foreach (IGrouping<string, Engine> engines in ans)
            {
                Console.WriteLine($"fuel: {engines.Key}");
                Console.WriteLine("\t{");
                foreach (var item in engines)
                {
                    Console.WriteLine("\t\t" + item.ToString());
                }
                Console.WriteLine("\t}");
            }
            DrawLine();
        }
        public static void Request5Ext()
        {
            Console.WriteLine("Группировка дизельных двигателей по тиу топлива\t\tExtenssion");
            var ans = collection.SelectMany(stck => stck).Select(engine => engine).Where(engine => engine is DiselEngine ? true : false).OrderBy(engine => (DiselEngine)engine).GroupBy(engine => ((DiselEngine)engine).fuel);
            foreach (IGrouping<string, Engine> engines in ans)
            {
                Console.WriteLine($"fuel: {engines.Key}");
                Console.WriteLine("\t{");
                foreach (var item in engines)
                {
                    Console.WriteLine("\t\t" + item.ToString());
                }
                Console.WriteLine("\t}");
            }
            DrawLine();
        }
        public static IEnumerable<T> Select<T>(this MyNewCollection source, Func<Engine, T> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                var res = new List<T>();
                foreach (Engine item in source)
                {
                    res.Add(selector(item));
                }
                return res;
            }
        }
        public static int Max(this MyNewCollection source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                var max = 0;
                foreach (var item in source)
                {
                    if (item.Weight > max)
                        max = item.Weight;
                }
                return max;
            }
        }


        public static void OrderBy(this MyNewCollection source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                QuickSort(source, 0, source.Count() - 1, (Engine t1, Engine t2) => t1.Weight < t2.Weight);
            }
        }
        public delegate bool comparator(Engine e1, Engine e2);
        public static void QuickSort(MyNewCollection source, int leftIndex, int rightIndex, comparator compare)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = source[leftIndex];

            while (i <= j)
            {
                while (compare(source[i], pivot))
                {
                    i++;
                }

                while (compare(pivot, source[j]))
                {
                    j--;
                }

                if (i <= j)
                {
                    var temp = source[i];
                    source[i] = source[j];
                    source[j] = temp;
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
            {
                QuickSort(source, leftIndex, j, compare);
            }

            if (i < rightIndex)
            {
                QuickSort(source, i, rightIndex, compare);
            }
        }
        public static void SecondTask()
        {
            MyNewCollection coll = new MyNewCollection("Coll new 1");
            coll.Fill(10);
            Console.WriteLine("MyNewCollection: " + coll.Name);
            foreach (var item in coll)
            {
                Console.WriteLine(item.ToString());
            }
            DrawLine();

            var maxWeight = (from engine in coll select engine.Weight).Max();
            Console.WriteLine($"Наибольший вес двигателя: {maxWeight}");
            DrawLine();

            Console.WriteLine("Всде двигатели с весом больше 5000");
            var ans = from engine in coll where engine.Weight > 5000 select engine;
            foreach (var item in ans)
            {
                Console.WriteLine(item.ToString());
            }
            DrawLine();

            Console.WriteLine("Сортировка двигателей по возрастанию веса");
            coll.OrderBy();
            foreach (var item in coll)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }           
}