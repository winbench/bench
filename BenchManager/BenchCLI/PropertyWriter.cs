using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    public static class PropertyWriter
    {
        public static void WritePropertyValue(object value)
        {
            if (value is string[])
            {
                foreach (var item in (string[])value)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.Write(value);
            }
        }
    }
}
