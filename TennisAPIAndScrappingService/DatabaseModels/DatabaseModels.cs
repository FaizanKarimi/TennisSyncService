using System;

namespace TennisAPIAndScrappingService.DatabaseModels
{
    public class DatabaseModels
    {
    }

    public class Matches
    {
        public decimal MatchId { get; set; }
        public int ContestGroupId { get; set; }
        public Nullable<int> ContestGroupRoundId { get; set; }
        public short MatchStatusId { get; set; }
        public DateTime MatchDate { get; set; }
        public bool IsLive { get; set; }
        public bool Lights { get; set; }
        public string HomeTeamShirt { get; set; }
        public string AwayTeamShirt { get; set; }
        public Nullable<short> CurrentMinutes { get; set; }
        public Nullable<System.DateTime> MatchStartTime { get; set; }
        public Nullable<System.DateTime> FirstHalfEndTime { get; set; }
        public Nullable<short> MinuteDifference { get; set; }
        public Nullable<bool> MinutePlusBit { get; set; }
        public string FirstToServe { get; set; }
        public string SportCCMatchId { get; set; }
        public Nullable<DateTime> LastUpdatedTime { get; set; }
        public Nullable<bool> IsLeagueTableUpdated { get; set; }
        public Nullable<Guid> AssignedToUserId { get; set; }
        public Nullable<Guid> AssignedByUserId { get; set; }
        public string AssignedByUserName { get; set; }
        public Nullable<DateTime> AssignDate { get; set; }
        public Nullable<DateTime> SportingLifeMatchDate { get; set; }
        public string CrawlerLink { get; set; }
        public Nullable<bool> AutoCrawl { get; set; }
        public string CreatedByRole { get; set; }
        public Nullable<long> ProviderMatchId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<int> SharkScoresMatchId { get; set; }
        public Nullable<int> CrawlFrom { get; set; }
        public Nullable<int> SportingbetMatchId { get; set; }
        public Nullable<int> LiveScoreMatchId { get; set; }
        public byte[] change_stamp { get; set; }
    }

    public class ContestGroup
    {
        public int ContestGroupId { get; set; }
        public Nullable<long> SystemId { get; set; }
        public short SportId { get; set; }
        public int LeagueId { get; set; }
        public short SeasonId { get; set; }
        public Nullable<short> CountryId { get; set; }
        public Nullable<short> SportOrganizationId { get; set; }
        public short GenderId { get; set; }
        public short LeagueTypeId { get; set; }
        public Nullable<short> LTTypeAndSurfaceId { get; set; }
        public Nullable<short> ContestTypeId { get; set; }
        public string ContestGroupName { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string ShortCode { get; set; }
        public string MobileShortName { get; set; }
        public bool IsInternational { get; set; }
        public bool IsActive { get; set; }
        public bool IsLive { get; set; }
        public string Ranking { get; set; }
        public Nullable<bool> HasTable { get; set; }
        public string NameEnglish { get; set; }
        public string NameDanish { get; set; }
        public string NameNorwegian { get; set; }
        public string NameSwedish { get; set; }
        public string NameDeutsche { get; set; }
        public string NameItalian { get; set; }
        public string NameSpanish { get; set; }
        public string NameSerbian { get; set; }
        public string SportCCContestGroupId { get; set; }
        public string CrawlerLink { get; set; }
        public Nullable<bool> AutoUpdateMatches { get; set; }
        public Nullable<int> SharkScoresId { get; set; }
        public Nullable<bool> Goals { get; set; }
        public Nullable<bool> Cards { get; set; }
        public Nullable<bool> Subs { get; set; }
        public Nullable<bool> Lineups { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<bool> Finished { get; set; }
        public Nullable<decimal> WinPoints { get; set; }
        public Nullable<decimal> WinETPenPoints { get; set; }
        public Nullable<decimal> LossPoints { get; set; }
        public Nullable<decimal> LossETPenPoints { get; set; }
        public byte[] change_stamp { get; set; }
        public Nullable<int> TotalContestTeams { get; set; }
        public string LeagueNote { get; set; }
        public Nullable<int> Bet365LeagueId { get; set; }
        public string FixtureLink1 { get; set; }
        public string FixtureLink2 { get; set; }
        public string FixtureLink3 { get; set; }
    }

    public class TennisMatchInfo
    {
        public decimal MatchId { get; set; }
        public int ContestGroupId { get; set; }
        public Nullable<int> FirstServePointsWonDividerTeam1 { get; set; }
        public Nullable<int> FirstServePointsWonDividendTeam1 { get; set; }
        public Nullable<int> FirstServePointsWonPercentTeam1 { get; set; }
        public Nullable<int> FirstServePointsWonDividerTeam2 { get; set; }
        public Nullable<int> FirstServePointsWonDividendTeam2 { get; set; }
        public Nullable<int> FirstServePointsWonPercentTeam2 { get; set; }
        public Nullable<int> BreakPointsConvertedPercentTeam1 { get; set; }
        public Nullable<int> BreakPointsConvertedDividendTeam1 { get; set; }
        public Nullable<int> BreakPointsConvertedDivisorTeam1 { get; set; }
        public Nullable<int> BreakPointsConvertedPercentTeam2 { get; set; }
        public Nullable<int> BreakPointsConvertedDividendTeam2 { get; set; }
        public Nullable<int> BreakPointsConvertedDivisorTeam2 { get; set; }
        public Nullable<int> ServicePointsWonPercentTeam1 { get; set; }
        public Nullable<int> ServicePointsWonDividendTeam1 { get; set; }
        public Nullable<int> ServicePointsWonDivisorTeam1 { get; set; }
        public Nullable<int> ServicePointsWonPercentTeam2 { get; set; }
        public Nullable<int> ServicePointsWonDividendTeam2 { get; set; }
        public Nullable<int> ServicePointsWonDivisorTeam2 { get; set; }
        public Nullable<int> TotalGamesWonPercentTeam1 { get; set; }
        public Nullable<int> TotalGamesWonDividendTeam1 { get; set; }
        public Nullable<int> TotalGamesWonDivisorTeam1 { get; set; }
        public Nullable<int> TotalGamesWonPercentTeam2 { get; set; }
        public Nullable<int> TotalGamesWonDividendTeam2 { get; set; }
        public Nullable<int> TotalGamesWonDivisorTeam2 { get; set; }
        public Nullable<int> TotalPointsWonPercentTeam1 { get; set; }
        public Nullable<int> TotalPointsWonDividendTeam1 { get; set; }
        public Nullable<int> TotalPointsWonDivisorTeam1 { get; set; }
        public Nullable<int> TotalPointsWonPercentTeam2 { get; set; }
        public Nullable<int> TotalPointsWonDividendTeam2 { get; set; }
        public Nullable<int> TotalPointsWonDivisorTeam2 { get; set; }
    }

    public class Players
    {
        public int PlayerId { get; set; }
        public Int16 SportId { get; set; }
        public string Sport { get; set; }
        public Int16? CountryId { get; set; }
        public string Country { get; set; }
        public Int16 GenderId { get; set; }
        public string Gender { get; set; }
        public Int16? PlayerPositionId { get; set; }
        public string PlayerPosition { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string PlayerHand { get; set; }
        public bool IsActive { get; set; }
        public string ShirtNo { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? ApiPlayerId { get; set; }
        public string SportCCPlayerId { get; set; }
        public string MarketValue { get; set; }
        public string Foot { get; set; }
        public string PlayerAgent { get; set; }
        public Int16 BirthCityId { get; set; }
        public string BirthCity { get; set; }
        public Int16 BirthCountryId { get; set; }
        public string BirthCountry { get; set; }
        public string PlayerImage { get; set; }
        public int? SharkScoresId { get; set; }
        public int? SoccerwayPlayerId { get; set; }
        public int? EnetPlayerId { get; set; }
        public string Age { get; set; }
        public string CountryOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int PlayerTeamId { get; set; }
        public bool PlayerActive { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int? StatsComPlayerId { get; set; }
        public string StatsComPlayerName { get; set; }
        public string CrowdyLink { get; set; }
        public decimal MatchId { get; set; }
    }

    public class PlayerOverview
    {
        public Nullable<int> CareerSingleTitle { get; set; }
        public Nullable<int> CareerMatchesWon { get; set; }
        public Nullable<int> YTDMatchWon { get; set; }
        public Nullable<int> CareerDoublesTitle { get; set; }
        public Nullable<int> CareerMatchesLost { get; set; }
        public Nullable<int> YTDMatchesLost { get; set; }
        public Nullable<int> WeekRankSingle { get; set; }
        public Nullable<int> HighRankSingle { get; set; }
        public Nullable<int> PlayerId { get; set; }
        public Nullable<int> YTDServiceGamesWon { get; set; }
        public Nullable<int> YTDReturnGamesWon { get; set; }
    }

    public class RankingPlayersProfiles
    {
        public int Id { get; set; }
        public Nullable<int> Move { get; set; }
        public string MoveType { get; set; }
        public Nullable<int> Ranking { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> Points { get; set; }
        public Nullable<int> TournPlayed { get; set; }
        public Nullable<int> PointDropping { get; set; }
        public Nullable<int> NextBest { get; set; }
        public Nullable<int> SportCCPlayerId { get; set; }
        public string AtpId { get; set; }
        public string PlayerLink { get; set; }
        public string CoachName { get; set; }
        public string Residence { get; set; }
        public string PlayerPosition { get; set; }
        public string TurnPro { get; set; }
        public Nullable<int> GenderId { get; set; }
        public Nullable<int> WtaTId { get; set; }
        public Nullable<int> PlayerId { get; set; }
    }

    public class TennisStatsType
    {
        public int StatsTypeId { get; set; }
        public string Value { get; set; }
    }

    public class TennisStats
    {
        public int PlayerId { get; set; }
        public int StatsTypeId { get; set; }
        public string Points { get; set; }
        public int SeasonId { get; set; }
    }

    public class ContestGroupRounds
    {
        public int ContestGroupRoundId { get; set; }
        public int ContestGroupId { get; set; }
        public string Round { get; set; }
    }

    public class ATPWorldTourStats
    {
        public string CrawlerLink { get; set; }
        public string FixtureLink3 { get; set; }
        public int ContestGroupId { get; set; }
        public decimal MatchId { get; set; }
    }

    public class MatchIdentity
    {
        public int MatchId { get; set; }
        public int ContestGroupId { get; set; }
        public MatchIdentity(int mId, int cId)
        {
            this.MatchId = mId;
            this.ContestGroupId = cId;
        }
    }

    public class LiveMatch
    {
        public int ContestGroupId { get; set; }
        public int MatchId { get; set; }
        public string FixtureLink3 { get; set; }
        public string CrawlerLink { get; set; }
    }
}