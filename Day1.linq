<Query Kind="Statements" />

string[] allLines = File.ReadAllLines(@"C:\Source\input.txt");
var allLines2 = allLines.Select(x => GetFuelCost(Convert.ToDouble(x)));
var sum = allLines2.Sum();
sum.Dump();


double GetFuelCost(double mass){
	var loopflag = true;
	double calculatedFuel = 0;
	while (loopflag){
		var tempFuel = Math.Floor((mass)/3) - 2;
		if(tempFuel <= 0) return calculatedFuel;
		else{
		 calculatedFuel += tempFuel;
		 mass = tempFuel;
		}
	}
	return 0;
}