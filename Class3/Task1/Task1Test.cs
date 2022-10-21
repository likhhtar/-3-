using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task1.Task1;

using hands = System.Collections.Generic.List<Card>;
using Hand = System.Collections.Generic.List<Card>;
using Table = System.Collections.Generic.List<Card>;

namespace Task1;

public class Tests
{
    [Test]
    public void RoundWinnerTest()
    {
        That(RoundWinner(new Card(Suit.Clubs, Rank.Ace), new Card(Suit.Clubs, Rank.Ace)), Is.EqualTo(null));
        That(RoundWinner(new Card(Suit.Clubs, Rank.Ten), new Card(Suit.Diamonds, Rank.Six)), Is.EqualTo(Player.FirstPlayer));
        That(RoundWinner(new Card(Suit.Clubs, Rank.King), new Card(Suit.Spades, Rank.Ace)), Is.EqualTo(Player.SecondPlayer));
        That(RoundWinner(new Card(Suit.Diamonds, Rank.Ten), new Card(Suit.Clubs, Rank.Six)), Is.EqualTo(Player.FirstPlayer));
        That(RoundWinner(new Card(Suit.Spades, Rank.King), new Card(Suit.Clubs, Rank.Ace)), Is.EqualTo(Player.SecondPlayer));
    }

    [Test]
    public void FullhandsTest()
    {
        var hands = FullDeck();
        That(hands, Has.Count.EqualTo(DeckSize));
    }

    [Test]
    public void RoundTest()
    {
        Dictionary<Player, Hand> hands = new Dictionary<Player, hands>();
        hands[Player.FirstPlayer] = new Hand
        {
            new Card(Suit.Clubs, Rank.Six),
            new Card(Suit.Clubs, Rank.King)
        };
        hands[Player.SecondPlayer] = new Hand
        {
            new Card(Suit.Diamonds, Rank.Six),
            new Card(Suit.Clubs, Rank.Ace)
        };

        var round = Round(hands);
        That(round.Item1, Is.EqualTo(Player.SecondPlayer));
        Table table = new Table
        {
            new Card(Suit.Clubs, Rank.Six),
            new Card(Suit.Diamonds, Rank.Six),
            new Card(Suit.Clubs, Rank.King),
            new Card(Suit.Clubs, Rank.Ace)
        };
        
        
        That(round.Item2, Is.EqualTo(table));
    }

    [Test]
    public void Game2CardsTest()
    {
        var six = new Card(Suit.Clubs, Rank.Six);
        var ace = new Card(Suit.Clubs, Rank.Ace);
        Dictionary<Player, List<Card>> hands = new Dictionary<Player, List<Card>>
        {
            { Player.FirstPlayer, new List<Card> {six} },
            { Player.SecondPlayer, new List<Card> {ace} }
        };
        var gameWinner = Game(hands);
        That(gameWinner, Is.EqualTo(Player.SecondPlayer));
    }
    
    private static T TODO<T>()
    {
        throw new NotImplementedException();
    }
}