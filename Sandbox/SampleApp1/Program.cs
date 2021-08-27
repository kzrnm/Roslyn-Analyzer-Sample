using System;

static void WriteLong(long num) => Console.WriteLine(num);

var i = 1;
WriteLong(1 * 2);
WriteLong(1 << 2);
WriteLong(i * 2);
WriteLong(i << 2);