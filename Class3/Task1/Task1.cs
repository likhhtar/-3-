// Колода
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;

// Масть
internal enum Suit
{
    Diamonds,
    Hearts,
    Clubs,
    Spades
}

// Значение
internal enum Rank
{
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Lady = 12,
    King = 13,
    Ace = 14
}

// Карта
record Card
{
    public Suit s;
    public Rank r;

    public Card(Suit suit, Rank rank)
    {
        s = suit;
        r = rank;
    }
}

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    FirstPlayer,
    SecondPlayer
}

namespace Task1
{
    public class Task1
    {

 /*
 * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
 * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
 * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
 * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
 * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
 *
 * Реализация должна сопровождаться тестами.
 */

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            if (card1.r == card2.r) return null;
            if (card1.r > card2.r) return Player.FirstPlayer;
            else return Player.SecondPlayer;
        }
        
// Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            Deck d = new Deck();
            
            var suits = Enum.GetValues(typeof(Suit));
            var ranks = Enum.GetValues(typeof(Rank));

            foreach (Suit s in suits)
            {
                foreach (Rank r in ranks)
                { 
                    Card c = new Card(s, r);   
                    d.Add(c); 
                }
            }

            return d;
        }

// Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам

        internal static Deck Shuffle(Deck d)
        {
            Random random = new Random();
            for (int i = 0; i < d.Count; i++)
            {
                int j = random.Next(0, d.Count - 1);
                var tmp = d[i];
                d[i] = d[j];
                d[j] = tmp;
            }

            return d;
        }
        
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            deck = Shuffle(deck);
            Dictionary<Player, Hand> result = new Dictionary<Player, Deck>();
            result[Player.FirstPlayer] = new Hand();
            result[Player.SecondPlayer] = new Hand();
            for (int i = 0; i < deck.Count; i++)
            {
                if (i % 2 == 0) result[Player.FirstPlayer].Add(deck[i]);
                else result[Player.SecondPlayer].Add(deck[i]);
            }

            return result;
        }

// Один раунд игры (в том числе спор при равных картах).
// Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static Tuple<Player, Table> Round(Dictionary<Player, Hand> hands)
        {
            Table result = new Table();
            var hand1 = hands[Player.FirstPlayer];
            var hand2 = hands[Player.SecondPlayer];

            for (int i = 0; i < hand1.Count; i++)
            {
                Console.WriteLine($"iteration {i}");
                result.Add(hand1[i]);
                result.Add(hand2[i]);
                var winner = RoundWinner(hand1[i], hand2[i]);
                if (winner.HasValue)
                {
                    return new Tuple<Player, Table>((Player)winner, result);
                }
            }

            return null;
        }

// Полный цикл игры (возвращается победивший игрок)
// в процессе игры печатаются ходы
        internal static Player Game(Dictionary<Player, Hand> hands)
        {
            while (hands[Player.FirstPlayer].Count > 0 && hands[Player.SecondPlayer].Count > 0)
            {
                var round = Round(hands);
                Player winner = round.Item1;
                Table table = round.Item2;
                for (int i = 0; i < table.Count; i++)
                {
                    Card c = table[i];
                    if (i % 2 == 0) Console.WriteLine($"First player puts card {c.r} {c.s} to table");
                    else Console.WriteLine($"Second player puts card {c.r} {c.s} to table");
                    hands[Player.FirstPlayer].Remove(c);
                    hands[Player.SecondPlayer].Remove(c);
                }
                Console.WriteLine($"{winner} wins roud!");

                foreach (Card c in table)
                {
                    hands[winner].Add(c);
                }
            }

            if (hands[Player.FirstPlayer].Count == 0) return Player.SecondPlayer;
            else return Player.FirstPlayer;
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            Console.WriteLine($"Победитель: {winner}");
        }
    }
}