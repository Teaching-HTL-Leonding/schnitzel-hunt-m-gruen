// Schnitzel Hunt
// https://github.com/Teaching-HTL-Leonding/schnitzel-hunt-m-gruen

#region Main Program
Console.OutputEncoding = System.Text.Encoding.Default;

#region Constants
const string APPETIZERS = "APPETIZERS";
const string MAIN_DISHES = "MAIN DISHES";
const string DESSERTS = "DESSERTS";
#endregion

System.Console.WriteLine("WHERE TO GET SCHNITZEL?");
System.Console.WriteLine("=========================");

string currentDirectory = $"{System.IO.Directory.GetCurrentDirectory()}\\MenuCards";

string[] menuCards = Directory.GetFiles(currentDirectory, "*.txt");

string cheapestSeitanSchnitzelRestaurant = string.Empty;
string mostExpensiveSeitanSchnitzelRestaurant = string.Empty;
decimal cheapestSeitanSchnitzelPrice = 0;
decimal mostExpensiveSeitanSchnitzelPrice = 0;

string[] AppetizersArray = new string[3];
string[] MainDishesArray = new string[3];
string[] DessertsArray = new string[3];

decimal lowestAppetizerPrice = 0;
decimal lowestMainDishPrice = 0;
decimal lowestDessertPrice = 0;

for (int i = 0; i < menuCards.Length; i++)
{
    string menuCard = ReadMenuCard(menuCards[i]);

    if (IsSchnitzelOnMenuCard(menuCard))
    {
        string restaurantName = Path.GetFileNameWithoutExtension(menuCards[i]);
        System.Console.WriteLine(restaurantName);
        System.Console.WriteLine(GetSchnitzelOnMenuCard(menuCard, out string dishTyp));
        if (IsSeitanSchnitzelOnMenuCard(menuCard))
        {
            decimal price = GetPriceOfSeitanSchnitzel(menuCard);
            if (price < cheapestSeitanSchnitzelPrice || cheapestSeitanSchnitzelPrice == 0)
            {
                cheapestSeitanSchnitzelPrice = price;
                cheapestSeitanSchnitzelRestaurant = restaurantName;
            }
            if (price > mostExpensiveSeitanSchnitzelPrice)
            {
                mostExpensiveSeitanSchnitzelPrice = price;
                mostExpensiveSeitanSchnitzelRestaurant = restaurantName;
            }
        }

        string[] menuCardLines = GetSchnitzelOnMenuCard(menuCard, out _).Replace("\t", "").Split("\n");
        string[] dishTypArray = dishTyp.Split("|");

        foreach (string line in dishTypArray)
        {
            System.Console.WriteLine(line);
        }

        for (int j = 0; j < menuCardLines.Length; j++)
        {
            decimal priceDish = GetPriceOfDish(menuCard, menuCardLines[j]);

            if (dishTypArray[j] == APPETIZERS)
            {
                if (priceDish < lowestAppetizerPrice || lowestAppetizerPrice == 0)
                {
                    lowestAppetizerPrice = priceDish;
                    AppetizersArray[0] = restaurantName;
                    AppetizersArray[1] = menuCardLines[j];
                    AppetizersArray[2] = $"{priceDish.ToString()}€";
                }
            }
            else if (dishTypArray[j] == MAIN_DISHES)
            {
                if (priceDish < lowestMainDishPrice || lowestMainDishPrice == 0)
                {
                    lowestMainDishPrice = priceDish;
                    MainDishesArray[0] = restaurantName;
                    MainDishesArray[1] = menuCardLines[j];
                    MainDishesArray[2] = $"{priceDish.ToString()}€";
                }
            }
            else if (dishTypArray[j] == DESSERTS)
            {
                if (priceDish < lowestDessertPrice || lowestDessertPrice == 0)
                {
                    lowestDessertPrice = priceDish;
                    DessertsArray[0] = restaurantName;
                    DessertsArray[1] = menuCardLines[j];
                    DessertsArray[2] = $"{priceDish.ToString()}€";
                }
            }
        }

    }

}
PrintCheapestSeitanSchnitzel(cheapestSeitanSchnitzelRestaurant, cheapestSeitanSchnitzelPrice);
PrintMostExpensiveSeitanSchnitzel(mostExpensiveSeitanSchnitzelRestaurant, mostExpensiveSeitanSchnitzelPrice);
PrintCheapMenu(AppetizersArray, MainDishesArray, DessertsArray);
#endregion

#region Methods 
string ReadMenuCard(string menuCardPath) => System.IO.File.ReadAllText(menuCardPath);

bool IsSchnitzelOnMenuCard(string menuCard) => menuCard.Contains("Schnitzel");

string GetSchnitzelOnMenuCard(string menuCard, out string dishTyp)
{
    string[] menuCardLines = menuCard.Split("\n");
    string schnitzelLine = string.Empty;
    dishTyp = string.Empty;

    for (int i = 0; i < menuCardLines.Length; i++)
    {
        if (menuCardLines[i].Contains("Schnitzel"))
        {
            dishTyp += $"{GetTypOfDish(menuCard, menuCardLines[i].Substring(0, menuCardLines[i].LastIndexOf(':')))}|";
            schnitzelLine += $"\t{menuCardLines[i].Substring(0, menuCardLines[i].LastIndexOf(':'))}\n";
        }
    }

    if (schnitzelLine.EndsWith("\n"))
    {
        schnitzelLine = schnitzelLine.Remove(schnitzelLine.Length - 1);
    }

    return schnitzelLine;
}

void PrintCheapestSeitanSchnitzel(string restaurantName, decimal price)
{
    System.Console.WriteLine("\nWHERE TO GET THE CHEAPEST SEITAN SCHNITZEL?");
    System.Console.WriteLine("==============================================");
    System.Console.WriteLine($"{restaurantName}, {price}€");
}

void PrintMostExpensiveSeitanSchnitzel(string restaurantName, decimal price)
{
    System.Console.WriteLine("\nWHERE TO GET THE MOST EXPENSIVE SEITAN SCHNITZEL?");
    System.Console.WriteLine("==================================================");
    System.Console.WriteLine($"{restaurantName}, {price}€");
}

bool IsSeitanSchnitzelOnMenuCard(string menuCard) => menuCard.Contains("Seitan Schnitzel", StringComparison.OrdinalIgnoreCase);

decimal GetPriceOfSeitanSchnitzel(string menuCard)
{
    decimal price = 0;
    string[] menuCardLines = menuCard.Split("\n");
    for (int i = 0; i < menuCardLines.Length - 1; i++)
    {
        if (IsSeitanSchnitzelOnMenuCard(menuCardLines[i]))
        {
            string priceString = menuCardLines[i].Substring(menuCardLines[i].LastIndexOf(':') + 1);
            priceString = priceString.Replace("€", "");
            priceString = priceString.Replace(".", ",");
            price = decimal.Parse(priceString);
        }
    }
    return price;
}

void PrintCheapMenu(string[] AppetizersArray, string[] MainDishesArray, string[] DessertsArray)
{
    System.Console.WriteLine("\nWHERE TO GET THE CHEAPEST SCHNITZEL FEAST?");
    System.Console.WriteLine("============================================");

    string appetizerString = "";
    System.Console.Write("Appetizers:");
    foreach (string appetizer in AppetizersArray) { appetizerString += $" {appetizer},"; }
    System.Console.WriteLine(appetizerString.Remove(appetizerString.Length - 1));

    string mainDishString = "";
    System.Console.Write("Main Dishes:");
    foreach (string mainDish in MainDishesArray) { mainDishString += $" {mainDish},"; }
    System.Console.WriteLine(mainDishString.Remove(mainDishString.Length - 1));

    string dessertString = "";
    System.Console.Write("Desserts:");
    foreach (string dessert in DessertsArray) { dessertString += $" {dessert},"; }
    System.Console.WriteLine(dessertString.Remove(dessertString.Length - 1));
}

string GetTypOfDish(string menuCard, string dish)
{
    string currentDishTyp = string.Empty;

    string[] menuCardLines = menuCard.Split("\n");
    for (int i = 0; i < menuCardLines.Length; i++)
    {
        if (menuCardLines[i] == APPETIZERS || menuCardLines[i] == MAIN_DISHES || menuCardLines[i] == DESSERTS)
        {
            currentDishTyp = menuCardLines[i];
        }
        if (menuCardLines[i].Contains(dish))
        {
            return currentDishTyp;
        }
    }
    return string.Empty;
}

decimal GetPriceOfDish(string menuCard, string dish)
{
    decimal price = 0;
    string[] menuCardLines = menuCard.Split("\n");
    for (int i = 0; i < menuCardLines.Length - 1; i++)
    {
        if (menuCardLines[i].Contains(dish))
        {
            string priceString = menuCardLines[i].Substring(menuCardLines[i].LastIndexOf(':') + 1);
            priceString = priceString.Replace("€", "");
            priceString = priceString.Replace(".", ",");
            price = decimal.Parse(priceString);
            return price;
        }
    }
    return 0;
}
#endregion