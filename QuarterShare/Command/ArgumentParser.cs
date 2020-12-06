using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{
    class ArgumentParser
    {
        public Dictionary<string, string> Values { get; }

        public ArgumentParser(string[] args)
        {
            Values = args.ToDictionary(
                 k => k.Split(new char[] { ':' }, 2)[0].ToLower(),
                 v => v.Split(new char[] { ':' }, 2).Count() > 1 ? v.Split(new char[] { ':' }, 2)[1] : "");
        }

        public void AddDefault(string val, string def)
        {
            if (Values.ContainsKey(val) != true)
                Values[val] = def;
        }
    }
}
