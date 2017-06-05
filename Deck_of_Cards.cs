using System;
using System.Collections.Generic;
using System.Linq;

namespace Deck_Of_Cards
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                List<List<Card>> players = new List<List<Card>>();

                int mainMenuOption = MainMenu(), numPlayers = SetPlayerCount(), numHandSize = SetHandSize(numPlayers), cardsToDraw;

                Deck deck = new Deck();
                deck.shuffle();

                for (int i = 0; i < numPlayers; i++)
                    players.Add(new List<Card>());

                for (int i = 0; i < numHandSize; i++)
                    for (int x = 0; x < numPlayers; x++)
                        players[x].Add(deck.drawCard());

                PrintCards(players);

                do
                {
                    cardsToDraw = DrawMoreCards(deck, numPlayers);

                    if (cardsToDraw > 0)
                    {
                        for (int i = 0; i < cardsToDraw; i++)
                            for (int x = 0; x < numPlayers; x++)
                                players[x].Add(deck.drawCard());

                        PrintCards(players);
                    }
                        
                } while (cardsToDraw != 0);

                if (deck.DeckOfCards.Count() == 0)
                    Console.WriteLine("\nThe deck is empty, press enter to return to the main menu.");
                else
                    Console.WriteLine("\nYou can no longer deal any more cards, press enter to return to the main menu.");
                Console.ReadLine();
            }
        }

        static int MainMenu()
        {
            int mainMenuOption = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu:\n\n    1)Deal Cards\n    2)Quit");

                int.TryParse(Console.ReadLine(), out mainMenuOption);

                if ((mainMenuOption < 1) || (mainMenuOption > 2))
                    mainMenuOption = 0;
                if (mainMenuOption == 2)
                    Environment.Exit(0);

            } while (mainMenuOption == 0);

            return mainMenuOption;
        }

        static int SetPlayerCount()
        {
            int playerCount = 0;

            do
            {
                Console.Write("Enter number of players: ");
                int.TryParse(Console.ReadLine(), out playerCount);

                if ((playerCount < 1) || (playerCount > Deck._DeckSize))
                    playerCount = 0;
                if (playerCount == 0)
                    Console.WriteLine("Number of players must be 1-" + Deck._DeckSize + "\n");
            } while (playerCount == 0);

            return playerCount;
        }

        static int SetHandSize(int playerCount)
        {
            int handSize = 0;

            do
            {
                Console.Write("Enter starting hand size: ");
                int.TryParse(Console.ReadLine(), out handSize);

                if ((handSize < 1) || (handSize * playerCount > Deck._DeckSize))
                    handSize = 0;
                if (handSize == 0)
                    Console.WriteLine("Starting hand size must be 1-" + Deck._DeckSize / playerCount + "\n");
            } while (handSize == 0);

            return handSize;
        }

        static int DrawMoreCards(Deck deck, int playerCount)
        {
            int cardsToDeal = 0;

            do
            {
                int cardsRemaining = deck.DeckOfCards.Count();
                int maxDeal = cardsRemaining / playerCount;

                if ((cardsRemaining == 0) || (maxDeal < 1))
                {
                    cardsToDeal = 0;
                    return cardsToDeal;
                }
                Console.Write("Enter the number of additional cards you wish to deal to each player ('x' to stop dealing cards): ");
                string drawCards = Console.ReadLine();
                
                if (drawCards == "x")
                    return cardsToDeal;

                int.TryParse(drawCards, out cardsToDeal);
                
                if ((cardsToDeal < 1) || ((cardsToDeal * playerCount) > cardsRemaining))
                    cardsToDeal = 0;
                if (cardsToDeal == 0)
                    Console.WriteLine("You only have " + cardsRemaining + " cards remaining, the most you can deal to each player is " +
                        maxDeal + "\n");
            } while (cardsToDeal == 0);

            return cardsToDeal;
        }

        static void PrintCards(List<List<Card>> players)
        {
            int playerNumber = 1;
            Console.WriteLine("");
            
            foreach (List<Card> player in players)
            {
                Console.WriteLine("Player " + playerNumber + " has the following cards:");
                foreach (Card card in player)
                    Console.WriteLine(card.DisplayCard());

                Console.WriteLine("");
                playerNumber++;
            }
        }
    }

    class Deck
    {
        public static int _DeckSize = 52;
        public List<Card> DeckOfCards { get; set; }

        public Deck()
        {
            DeckOfCards = new List<Card>();

            for (int i = 0; i < _DeckSize; i++)
                DeckOfCards.Add(new Card(i));
        }

        public void shuffle()
        {
            Random rnd = new Random();
            DeckOfCards = DeckOfCards.OrderBy(x => rnd.Next()).ToList();
        }

        public Card drawCard()
        {
            Card topCard = DeckOfCards.First();
            DeckOfCards.Remove(topCard);

            return topCard;
        }
    }

    class Card
    {
        Value cardValue;
        Suit cardSuit;
        enum Value
        {
            Ace = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13
        }
        enum Suit
        {
            Clubs, Hearts, Spades, Diamonds
        }

        public Card(int cardNumber)
        {
            cardValue = (Value)(cardNumber % 13 + 1);

            if (cardNumber < 14)
                cardSuit = Suit.Clubs;
            else if (cardNumber < 27)
                cardSuit = Suit.Hearts;
            else if (cardNumber < 40)
                cardSuit = Suit.Spades;
            else if (cardNumber < 54)
                cardSuit = Suit.Diamonds;
        }

        public string DisplayCard()
        {
            return cardValue + " of " + cardSuit;
        }
    }
}
