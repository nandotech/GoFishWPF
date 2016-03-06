using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoFishWPF
{
    class Player
    {
        private string name;
        public string Name { get { return name; } }
        private Random random;
        private Deck cards;
       // private TextBox textBoxOnForm;
        private Game game;

        /// <summary>
        /// Constructor
        /// Initializes 4 private fields and then adds a line
        /// to the textbox control on the form that says "Joe has just
        /// joined the game" - but uset he name in the private field, and don't forget to add
        /// a line break at the end of every line you add to the text box...
        /// </summary>
        /// <param name="name"></param>
        /// <param name="random"></param>
        /// <param name="textBoxOnForm"></param>
        public Player(string name, Random random, Game game)
        {
            this.name = name;
            this.random = random;
            this.game = game;
            this.cards = new Deck(new Card[] { });
            game.AddProgress(name + " has just joined the game");
            //textBoxOnForm.Text += name + " has just joined the game" + Environment.NewLine;
        }

        public IEnumerable<Values> PullOutBooks()
        {

            List<Values> books = new List<Values>();
            for (int i = 1; i <= 13; i++)
            {
                Values value = (Values)i;
                int howMany = 0;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).Value == value)
                        howMany++;
                if (howMany == 4)
                {
                    books.Add(value);
                    for (int card = cards.Count - 1; card >= 0; card--)
                        cards.Deal(card);
                }
            }
            return books;
        }

        public Values GetRandomValue()
        {
            //method gets a random value that must bein the deck
            Card randomCard = cards.Peek(random.Next(cards.Count));
            return randomCard.Value;
        }

        /// <summary>
        /// Opponent asks if I have any cards of certain value.
        /// Use Deck.PulloutValues() to pull out the values. Add a line to textbox
        /// that says, "Joe has 3 sixes"  - use the new Card.Plural() static method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Deck DoYouHaveAny(Values value)
        {
            Deck cardsIHave = cards.PullOutValues(value);
            game.AddProgress(Name + " has " + cardsIHave.Count + " " + Card.Plural(value));
            return cardsIHave;
        }
        /// <summary>
        /// Overloaded version of AskForACard()
        /// </summary>
        /// <param name="players"></param>
        /// <param name="myIndex"></param>
        /// <param name="stock"></param>
        public void AskForACard(List<Player> players, int myIndex, Deck stock)
        {
            Values randomValue = GetRandomValue();
            AskForACard(players, myIndex, stock, randomValue);
        }
        /// <summary>
        /// Ask the other players for a value.  Firstr add a line to the TextBox:
        /// "Joe asks if anyone has a Queen". Then go through the list of players that was passed in
        /// as a paramter and ask each player if he has any of hte value (using his DoYouHaveAny() method)
        /// He'll pass you a deck of cards - add them to my deck.  Keep track of how many cards were added.
        /// If tehre weren't any, you'll need to deal yourself a card from the stock (which was passed
        /// as a parameter), and you'll have to add a line in the TextBox, "Joe had to draw from the stock"
        /// </summary>
        /// <param name="players">List of players to ask</param>
        /// <param name="myIndex">My Player index</param>
        /// <param name="stock">Deck object of remaining stock</param>
        /// <param name="value">Value to ask for</param>
        public void AskForACard(List<Player> players, int myIndex, Deck stock, Values value)
        {
            game.AddProgress(Name + " asks if anyone has a " + value);

            int totalCardsGiven = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (i != myIndex)
                {
                    Player player = players[i];
                    Deck CardsGiven = player.DoYouHaveAny(value);
                    totalCardsGiven += CardsGiven.Count;
                    while (CardsGiven.Count > 0)
                        cards.Add(CardsGiven.Deal());
                }
            }
            if (totalCardsGiven == 0)
            {
                game.AddProgress(Name + " must draw from the stock.");
                cards.Add(stock.Deal());
            }
        }



        public int CardCount { get { return cards.Count; } }
        public void TakeCard(Card card) { cards.Add(card); }
        public IEnumerable<string> GetCardNames() { return cards.GetCardNames(); }
        public Card Peek(int cardNumber) { return cards.Peek(cardNumber); }
        public void SortHand() { cards.SortByValue(); }
    }
}

