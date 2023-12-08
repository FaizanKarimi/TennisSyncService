using System.Collections.Generic;

namespace TennisAPIAndScrappingService.ResponseModels
{
    public class ResponseModels { }

    public class ResponseData
    {
        public Tournament Tournament { get; set; }
        public Match Match { get; set; }
    }

    public class Tournament
    {
        public string EventId { get; set; }
        public int EventYear { get; set; }
        public string TournamentName { get; set; }
    }

    public class Match
    {
        public string MatchId { get; set; }
        public Round Round { get; set; }
        public PlayerTeam1 PlayerTeam1 { get; set; }
        public PlayerTeam2 PlayerTeam2 { get; set; }
    }

    public class Round
    {
        public string RoundId { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

    public class PlayerTeam1
    {
        public string PlayerId { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public int? SeedPlayerTeam { get; set; }
        public YearToDateStats YearToDateStats { get; set; }
        public List<Sets> Sets { get; set; }
    }

    public class PlayerTeam2
    {
        public string PlayerId { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerLastName { get; set; }
        public int? SeedPlayerTeam { get; set; }
        public YearToDateStats YearToDateStats { get; set; }
        public List<Sets> Sets { get; set; }
    }

    public class Sets
    {
        public int SetNumber { get; set; }
        public int? SetScore { get; set; }
        public int? TieBreakScore { get; set; }
        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public ServiceStats ServiceStats { get; set; }
        public ReturnStats ReturnStats { get; set; }
        public PointStats PointStats { get; set; }
    }

    public class FirstServePointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class ServiceStats
    {
        public FirstServe FirstServe { get; set; }
        public FirstServePointsWon FirstServePointsWon { get; set; }
        public SecondServePointsWon SecondServePointsWon { get; set; }
        public BreakPointsSaved BreakPointsSaved { get; set; }
        public ServiceGamesPlayed ServiceGamesPlayed { get; set; }
    }

    public class FirstServe
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class SecondServePointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class BreakPointsSaved
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class ServiceGamesPlayed
    {
        public int Number { get; set; }
    }

    public class ReturnStats
    {
        public FirstServeReturnPointsWon FirstServeReturnPointsWon { get; set; }
        public SecondServeReturnPointsWon SecondServeReturnPointsWon { get; set; }
        public BreakPointsConverted BreakPointsConverted { get; set; }
        public ReturnGamesPlayed ReturnGamesPlayed { get; set; }
    }

    public class FirstServeReturnPointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class SecondServeReturnPointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class BreakPointsConverted
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class ReturnGamesPlayed
    {
        public int Number { get; set; }
    }

    public class TotalServicePointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class TotalReturnPointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class TotalPointsWon
    {
        public int Percent { get; set; }
        public int Dividend { get; set; }
        public int Divisor { get; set; }
    }

    public class PointStats
    {
        public TotalServicePointsWon TotalServicePointsWon { get; set; }
        public TotalReturnPointsWon TotalReturnPointsWon { get; set; }
        public TotalPointsWon TotalPointsWon { get; set; }
    }

    public class YearToDateStats
    {
        public ServiceRecordStats ServiceRecordStats { get; set; }
        public ReturnRecordStats ReturnRecordStats { get; set; }
    }

    public class ServiceRecordStats
    {
        public ServiceGamesWon ServiceGamesWon { get; set; }
    }

    public class ServiceGamesWon
    {
        public int? Percent { get; set; }
    }

    public class ReturnGamesWon
    {
        public int? Percent { get; set; }
    }

    public class ReturnRecordStats
    {
        public ReturnGamesWon ReturnGamesWon { get; set; }
    }

    public class Data
    {
        public int current { get; set; }
        public int rowCount { get; set; }
        public List<Rows> rows { get; set; }
        public int total { get; set; }
    }

    public class Rows
    {
        public int rank { get; set; }
        public int playerId { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public Country country { get; set; }
        public int points { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string id { get; set; }
    }

    #region Dilawar
    public class WTARanking
    {
        public int Rank { get; set; }
        public string Move { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Points { get; set; }
        public int Tourn { get; set; }
        public string Link { get; set; }
    }
    #endregion

    #region Mudasser
    public class SportingbetTennisMatch
    {
        public int id { get; set; }
        public string cmt { get; set; }
        public string w { get; set; }
        public string wc { get; set; }
        public string st { get; set; }
        public string stm { get; set; }
        public string sd { get; set; }
        public string serv { get; set; }
        public string stid { get; set; }
        public SportingbetTennisMatchPlayer p1 { get; set; }
        public SportingbetTennisMatchPlayer p2 { get; set; }
    }

    public class SportingbetTennisMatchPlayer
    {
        public string rs { get; set; }
        public string sw { get; set; }
        public string s1 { get; set; }
        public string s2 { get; set; }
        public string s3 { get; set; }
        public string s4 { get; set; }
        public string s5 { get; set; }
        public string t1 { get; set; }
        public string t2 { get; set; }
        public string t3 { get; set; }
        public string t4 { get; set; }
        public string t5 { get; set; }
        public string gamescore { get; set; }
        public string fr { get; set; }
    }

    public class sofascoretennismatch
    {
        public int id { get; set; }
        public string formatedStartDate { get; set; }
        public string startTime { get; set; }
        public int? firstToServe { get; set; }
        public bool ShouldSerializefirstToServe()
        {
            return firstToServe.HasValue;
        }
        public string statusDescription { get; set; }
        public string winnerCode { get; set; }
        public sofasscoretennisstatus status { get; set; }
        public sofascoretennisscore homeScore { get; set; }
        public sofascoretennisscore awayScore { get; set; }
    }

    public class sofasscoretennisstatus
    {
        public string type { get; set; }
    }

    public class sofascoretennisscore
    {
        public int? current { get; set; }
        public bool ShouldSerializecurrent()
        {
            return current.HasValue;
        }
        public int? period1 { get; set; }
        public bool ShouldSerializeperiod1()
        {
            return period1.HasValue;
        }
        public int? period2 { get; set; }
        public bool ShouldSerializeperiod2()
        {
            return period2.HasValue;
        }
        public int? period3 { get; set; }
        public bool ShouldSerializeperiod3()
        {
            return period3.HasValue;
        }
        public int? period4 { get; set; }
        public bool ShouldSerializeperiod4()
        {
            return period4.HasValue;
        }
        public int? period5 { get; set; }
        public bool ShouldSerializeperiod5()
        {
            return period5.HasValue;
        }
        public string point { get; set; }

        public int? period1TieBreak { get; set; }
        public bool ShouldSerializeperiod1TieBreak()
        {
            return period1TieBreak.HasValue;
        }
        public int? period2TieBreak { get; set; }
        public bool ShouldSerializeperiod2TieBreak()
        {
            return period2TieBreak.HasValue;
        }
        public int? period3TieBreak { get; set; }
        public bool ShouldSerializeperiod3TieBreak()
        {
            return period3TieBreak.HasValue;
        }
        public int? period4TieBreak { get; set; }
        public bool ShouldSerializeperiod4TieBreak()
        {
            return period4TieBreak.HasValue;
        }
        public int? period5TieBreak { get; set; }
        public bool ShouldSerializeperiod5TieBreak()
        {
            return period5TieBreak.HasValue;
        }
    }
    #endregion
}