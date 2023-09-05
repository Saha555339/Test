namespace Test
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string inputString = Console.ReadLine();
            Dictionary<int, string> replaceDels = new() { [3] = "dog", [5] = "cat", [4] = "muzz", [7] = "guzz" };
            Dictionary<List<int>, string> strongRules = new() { [new List<int>() { 3, 5 }] = "good-boy" };
            var replacer = new ResultCheck(replaceDels);
            Console.WriteLine(replacer.Replace(inputString, strongRules));
        }
    }

    internal class ResultCheck
    {
        private Dictionary<int, string> _replaceDels { get; set; }
        public ResultCheck(Dictionary<int, string> replaceDels)
        {
            _replaceDels = replaceDels;
        }

        public string Replace(string s)
        {
                
                var strings = new List<string>();
                var inputValues = s?.Split(", ").Select(int.Parse).ToList();
                foreach (int i in inputValues)
                {
                    strings.Add(DoReplace(i, _replaceDels, 0));
                }

                return string.Join(", ", strings);

        }
        private string DoReplace(int i, Dictionary<int, string> replaceDels, int count_replace)
        {
            var count_dels = count_replace;
            string curr_s = "";

            foreach (int key in replaceDels.Keys)
            {
                if (count_dels == 0)
                {
                    if (i % key == 0)
                    {
                        curr_s += replaceDels[key];
                        count_dels += 1;
                    }
                }
                else
                {
                    if (i % key == 0)
                        curr_s += $"-{replaceDels[key]}";
                }
            }
            if (count_dels == 0)
                curr_s = i.ToString();

            return curr_s;

        }

        private Dictionary<int, string> findNewRules(List<List<int>> new_rules_keys_complited)
        {
            var keysInLists = new_rules_keys_complited.SelectMany(list => list).ToList();

            var resultDict = _replaceDels.Where(kvp => !keysInLists.Contains(kvp.Key))
                                                   .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return resultDict;

        }

        public string Replace(string s, Dictionary<List<int>, string> strongRules)
        {
            var strings = new List<string>();
            var inputValues = s?.Split(", ").Select(int.Parse).ToList();

            foreach (int i in inputValues)
            {
                var count_dels = 0;
                string curr_s = "";
                List<List<int>> new_rules_complited = new();

                foreach (List<int> strong_rule in strongRules.Keys)
                {
                    bool strong_rules_complited = true;
                    int j = 0;
                    while (j<strong_rule.Count && strong_rules_complited)
                    {
                        if (i % strong_rule[j] != 0)
                            strong_rules_complited = false;
                        j++;
                    }
                    if (strong_rules_complited)
                    {
                        new_rules_complited.Add(strong_rule);
                        if (count_dels == 0)
                        {
                            curr_s += strongRules[strong_rule];
                            count_dels++;
                        }
                        else
                            curr_s += $"-{strongRules[strong_rule]}";
                    }

                }
                if (new_rules_complited.Count == 0)
                {
                    strings.Add(DoReplace(i, _replaceDels, count_dels));
                }
                else
                {
                    var new_rules = findNewRules(new_rules_complited);
                    strings.Add(curr_s + DoReplace(i, new_rules, count_dels));
                }
            }

            return string.Join(", ", strings);
        }
    }
}