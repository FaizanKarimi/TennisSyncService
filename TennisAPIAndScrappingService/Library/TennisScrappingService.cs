using Dapper;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using TennisAPIAndScrappingService.DatabaseModels;
using TennisAPIAndScrappingService.ResponseModels;

namespace TennisAPIAndScrappingService.Library
{
    public class TennisScrappingService
    {
        #region Private Members
        private string _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        #endregion

        #region Public Methods
        public void ScrapATPWorldTourTennisMatchData(object state)
        {
            try
            {
                string json = string.Empty;
                int seasonId = DateTime.Now.Year;
                List<ATPWorldTourStats> aTPWorldTourStatsList = _GetATPWorldTourContestGroupsAndMatches();
                foreach (ATPWorldTourStats aTPWorldTourStats in aTPWorldTourStatsList)
                {
                    try
                    {
                        string url = string.Concat("http://www.atpworldtour.com/-/ajax/MatchStats/Live/", seasonId, "/", aTPWorldTourStats.FixtureLink3 + "/", aTPWorldTourStats.CrawlerLink);
                        using (TennisWebClient _webClient = new TennisWebClient())
                        {
                            json = _webClient.DownloadString(url);
                        }

                        ResponseData _responseData = JsonConvert.DeserializeObject<ResponseData>(json);

                        Match _tennisMatch = _responseData.Match;
                        PlayerTeam1 playerTeamOne = _tennisMatch?.PlayerTeam1;
                        PlayerTeam2 playerTeamTwo = _tennisMatch?.PlayerTeam2;

                        #region Player One Stats Objects
                        Sets servicePointsWonPlayerOne = playerTeamOne?.Sets.FirstOrDefault();
                        Sets breakPointsConvertedPlayerOne = playerTeamOne?.Sets.FirstOrDefault();
                        Sets pointsWonPlayerOne = playerTeamOne?.Sets.FirstOrDefault();
                        Sets serviceGamesWonPlayerOne = playerTeamOne?.Sets.FirstOrDefault();
                        List<Sets> gamesWonPlayerOne = playerTeamOne?.Sets;
                        #endregion

                        #region Player Two Stats Objects
                        Sets servicePointsWonPlayerTwo = playerTeamTwo?.Sets.FirstOrDefault();
                        Sets breakPointsConvertedPlayerTwo = playerTeamTwo?.Sets.FirstOrDefault();
                        Sets pointsWonPlayerTwo = playerTeamTwo?.Sets.FirstOrDefault();
                        Sets serviceGamesWonPlayerTwo = playerTeamTwo?.Sets.FirstOrDefault();
                        List<Sets> gamesWonPlayerTwo = playerTeamTwo?.Sets;
                        #endregion

                        #region Player One Stats
                        int? servicePointsWonPercentPlayerOne = servicePointsWonPlayerOne?.Stats.PointStats.TotalServicePointsWon.Percent;
                        int? servicePointsWonDividendPlayerOne = servicePointsWonPlayerOne?.Stats.PointStats.TotalServicePointsWon.Dividend;
                        int? servicePointsWonDivisorPlayerOne = servicePointsWonPlayerOne?.Stats.PointStats.TotalServicePointsWon.Divisor;

                        int? breakPointsConvertedPercentPlayerOne = breakPointsConvertedPlayerOne?.Stats.ReturnStats.BreakPointsConverted.Percent;
                        int? breakPointsConvertedDividendPlayerOne = breakPointsConvertedPlayerOne?.Stats.ReturnStats.BreakPointsConverted.Dividend;
                        int? breakPointsConvertedDivisorPlayerOne = breakPointsConvertedPlayerOne?.Stats.ReturnStats.BreakPointsConverted.Divisor;

                        int? pointsWonPercentPlayerOne = pointsWonPlayerOne?.Stats.PointStats.TotalPointsWon.Percent;
                        int? pointsWonDividendPlayerOne = pointsWonPlayerOne?.Stats.PointStats.TotalPointsWon.Dividend;
                        int? pointsWonDivisorPlayerOne = pointsWonPlayerOne?.Stats.PointStats.TotalPointsWon.Divisor;

                        int? serviceGamesWonPercentPlayerOne = serviceGamesWonPlayerOne?.Stats.ServiceStats.FirstServePointsWon.Percent;
                        int? serviceGamesWonDividendPlayerOne = serviceGamesWonPlayerOne?.Stats.ServiceStats.FirstServePointsWon.Dividend;
                        int? serviceGamesWonDivisorPlayerOne = serviceGamesWonPlayerOne?.Stats.ServiceStats.FirstServePointsWon.Divisor;

                        int gamesWonPercentPlayerOne = _CalculateGamesWonPercent(Convert.ToInt32(gamesWonPlayerTwo.Sum(x => x.SetScore) + gamesWonPlayerOne.Sum(x => x.SetScore)), Convert.ToInt32(gamesWonPlayerOne.Sum(x => x.SetScore)));
                        int? gamesWonDividendPlayerOne = gamesWonPlayerOne.Sum(x => x.SetScore);
                        #endregion

                        #region Player Two Stats
                        int? servicePointsWonPercentPlayerTwo = servicePointsWonPlayerTwo?.Stats.PointStats.TotalServicePointsWon.Percent;
                        int? servicePointsWonDividendPlayerTwo = servicePointsWonPlayerTwo?.Stats.PointStats.TotalServicePointsWon.Dividend;
                        int? servicePointsWonDivisorPlayerTwo = servicePointsWonPlayerTwo?.Stats.PointStats.TotalServicePointsWon.Divisor;

                        int? breakPointsConvertedPercentPlayerTwo = breakPointsConvertedPlayerTwo?.Stats.ReturnStats.BreakPointsConverted.Percent;
                        int? breakPointsConvertedDividendPlayerTwo = breakPointsConvertedPlayerTwo?.Stats.ReturnStats.BreakPointsConverted.Dividend;
                        int? breakPointsConvertedDivisorPlayerTwo = breakPointsConvertedPlayerTwo?.Stats.ReturnStats.BreakPointsConverted.Divisor;

                        int? pointsWonPercentPlayerTwo = pointsWonPlayerTwo?.Stats.PointStats.TotalPointsWon.Percent;
                        int? pointsWonDividendPlayerTwo = pointsWonPlayerTwo?.Stats.PointStats.TotalPointsWon.Dividend;
                        int? pointsWonDivisorPlayerTwo = pointsWonPlayerTwo?.Stats.PointStats.TotalPointsWon.Divisor;

                        int? serviceGamesWonPercentPlayerTwo = serviceGamesWonPlayerTwo?.Stats.ServiceStats.FirstServePointsWon.Percent;
                        int? serviceGamesWonDividendPlayerTwo = serviceGamesWonPlayerTwo?.Stats.ServiceStats.FirstServePointsWon.Dividend;
                        int? serviceGamesWonDivisorPlayerTwo = serviceGamesWonPlayerTwo?.Stats.ServiceStats.FirstServePointsWon.Divisor;

                        var gamesWonPercentPlayerTwo = _CalculateGamesWonPercent(Convert.ToInt32(gamesWonPlayerTwo.Sum(x => x.SetScore) + gamesWonPlayerOne.Sum(x => x.SetScore)), Convert.ToInt32(gamesWonPlayerTwo.Sum(x => x.SetScore)));
                        var gamesWonDividendPlayerTwo = gamesWonPlayerTwo.Sum(x => x.SetScore);
                        #endregion

                        //Getting tennis match info, if exist then update else insert.
                        TennisMatchInfo _updatedTennisMatchInfo = _GetTennisMatchInfo(aTPWorldTourStats.MatchId, aTPWorldTourStats.ContestGroupId);
                        if (_updatedTennisMatchInfo != null)
                        {
                            #region Update Tennis Match Info
                            _updatedTennisMatchInfo.MatchId = aTPWorldTourStats.MatchId;
                            _updatedTennisMatchInfo.ContestGroupId = aTPWorldTourStats.ContestGroupId;
                            _updatedTennisMatchInfo.BreakPointsConvertedDividendTeam1 = breakPointsConvertedDividendPlayerOne;
                            _updatedTennisMatchInfo.BreakPointsConvertedDivisorTeam1 = breakPointsConvertedDivisorPlayerOne;
                            _updatedTennisMatchInfo.BreakPointsConvertedPercentTeam1 = breakPointsConvertedPercentPlayerOne;
                            _updatedTennisMatchInfo.ServicePointsWonPercentTeam1 = servicePointsWonPercentPlayerOne;
                            _updatedTennisMatchInfo.ServicePointsWonDividendTeam1 = servicePointsWonDividendPlayerOne;
                            _updatedTennisMatchInfo.ServicePointsWonDivisorTeam1 = servicePointsWonDivisorPlayerOne;
                            _updatedTennisMatchInfo.TotalPointsWonPercentTeam1 = pointsWonPercentPlayerOne;
                            _updatedTennisMatchInfo.TotalPointsWonDividendTeam1 = pointsWonDividendPlayerOne;
                            _updatedTennisMatchInfo.TotalPointsWonDivisorTeam1 = pointsWonDivisorPlayerOne;
                            _updatedTennisMatchInfo.BreakPointsConvertedDividendTeam2 = breakPointsConvertedDividendPlayerTwo;
                            _updatedTennisMatchInfo.BreakPointsConvertedDivisorTeam2 = breakPointsConvertedDivisorPlayerTwo;
                            _updatedTennisMatchInfo.BreakPointsConvertedPercentTeam2 = breakPointsConvertedPercentPlayerTwo;
                            _updatedTennisMatchInfo.ServicePointsWonPercentTeam2 = servicePointsWonPercentPlayerTwo;
                            _updatedTennisMatchInfo.ServicePointsWonDividendTeam2 = servicePointsWonDividendPlayerTwo;
                            _updatedTennisMatchInfo.ServicePointsWonDivisorTeam2 = servicePointsWonDivisorPlayerTwo;
                            _updatedTennisMatchInfo.TotalPointsWonPercentTeam2 = pointsWonPercentPlayerTwo;
                            _updatedTennisMatchInfo.TotalPointsWonDividendTeam2 = pointsWonDividendPlayerTwo;
                            _updatedTennisMatchInfo.TotalPointsWonDivisorTeam2 = pointsWonDivisorPlayerTwo;
                            _updatedTennisMatchInfo.TotalGamesWonPercentTeam1 = gamesWonPercentPlayerOne;
                            _updatedTennisMatchInfo.TotalGamesWonDividendTeam1 = gamesWonDividendPlayerOne;
                            _updatedTennisMatchInfo.TotalGamesWonPercentTeam2 = gamesWonPercentPlayerTwo;
                            _updatedTennisMatchInfo.TotalGamesWonDividendTeam2 = gamesWonDividendPlayerTwo;
                            _updatedTennisMatchInfo.FirstServePointsWonPercentTeam1 = serviceGamesWonPercentPlayerOne;
                            _updatedTennisMatchInfo.FirstServePointsWonDividendTeam1 = serviceGamesWonDividendPlayerOne;
                            _updatedTennisMatchInfo.FirstServePointsWonDividerTeam1 = serviceGamesWonDivisorPlayerOne;
                            _updatedTennisMatchInfo.FirstServePointsWonPercentTeam2 = serviceGamesWonPercentPlayerTwo;
                            _updatedTennisMatchInfo.FirstServePointsWonDividendTeam2 = serviceGamesWonDividendPlayerTwo;
                            _updatedTennisMatchInfo.FirstServePointsWonDividerTeam2 = serviceGamesWonDivisorPlayerTwo;
                            //Update the existing match info.
                            _UpdateTennisMatchInfo(_updatedTennisMatchInfo);
                            #endregion
                        }
                        else
                        {
                            #region Insert Tennis Match Info
                            TennisMatchInfo tennisMatchInfo = new TennisMatchInfo()
                            {
                                MatchId = aTPWorldTourStats.MatchId,
                                ContestGroupId = aTPWorldTourStats.ContestGroupId,
                                BreakPointsConvertedDividendTeam1 = breakPointsConvertedDividendPlayerOne,
                                BreakPointsConvertedDivisorTeam1 = breakPointsConvertedDivisorPlayerOne,
                                BreakPointsConvertedPercentTeam1 = breakPointsConvertedPercentPlayerOne,
                                ServicePointsWonPercentTeam1 = servicePointsWonPercentPlayerOne,
                                ServicePointsWonDividendTeam1 = servicePointsWonDividendPlayerOne,
                                ServicePointsWonDivisorTeam1 = servicePointsWonDivisorPlayerOne,
                                TotalPointsWonPercentTeam1 = pointsWonPercentPlayerOne,
                                TotalPointsWonDividendTeam1 = pointsWonDividendPlayerOne,
                                TotalPointsWonDivisorTeam1 = pointsWonDivisorPlayerOne,
                                BreakPointsConvertedDividendTeam2 = breakPointsConvertedDividendPlayerTwo,
                                BreakPointsConvertedDivisorTeam2 = breakPointsConvertedDivisorPlayerTwo,
                                BreakPointsConvertedPercentTeam2 = breakPointsConvertedPercentPlayerTwo,
                                ServicePointsWonPercentTeam2 = servicePointsWonPercentPlayerTwo,
                                ServicePointsWonDividendTeam2 = servicePointsWonDividendPlayerTwo,
                                ServicePointsWonDivisorTeam2 = servicePointsWonDivisorPlayerTwo,
                                TotalPointsWonPercentTeam2 = pointsWonPercentPlayerTwo,
                                TotalPointsWonDividendTeam2 = pointsWonDividendPlayerTwo,
                                TotalPointsWonDivisorTeam2 = pointsWonDivisorPlayerTwo,
                                TotalGamesWonPercentTeam1 = gamesWonPercentPlayerOne,
                                TotalGamesWonDividendTeam1 = gamesWonDividendPlayerOne,
                                TotalGamesWonPercentTeam2 = gamesWonPercentPlayerTwo,
                                TotalGamesWonDividendTeam2 = gamesWonDividendPlayerTwo,
                                FirstServePointsWonPercentTeam1 = serviceGamesWonPercentPlayerOne,
                                FirstServePointsWonDividendTeam1 = serviceGamesWonDividendPlayerOne,
                                FirstServePointsWonDividerTeam1 = serviceGamesWonDivisorPlayerOne,
                                FirstServePointsWonPercentTeam2 = serviceGamesWonPercentPlayerTwo,
                                FirstServePointsWonDividendTeam2 = serviceGamesWonDividendPlayerTwo,
                                FirstServePointsWonDividerTeam2 = serviceGamesWonDivisorPlayerTwo
                            };
                            //Insert the match info.
                            _InsertTennisMatchInfo(tennisMatchInfo);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        _LogException(ex.Message, "ScrapATPWorldTourTennisMatchData");
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ScrapATPWorldTourTennisMatchData");
            }
        }

        public void ScrapTennisH2HData(object state)
        {
            try
            {
                string json = string.Empty;
                int season = DateTime.Now.Year;
                List<ATPWorldTourStats> aTPWorldTourStatsList = _GetATPWorldTourContestGroupsAndMatches();
                foreach (ATPWorldTourStats aTPWorldTourStats in aTPWorldTourStatsList)
                {
                    try
                    {
                        string url = string.Concat("http://www.atpworldtour.com/-/ajax/MatchStats/Live/", season, "/" + aTPWorldTourStats.FixtureLink3, "/", aTPWorldTourStats.CrawlerLink);
                        using (TennisWebClient _webClient = new TennisWebClient())
                        {
                            json = _webClient.DownloadString(url);
                        }

                        ResponseData _response = JsonConvert.DeserializeObject<ResponseData>(json);
                        var playerOneServiceRecordStats = _response.Match.PlayerTeam1.YearToDateStats.ServiceRecordStats;
                        var playerOneReturnRecordsStats = _response.Match.PlayerTeam1.YearToDateStats.ReturnRecordStats;

                        var playerTwoServiceRecordStats = _response.Match.PlayerTeam2.YearToDateStats.ServiceRecordStats;
                        var playerTwoReturnRecordsStats = _response.Match.PlayerTeam2.YearToDateStats.ReturnRecordStats;

                        var playerIdOne = _response.Match.PlayerTeam1.PlayerId;
                        var playerIdTwo = _response.Match.PlayerTeam2.PlayerId;

                        var playerOneRankingPlayerProfile = _GetPlayerProfileInformationForTennis(playerIdOne);
                        var playerTwoRankingPlayerProfile = _GetPlayerProfileInformationForTennis(playerIdTwo);

                        #region Player One Ranking Profile
                        if (playerOneRankingPlayerProfile != null)
                        {
                            DatabaseModels.PlayerOverview playerOverviewOne = new DatabaseModels.PlayerOverview()
                            {
                                YTDServiceGamesWon = playerOneServiceRecordStats.ServiceGamesWon.Percent,
                                YTDReturnGamesWon = playerOneReturnRecordsStats.ReturnGamesWon.Percent,
                                PlayerId = playerOneRankingPlayerProfile.Id
                            };
                            _UpdatePlayerOverviewYTD(playerOverviewOne);
                        }
                        #endregion

                        #region Player Two Ranking Profile
                        if (playerTwoRankingPlayerProfile != null)
                        {
                            DatabaseModels.PlayerOverview playerOverviewTwo = new DatabaseModels.PlayerOverview()
                            {
                                YTDServiceGamesWon = playerTwoServiceRecordStats.ServiceGamesWon.Percent,
                                YTDReturnGamesWon = playerTwoReturnRecordsStats.ReturnGamesWon.Percent,
                                PlayerId = playerTwoRankingPlayerProfile.Id
                            };
                            _UpdatePlayerOverviewYTD(playerOverviewTwo);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _LogException(ex.Message, "ScrapTennisH2HData");
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ScrapTennisH2HData");
            }
        }

        public void AverageAcePerMatch(object state)
        {
            try
            {
                double unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string json = string.Empty;
                string url = string.Concat("http://www.ultimatetennisstatistics.com/statsLeadersTable?current=1&rowCount=-1&sort%5Bvalue%5D=desc&searchPhrase=&category=acesPerMatch&season=2018&fromDate=&toDate=&level=&bestOf=&surface=&indoor=&round=&result=&tournamentId=&opponent=&countryId=&minEntries=&_=", unixTimestamp);
                Data _data = null;
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString(url);
                }
                _data = JsonConvert.DeserializeObject<Data>(json);
                _InsertData(_data.rows, 3);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "AverageAcePerMatch");
            }
        }

        public void ServiceGamesWon(object state)
        {
            try
            {
                double unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string json = string.Empty;
                string url = string.Concat("http://www.ultimatetennisstatistics.com/statsLeadersTable?current=1&rowCount=-1&sort%5Bvalue%5D=desc&searchPhrase=&category=serviceGamesWonPct&season=2018&fromDate=&toDate=&level=&bestOf=&surface=&indoor=&round=&result=&tournamentId=&opponent=&countryId=&minEntries=&_=", unixTimestamp);
                Data _data = null;
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString(url);
                }

                _data = JsonConvert.DeserializeObject<Data>(json);
                _InsertData(_data.rows, 1);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ServiceGamesWon");
            }
        }

        public void ReturnGamesWon(object state)
        {
            try
            {
                double unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string json = string.Empty;
                string url = string.Concat("http://www.ultimatetennisstatistics.com/statsLeadersTable?current=1&rowCount=-1&sort%5Bvalue%5D=desc&searchPhrase=&category=returnGamesWonPct&season=2018&fromDate=&toDate=&level=&bestOf=&surface=&indoor=&round=&result=&tournamentId=&opponent=&countryId=&minEntries=&_=", unixTimestamp);
                Data _data = null;
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString(url);
                }
                _data = JsonConvert.DeserializeObject<Data>(json);
                _InsertData(_data.rows, 2);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ReturnGamesWon");
            }
        }

        public void SinglesRanking(object state)
        {
            try
            {
                double unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string json = string.Empty;
                string url = string.Concat("http://www.ultimatetennisstatistics.com/rankingsTableTable?current=1&rowCount=100&searchPhrase=&rankType=RANK&season=&date=&_=", unixTimestamp);
                Data _data = null;
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString(url);
                }
                _data = JsonConvert.DeserializeObject<Data>(json);
                _InsertSingleRankingData(_data.rows, 4);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ReturnGamesWon");
            }
        }

        public void ScrapATPRankingDataSingle(object state)
        {
            try
            {
                string rankCategory = "Single";
                HtmlDocument document = new HtmlDocument();
                string url = "http://www.atpworldtour.com/en/rankings/singles?rankDate=2018-06-25&rankRange=1-5000&ajax=true";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document = new HtmlDocument();
                    document.LoadHtml(page);
                }

                var parentDiv = document.DocumentNode.SelectSingleNode("//*[@id='rankingDetailAjaxContainer']");
                int colums = document.DocumentNode.SelectNodes("//tr").Count();

                using (var db = new SportsDataPanelEntities())
                {
                    for (int i = 1; i < colums; i++)
                    {
                        string moveUpValue = string.Empty;
                        string moveDownValue = string.Empty;
                        string hyphenValue = string.Empty;

                        var ranking = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[1]").InnerHtml);
                        var playerName = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[4]/a").InnerHtml;
                        var playerlink = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[4]/a").Attributes["href"].Value;
                        var country = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[3]/div/div/img").Attributes["alt"].Value;
                        var playerAge = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[5]").InnerHtml);
                        var points = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[6]/a").InnerHtml;
                        var tournamentsPlayed = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[7]/a").InnerHtml;
                        var pointsDropping = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[8]").InnerHtml);
                        var nextBest = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[9]").InnerHtml);

                        playerlink = playerlink.Replace("overview", "");
                        string[] playerLinkSplit = playerlink.Split('/');
                        string atpid = playerLinkSplit[4];

                        var move = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]");
                        string moveType = string.Empty;
                        if (move != null)
                        {
                            var moveUp = move.SelectSingleNode("div[@class='move-up']");
                            if (moveUp != null)
                            {
                                moveUpValue = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]/div[2]").InnerHtml);
                                moveType = "+";
                            }
                            else
                            {
                                moveDownValue = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]/div[2]").InnerHtml);
                                if (string.IsNullOrEmpty(moveDownValue))
                                    hyphenValue = "-";
                                else
                                    moveType = "-";
                            }
                        }

                        string moveValue = string.IsNullOrEmpty(moveUpValue) ? moveDownValue : moveUpValue;

                        try
                        {
                            RankingPlayersProfiles rankingItem = new RankingPlayersProfiles();
                            rankingItem.Ranking = Convert.ToInt32(ranking);
                            rankingItem.Move = string.IsNullOrEmpty(moveValue) ? 0 : Convert.ToInt32(moveValue);
                            rankingItem.MoveType = string.IsNullOrEmpty(moveType) ? "0" : moveType;
                            rankingItem.Country = country;
                            rankingItem.PlayerLink = playerlink;
                            rankingItem.AtpId = atpid;
                            rankingItem.FirstName = playerName.Split(' ').FirstOrDefault();
                            rankingItem.LastName = playerName.Split(' ').LastOrDefault();
                            rankingItem.Age = Convert.ToInt32(playerAge);
                            rankingItem.Points = Convert.ToInt32(points);
                            rankingItem.TournPlayed = Convert.ToInt32(tournamentsPlayed);
                            rankingItem.PointDropping = Convert.ToInt32(pointsDropping);
                            rankingItem.NextBest = Convert.ToInt32(nextBest);
                            rankingItem.GenderId = (int)Gender.MALE;
                            rankingItem.Type = rankCategory;

                            var playerProfile = db.RankingPlayersProfiles.FirstOrDefault(r => r.AtpId == atpid && r.Type == rankCategory);

                            if (rankCategory.Equals("RaceToLondon"))
                            {
                                var singleRank = db.RankingPlayersProfiles.FirstOrDefault(r => r.AtpId == atpid && r.Type == "Single");
                                if (singleRank != null)
                                {
                                    rankingItem.PlayerId = playerProfile.PlayerId == null ? singleRank.PlayerId : playerProfile.PlayerId;
                                    rankingItem.SportCCPlayerId = playerProfile.SportCCPlayerId == null ? singleRank.SportCCPlayerId : playerProfile.SportCCPlayerId;
                                }
                            }

                            if (playerProfile == null)
                                db.RankingPlayersProfiles.Add(rankingItem);
                            else
                            {
                                playerProfile.Ranking = rankingItem.Ranking;
                                playerProfile.Move = rankingItem.Move;
                                playerProfile.MoveType = rankingItem.MoveType;
                                playerProfile.PlayerLink = playerlink;
                                rankingItem.Age = rankingItem.Age;
                                playerProfile.Points = rankingItem.Points;
                                playerProfile.TournPlayed = rankingItem.TournPlayed;
                                playerProfile.PointDropping = rankingItem.PointDropping;
                                playerProfile.NextBest = rankingItem.NextBest;
                                playerProfile.GenderId = (int)Gender.MALE;
                                playerProfile.Type = rankCategory;
                                if (rankCategory.Equals("RaceToLondon"))
                                {
                                    playerProfile.PlayerId = rankingItem.PlayerId;
                                    playerProfile.SportCCPlayerId = rankingItem.SportCCPlayerId;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _LogException(ex.Message, "ScrapATPRankingDataSingle/EF");
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ScrapATPRankingDataSingle");
            }
        }

        public void ScrapATPRankingDataRaceToLondon(object state)
        {
            try
            {
                string rankCategory = "RaceToLondon";
                HtmlDocument document = new HtmlDocument();
                string url = "http://www.atpworldtour.com/en/rankings/singles-race-to-london?rankDate=2018-06-25&rankRange=1-5000";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document = new HtmlDocument();
                    document.LoadHtml(page);
                }

                var parentDiv = document.DocumentNode.SelectSingleNode("//*[@id='rankingDetailAjaxContainer']");
                int colums = document.DocumentNode.SelectNodes("//tr").Count();

                using (var db = new SportsDataPanelEntities())
                {
                    for (int i = 1; i < colums; i++)
                    {
                        string moveUpValue = string.Empty;
                        string moveDownValue = string.Empty;
                        string hyphenValue = string.Empty;

                        var ranking = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[1]").InnerHtml);
                        var playerName = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[4]/a").InnerHtml;
                        var playerlink = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[4]/a").Attributes["href"].Value;
                        var country = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[3]/div/div/img").Attributes["alt"].Value;
                        var playerAge = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[5]").InnerHtml);
                        var points = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[6]/a").InnerHtml;
                        var tournamentsPlayed = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[7]/a").InnerHtml;
                        var pointsDropping = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[8]").InnerHtml);
                        var nextBest = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[9]").InnerHtml);

                        playerlink = playerlink.Replace("overview", "");
                        string[] playerLinkSplit = playerlink.Split('/');
                        string atpid = playerLinkSplit[4];

                        var move = parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]");
                        string moveType = string.Empty;
                        if (move != null)
                        {
                            var moveUp = move.SelectSingleNode("div[@class='move-up']");
                            if (moveUp != null)
                            {
                                moveUpValue = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]/div[2]").InnerHtml);
                                moveType = "+";
                            }
                            else
                            {
                                moveDownValue = _ExtractNumber(parentDiv.SelectSingleNode("table/tbody/tr[" + i + "]/td[2]/div[2]").InnerHtml);
                                if (string.IsNullOrEmpty(moveDownValue))
                                    hyphenValue = "-";
                                else
                                    moveType = "-";
                            }
                        }

                        string moveValue = string.IsNullOrEmpty(moveUpValue) ? moveDownValue : moveUpValue;

                        try
                        {
                            RankingPlayersProfiles rankingItem = new RankingPlayersProfiles();
                            try
                            {
                                rankingItem.Ranking = Convert.ToInt32(ranking);
                                rankingItem.Move = string.IsNullOrEmpty(moveValue) ? 0 : Convert.ToInt32(moveValue);
                                rankingItem.MoveType = string.IsNullOrEmpty(moveType) ? "0" : moveType;
                                rankingItem.Country = country;
                                rankingItem.PlayerLink = playerlink;
                                rankingItem.AtpId = atpid;
                                rankingItem.FirstName = playerName.Split(' ').FirstOrDefault();
                                rankingItem.LastName = playerName.Split(' ').LastOrDefault();
                                rankingItem.Age = Convert.ToInt32(playerAge);
                                rankingItem.Points = Convert.ToInt32(points);
                                rankingItem.TournPlayed = Convert.ToInt32(tournamentsPlayed);
                                rankingItem.PointDropping = Convert.ToInt32(pointsDropping);
                                rankingItem.NextBest = Convert.ToInt32(nextBest);
                                rankingItem.GenderId = (int)Gender.MALE;
                                rankingItem.Type = rankCategory;
                            }
                            catch (Exception ex)
                            {
                                _LogException(ex.Message + "Node donot exist", "ScrapATPRankingDataRaceToLondon");
                            }

                            var playerProfile = db.RankingPlayersProfiles.FirstOrDefault(r => r.AtpId == atpid && r.Type == rankCategory);

                            if (rankCategory.Equals("RaceToLondon"))
                            {
                                var singleRank = db.RankingPlayersProfiles.FirstOrDefault(r => r.AtpId == atpid && r.Type == "Single");
                                if (singleRank != null)
                                {
                                    rankingItem.PlayerId = singleRank.PlayerId;
                                    rankingItem.SportCCPlayerId = singleRank.SportCCPlayerId;
                                }
                            }

                            if (playerProfile == null)
                                db.RankingPlayersProfiles.Add(rankingItem);
                            else
                            {
                                playerProfile.Ranking = rankingItem.Ranking;
                                playerProfile.Move = rankingItem.Move;
                                playerProfile.MoveType = rankingItem.MoveType;
                                playerProfile.PlayerLink = playerlink;
                                rankingItem.Age = rankingItem.Age;
                                playerProfile.Points = rankingItem.Points;
                                playerProfile.TournPlayed = rankingItem.TournPlayed;
                                playerProfile.PointDropping = rankingItem.PointDropping;
                                playerProfile.NextBest = rankingItem.NextBest;
                                playerProfile.GenderId = (int)Gender.MALE;
                                playerProfile.Type = rankCategory;
                                if (rankCategory.Equals("RaceToLondon"))
                                {
                                    playerProfile.PlayerId = rankingItem.PlayerId;
                                    playerProfile.SportCCPlayerId = rankingItem.SportCCPlayerId;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _LogException(ex.Message, "ScrapATPRankingDataRaceToLondon/EF");
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "ScrapATPRankingDataRaceToLondon");
            }
        }

        public void GetWTARankingData(object state)
        {
            try
            {
                string json = string.Empty;
                try
                {
                    using (TennisWebClient _webClient = new TennisWebClient())
                    {
                        json = _webClient.DownloadString("http://www.wtatennis.com/node/234741/singles/ranking.json");
                    }
                }
                catch (Exception e)
                {
                    _LogException(e.Message, "GetWTARankingData-WebClient");
                }

                if (!string.IsNullOrEmpty(json))
                {
                    List<WTARanking> wtadata = JsonConvert.DeserializeObject<List<WTARanking>>(json);

                    using (var db = new SportsDataPanelEntities())
                    {
                        foreach (var item in wtadata)
                        {
                            try
                            {
                                RankingPlayersProfiles rankingItem = new RankingPlayersProfiles();

                                rankingItem.Age = item.Age;

                                var counrty_item = item.Country.Split('\"');
                                rankingItem.Country = counrty_item[1];

                                var playerlink_item = item.FullName.Split('\"');
                                rankingItem.PlayerLink = playerlink_item[3];

                                var atp_id = playerlink_item[3].Split('/');
                                rankingItem.AtpId = atp_id[2];

                                var player_item = item.FullName.Split('>');
                                var fullname = player_item[3].Replace("</a", "").Split(' ');
                                rankingItem.FirstName = fullname[0];
                                rankingItem.LastName = fullname[1];

                                rankingItem.GenderId = (int)Gender.FEMALE;
                                rankingItem.TournPlayed = item.Tourn;
                                rankingItem.Ranking = item.Rank;
                                rankingItem.Points = item.Points;

                                var record = db.RankingPlayersProfiles.FirstOrDefault(r => r.AtpId == rankingItem.AtpId);

                                if (record == null)
                                    db.RankingPlayersProfiles.Add(rankingItem);
                                else
                                {
                                    record.FirstName = rankingItem.FirstName;
                                    record.LastName = rankingItem.LastName;
                                    record.Age = rankingItem.Age;
                                    record.Country = rankingItem.Country;
                                    record.PlayerLink = rankingItem.PlayerLink;
                                    record.Ranking = rankingItem.Ranking;
                                    record.Points = rankingItem.Points;
                                }

                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                _LogException(ex.Message, "GetWTARankingData");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetWTARankingData");
            }
        }

        public void GetWomenDataForPlayerProfile(object state)
        {
            try
            {
                List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
                using (var db = new SportsDataPanelEntities())
                {
                    list = db.RankingPlayersProfiles.Where(s => s.GenderId == 4).OrderBy(r => r.Ranking).ToList();
                }

                foreach (var item in list)
                {
                    string playerUrl = item.AtpId;
                    int playerId = item.PlayerId ?? 0;
                    if (playerId != 0)
                    {
                        _GetWomenPlayerProfile(playerUrl, item.Id, playerId, item.FirstName, item.LastName);
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetWomenDataForPlayerProfile/Parent");
            }
        }

        public void GetWomenDataForPlayerImage(object state)
        {
            try
            {
                List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
                using (var db = new SportsDataPanelEntities())
                {
                    list = db.RankingPlayersProfiles.Where(s => s.GenderId == 4).OrderBy(r => r.Ranking).ToList();
                }

                foreach (var item in list)
                {
                    string playerUrl = item.AtpId;
                    int playerId = item.PlayerId ?? 0;
                    if (playerId != 0)
                    {
                        _GetWomenPlayerImage(playerUrl, item.Id, playerId, item.FirstName, item.LastName);
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetWomenDataForPlayerImage");
            }
        }

        public void GetMenDataForPlayerProfileSingle(object state)
        {
            string type = "Single";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                string playerUrl = item.PlayerLink;
                int playerId = item.PlayerId ?? 0;
                if (playerId != 0)
                {
                    _GetMenPlayerProfile(playerUrl, playerId);
                }
            }
            _GetMenDataForPlayerImageSingle();
        }

        public void GetMenDataForPlayerProfileRaceToLondon(object state)
        {
            string type = "RaceToLondon";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                string playerUrl = item.PlayerLink;
                int playerId = item.PlayerId ?? 0;
                if (playerId != 0)
                {
                    _GetMenPlayerProfile(playerUrl, playerId);
                }
            }
            _GetMenDataForPlayerImageRaceToLondon();
        }

        public void GetMenDataForPlayerOverview(object state)
        {
            string type = "Single";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                _GetMenPlayerOverview(item.PlayerLink, item.Id, type, "GetMenDataForPlayerOverview");
            }
            _GetMenDataForPlayerRaceToLondonOverview();
        }

        public void GetMenDataForPlayerGrandSlamSingleResult(object state)
        {
            string type = "Single";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                for (int i = 0; i <= 7; i++)
                {
                    _GetMenPlayerGrandSlamSingeResult(item.PlayerLink, item.Id, "201" + i, type);
                }
            }
            _GetMenDataForPlayerGrandSlamRaceToLondonResult();
        }

        public void CrawlSportingbetTennisMatches(object state)
        {
            try
            {
                Random rand = new Random();
                byte[] buf = new byte[8];
                rand.NextBytes(buf);
                long longRand = BitConverter.ToInt64(buf, 0);
                var tennisMatches = CrawlSharkScoresMatchByURL("https://sportingbet.enetscores.com//default/livescore/update.2.json?_=" + Math.Abs(longRand).ToString());
                if (tennisMatches != null)
                {
                    UpdateTennisStatusesAndScores(tennisMatches);
                }
            }
            catch (Exception e)
            {
                using (StreamWriter sw = File.AppendText(@"c:\SportingbetCrawler.txt"))
                {
                    sw.WriteLine("===");
                    sw.WriteLine("EXCEPTION IN CRAWLING Sportinbet update.2.json");
                    sw.WriteLine("Date: " + DateTime.Now + " ||| Exception Message: " + e.Message);
                    sw.WriteLine("===");
                }
            }
        }

        public void DownloadSofaScoresTennisMatches(object state)
        {
            string json = string.Empty;
            try
            {
                //Array.ForEach(Directory.GetFiles(@"C:\SportsDataPanel\sofascore\tennis\"), File.Delete);
                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime now = DateTime.UtcNow;
                TimeSpan ts = now - start;
                Int64 ms = (Int64)ts.TotalSeconds;
                XElement sofascore = new XElement("sofascore");
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString("https://www.sofascore.com/tennis/livescore/json?_=" + ms);
                }
                var icehockey = (JObject)JsonConvert.DeserializeObject(json);
                if (icehockey["sportItem"].Count() > 0)
                {
                    var tournaments = icehockey["sportItem"]["tournaments"].ToList();
                    foreach (var tour in tournaments)
                    {
                        var tourEvents = tour["events"].ToList();
                        foreach (var evnt in tourEvents)
                        {
                            var match = evnt.ToObject<sofascoretennismatch>();
                            sofascore.Add(SofaScoreEvent<sofascoretennismatch>(match));
                        }
                    }
                    sofascore.Save(@"C:\SportsDataPanel\sofascore\tennis\all.xml");
                }
            }
            catch (Exception e)
            {
                using (StreamWriter sw = File.AppendText(@"c:\SofaScoreCrawler.txt"))
                {
                    sw.WriteLine("Tennis###Exception### " + DateTime.Now);
                    sw.WriteLine(e.Message);
                    sw.WriteLine("==========");
                }
            }
        }

        public void CrawlWTALiveMatches(object state)
        {
            string url = string.Empty;
            string wtaMatchId = string.Empty;
            List<LiveMatch> liveMatches = new List<LiveMatch>();
            liveMatches = GetLiveMatchesWTA(9);
            foreach (LiveMatch match in liveMatches)
            {
                wtaMatchId = match.CrawlerLink;
                string wtaContestId = match.FixtureLink3;
                int season = DateTime.Now.Year;
                url = "http://www.wtatennis.com/match-stats/" + season + "/" + wtaContestId + "/" + wtaMatchId;
                HtmlDocument document = new HtmlDocument();
                TennisMatchInfo tennisMatchInfo = new TennisMatchInfo();
                string page;
                try
                {
                    using (TennisWebClient client = new TennisWebClient())
                    {
                        page = client.DownloadString(url);
                        var json = JObject.Parse(page)["data"];
                        document.LoadHtml(json.ToString());
                    }
                    HtmlNode serviceBlock, returnBlock, pointsBlock;

                    HtmlNodeCollection staticsBlock = document.DocumentNode.SelectNodes(".//div[@class='statistics-block']");
                    if (staticsBlock != null)
                    {
                        serviceBlock = staticsBlock[0];
                        returnBlock = staticsBlock[1];
                        pointsBlock = staticsBlock[2];
                        int h = 0, a = 2;
                        string statsRowClass = ".//div[@class='msm-sr']";

                        #region ServiceBlock
                        if (serviceBlock != null)
                        {
                            try
                            {
                                HtmlNode firstServeWonContainerDiv = serviceBlock.SelectNodes(statsRowClass)[3];
                                HtmlNodeCollection firstServeWonDiv = firstServeWonContainerDiv.SelectNodes("./div");
                                int outFirstServePointsWonPercentTeam1;
                                Int32.TryParse(firstServeWonDiv[h].InnerHtml.Trim(), out outFirstServePointsWonPercentTeam1);
                                tennisMatchInfo.FirstServePointsWonPercentTeam1 = outFirstServePointsWonPercentTeam1;
                                int outFirstServePointsWonPercentTeam2;
                                Int32.TryParse(firstServeWonDiv[a].InnerHtml.Trim(), out outFirstServePointsWonPercentTeam2);
                                tennisMatchInfo.FirstServePointsWonPercentTeam2 = outFirstServePointsWonPercentTeam2;
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                _LogException(ex.Message, "CrawlWTALive/ServiceBlock");
                            }
                        }
                        #endregion

                        #region ReturnBlock
                        if (returnBlock != null)
                        {
                            try
                            {
                                HtmlNode breakPointsContainerDiv = returnBlock.SelectNodes(statsRowClass)[1];
                                HtmlNodeCollection breakPointsDiv = breakPointsContainerDiv.SelectNodes("./div");
                                int outBreakPointsConvertedPercentTeam1;
                                int.TryParse(breakPointsDiv[h].InnerHtml.Trim(), out outBreakPointsConvertedPercentTeam1);
                                tennisMatchInfo.BreakPointsConvertedPercentTeam1 = outBreakPointsConvertedPercentTeam1;
                                int outBreakPointsConvertedPercentTeam2;
                                int.TryParse(breakPointsDiv[a].InnerHtml.Trim(), out outBreakPointsConvertedPercentTeam2);
                                tennisMatchInfo.BreakPointsConvertedPercentTeam2 = outBreakPointsConvertedPercentTeam2;
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                _LogException(ex.Message, "CrawlWTALive/ReturnBlock");
                            }
                        }
                        #endregion

                        #region PointsBlock
                        if (pointsBlock != null)
                        {
                            try
                            {
                                HtmlNode totalServicePointsContainerDiv = pointsBlock.SelectNodes(statsRowClass)[0];
                                HtmlNode totalPointsContainerDiv = pointsBlock.SelectNodes(statsRowClass)[2];
                                HtmlNodeCollection totalServiceDiv = totalServicePointsContainerDiv.SelectNodes("./div");
                                HtmlNodeCollection totalPointsDiv = totalPointsContainerDiv.SelectNodes("./div");
                                int outServicePointsWonPercentTeam1;
                                int.TryParse(totalServiceDiv[h].InnerHtml.Trim(), out outServicePointsWonPercentTeam1);
                                tennisMatchInfo.ServicePointsWonPercentTeam1 = outServicePointsWonPercentTeam1;
                                int outServicePointsWonPercentTeam2;
                                int.TryParse(totalServiceDiv[a].InnerHtml.Trim(), out outServicePointsWonPercentTeam2);
                                tennisMatchInfo.ServicePointsWonPercentTeam2 = outServicePointsWonPercentTeam2;
                                int outTotalPointsWonPercentTeam1;
                                int.TryParse(totalPointsDiv[h].InnerHtml.Trim(), out outTotalPointsWonPercentTeam1);
                                tennisMatchInfo.TotalPointsWonPercentTeam1 = outTotalPointsWonPercentTeam1;
                                int outTotalPointsWonPercentTeam2;
                                int.TryParse(totalPointsDiv[a].InnerHtml.Trim(), out outTotalPointsWonPercentTeam2);
                                tennisMatchInfo.TotalPointsWonPercentTeam2 = outTotalPointsWonPercentTeam2;
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                _LogException(ex.Message, "CrawlWTALive/PointsBlock");
                            }
                        }
                        #endregion
                    }

                    #region GameWon
                    HtmlNodeCollection resultBlock = document.DocumentNode.SelectNodes(".//div[@class='msm-result-table']/table");
                    if (resultBlock != null)
                    {
                        HtmlNodeCollection resultTableRows = resultBlock[0].SelectNodes(".//tr");
                        if (resultTableRows != null)
                        {
                            int numberOfSets = resultTableRows.Count - 2;
                            int homeTotalGamesWon = 0, awayTotalGamesWon = 0;
                            string homeScoreSelector = ".//td[@class='team_a']";
                            string awayScoreSelector = ".//td[@class='team_b']";
                            try
                            {
                                for (int i = 0; i < numberOfSets; i++)
                                {
                                    string homeSetScore = resultTableRows[i].SelectNodes(homeScoreSelector)[0].InnerText.Trim();
                                    string awaySetScore = resultTableRows[i].SelectNodes(awayScoreSelector)[0].InnerText.Trim();
                                    string[] homeSetScoreTokens = homeSetScore.Split('\n');
                                    string[] awaySetScoreTokens = awaySetScore.Split('\n');
                                    int outhomeTotalGamesWon;
                                    int.TryParse(homeSetScoreTokens[0], out outhomeTotalGamesWon);
                                    homeTotalGamesWon += outhomeTotalGamesWon;
                                    int outawayTotalGamesWon;
                                    int.TryParse(awaySetScoreTokens[0], out outawayTotalGamesWon);
                                    awayTotalGamesWon += outawayTotalGamesWon;
                                }
                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                _LogException(ex.Message, "CrawlWTALive/GameWon");
                            }

                            try
                            {
                                int totalGamesPlayed = homeTotalGamesWon + awayTotalGamesWon;
                                if (totalGamesPlayed == 0)
                                {
                                    tennisMatchInfo.TotalGamesWonPercentTeam1 = 0;
                                    tennisMatchInfo.TotalGamesWonPercentTeam2 = 0;
                                    tennisMatchInfo.TotalGamesWonDividendTeam1 = Convert.ToInt32(homeTotalGamesWon);
                                    tennisMatchInfo.TotalGamesWonDividendTeam2 = Convert.ToInt32(awayTotalGamesWon);
                                }
                                else
                                {
                                    int homeTotalGamesWonPercentage = Convert.ToInt32((homeTotalGamesWon * 100) / totalGamesPlayed);
                                    int awayTotalGamesWonPercentage = Convert.ToInt32((awayTotalGamesWon * 100) / totalGamesPlayed);
                                    tennisMatchInfo.TotalGamesWonPercentTeam1 = Convert.ToInt32(homeTotalGamesWonPercentage);
                                    tennisMatchInfo.TotalGamesWonPercentTeam2 = Convert.ToInt32(awayTotalGamesWonPercentage);
                                    tennisMatchInfo.TotalGamesWonDividendTeam1 = Convert.ToInt32(homeTotalGamesWon);
                                    tennisMatchInfo.TotalGamesWonDividendTeam2 = Convert.ToInt32(awayTotalGamesWon);
                                }
                            }
                            catch (Exception ex)
                            {
                                _LogException(ex.Message, "CrawlWTALive/Zero");
                            }
                        }
                    }
                    #endregion

                    MatchIdentity matchIdentity = new MatchIdentity(match.MatchId, match.ContestGroupId);
                    saveDataWTA(tennisMatchInfo, matchIdentity);
                }
                catch (Exception ex)
                {
                    _LogException(ex.Message, "CrawlWTALive/liveMatches");
                }
            }
        }

        public void ScrapWTAStats(object state)
        {
            #region ServiceGamesWon And Avg.Aces Per Match
            try
            {
                int servicePageCount = 0;
                do
                {
                    HtmlDocument document = new HtmlDocument();
                    string url = string.Empty;
                    url = string.Concat("http://www.wtatennis.com/stats?page=", servicePageCount);
                    using (TennisWebClient _webClient = new TennisWebClient())
                    {
                        var page = _webClient.DownloadString(url);
                        document.LoadHtml(page);
                    }

                    var serveDiv = document.DocumentNode.SelectNodes(".//div[@class='view-content']").FirstOrDefault();
                    var tableRows = serveDiv.SelectNodes(".//tbody/tr").ToList().Count;

                    try
                    {
                        for (int i = 1; i <= tableRows; i++)
                        {
                            #region GettingPlayerId            
                            var playerAnchor = serveDiv.SelectNodes(".//tbody/tr[" + i + "]/td[2]/div").FirstOrDefault();
                            var anchor = playerAnchor.Descendants("a").ToList()[0];
                            var href = anchor.GetAttributeValue("href", null);
                            var playerId = href.Split('/')[3];
                            #endregion

                            #region Service Games Won
                            var serviceGamesWonHtml = serveDiv.SelectNodes(".//tbody/tr[" + i + "]/td[12]/span/div/div/div").FirstOrDefault();
                            var serviceGamesWon = string.Concat(serviceGamesWonHtml.InnerHtml, "%");
                            #endregion

                            #region Avg.Games Won
                            var totalMatches = serveDiv.SelectNodes(".//tbody/tr[" + i + "]/td[4]/div/div/div").FirstOrDefault().InnerHtml;
                            var aces = serveDiv.SelectNodes(".//tbody/tr[" + i + "]/td[5]/div/div/div").FirstOrDefault().InnerHtml;
                            decimal averageAces = Math.Round(Convert.ToDecimal(Convert.ToDecimal(aces) / Convert.ToDecimal(totalMatches)), 2);
                            #endregion

                            Players player = _GetPlayer(Convert.ToInt32(playerId));

                            #region Service Games Won (DB Operation)            
                            if (player != null)
                            {
                                TennisStats alreadyExistedTennisStats = _GetTennisStats(player.PlayerId, 1);
                                if (alreadyExistedTennisStats != null)
                                {
                                    alreadyExistedTennisStats.Points = serviceGamesWon;
                                    _UpdateTennisStats(alreadyExistedTennisStats);
                                }
                                else
                                {
                                    TennisStats serviceGamesWonStats = new TennisStats()
                                    {
                                        PlayerId = player.PlayerId,
                                        Points = serviceGamesWon,
                                        SeasonId = 54,
                                        StatsTypeId = 1
                                    };
                                    _InsertTennisStats(serviceGamesWonStats);
                                }
                            }
                            #endregion

                            #region Avg.Games Won (DB Operation)
                            if (player != null)
                            {
                                TennisStats alreadyExistedTennisStats = _GetTennisStats(player.PlayerId, 3);
                                if (alreadyExistedTennisStats != null)
                                {
                                    alreadyExistedTennisStats.Points = Convert.ToString(averageAces);
                                    _UpdateTennisStats(alreadyExistedTennisStats);
                                }
                                else
                                {
                                    TennisStats serviceGamesWonStats = new TennisStats()
                                    {
                                        PlayerId = player.PlayerId,
                                        Points = Convert.ToString(averageAces),
                                        SeasonId = 54,
                                        StatsTypeId = 3
                                    };
                                    _InsertTennisStats(serviceGamesWonStats);
                                }
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        _LogException(ex.Message, "WTA/ServiceGamesWon And Avg.Aces Per Match / ForLoop");
                    }

                    servicePageCount++;
                } while (servicePageCount != 12);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "WTA/ServiceGamesWon And Avg.Aces Per Match");
            }
            #endregion

            #region Return Games Won
            try
            {
                int returnPageCount = 0;
                do
                {
                    HtmlDocument document = new HtmlDocument();
                    string url = string.Concat("http://www.wtatennis.com/stats?page=", returnPageCount);
                    using (TennisWebClient _webClient = new TennisWebClient())
                    {
                        var page = _webClient.DownloadString(url);
                        document.LoadHtml(page);
                    }

                    var returnDiv = document.DocumentNode.SelectNodes("/html/body/div[1]/div/div[2]/div[3]/div[2]/div/div/div[2]/div[2]").FirstOrDefault();
                    var viewContent = returnDiv.SelectNodes("/html/body/div[1]/div/div[2]/div[3]/div[2]/div/div/div[2]/div[2]/div/div[2]").FirstOrDefault();
                    var tableRows = viewContent.SelectNodes(".//tbody/tr").ToList().Count;

                    for (int i = 1; i < tableRows; i++)
                    {
                        #region GettingPlayerId            
                        var playerAnchor = viewContent.SelectNodes(".//tbody/tr[" + i + "]/td[2]/div").FirstOrDefault();
                        var anchor = playerAnchor.Descendants("a").ToList()[0];
                        var href = anchor.GetAttributeValue("href", null);
                        var playerId = href.Split('/')[3];
                        #endregion

                        #region Return Games Won
                        var returnGamesWonHtml = viewContent.SelectNodes(".//tbody/tr[" + i + "]/td[9]/span/div/div/div").FirstOrDefault();
                        var returnGamesWon = string.Concat(returnGamesWonHtml.InnerHtml, "%");
                        #endregion

                        #region Return Games Won (DB Operation)
                        Players player = _GetPlayer(Convert.ToInt32(playerId));
                        if (player != null)
                        {
                            TennisStats alreadyExistedTennisStats = _GetTennisStats(player.PlayerId, 2);
                            if (alreadyExistedTennisStats != null)
                            {
                                alreadyExistedTennisStats.Points = returnGamesWon;
                                _UpdateTennisStats(alreadyExistedTennisStats);
                            }
                            else
                            {
                                TennisStats serviceGamesWonStats = new TennisStats()
                                {
                                    PlayerId = player.PlayerId,
                                    Points = returnGamesWon,
                                    SeasonId = 54,
                                    StatsTypeId = 2
                                };
                                _InsertTennisStats(serviceGamesWonStats);
                            }
                        }
                        #endregion
                    }

                    returnPageCount++;
                } while (returnPageCount != 12);
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "WTA/Return Games Won");
            }
            #endregion
        }
        #endregion

        #region Private Methods
        #region Database
        private void _InsertTennisStats(TennisStats tennisStats)
        {
            const string _sqlInsertQuery = "INSERT INTO TennisStats (PlayerId, StatsTypeId, Points, SeasonId) VALUES (@PlayerId, @StatsTypeId, @Points, @SeasonId)";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                int result = _connection.Execute(_sqlInsertQuery, new
                {
                    PlayerId = tennisStats.PlayerId,
                    StatsTypeId = tennisStats.StatsTypeId,
                    Points = tennisStats.Points.Trim(),
                    SeasonId = 54
                });
            }
        }

        private Players _GetPlayer(int playerId)
        {
            Players player = null;
            const string _sqlSelectQuery = @"SELECT * FROM Players WHERE SharkScoresId = @PlayerId AND SportId = @SportId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                player = _connection.Query<Players>(_sqlSelectQuery, new { PlayerId = playerId, SportId = 2 }).FirstOrDefault();
            }
            return player;
        }

        private void _UpdatePlayerOverviewYTD(DatabaseModels.PlayerOverview playerOverview)
        {
            string _sqlQuery = @"UPDATE PlayerOverview SET YTDServiceGamesWon = @YTDServiceGamesWon, YTDReturnGamesWon = @YTDReturnGamesWon WHERE PlayerId = @PlayerId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                int result = _connection.Execute(_sqlQuery,
                    new
                    {
                        YTDServiceGamesWon = playerOverview.YTDServiceGamesWon,
                        YTDReturnGamesWon = playerOverview.YTDReturnGamesWon,
                        PlayerId = playerOverview.PlayerId
                    });
            }
        }

        private RankingPlayersProfiles _GetPlayerProfileInformationForTennis(string playerId)
        {
            RankingPlayersProfiles rankingPlayersProfiles = null;
            const string _sqlSelectQuery = @"SELECT * FROM RankingPlayersProfiles WHERE AtpId = @PlayerId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                rankingPlayersProfiles = _connection.Query<RankingPlayersProfiles>(_sqlSelectQuery, new { PlayerId = playerId }).FirstOrDefault();
            }
            return rankingPlayersProfiles;
        }

        private TennisMatchInfo _GetTennisMatchInfo(decimal matchId, int contestGroupId)
        {
            TennisMatchInfo tennisMatchInfo = null;
            const string _sqlSelectQuery = @"SELECT * FROM TennisMatchInfo WHERE ContestGroupId = @ContestGroupId AND MatchId = @MatchId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                tennisMatchInfo = _connection.Query<TennisMatchInfo>(_sqlSelectQuery, new { ContestGroupId = contestGroupId, MatchId = matchId }).FirstOrDefault();
            }
            return tennisMatchInfo;
        }

        private List<ATPWorldTourStats> _GetATPWorldTourContestGroupsAndMatches()
        {
            List<ATPWorldTourStats> aTPWorldTourStatList = null;
            string _sqlQuery = @"GetTennisLiveContestGroupsATPWorldTour";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                aTPWorldTourStatList = _connection.Query<ATPWorldTourStats>(_sqlQuery, commandType: CommandType.StoredProcedure).ToList();
            }
            return aTPWorldTourStatList;
        }

        private void _InsertTennisMatchInfo(TennisMatchInfo tennisMatchInfo)
        {
            string _sqlQuery = @"INSERT INTO TennisMatchInfo (MatchId,ContestGroupId,BreakPointsConvertedDividendTeam1,BreakPointsConvertedDivisorTeam1,BreakPointsConvertedPercentTeam1,
                                 ServicePointsWonPercentTeam1,ServicePointsWonDividendTeam1,ServicePointsWonDivisorTeam1,TotalPointsWonPercentTeam1,TotalPointsWonDividendTeam1,
                                 TotalPointsWonDivisorTeam1,BreakPointsConvertedDividendTeam2,BreakPointsConvertedDivisorTeam2,BreakPointsConvertedPercentTeam2,ServicePointsWonPercentTeam2,
                                 ServicePointsWonDividendTeam2,ServicePointsWonDivisorTeam2,TotalPointsWonPercentTeam2,TotalPointsWonDividendTeam2,TotalPointsWonDivisorTeam2,
                                 TotalGamesWonPercentTeam1, TotalGamesWonDividendTeam1, TotalGamesWonPercentTeam2, TotalGamesWonDividendTeam2, FirstServePointsWonPercentTeam1,
                                 FirstServePointsWonDividendTeam1,FirstServePointsWonDividerTeam1, FirstServePointsWonPercentTeam2, FirstServePointsWonDividendTeam2, FirstServePointsWonDividerTeam2) 
                                VALUES (@MatchId, @ContestGroupId,@BreakPointsConvertedDividendTeam1,@BreakPointsConvertedDivisorTeam1,@BreakPointsConvertedPercentTeam1,
                                 @ServicePointsWonPercentTeam1,@ServicePointsWonDividendTeam1,@ServicePointsWonDivisorTeam1,@TotalPointsWonPercentTeam1,@TotalPointsWonDividendTeam1,
                                 @TotalPointsWonDivisorTeam1,@BreakPointsConvertedDividendTeam2,@BreakPointsConvertedDivisorTeam2,@BreakPointsConvertedPercentTeam2,@ServicePointsWonPercentTeam2,
                                 @ServicePointsWonDividendTeam2,@ServicePointsWonDivisorTeam2,@TotalPointsWonPercentTeam2,@TotalPointsWonDividendTeam2,@TotalPointsWonDivisorTeam2,
                                 @TotalGamesWonPercentTeam1,@TotalGamesWonDividendTeam1,@TotalGamesWonPercentTeam2,@TotalGamesWonDividendTeam2,@FirstServePointsWonPercentTeam1,
                                 @FirstServePointsWonDividendTeam1,@FirstServePointsWonDividerTeam1,@FirstServePointsWonPercentTeam2,@FirstServePointsWonDividendTeam2,@FirstServePointsWonDividerTeam2)";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                int result = _connection.Execute(_sqlQuery,
                    new
                    {
                        MatchId = tennisMatchInfo.MatchId,
                        ContestGroupId = tennisMatchInfo.ContestGroupId,
                        BreakPointsConvertedDividendTeam1 = tennisMatchInfo.BreakPointsConvertedDividendTeam1,
                        BreakPointsConvertedDivisorTeam1 = tennisMatchInfo.BreakPointsConvertedDivisorTeam1,
                        BreakPointsConvertedPercentTeam1 = tennisMatchInfo.BreakPointsConvertedPercentTeam1,
                        ServicePointsWonPercentTeam1 = tennisMatchInfo.ServicePointsWonPercentTeam1,
                        ServicePointsWonDividendTeam1 = tennisMatchInfo.ServicePointsWonDividendTeam1,
                        ServicePointsWonDivisorTeam1 = tennisMatchInfo.ServicePointsWonDivisorTeam1,
                        TotalPointsWonPercentTeam1 = tennisMatchInfo.TotalPointsWonPercentTeam1,
                        TotalPointsWonDividendTeam1 = tennisMatchInfo.TotalPointsWonDividendTeam1,
                        TotalPointsWonDivisorTeam1 = tennisMatchInfo.TotalPointsWonDivisorTeam1,
                        BreakPointsConvertedDividendTeam2 = tennisMatchInfo.BreakPointsConvertedDividendTeam2,
                        BreakPointsConvertedDivisorTeam2 = tennisMatchInfo.BreakPointsConvertedDivisorTeam2,
                        BreakPointsConvertedPercentTeam2 = tennisMatchInfo.BreakPointsConvertedPercentTeam2,
                        ServicePointsWonPercentTeam2 = tennisMatchInfo.ServicePointsWonPercentTeam2,
                        ServicePointsWonDividendTeam2 = tennisMatchInfo.ServicePointsWonDividendTeam2,
                        ServicePointsWonDivisorTeam2 = tennisMatchInfo.ServicePointsWonDivisorTeam2,
                        TotalPointsWonPercentTeam2 = tennisMatchInfo.TotalPointsWonPercentTeam2,
                        TotalPointsWonDividendTeam2 = tennisMatchInfo.TotalPointsWonDividendTeam2,
                        TotalPointsWonDivisorTeam2 = tennisMatchInfo.TotalPointsWonDivisorTeam2,
                        TotalGamesWonPercentTeam1 = tennisMatchInfo.TotalGamesWonPercentTeam1,
                        TotalGamesWonDividendTeam1 = tennisMatchInfo.TotalGamesWonDividendTeam1,
                        TotalGamesWonPercentTeam2 = tennisMatchInfo.TotalGamesWonPercentTeam2,
                        TotalGamesWonDividendTeam2 = tennisMatchInfo.TotalGamesWonDividendTeam2,
                        FirstServePointsWonPercentTeam1 = tennisMatchInfo.FirstServePointsWonPercentTeam1,
                        FirstServePointsWonDividendTeam1 = tennisMatchInfo.FirstServePointsWonDividendTeam1,
                        FirstServePointsWonDividerTeam1 = tennisMatchInfo.FirstServePointsWonDividerTeam1,
                        FirstServePointsWonPercentTeam2 = tennisMatchInfo.FirstServePointsWonPercentTeam2,
                        FirstServePointsWonDividendTeam2 = tennisMatchInfo.FirstServePointsWonDividendTeam2,
                        FirstServePointsWonDividerTeam2 = tennisMatchInfo.FirstServePointsWonDividerTeam2
                    });
            }
        }

        private void _UpdateTennisMatchInfo(TennisMatchInfo tennisMatchInfo)
        {
            const string _sqlQuery = @"UPDATE TennisMatchInfo SET 
                                        BreakPointsConvertedDividendTeam1 = @BreakPointsConvertedDividendTeam1,
                                        BreakPointsConvertedDivisorTeam1  = @BreakPointsConvertedDivisorTeam1,
                                        BreakPointsConvertedPercentTeam1  = @BreakPointsConvertedPercentTeam1,
                                        ServicePointsWonPercentTeam1      = @ServicePointsWonPercentTeam1,
                                        ServicePointsWonDividendTeam1     = @ServicePointsWonDividendTeam1,
                                        ServicePointsWonDivisorTeam1      = @ServicePointsWonDivisorTeam1,
                                        TotalPointsWonPercentTeam1        = @TotalPointsWonPercentTeam1,
                                        TotalPointsWonDividendTeam1       = @TotalPointsWonDividendTeam1,
                                        TotalPointsWonDivisorTeam1        = @TotalPointsWonDivisorTeam1,
                                        BreakPointsConvertedDividendTeam2 = @BreakPointsConvertedDividendTeam2,
                                        BreakPointsConvertedDivisorTeam2  = @BreakPointsConvertedDivisorTeam2,
                                        BreakPointsConvertedPercentTeam2  = @BreakPointsConvertedPercentTeam2,
                                        ServicePointsWonPercentTeam2      = @ServicePointsWonPercentTeam2,
                                        ServicePointsWonDividendTeam2     = @ServicePointsWonDividendTeam2,
                                        ServicePointsWonDivisorTeam2      = @ServicePointsWonDivisorTeam2,
                                        TotalPointsWonPercentTeam2        = @TotalPointsWonPercentTeam2,
                                        TotalPointsWonDividendTeam2       = @TotalPointsWonDividendTeam2,
                                        TotalPointsWonDivisorTeam2        = @TotalPointsWonDivisorTeam2,
                                        TotalGamesWonPercentTeam1         = @TotalGamesWonPercentTeam1,
                                        TotalGamesWonDividendTeam1        = @TotalGamesWonDividendTeam1,
                                        TotalGamesWonPercentTeam2         = @TotalGamesWonPercentTeam2,
                                        TotalGamesWonDividendTeam2        = @TotalGamesWonDividendTeam2,
                                        FirstServePointsWonPercentTeam1   = @FirstServePointsWonPercentTeam1,
                                        FirstServePointsWonDividendTeam1  = @FirstServePointsWonDividendTeam1,
                                        FirstServePointsWonDividerTeam1   = @FirstServePointsWonDividerTeam1,
                                        FirstServePointsWonPercentTeam2   = @FirstServePointsWonPercentTeam2,
                                        FirstServePointsWonDividendTeam2  = @FirstServePointsWonDividendTeam2,
                                        FirstServePointsWonDividerTeam2   = @FirstServePointsWonDividerTeam2
                                        WHERE MatchId = @MatchId AND ContestGroupId = @ContestGroupId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                int result = _connection.Execute(_sqlQuery,
                    new
                    {
                        MatchId = tennisMatchInfo.MatchId,
                        ContestGroupId = tennisMatchInfo.ContestGroupId,
                        BreakPointsConvertedDividendTeam1 = tennisMatchInfo.BreakPointsConvertedDividendTeam1,
                        BreakPointsConvertedDivisorTeam1 = tennisMatchInfo.BreakPointsConvertedDivisorTeam1,
                        BreakPointsConvertedPercentTeam1 = tennisMatchInfo.BreakPointsConvertedPercentTeam1,
                        ServicePointsWonPercentTeam1 = tennisMatchInfo.ServicePointsWonPercentTeam1,
                        ServicePointsWonDividendTeam1 = tennisMatchInfo.ServicePointsWonDividendTeam1,
                        ServicePointsWonDivisorTeam1 = tennisMatchInfo.ServicePointsWonDivisorTeam1,
                        TotalPointsWonPercentTeam1 = tennisMatchInfo.TotalPointsWonPercentTeam1,
                        TotalPointsWonDividendTeam1 = tennisMatchInfo.TotalPointsWonDividendTeam1,
                        TotalPointsWonDivisorTeam1 = tennisMatchInfo.TotalPointsWonDivisorTeam1,
                        BreakPointsConvertedDividendTeam2 = tennisMatchInfo.BreakPointsConvertedDividendTeam2,
                        BreakPointsConvertedDivisorTeam2 = tennisMatchInfo.BreakPointsConvertedDivisorTeam2,
                        BreakPointsConvertedPercentTeam2 = tennisMatchInfo.BreakPointsConvertedPercentTeam2,
                        ServicePointsWonPercentTeam2 = tennisMatchInfo.ServicePointsWonPercentTeam2,
                        ServicePointsWonDividendTeam2 = tennisMatchInfo.ServicePointsWonDividendTeam2,
                        ServicePointsWonDivisorTeam2 = tennisMatchInfo.ServicePointsWonDivisorTeam2,
                        TotalPointsWonPercentTeam2 = tennisMatchInfo.TotalPointsWonPercentTeam2,
                        TotalPointsWonDividendTeam2 = tennisMatchInfo.TotalPointsWonDividendTeam2,
                        TotalPointsWonDivisorTeam2 = tennisMatchInfo.TotalPointsWonDivisorTeam2,
                        TotalGamesWonPercentTeam1 = tennisMatchInfo.TotalGamesWonPercentTeam1,
                        TotalGamesWonDividendTeam1 = tennisMatchInfo.TotalGamesWonDividendTeam1,
                        TotalGamesWonPercentTeam2 = tennisMatchInfo.TotalGamesWonPercentTeam2,
                        TotalGamesWonDividendTeam2 = tennisMatchInfo.TotalGamesWonDividendTeam2,
                        FirstServePointsWonPercentTeam1 = tennisMatchInfo.FirstServePointsWonPercentTeam1,
                        FirstServePointsWonDividendTeam1 = tennisMatchInfo.FirstServePointsWonDividendTeam1,
                        FirstServePointsWonDividerTeam1 = tennisMatchInfo.FirstServePointsWonDividerTeam1,
                        FirstServePointsWonPercentTeam2 = tennisMatchInfo.FirstServePointsWonPercentTeam2,
                        FirstServePointsWonDividendTeam2 = tennisMatchInfo.FirstServePointsWonDividendTeam2,
                        FirstServePointsWonDividerTeam2 = tennisMatchInfo.FirstServePointsWonDividerTeam2
                    });
            }
        }

        private TennisStats _GetTennisStats(int playerId, int statsTypeId)
        {
            TennisStats tennisStats = null;
            const string _sqlQuery = @"SELECT * FROM TennisStats WHERE PlayerId = @PlayerId AND StatsTypeId = @StatsTypeId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                tennisStats = _connection.Query<TennisStats>(_sqlQuery, new { PlayerId = playerId, StatsTypeId = statsTypeId }).FirstOrDefault();
            }
            return tennisStats;
        }

        private void _UpdateTennisStats(TennisStats tennisStats)
        {
            const string _sqlQuery = @"Update TennisStats SET Points = @Points WHERE PlayerId = @PlayerId AND StatsTypeId = @StatsTypeId";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                int result = _connection.Execute(_sqlQuery, new
                {
                    PlayerId = tennisStats.PlayerId,
                    StatsTypeId = tennisStats.StatsTypeId,
                    Points = tennisStats.Points.Trim()
                });
            }
        }

        private void _InsertData(List<Rows> rows, int statsTypeId)
        {
            int[] PlayerIds = rows.Select(x => x.playerId).ToArray();
            const string _sqlSelectQuery = @"SELECT * FROM Players WHERE SharkScoresId IN @PlayerIds";
            const string _sqlInsertQuery = "INSERT INTO TennisStats (PlayerId, StatsTypeId, Points, SeasonId) VALUES (@PlayerId, @StatsTypeId, @Points, @SeasonId)";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                List<Players> players = _connection.Query<Players>(_sqlSelectQuery, new { PlayerIds = PlayerIds }).ToList();
                foreach (Players _player in players)
                {
                    TennisStats tennisStats = new TennisStats()
                    {
                        PlayerId = players.FirstOrDefault(x => x.SharkScoresId == _player.SharkScoresId).PlayerId,
                        Points = rows.FirstOrDefault(x => x.playerId == _player.SharkScoresId).value,
                        SeasonId = 54,
                        StatsTypeId = statsTypeId
                    };
                    TennisStats existedtennisStats = _GetTennisStats(tennisStats.PlayerId, tennisStats.StatsTypeId);
                    if (existedtennisStats != null)
                    {
                        //Update stats.
                        _UpdateTennisStats(tennisStats);
                    }
                    else
                    {
                        //Insert stats.
                        int result = _connection.Execute(_sqlInsertQuery, new
                        {
                            PlayerId = tennisStats.PlayerId,
                            StatsTypeId = statsTypeId,
                            Points = tennisStats.Points.Trim(),
                            SeasonId = 54
                        });
                    }
                }
            }
        }

        private void _InsertSingleRankingData(List<Rows> rows, int statsTypeId)
        {
            int[] PlayerIds = rows.Select(x => x.playerId).ToArray();
            const string _sqlSelectQuery = @"SELECT * FROM Players WHERE SharkScoresId IN @PlayerIds";
            const string _sqlInsertQuery = "INSERT INTO TennisStats (PlayerId, StatsTypeId, Points, SeasonId) VALUES (@PlayerId, @StatsTypeId, @Points, @SeasonId)";
            using (SqlConnection _connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                List<Players> players = _connection.Query<Players>(_sqlSelectQuery, new { PlayerIds = PlayerIds }).ToList();
                foreach (Players _player in players)
                {
                    TennisStats tennisStats = new TennisStats()
                    {
                        PlayerId = players.FirstOrDefault(x => x.SharkScoresId == _player.SharkScoresId).PlayerId,
                        Points = rows.FirstOrDefault(x => x.playerId == _player.SharkScoresId).points.ToString(),
                        SeasonId = 54,
                        StatsTypeId = statsTypeId
                    };
                    TennisStats existedTennisStats = _GetTennisStats(tennisStats.PlayerId, tennisStats.StatsTypeId);
                    if (existedTennisStats != null)
                    {
                        //Update stats.
                        _UpdateTennisStats(tennisStats);
                    }
                    else
                    {
                        //Insert stats.
                        int result = _connection.Execute(_sqlInsertQuery, new
                        {
                            PlayerId = tennisStats.PlayerId,
                            StatsTypeId = statsTypeId,
                            Points = tennisStats.Points.Trim(),
                            SeasonId = 54
                        });
                    }
                }
            }
        }

        private void _updateDataWTA(TennisMatchInfo matchInfo, int matchId)
        {
            string updateCmd = "UPDATE TennisMatchInfo " +
                      "SET FirstServePointsWonPercentTeam1 ='" + matchInfo.FirstServePointsWonPercentTeam1 + "'," +
                      "FirstServePointsWonPercentTeam2 = '" + matchInfo.FirstServePointsWonPercentTeam2 + "'," +
                      "BreakPointsConvertedPercentTeam1 = '" + matchInfo.BreakPointsConvertedPercentTeam1 + "'," +
                      "BreakPointsConvertedPercentTeam2= '" + matchInfo.BreakPointsConvertedPercentTeam2 + "'," +
                      "ServicePointsWonPercentTeam1= '" + matchInfo.ServicePointsWonPercentTeam1 + "'," +
                      "ServicePointsWonPercentTeam2='" + matchInfo.ServicePointsWonPercentTeam2 + "'," +
                      "TotalGamesWonPercentTeam1='" + matchInfo.TotalGamesWonPercentTeam1 + "'," +
                      "TotalGamesWonPercentTeam2='" + matchInfo.TotalGamesWonPercentTeam2 + "'," +
                      "TotalGamesWonDividendTeam1='" + matchInfo.TotalGamesWonDividendTeam1 + "'," +
                      "TotalGamesWonDividendTeam2= '" + matchInfo.TotalGamesWonDividendTeam2 + "'," +
                      "TotalPointsWonPercentTeam1= '" + matchInfo.TotalPointsWonPercentTeam1 + "'," +
                      "TotalPointsWonPercentTeam2='" + matchInfo.TotalPointsWonPercentTeam2 + "'" +
                      "WHERE MatchId=" + matchId;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(updateCmd, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void _insertDataWTA(TennisMatchInfo matchInfo, int matchId, int contestId)
        {
            string insertCmd = "INSERT INTO TennisMatchInfo (MatchId,ContestGroupId,FirstServePointsWonPercentTeam1,FirstServePointsWonPercentTeam2," +
                " BreakPointsConvertedPercentTeam1,BreakPointsConvertedPercentTeam2,ServicePointsWonPercentTeam1,ServicePointsWonPercentTeam2," +
                "TotalGamesWonPercentTeam1,TotalGamesWonPercentTeam2,TotalGamesWonDividendTeam1,TotalGamesWonDividendTeam2," +
                "TotalPointsWonPercentTeam1,TotalPointsWonPercentTeam2) " +
                "Values (" + matchId + ", " + contestId + ", '" + matchInfo.FirstServePointsWonPercentTeam1 + "','" + matchInfo.FirstServePointsWonPercentTeam2 + "'," +
                "'" + matchInfo.BreakPointsConvertedPercentTeam1 + "','" + matchInfo.BreakPointsConvertedPercentTeam2 + "','" + matchInfo.ServicePointsWonPercentTeam1 + "'," +
                "'" + matchInfo.ServicePointsWonPercentTeam2 + "','" + matchInfo.TotalGamesWonPercentTeam1 + "','" + matchInfo.TotalGamesWonPercentTeam2 + "'," +
                "'" + matchInfo.TotalGamesWonDividendTeam1 + "','" + matchInfo.TotalGamesWonDividendTeam2 + "','" + matchInfo.TotalPointsWonPercentTeam1 + "','" + matchInfo.TotalPointsWonPercentTeam2 + "')";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(insertCmd, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private List<LiveMatch> GetLiveMatchesWTA(int sportOrgId)
        {
            List<LiveMatch> liveMatches = new List<LiveMatch>();
            using (DataTable _dataTable = new DataTable())
            {
                using (SqlConnection _connection = new SqlConnection(_connectionString))
                {
                    _connection.Open();
                    using (SqlCommand _command = new SqlCommand("GetTennisLiveContestsForWTA", _connection))
                    {
                        _command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter sqlDataApdater = new SqlDataAdapter(_command);
                        sqlDataApdater.Fill(_dataTable);
                    }
                }

                foreach (DataRow _dataRow in _dataTable.Rows)
                {
                    LiveMatch match = new LiveMatch()
                    {
                        ContestGroupId = Convert.ToInt32(_dataRow["ContestGroupId"]),
                        MatchId = Convert.ToInt32(_dataRow["MatchId"]),
                        CrawlerLink = Convert.ToString(_dataRow["CrawlerLink"]),
                        FixtureLink3 = Convert.ToString(_dataRow["FixtureLink3"]),
                    };
                    liveMatches.Add(match);
                }
            }
            return liveMatches;
        }

        private bool saveDataWTA(TennisMatchInfo matchInfo, MatchIdentity matchIdentity)
        {
            bool status = false;
            int matchId = matchIdentity.MatchId, contestGroupId = matchIdentity.ContestGroupId;
            try
            {
                SqlConnection con = new SqlConnection(_connectionString);
                string selectInfoCmd = "SELECT TOP(1) MatchId FROM TennisMatchInfo where MatchId=" + matchId;
                con.Open();
                using (SqlCommand cmd = new SqlCommand(selectInfoCmd, con))
                {
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        con.Close();
                        _updateDataWTA(matchInfo, matchId);
                    }
                    else
                    {
                        con.Close();
                        _insertDataWTA(matchInfo, matchId, contestGroupId);
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                throw ex;
            }
            return status;
        }
        #endregion

        #region Others
        private XElement SofaScoreEvent<T>(T o)
        {
            //string filePath = @"C:\XMLFileOCSIXLOGICSCOM\betfair\pre-match\" + filename;

            XmlDocument xmlDoc = new XmlDocument();
            XPathNavigator nav = xmlDoc.CreateNavigator();
            using (XmlWriter writer = nav.AppendChild())
            {
                XmlSerializer ser = new XmlSerializer(typeof(T), new XmlRootAttribute("events"));
                ser.Serialize(writer, o); // error
            }
            return XElement.Parse(xmlDoc.InnerXml);
        }

        public void _GetMenDataForPlayerRaceToLondonOverview()
        {
            string type = "RaceToLondon";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
                db.Database.Connection.Close();
                db.Database.Connection.Dispose();
            }

            foreach (var item in list)
            {
                _GetMenPlayerOverview(item.PlayerLink, item.Id, type, "_GetMenDataForPlayerRaceToLondonOverview");
            }
        }

        private void _GetMenDataForPlayerGrandSlamRaceToLondonResult()
        {
            string type = "RaceToLondon";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                for (int i = 0; i <= 7; i++)
                {
                    _GetMenPlayerGrandSlamSingeResult(item.PlayerLink, item.Id, "201" + i, type);
                }
            }
        }

        private int _CalculateGamesWonPercent(int totalGames, int wonGames)
        {
            int result = 0;
            // Check if there is not a single game played then exclude it else calculate the percentage.
            if (totalGames != 0)
                result = Convert.ToInt32((wonGames * 100) / totalGames);
            return result;
        }

        private void _GetMenPlayerImage(string playerUrl, int playerId)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                string url = @"http://www.atpworldtour.com" + playerUrl + "overview";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var heroImage = document.DocumentNode.SelectNodes(".//div[@class='player-profile-hero-image']/img");
                if (heroImage != null)
                {
                    string imageUrl = heroImage.FirstOrDefault().Attributes["src"].Value;
                    imageUrl = "http://www.atpworldtour.com" + imageUrl;
                    _DownloadMenHeroImage(imageUrl, playerId);
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetMenPlayerImage");
            }
        }

        private void _DownloadMenHeroImage(string ImageUrl, int playerId)
        {
            try
            {
                Image img = null;
                using (TennisWebClient wc = new TennisWebClient())
                {
                    byte[] data = wc.DownloadData(ImageUrl);
                    MemoryStream ms = new MemoryStream(data);
                    img = Image.FromStream(ms);
                }

                img.Save(@"C:\inetpub\wwwroot\languages.feedsxml.com\players\" + playerId + "-1.png", System.Drawing.Imaging.ImageFormat.Png);
                _SaveNewsImageFromURL(@"C:\inetpub\wwwroot\languages.feedsxml.com\players\" + playerId + "-1.png", playerId.ToString());
                File.Delete(@"C:\inetpub\wwwroot\languages.feedsxml.com\players\" + playerId + "-1.png");
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "DownloadMenHeroImage");
            }
        }

        private void _SaveNewsImageFromURL(string url, string fileName)
        {
            try
            {
                using (Image sourceImage = Image.FromFile(url))
                {
                    _FixedSize(sourceImage, 260, 260).Save(@"C:\inetpub\wwwroot\languages.feedsxml.com\players\" + fileName + ".png", ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private Image _FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private void _GetMenPlayerProfile(string playerUrl, int playerId)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();

                string url = @"http://www.atpworldtour.com" + playerUrl + "overview";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var playerProfileDiv = document.DocumentNode.SelectNodes(".//div[@class='player-profile-hero-table']");
                var row = playerProfileDiv.FirstOrDefault().SelectNodes(".//tr");

                //first row 

                var td_of_row_1 = row[0].SelectNodes(".//td");

                var td1_of_td_of_row_1 = td_of_row_1[0].SelectNodes(".//div[@class='table-big-value']").FirstOrDefault();

                string age = td1_of_td_of_row_1.InnerText;
                age = age.Split('(').First();
                string dateofbirth = td1_of_td_of_row_1.SelectNodes(".//span[@class='table-birthday']").FirstOrDefault().InnerText;
                dateofbirth = dateofbirth.Replace("(", "").Replace(")", "");
                var td2_of_td_of_row_1 = td_of_row_1[1].SelectNodes(".//div[@class='table-big-value']").FirstOrDefault();

                string turnedpro = td2_of_td_of_row_1.InnerText;

                var td3_of_td_of_row_1 = td_of_row_1[2].SelectNodes(".//span[@class='table-weight-lbs']").FirstOrDefault();

                string weight = td3_of_td_of_row_1.InnerText;

                var td4_of_td_of_row_1 = td_of_row_1[3].SelectNodes(".//span[@class='table-height-cm-wrapper']").FirstOrDefault();

                string height = td4_of_td_of_row_1.InnerText;

                // second row

                var td_of_row_2 = row[1].SelectNodes(".//td");

                var td1_of_td_of_row_2 = td_of_row_2[0].SelectNodes(".//div[@class='table-value']").FirstOrDefault();
                string birthplace = td1_of_td_of_row_2.InnerText;

                var td2_of_td_of_row_2 = td_of_row_2[1].SelectNodes(".//div[@class='table-value']").FirstOrDefault();
                string residence = td2_of_td_of_row_2.InnerText;

                var td3_of_td_of_row_2 = td_of_row_2[2].SelectNodes(".//div[@class='table-value']").FirstOrDefault();
                string plays = td3_of_td_of_row_2.InnerText;

                var td4_of_td_of_row_2 = td_of_row_2[3].SelectNodes(".//div[@class='table-value']").FirstOrDefault();
                string coach = td4_of_td_of_row_2.InnerText;

                using (var db = new SportsDataPanelEntities())
                {
                    var player = db.Players.FirstOrDefault(p => p.PlayerId == playerId);
                    if (player != null)
                    {
                        player.Age = age.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        player.DateOfBirth = Convert.ToDateTime(dateofbirth);
                        player.Weight = weight;
                        string convertedHeigth = height.Replace("(", string.Empty).Replace(")", string.Empty);
                        player.Height = convertedHeigth;
                        player.CountryOfBirth = birthplace.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        player.PlaceOfBirth = birthplace.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }

                    var rankingData = db.RankingPlayersProfiles.FirstOrDefault(r => r.PlayerId == playerId);
                    if (rankingData != null)
                    {
                        rankingData.TurnPro = turnedpro.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        rankingData.Residence = residence;
                        rankingData.CoachName = coach.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        rankingData.PlayerPosition = plays.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    }

                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    _LogException("Entity of type " + eve.Entry.Entity.GetType().Name + "in state " + eve.Entry.State + " has the following validation errors:", "DbEntityValidationException");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _LogException("- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage, "ForEach");
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetMenPlayerProfile");
            }
        }

        private void _GetMenDataForPlayerImageSingle()
        {
            string type = "Single";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                string playerUrl = item.PlayerLink;
                int playerId = item.PlayerId ?? 0;
                if (playerId != 0)
                {
                    _GetMenPlayerImage(playerUrl, playerId);
                }
            }
        }

        private void _GetMenDataForPlayerImageRaceToLondon()
        {
            string type = "RaceToLondon";
            List<RankingPlayersProfiles> list = new List<RankingPlayersProfiles>();
            using (var db = new SportsDataPanelEntities())
            {
                list = db.RankingPlayersProfiles.Where(s => s.GenderId == 3 && s.Type == type).OrderBy(r => r.Ranking).ToList();
            }

            foreach (var item in list)
            {
                string playerUrl = item.PlayerLink;
                int playerId = item.PlayerId ?? 0;
                if (playerId != 0)
                {
                    _GetMenPlayerImage(playerUrl, playerId);
                }
            }
        }

        private void _GetWomenPlayerImage(string playerUrl, int rankingId, int playerId, string firstName, string lastName)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                string url = @"http://www.wtatennis.com/players/player/" + playerUrl + "/title/" + firstName + "-" + lastName + "-0";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var profileImage = document.DocumentNode.SelectNodes(".//div[@class='field field--name-field-player-image field--type-image field--label-hidden']").FirstOrDefault();

                var img = profileImage.SelectNodes(".//img");
                if (img != null)
                {
                    string imageUrl = img.FirstOrDefault().Attributes["src"].Value;
                    _DownloadWomenHeroImage(imageUrl, playerId);
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetWomenPlayerImage");
            }
        }

        private void _DownloadWomenHeroImage(string ImageUrl, int playerId)
        {
            try
            {
                using (TennisWebClient client = new TennisWebClient())
                {
                    string path = @"C:\inetpub\wwwroot\languages.feedsxml.com\players\" + playerId + ".png";
                    client.DownloadFile(new Uri(ImageUrl), path);
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "DownloadWomenHeroImage");
            }
        }

        private void _GetWomenPlayerProfile(string playerUrl, int rankingId, int playerId, string firstName, string lastName)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                string url = @"http://www.wtatennis.com/players/player/" + playerUrl + "/title/" + firstName + "-" + lastName + "-0";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var playerProfileDiv = document.DocumentNode.SelectNodes(".//div[@class='group-info field-group-div']").FirstOrDefault();

                var ageDiv = playerProfileDiv.SelectNodes(".//div[@class='group-age field-group-div']");
                string age = string.Empty;
                if (ageDiv != null)
                {
                    var ageSubDiv = ageDiv.FirstOrDefault().SelectNodes(".//div[@class='field field--name-field-age field--type-number-integer field--label-hidden']").FirstOrDefault();
                    age = ageSubDiv.SelectNodes(".//div[@class='field__item even']").FirstOrDefault().InnerText;
                }

                string dob = string.Empty;
                var dobSubDiv = document.DocumentNode.SelectNodes(".//div[@class='field field--name-field-date-of-birth field--type-datetime field--label-hidden']");
                if (dobSubDiv != null)
                {
                    dob = dobSubDiv.FirstOrDefault().SelectNodes(".//span[@class='date-display-single']").FirstOrDefault().InnerText;
                }

                string height = string.Empty;
                var heightDiv = playerProfileDiv.SelectNodes(".//div[@class='field field--name-field-height field--type-text field--label-hidden']");
                if (heightDiv != null)
                {
                    height = heightDiv.FirstOrDefault().SelectNodes(".//div[@class='field__item even']").FirstOrDefault().InnerText;
                    height = height.Split('(').LastOrDefault().Replace(")", "");
                }

                var playsDiv = playerProfileDiv.SelectNodes(".//div[@class='field field--name-field-playhand field--type-text field--label-hidden']");
                string plays = string.Empty;
                if (playsDiv != null)
                {
                    plays = playsDiv.FirstOrDefault().SelectNodes(".//div[@class='field__item even']").FirstOrDefault().InnerText;
                }

                var pobDiv = playerProfileDiv.SelectNodes(".//div[@class='field field--name-field-birthcity field--type-text field--label-hidden']");
                string pob = string.Empty;
                if (pobDiv != null)
                {
                    pob = pobDiv.FirstOrDefault().SelectNodes(".//div[@class='field__item even']").FirstOrDefault().InnerText;
                }
                //Overview

                var table1 = document.DocumentNode.SelectNodes(".//table[@class='wta-table table-player-overview']").FirstOrDefault();
                var tr1 = table1.SelectNodes(".//tbody/tr");

                PlayerOverview overview = new PlayerOverview();
                var careerlast = tr1[0].SelectNodes(".//td[@class='career last']");
                if (careerlast != null)
                {
                    overview.CareerSingleTitle = Convert.ToInt32(careerlast.FirstOrDefault().InnerText);
                }

                var ytd1 = tr1[2].SelectNodes(".//td[@class='ytd']");
                string ytd = string.Empty;
                if (ytd1 != null)
                {
                    ytd = ytd1.FirstOrDefault().InnerText;
                    overview.YTDMatchWon = Convert.ToInt32(ytd.Split('/').FirstOrDefault());
                    overview.YTDMatchesLost = Convert.ToInt32(ytd.Split('/').LastOrDefault());
                }

                var careerlast2 = tr1[2].SelectNodes(".//td[@class='career last']");
                if (careerlast2 != null)
                {
                    var winLost = careerlast2.FirstOrDefault().InnerText;
                    overview.CareerMatchesWon = Convert.ToInt32(winLost.Split('/').FirstOrDefault());
                    overview.CareerMatchesLost = Convert.ToInt32(winLost.Split('/').LastOrDefault());
                }
                var wtatable = document.DocumentNode.SelectNodes(".//table[@class='wta-table table-player-overview']");
                if (wtatable != null)
                {
                    var table2 = wtatable.LastOrDefault();
                    var tr2 = table1.SelectNodes(".//tbody/tr");
                    overview.CareerDoublesTitle = Convert.ToInt32(tr2[0].SelectNodes(".//td[@class='career last']").FirstOrDefault().InnerText);
                }

                var yet = document.DocumentNode.SelectNodes(".//table[@id='year-end-table']/tbody/tr");

                if (yet != null)
                {
                    var table3 = yet.FirstOrDefault();
                    var td3 = table3.SelectNodes(".//td")[1].InnerText;
                    overview.HighRankSingle = Convert.ToInt32(td3);
                }

                //WTA Tournament ID

                var entryIdInput = document.DocumentNode.SelectNodes(".//input[@id='entity_id']");
                int entryId = 0;
                if (entryIdInput != null)
                {
                    entryId = Convert.ToInt32(entryIdInput.FirstOrDefault().Attributes["value"].Value);
                }

                overview.PlayerId = rankingId;

                using (var db = new SportsDataPanelEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var playerOverview = db.PlayerOverview.FirstOrDefault(po => po.PlayerId == rankingId);

                    if (playerOverview == null)
                        db.PlayerOverview.Add(overview);
                    else
                    {
                        playerOverview.CareerDoublesTitle = overview.CareerDoublesTitle;
                        playerOverview.CareerSingleTitle = overview.CareerSingleTitle;
                        playerOverview.CareerMatchesWon = overview.CareerMatchesWon;
                        playerOverview.CareerMatchesLost = overview.CareerMatchesLost;
                        playerOverview.HighRankSingle = overview.HighRankSingle;
                        playerOverview.WeekRankSingle = overview.WeekRankSingle;
                        playerOverview.YTDMatchesLost = overview.YTDMatchesLost;
                        playerOverview.YTDMatchWon = overview.YTDMatchWon;
                    }

                    var player = db.Players.FirstOrDefault(p => p.PlayerId == playerId);
                    if (player != null)
                    {
                        player.Age = age.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                        player.DateOfBirth = Convert.ToDateTime(dob);
                        player.Weight = string.Empty;
                        player.Height = height;
                        player.CountryOfBirth = (pob.Contains(',') ? pob.Split(',').FirstOrDefault() : pob);
                        player.PlaceOfBirth = (pob.Contains(',') ? pob.Split(',').LastOrDefault() : pob);
                    }

                    var rankingData = db.RankingPlayersProfiles.FirstOrDefault(r => r.PlayerId == playerId);
                    if (rankingData != null)
                    {
                        rankingData.TurnPro = string.Empty;
                        rankingData.Residence = string.Empty;
                        rankingData.CoachName = string.Empty;
                        rankingData.PlayerPosition = plays;
                        rankingData.WtaTId = entryId;
                    }

                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    _LogException("Entity of type " + eve.Entry.Entity.GetType().Name + "in state " + eve.Entry.State + " has the following validation errors:", "DbEntityValidationException");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        _LogException("- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage, "ForEach");
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetWomenPlayerProfile");
            }
        }

        private void _GetMenPlayerGrandSlamSingeResult(string playerUrl, int playerId, string year, string type)
        {
            try
            {
                GrandSlamInfo grandSlamInfo = new GrandSlamInfo();
                HtmlDocument document = new HtmlDocument();
                string url = @"http://www.atpworldtour.com" + playerUrl + "player-activity?year=" + year + "&matchType=singles";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var maindiv = document.DocumentNode.SelectNodes(".//div[@data-filtered-module='playerActivityTables']");
                var middlediv = maindiv.FirstOrDefault().SelectNodes(".//div[@class='activity-tournament-table']");
                foreach (var item in middlediv)
                {
                    var table1 = item.SelectNodes(".//table[@class='tourney-results-wrapper']").FirstOrDefault();
                    var anchor = table1.SelectNodes(".//td[@class='title-content']/a");

                    if (anchor != null)
                    {
                        string competitionName = anchor.FirstOrDefault().Attributes["data-ga-label"].Value;

                        if (competitionName != null && (competitionName == "US Open" || competitionName == "Wimbledon" || competitionName == "Roland Garros" || competitionName == "Australian Open"))
                        {
                            string round = item.SelectNodes(".//table[@class='mega-table']/tbody/tr").FirstOrDefault().SelectNodes(".//td").FirstOrDefault().InnerText;

                            if (competitionName == "US Open")
                            {
                                grandSlamInfo.UsOpen = round;
                            }
                            else if (competitionName == "Wimbledon")
                            {
                                grandSlamInfo.Wimbledon = round;
                            }
                            else if (competitionName == "Roland Garros")
                            {
                                grandSlamInfo.RolandGarros = round;
                            }
                            else if (competitionName == "Australian Open")
                            {
                                grandSlamInfo.AustralianOpen = round;
                            }
                        }
                    }
                }

                grandSlamInfo.Year = year;
                grandSlamInfo.PlayerId = playerId;

                using (var db = new SportsDataPanelEntities())
                {
                    var GrandSlamInfoes = db.GrandSlamInfo.Where(g => g.PlayerId == playerId && g.Year == year).FirstOrDefault();
                    if (GrandSlamInfoes != null)
                    {
                        GrandSlamInfoes.Wimbledon = grandSlamInfo.Wimbledon;
                        GrandSlamInfoes.UsOpen = grandSlamInfo.UsOpen;
                        GrandSlamInfoes.RolandGarros = grandSlamInfo.RolandGarros;
                        GrandSlamInfoes.AustralianOpen = grandSlamInfo.AustralianOpen;
                    }
                    else
                    {
                        db.GrandSlamInfo.Add(grandSlamInfo);
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "GetMenPlayerGrandSlamSingeResult");
            }
        }

        private void _GetMenPlayerOverview(string playerUrl, int playerId, string type, string callerName = null)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();

                string url = @"http://www.atpworldtour.com" + playerUrl + "fedex-atp-win-loss";
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    var page = _webClient.DownloadString(url);
                    document.LoadHtml(page);
                }

                var maindiv = document.DocumentNode.SelectNodes(".//div[@id='matchRecordTableContainer']");
                var tr = maindiv.FirstOrDefault().SelectNodes(".//table[@class='mega-table']")?.FirstOrDefault()?.SelectNodes(".//tbody/tr").FirstOrDefault();
                if (tr != null)
                {
                    var td = tr.SelectNodes(".//td");

                    string ytdw = td[2].InnerText;
                    string ytdl = td[4].InnerText;

                    string cw = td[7].InnerText;
                    string cl = td[9].InnerText;

                    string tt = td[11].InnerText;

                    PlayerOverview playerOverview = new PlayerOverview();
                    playerOverview.CareerMatchesWon = Convert.ToInt32(cw);
                    playerOverview.CareerMatchesLost = Convert.ToInt32(cl);

                    playerOverview.YTDMatchWon = Convert.ToInt32(ytdw);
                    playerOverview.YTDMatchesLost = Convert.ToInt32(ytdl);

                    playerOverview.CareerSingleTitle = Convert.ToInt32(tt);
                    playerOverview.PlayerId = playerId;

                    using (var db = new SportsDataPanelEntities())
                    {
                        var PlayerOverview = db.PlayerOverview.Where(p => p.PlayerId == playerId).FirstOrDefault();

                        if (PlayerOverview != null)
                        {
                            PlayerOverview.CareerMatchesWon = playerOverview.CareerMatchesWon;
                            PlayerOverview.CareerMatchesLost = playerOverview.CareerMatchesLost;
                            PlayerOverview.YTDMatchWon = playerOverview.YTDMatchWon;
                            PlayerOverview.YTDMatchesLost = playerOverview.YTDMatchesLost;
                            PlayerOverview.CareerSingleTitle = playerOverview.CareerSingleTitle;
                        }
                        else
                        {
                            db.PlayerOverview.Add(playerOverview);
                        }

                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _LogException(ex.Message, "_GetMenPlayerOverview / " + callerName);
            }
        }

        private void _LogException(string message, string methodName)
        {
            using (StreamWriter _streamWriter = File.AppendText(@"c:\TennisScrapper.txt"))
            {
                _streamWriter.WriteLine("Exception in method: " + methodName);
                _streamWriter.WriteLine("Exception occured at: " + DateTime.Now.ToString());
                _streamWriter.WriteLine("Exception message is: " + message);
                _streamWriter.WriteLine("================================================================================================");
            }
        }

        private string _ConvertDateTime()
        {
            string rankDate = string.Empty;
            DateTime now = DateTime.Now;
            var year = now.Year;
            string month = now.Month > 9 ? now.Month.ToString() : "0" + now.Month;
            var date = now.Day;
            rankDate = string.Concat(year, "-", month, "-", date);
            return rankDate;
        }

        private string _ExtractNumber(string original)
        {
            string value = string.Empty;
            try
            {
                value = new string(original.Where(c => Char.IsDigit(c)).ToArray());
            }
            catch (Exception)
            {
            }
            return value;
        }

        private void UpdateTennisStatusesAndScores(List<SportingbetTennisMatch> sourceMatches)
        {
            using (var db = new SportsDataPanelEntities())
            {
                foreach (var srcMatch in sourceMatches)
                {
                    var dbMatch = db.Matches.Where(mtch => mtch.SportingbetMatchId == srcMatch.id).FirstOrDefault();
                    if (dbMatch != null)
                    {
                        //match Time
                        DateTime srcDateTime = DateTime.MaxValue;
                        try
                        {
                            srcDateTime = DateTime.ParseExact(srcMatch.sd, "yyyy-MM-dd HH:mm:ss", null);
                        }
                        catch (System.Exception)
                        {
                            srcDateTime = DateTime.MaxValue;
                        }
                        if (srcDateTime != DateTime.MaxValue)
                        {
                            dbMatch.MatchDate = srcDateTime;
                        }
                        var scores = dbMatch.MatchScores.ToList();
                        var firstSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 11).ToList();
                        if (firstSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < firstSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(firstSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var firstSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 11).FirstOrDefault();

                        var secondSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 12).ToList();
                        if (secondSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < secondSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(secondSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var secondSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 12).FirstOrDefault();

                        var thirdSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 13).ToList();
                        if (thirdSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < thirdSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(thirdSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var thirdSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 13).FirstOrDefault();

                        var fourthSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 14).ToList();
                        if (fourthSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < fourthSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(fourthSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var fourthSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 14).FirstOrDefault();

                        var fifthSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 15).ToList();
                        if (fifthSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < fifthSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(fifthSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var fifthSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 15).FirstOrDefault();

                        var cpScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 16).ToList();
                        if (cpScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < cpScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(cpScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var cpScore = scores.Where(scr => scr.ScoreInfoTypeId == 16).FirstOrDefault();

                        var csSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 9).ToList();
                        if (csSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < csSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(csSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var csSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 9).FirstOrDefault();

                        var cfsSetScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 10).ToList();
                        if (cfsSetScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < cfsSetScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(cfsSetScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var cfsSetScore = scores.Where(scr => scr.ScoreInfoTypeId == 10).FirstOrDefault();

                        var finishedScoreAll = scores.Where(scr => scr.ScoreInfoTypeId == 17).ToList();
                        if (finishedScoreAll.Count() > 1)
                        {
                            for (int i = 1; i < finishedScoreAll.Count(); i++)
                            {
                                db.MatchScores.Remove(finishedScoreAll[i]);
                                db.SaveChanges();
                            }
                        }
                        var finishedScore = scores.Where(scr => scr.ScoreInfoTypeId == 17).FirstOrDefault();

                        if (!string.IsNullOrEmpty(srcMatch.serv) && !srcMatch.serv.Equals("0") && (srcMatch.serv.Equals("1") || srcMatch.serv.Equals("2")))
                        {
                            dbMatch.FirstToServe = srcMatch.serv;
                        }
                        if (!string.IsNullOrEmpty(srcMatch.p1.gamescore))
                        {
                            if (cpScore == null)
                            {
                                MatchScores cp = new MatchScores();
                                cp.ScoreInfoTypeId = 16;
                                cp.HomeScore = srcMatch.p1.gamescore;
                                cp.AwayScore = srcMatch.p2.gamescore;
                                dbMatch.MatchScores.Add(cp);
                            }
                            else
                            {
                                cpScore.HomeScore = srcMatch.p1.gamescore;
                                cpScore.AwayScore = srcMatch.p2.gamescore;
                            }
                        }
                        else
                        {
                            if (cpScore != null)
                            {
                                cpScore.HomeScore = "0";
                                cpScore.AwayScore = "0";
                            }
                        }
                        int cfsHomeScore = 0, cfsAwayScore = 0;
                        if (!int.TryParse(srcMatch.p1.sw, out cfsHomeScore))
                            cfsHomeScore = 0;
                        if (!int.TryParse(srcMatch.p2.sw, out cfsAwayScore))
                            cfsAwayScore = 0;

                        #region 1st Set
                        //1set
                        if (!string.IsNullOrEmpty(srcMatch.p1.s1))
                        {
                            dbMatch.MatchStatusId = 26;
                            if ((cfsHomeScore == 0 && cfsAwayScore == 1) || (cfsHomeScore == 1 && cfsAwayScore == 0))
                            {
                                if (csSetScore != null)
                                {
                                    csSetScore.HomeScore = "0";
                                    csSetScore.AwayScore = "0";
                                    csSetScore.TieBreak = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(srcMatch.p1.s2) || srcMatch.st.ToLower() == "finished" || (cfsHomeScore == 0 && cfsAwayScore == 1) || (cfsHomeScore == 1 && cfsAwayScore == 0))
                            {
                                if (firstSetScore == null)
                                {
                                    MatchScores firstScore = new MatchScores();
                                    firstScore.ScoreInfoTypeId = 11;
                                    firstScore.HomeScore = srcMatch.p1.s1;
                                    firstScore.AwayScore = srcMatch.p2.s1;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t1))
                                    {
                                        firstScore.HomeTieBreakScore = srcMatch.p1.t1;
                                        firstScore.AwayTieBreakScore = srcMatch.p2.t1;
                                        firstScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(firstScore);
                                }
                                else
                                {
                                    firstSetScore.HomeScore = srcMatch.p1.s1;
                                    firstSetScore.AwayScore = srcMatch.p2.s1;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t1))
                                    {
                                        firstSetScore.HomeTieBreakScore = srcMatch.p1.t1;
                                        firstSetScore.AwayTieBreakScore = srcMatch.p2.t1;
                                        firstSetScore.TieBreak = true;
                                    }
                                }
                            }
                            else
                            {
                                if (csSetScore == null)
                                {
                                    MatchScores csScore = new MatchScores();
                                    csScore.ScoreInfoTypeId = 9;
                                    csScore.HomeScore = srcMatch.p1.s1;
                                    csScore.AwayScore = srcMatch.p2.s1;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t1))
                                    {
                                        csScore.HomeTieBreakScore = srcMatch.p1.t1;
                                        csScore.AwayTieBreakScore = srcMatch.p2.t1;
                                        csScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(csScore);
                                }
                                else
                                {
                                    int p1Score = int.Parse(srcMatch.p1.s1);
                                    int p2Score = int.Parse(srcMatch.p2.s1);
                                    if ((p1Score >= 6 || p2Score >= 6) && Math.Abs(p1Score - p2Score) >= 2)
                                    {
                                        csSetScore.HomeScore = "0";
                                        csSetScore.AwayScore = "0";
                                        csSetScore.TieBreak = false;
                                        csSetScore.HomeTieBreakScore = "";
                                        csSetScore.AwayTieBreakScore = "";
                                    }
                                    else
                                    {
                                        csSetScore.HomeScore = srcMatch.p1.s1;
                                        csSetScore.AwayScore = srcMatch.p2.s1;
                                    }
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t1))
                                    {
                                        csSetScore.HomeTieBreakScore = srcMatch.p1.t1;
                                        csSetScore.AwayTieBreakScore = srcMatch.p2.t1;
                                        csSetScore.TieBreak = true;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 2nd Set
                        //2set
                        if (!string.IsNullOrEmpty(srcMatch.p1.s2))
                        {
                            dbMatch.MatchStatusId = 27;
                            if (csSetScore != null)
                            {
                                csSetScore.HomeScore = "0";
                                csSetScore.AwayScore = "0";
                                csSetScore.TieBreak = false;
                            }
                            //second set score
                            if (!string.IsNullOrEmpty(srcMatch.p1.s3) || srcMatch.st.ToLower() == "finished" || (cfsHomeScore == 0 && cfsAwayScore == 2) || (cfsHomeScore == 2 && cfsAwayScore == 0) || (cfsHomeScore == 1 && cfsAwayScore == 1))
                            {
                                if (secondSetScore == null)
                                {
                                    MatchScores secondScore = new MatchScores();
                                    secondScore.ScoreInfoTypeId = 12;
                                    secondScore.HomeScore = srcMatch.p1.s2;
                                    secondScore.AwayScore = srcMatch.p2.s2;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t2))
                                    {
                                        secondScore.HomeTieBreakScore = srcMatch.p1.t2;
                                        secondScore.AwayTieBreakScore = srcMatch.p2.t2;
                                        secondScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(secondScore);
                                }
                                else
                                {
                                    secondSetScore.HomeScore = srcMatch.p1.s2;
                                    secondSetScore.AwayScore = srcMatch.p2.s2;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t2))
                                    {
                                        secondSetScore.HomeTieBreakScore = srcMatch.p1.t2;
                                        secondSetScore.AwayTieBreakScore = srcMatch.p2.t2;
                                        secondSetScore.TieBreak = true;
                                    }
                                }
                            }
                            else
                            {
                                if (csSetScore == null)
                                {
                                    MatchScores csScore = new MatchScores();
                                    csScore.ScoreInfoTypeId = 9;
                                    csScore.HomeScore = srcMatch.p1.s2;
                                    csScore.AwayScore = srcMatch.p2.s2;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t2))
                                    {
                                        csScore.HomeTieBreakScore = srcMatch.p1.t2;
                                        csScore.AwayTieBreakScore = srcMatch.p2.t2;
                                        csScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(csScore);
                                }
                                else
                                {
                                    int p1Score = int.Parse(srcMatch.p1.s2);
                                    int p2Score = int.Parse(srcMatch.p2.s2);
                                    if ((p1Score >= 6 || p2Score >= 6) && Math.Abs(p1Score - p2Score) >= 2)
                                    {
                                        csSetScore.HomeScore = "0";
                                        csSetScore.AwayScore = "0";
                                        csSetScore.TieBreak = false;
                                        csSetScore.HomeTieBreakScore = "";
                                        csSetScore.AwayTieBreakScore = "";
                                    }
                                    else
                                    {
                                        csSetScore.HomeScore = srcMatch.p1.s2;
                                        csSetScore.AwayScore = srcMatch.p2.s2;
                                    }
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t2))
                                    {
                                        csSetScore.HomeTieBreakScore = srcMatch.p1.t2;
                                        csSetScore.AwayTieBreakScore = srcMatch.p2.t2;
                                        csSetScore.TieBreak = true;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 3rd Set
                        //3set
                        if (!string.IsNullOrEmpty(srcMatch.p1.s3))
                        {
                            dbMatch.MatchStatusId = 28;
                            if (csSetScore != null)
                            {
                                csSetScore.HomeScore = "0";
                                csSetScore.AwayScore = "0";
                                csSetScore.TieBreak = false;
                            }
                            //third set score
                            if (!string.IsNullOrEmpty(srcMatch.p1.s4) || srcMatch.st.ToLower() == "finished" || (cfsHomeScore == 1 && cfsAwayScore == 2) || (cfsHomeScore == 2 && cfsAwayScore == 1) || (cfsHomeScore == 3 && cfsAwayScore == 0) || (cfsHomeScore == 0 && cfsAwayScore == 3))
                            {
                                if (thirdSetScore == null)
                                {
                                    MatchScores thirdScore = new MatchScores();
                                    thirdScore.ScoreInfoTypeId = 13;
                                    thirdScore.HomeScore = srcMatch.p1.s3;
                                    thirdScore.AwayScore = srcMatch.p2.s3;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t3))
                                    {
                                        thirdScore.HomeTieBreakScore = srcMatch.p1.t3;
                                        thirdScore.AwayTieBreakScore = srcMatch.p2.t3;
                                        thirdScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(thirdScore);
                                }
                                else
                                {
                                    thirdSetScore.HomeScore = srcMatch.p1.s3;
                                    thirdSetScore.AwayScore = srcMatch.p2.s3;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t3))
                                    {
                                        thirdSetScore.HomeTieBreakScore = srcMatch.p1.t3;
                                        thirdSetScore.AwayTieBreakScore = srcMatch.p2.t3;
                                        thirdSetScore.TieBreak = true;
                                    }
                                }
                            }
                            else
                            {
                                if (csSetScore == null)
                                {
                                    MatchScores csScore = new MatchScores();
                                    csScore.ScoreInfoTypeId = 9;
                                    csScore.HomeScore = srcMatch.p1.s3;
                                    csScore.AwayScore = srcMatch.p2.s3;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t3))
                                    {
                                        csScore.HomeTieBreakScore = srcMatch.p1.t3;
                                        csScore.AwayTieBreakScore = srcMatch.p2.t3;
                                        csScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(csScore);
                                }
                                else
                                {
                                    int p1Score = int.Parse(srcMatch.p1.s3);
                                    int p2Score = int.Parse(srcMatch.p2.s3);
                                    if ((p1Score >= 6 || p2Score >= 6) && Math.Abs(p1Score - p2Score) >= 2)
                                    {
                                        csSetScore.HomeScore = "0";
                                        csSetScore.AwayScore = "0";
                                        csSetScore.TieBreak = false;
                                        csSetScore.HomeTieBreakScore = "";
                                        csSetScore.AwayTieBreakScore = "";
                                    }
                                    else
                                    {
                                        csSetScore.HomeScore = srcMatch.p1.s3;
                                        csSetScore.AwayScore = srcMatch.p2.s3;
                                    }
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t3))
                                    {
                                        csSetScore.HomeTieBreakScore = srcMatch.p1.t3;
                                        csSetScore.AwayTieBreakScore = srcMatch.p2.t3;
                                        csSetScore.TieBreak = true;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 4th set

                        //4set
                        if (!string.IsNullOrEmpty(srcMatch.p1.s4))
                        {
                            dbMatch.MatchStatusId = 29;
                            if (csSetScore != null)
                            {
                                csSetScore.HomeScore = "0";
                                csSetScore.AwayScore = "0";
                                csSetScore.TieBreak = false;
                            }
                            //fourth set score
                            if (!string.IsNullOrEmpty(srcMatch.p1.s5) || srcMatch.st.ToLower() == "finished" || (cfsHomeScore == 1 && cfsAwayScore == 3) || (cfsHomeScore == 3 && cfsAwayScore == 1) || (cfsHomeScore == 2 && cfsAwayScore == 2))
                            {
                                if (fourthSetScore == null)
                                {
                                    MatchScores fourthScore = new MatchScores();
                                    fourthScore.ScoreInfoTypeId = 14;
                                    fourthScore.HomeScore = srcMatch.p1.s4;
                                    fourthScore.AwayScore = srcMatch.p2.s4;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t4))
                                    {
                                        fourthScore.HomeTieBreakScore = srcMatch.p1.t4;
                                        fourthScore.AwayTieBreakScore = srcMatch.p2.t4;
                                        fourthScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(fourthScore);
                                }
                                else
                                {
                                    fourthSetScore.HomeScore = srcMatch.p1.s4;
                                    fourthSetScore.AwayScore = srcMatch.p2.s4;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t4))
                                    {
                                        fourthSetScore.HomeTieBreakScore = srcMatch.p1.t4;
                                        fourthSetScore.AwayTieBreakScore = srcMatch.p2.t4;
                                        fourthSetScore.TieBreak = true;
                                    }
                                }
                            }
                            else
                            {
                                if (csSetScore == null)
                                {
                                    MatchScores csScore = new MatchScores();
                                    csScore.ScoreInfoTypeId = 9;
                                    csScore.HomeScore = srcMatch.p1.s4;
                                    csScore.AwayScore = srcMatch.p2.s4;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t4))
                                    {
                                        csScore.HomeTieBreakScore = srcMatch.p1.t4;
                                        csScore.AwayTieBreakScore = srcMatch.p2.t4;
                                        csScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(csScore);
                                }
                                else
                                {
                                    int p1Score = int.Parse(srcMatch.p1.s4);
                                    int p2Score = int.Parse(srcMatch.p2.s4);
                                    if ((p1Score >= 6 || p2Score >= 6) && Math.Abs(p1Score - p2Score) >= 2)
                                    {
                                        csSetScore.HomeScore = "0";
                                        csSetScore.AwayScore = "0";
                                        csSetScore.TieBreak = false;
                                        csSetScore.HomeTieBreakScore = "";
                                        csSetScore.AwayTieBreakScore = "";
                                    }
                                    else
                                    {
                                        csSetScore.HomeScore = srcMatch.p1.s4;
                                        csSetScore.AwayScore = srcMatch.p2.s4;
                                    }
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t4))
                                    {
                                        csSetScore.HomeTieBreakScore = srcMatch.p1.t4;
                                        csSetScore.AwayTieBreakScore = srcMatch.p2.t4;
                                        csSetScore.TieBreak = true;
                                    }
                                }
                            }
                        }

                        #endregion

                        #region 5th set
                        //5set
                        if (!string.IsNullOrEmpty(srcMatch.p1.s5))
                        {
                            dbMatch.MatchStatusId = 30;
                            if (csSetScore != null)
                            {
                                csSetScore.HomeScore = "0";
                                csSetScore.AwayScore = "0";
                                csSetScore.TieBreak = false;
                            }
                            if (srcMatch.st.ToLower() == "finished")
                            {
                                if (fifthSetScore == null)
                                {
                                    MatchScores fifthScore = new MatchScores();
                                    fifthScore.ScoreInfoTypeId = 15;
                                    fifthScore.HomeScore = srcMatch.p1.s5;
                                    fifthScore.AwayScore = srcMatch.p2.s5;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t5))
                                    {
                                        fifthScore.HomeTieBreakScore = srcMatch.p1.t5;
                                        fifthScore.AwayTieBreakScore = srcMatch.p2.t5;
                                        fifthScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(fifthScore);
                                }
                                else
                                {
                                    fifthSetScore.HomeScore = srcMatch.p1.s5;
                                    fifthSetScore.AwayScore = srcMatch.p2.s5;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t5))
                                    {
                                        fifthSetScore.HomeTieBreakScore = srcMatch.p1.t5;
                                        fifthSetScore.AwayTieBreakScore = srcMatch.p2.t5;
                                        fifthSetScore.TieBreak = true;
                                    }
                                }
                            }
                            else
                            {
                                if (csSetScore == null)
                                {
                                    MatchScores csScore = new MatchScores();
                                    csScore.ScoreInfoTypeId = 9;
                                    csScore.HomeScore = srcMatch.p1.s5;
                                    csScore.AwayScore = srcMatch.p2.s5;
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t5))
                                    {
                                        csScore.HomeTieBreakScore = srcMatch.p1.t5;
                                        csScore.AwayTieBreakScore = srcMatch.p2.t5;
                                        csScore.TieBreak = true;
                                    }
                                    dbMatch.MatchScores.Add(csScore);
                                }
                                else
                                {
                                    int p1Score = int.Parse(srcMatch.p1.s5);
                                    int p2Score = int.Parse(srcMatch.p2.s5);
                                    if ((p1Score >= 6 || p2Score >= 6) && Math.Abs(p1Score - p2Score) >= 2)
                                    {
                                        csSetScore.HomeScore = "0";
                                        csSetScore.AwayScore = "0";
                                        csSetScore.TieBreak = false;
                                        csSetScore.HomeTieBreakScore = "";
                                        csSetScore.AwayTieBreakScore = "";
                                    }
                                    else
                                    {
                                        csSetScore.HomeScore = srcMatch.p1.s5;
                                        csSetScore.AwayScore = srcMatch.p2.s5;
                                    }
                                    if (!string.IsNullOrEmpty(srcMatch.p1.t5))
                                    {
                                        csSetScore.HomeTieBreakScore = srcMatch.p1.t5;
                                        csSetScore.AwayTieBreakScore = srcMatch.p2.t5;
                                        csSetScore.TieBreak = true;
                                    }
                                }
                            }
                        }

                        #endregion

                        if (srcMatch.st.ToLower() == "inprogress")
                        {
                            if (cfsSetScore == null)
                            {
                                MatchScores cfsScore = new MatchScores();
                                cfsScore.ScoreInfoTypeId = 10;
                                cfsScore.HomeScore = srcMatch.p1.sw;
                                cfsScore.AwayScore = srcMatch.p2.sw;
                                dbMatch.MatchScores.Add(cfsScore);
                            }
                            else
                            {
                                cfsSetScore.HomeScore = srcMatch.p1.sw;
                                cfsSetScore.AwayScore = srcMatch.p2.sw;
                            }
                        }
                        if (srcMatch.st.ToLower() == "finished")
                        {
                            if (cfsSetScore != null)
                            {
                                db.MatchScores.Remove(cfsSetScore);
                            }
                            if (!string.IsNullOrEmpty(srcMatch.cmt))
                            {
                                if (srcMatch.cmt.ToLower() == "retired")
                                {
                                    //delete cs, cp scores
                                    if (cpScore != null)
                                    {
                                        db.MatchScores.Remove(cpScore);
                                    }
                                    if (csSetScore != null)
                                    {
                                        db.MatchScores.Remove(csSetScore);
                                    }
                                    //set match status
                                    if (srcMatch.p1.fr.Equals("won"))
                                    {
                                        dbMatch.MatchStatusId = 33;
                                    }
                                    else
                                    {
                                        dbMatch.MatchStatusId = 32;
                                    }
                                    //add finished scores
                                    if (finishedScore == null)
                                    {
                                        MatchScores finScore = new MatchScores();
                                        finScore.ScoreInfoTypeId = 17;
                                        finScore.HomeScore = srcMatch.p1.sw;
                                        finScore.AwayScore = srcMatch.p2.sw;
                                        dbMatch.MatchScores.Add(finScore);
                                    }
                                    else
                                    {
                                        finishedScore.HomeScore = srcMatch.p1.sw;
                                        finishedScore.AwayScore = srcMatch.p2.sw;
                                    }
                                }
                                else if (srcMatch.cmt.ToLower() == "w.o.")
                                {
                                    if (srcMatch.p1.fr.Equals("won"))
                                    {
                                        dbMatch.MatchStatusId = 34;
                                    }
                                    else
                                    {
                                        dbMatch.MatchStatusId = 35;
                                    }
                                }
                                else
                                {
                                    dbMatch.MatchStatusId = 25;
                                    if (cpScore != null)
                                    {
                                        db.MatchScores.Remove(cpScore);
                                    }
                                    if (csSetScore != null)
                                    {
                                        db.MatchScores.Remove(csSetScore);
                                    }
                                    if (finishedScore == null)
                                    {
                                        MatchScores finScore = new MatchScores();
                                        finScore.ScoreInfoTypeId = 17;
                                        finScore.HomeScore = srcMatch.p1.sw;
                                        finScore.AwayScore = srcMatch.p2.sw;
                                        dbMatch.MatchScores.Add(finScore);
                                    }
                                    else
                                    {
                                        finishedScore.HomeScore = srcMatch.p1.sw;
                                        finishedScore.AwayScore = srcMatch.p2.sw;
                                    }
                                }
                            }
                        }
                        else if (srcMatch.st.ToLower() == "interrupted")
                        {
                            dbMatch.MatchStatusId = 127;
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        private List<SportingbetTennisMatch> CrawlSharkScoresMatchByURL(string sportingbetUrl)
        {
            List<SportingbetTennisMatch> model = new List<SportingbetTennisMatch>();
            string json = string.Empty;
            try
            {
                using (TennisWebClient _webClient = new TennisWebClient())
                {
                    json = _webClient.DownloadString(sportingbetUrl);
                }
                var tennisJson = (JObject)JsonConvert.DeserializeObject(json);
                var tennisMatches = tennisJson.Properties();
                foreach (var m in tennisMatches)
                {
                    if (m.Name != "ts" && m.Name != "t")
                    {
                        var match = m.Value.ToObject<SportingbetTennisMatch>();
                        match.id = int.Parse(m.Name);
                        model.Add(match);
                    }
                }
            }
            catch (Exception)
            {
                model = null;
            }
            return model;
        }
        #endregion
        #endregion

        #region Custom WebClient Class
        private class TennisWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                request.KeepAlive = true;
                request.Proxy = new WebProxy("127.0.0.1:24000");
                return request;
            }
        }
        #endregion                
    }

    public enum Gender
    {
        MALE = 3,
        FEMALE = 4,
        OTHER = 5
    }
}