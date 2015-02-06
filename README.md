# dotNet-StringLogicalComparer

Just few words of code

var array = new[] { "AB1", "AB10", "AB21", "AB3", "AB11", "AB2", "AB20", "AB30", "AB31" };

var comparer = StringComparer.Ordinal;
Array.Sort(array, comparer);

//Result after sort with standart Comparer: AB1, AB10, AB11, AB2, AB20, AB21, AB3, AB30, AB31

var comparer = new StringLogicalComparer();
Array.Sort(array, comparer);

//Result after sort with Logical Comparer: AB1, AB2, AB3, AB10, AB11, AB20, AB21, AB30, AB31

and also it supports floating numbers too

check it out an enjoy ;)
