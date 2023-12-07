using System.Globalization;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days._7;

public class DaySeven
{
    public long ResolvePartOneSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(7, "SmallInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartOneLongInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(7, "LongInput-1.txt");
        return BodyPartOne(input);
    }

    public long ResolvePartTwoSmallInput()
    {
        // read file row by row and create a list of string
        var input = Utils.ReadAllLines(7, "SmallInput-2.txt");
        return BodyPartTwo(input);
    }

    public long ResolvePartTwoLongInput()
    {
        var input = Utils.ReadAllLines(7, "LongInput-2.txt");
        return BodyPartTwo(input);
    }

    #region Body

    private long BodyPartOne(string[] input)
    {
        var hands = input.Select(x => new Hand(x.Split(" ")[0], int.Parse(x.Split(" ")[1]))).ToList();
        hands.Sort((x, y) => x.CompareTo(y));
        
        hands.ForEach(x => Console.WriteLine($"{x.Card} {x.TypeOfHand}"));
        
        long result = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            result += (i+1) * hands[i].Bid;
        }
        
        return result;
    }

    private long BodyPartTwo(string[] input)
    {
        var hands = input.Select(x => new Hand(x.Split(" ")[0], int.Parse(x.Split(" ")[1]), true)).ToList();
        hands.Sort((x, y) => x.CompareTo(y));
        
        // hands.ForEach(x => Console.WriteLine($"{x.Card} {x.TypeOfHand}"));
        
        long result = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            result += (i+1) * hands[i].Bid;
        }
        
        return result;
    }

    #endregion
}

public class Hand
{
    public string Card { get; set; }
    public string[] CardsArray => Card.ToArray().Select(x => x.ToString()).ToArray();
    
    public int Bid { get; set; }
    
    private Dictionary<string, int> _cardsDictionary = new();

    public Hand(string card, int bid, bool isSecond = false)
    {
        Card = card;
        Bid = bid;
        foreach (var x in CardsArray)
        {
            if (_cardsDictionary.ContainsKey(x))
                _cardsDictionary[x]++;
            else
                _cardsDictionary.Add(x, 1);
        }

        if (!isSecond)
            return;
        
        if (_cardsDictionary.ContainsKey("J"))
        {
            Console.WriteLine("ToReplace: " + Card);
            if (TypeOfHand == TypeOfHands.HighCard)
            {
                var toReplace = _cardsDictionary.Select(x => x.Key).ToList();
                toReplace.Sort((x, y) => MyStringCompare(x, y));
                Card = Card.Replace("J", toReplace[4] == "J" ? toReplace[3] : toReplace[4]);
            }
            else if (TypeOfHand == TypeOfHands.OnePair)
            {
                var toReplace = _cardsDictionary.First(x => x.Value == 2).Key;
                Card = Card.Replace("J", toReplace == "J" ? "A" : toReplace);
            }
            else if (TypeOfHand == TypeOfHands.TwoPairs)
            {
                var first = _cardsDictionary.Where(x => x.Value == 2).First();
                var second = _cardsDictionary.Where(x => x.Value == 2).Last();
                var result = MyStringCompare(first.Key, second.Key);
                var toReplace = "";
                if(first.Key == "J")
                    toReplace = second.Key;
                else if(second.Key == "J")
                    toReplace = first.Key;
                else if(result == 1)
                    toReplace = first.Key;
                else
                    toReplace = second.Key;
                
                Card = Card.Replace("J", toReplace);
            }
            else if (TypeOfHand == TypeOfHands.ThreeOfAKind)
            {
                var toReplace = _cardsDictionary.First(x => x.Value == 3).Key;
                var toReplace2 = _cardsDictionary.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                if(toReplace == "J")
                    toReplace = MyStringCompare(toReplace2[0], toReplace2[1]) == 1 ? toReplace2[0] : toReplace2[1];
                Card = Card.Replace("J", toReplace == "J" ? "A" : toReplace);
            }
            else if (TypeOfHand == TypeOfHands.FullHouse)
            {
                var toReplace = _cardsDictionary.First(x => x.Value == 3).Key;
                Card = Card.Replace("J", toReplace == "J" ? "A" : toReplace);
            }
            else if (TypeOfHand == TypeOfHands.FourOfAKind)
            {
                var toReplace = _cardsDictionary.First(x => x.Value == 4).Key;
                Card = Card.Replace("J", toReplace == "J" ? "A" : toReplace);
            }
            else if (TypeOfHand == TypeOfHands.FiveOfAKind)
            {
                Card = Card.Replace("J", "A");
            }
            
            Console.WriteLine("Replaced: " + Card);
        }
        
        _cardsDictionary = new();
        foreach (var x in CardsArray)
        {
            if (_cardsDictionary.ContainsKey(x))
                _cardsDictionary[x]++;
            else
                _cardsDictionary.Add(x, 1);
        }
    }
    
    public TypeOfHands TypeOfHand => GetTypeOfHand();

    private TypeOfHands GetTypeOfHand()
    {
        if(_cardsDictionary.Count == 5)
            return TypeOfHands.HighCard;
        
        if(_cardsDictionary.Count == 4)
            return TypeOfHands.OnePair;
        
        if(_cardsDictionary.Count == 3)
        {
            if(_cardsDictionary.Any(x => x.Value == 3))
                return TypeOfHands.ThreeOfAKind;
            return TypeOfHands.TwoPairs;
        }
        
        if(_cardsDictionary.Count == 2)
        {
            if(_cardsDictionary.Any(x => x.Value == 4))
                return TypeOfHands.FourOfAKind;
            return TypeOfHands.FullHouse;
        }
        
        return TypeOfHands.FiveOfAKind;
    }

    public int CompareTo(Hand other)
    {
        if(TypeOfHand > other.TypeOfHand)
            return 1;
        
        if(TypeOfHand < other.TypeOfHand)
            return -1;
        
        // if(TypeOfHand == TypeOfHands.HighCard)
            return CompareHighCard(other);
        
        if(TypeOfHand == TypeOfHands.OnePair)
            return CompareOnePair(other);
        
        if(TypeOfHand == TypeOfHands.TwoPairs)
            return CompareTwoPairs(other);
        
        if(TypeOfHand == TypeOfHands.ThreeOfAKind)
            return CompareThreeOfAKind(other);
        
        if(TypeOfHand == TypeOfHands.FullHouse)
            return CompareFullHouse(other);
        
        if(TypeOfHand == TypeOfHands.FourOfAKind)
            return CompareFourOfAKind(other);
            
        return CompareFiveOfAKind(other);
    }

    private int CompareFiveOfAKind(Hand other)
    {
        var fiveOfAKind = _cardsDictionary.First(x => x.Value == 5).Key;
        var otherFiveOfAKind = other._cardsDictionary.First(x => x.Value == 5).Key;
        return MyStringCompare(fiveOfAKind, otherFiveOfAKind);
    }

    private int MyStringCompare(string fiveOfAKind, string otherFiveOfAKind)
    {
        if(char.IsDigit(fiveOfAKind[0]) && char.IsDigit(otherFiveOfAKind[0]))
            return int.Parse(fiveOfAKind).CompareTo(int.Parse(otherFiveOfAKind));
        
        if(char.IsDigit(fiveOfAKind[0]))
            return -1;
        
        if(char.IsDigit(otherFiveOfAKind[0]))
            return 1;
        
        if(fiveOfAKind == "T")
            return -1;
        
        if(otherFiveOfAKind == "T")
            return 1;
        
        if(fiveOfAKind == "J")
            return -1;
        
        if(otherFiveOfAKind == "J")
            return 1;
        
        if(fiveOfAKind == "Q")
            return -1;
        
        if(otherFiveOfAKind == "Q")
            return 1;
        
        if(fiveOfAKind == "K")
            return -1;
        
        if(otherFiveOfAKind == "K")
            return 1;
        
        if(fiveOfAKind == "A")
            return -1;
        
        if(otherFiveOfAKind == "A")
            return 1;
        
        return String.Compare(fiveOfAKind, otherFiveOfAKind, StringComparison.Ordinal);
    }

    private int CompareFourOfAKind(Hand other)
    {
        var fourOfAKind = _cardsDictionary.First(x => x.Value == 4).Key;
        var otherFourOfAKind = other._cardsDictionary.First(x => x.Value == 4).Key;
        if (fourOfAKind == otherFourOfAKind)
            return CompareHighCard(other);
        return MyStringCompare(fourOfAKind, otherFourOfAKind);
    }

    private int CompareFullHouse(Hand other)
    {
        var threeOfAKind = _cardsDictionary.First(x => x.Value == 3).Key;
        var otherThreeOfAKind = other._cardsDictionary.First(x => x.Value == 3).Key;
        if (threeOfAKind == otherThreeOfAKind)
            return CompareOnePair(other);
        return MyStringCompare(threeOfAKind, otherThreeOfAKind);
    }

    private int CompareThreeOfAKind(Hand other)
    {
        var threeOfAKind = _cardsDictionary.First(x => x.Value == 3).Key;
        var otherThreeOfAKind = other._cardsDictionary.First(x => x.Value == 3).Key;
        if (threeOfAKind == otherThreeOfAKind)
            return CompareHighCard(other);
        return MyStringCompare(threeOfAKind, otherThreeOfAKind);
    }

    private int CompareTwoPairs(Hand other)
    {
        var firstPair = _cardsDictionary.First(x => x.Value == 2).Key;
        var secondPair = _cardsDictionary.Last(x => x.Value == 2).Key;
        var otherFirstPair = other._cardsDictionary.First(x => x.Value == 2).Key;
        var otherSecondPair = other._cardsDictionary.Last(x => x.Value == 2).Key;
        
        var firstList = new List<string> {firstPair, secondPair};
        var secondList = new List<string> {otherFirstPair, otherSecondPair};
        firstList.Sort((x, y) => MyStringCompare(x, y));
        secondList.Sort((x, y) => MyStringCompare(x, y));
        
        firstPair = firstList[1];
        secondPair = firstList[0];
        otherFirstPair = secondList[1];
        otherSecondPair = secondList[0];
        
        if (firstPair == otherFirstPair && secondPair == otherSecondPair)
            return CompareHighCard(other);
        if (firstPair == otherFirstPair)
            return MyStringCompare(secondPair, otherSecondPair);
        return MyStringCompare(firstPair, otherFirstPair);
    }

    private int CompareOnePair(Hand other)
    {
        var firstPair = _cardsDictionary.First(x => x.Value == 2).Key;
        var secondPair = other._cardsDictionary.First(x => x.Value == 2).Key;
        if (firstPair == secondPair)
            return CompareHighCard(other);
        return MyStringCompare(firstPair, secondPair);
    }

    private int CompareHighCard(Hand other)
    {
        for (int i = 0; i < CardsArray.Length; i++)
        {
            if (CardsArray[i] == other.CardsArray[i])
                continue;
            return MyStringCompare(CardsArray[i], other.CardsArray[i]);
        }
        // {
        //     if (temp[i] == temp2[i])
        //         continue;
        //     return MyStringCompare(temp[i], temp2[i]);
        // }
        
        return 0;
    }
}

public enum TypeOfHands
{
    HighCard = 1,
    OnePair = 2,
    TwoPairs = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}