using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using TennisAPIAndScrappingService.Library;

namespace TennisAPIAndScrappingService
{
    public partial class TennisService : ServiceBase
    {
        #region Commented Timers And Callbacks
        //private Timer _timerSinglesRanking;
        //private Timer _timerCrawlSportingbetTennisMatches;
        //private Timer _timerDownloadSofaScoresTennisMatches;
        //private TimerCallback _timerCallbackSinglesRanking; 
        //private TimerCallback _timerCallbackCrawlSportingbetTennisMatches;
        //private TimerCallback _timerCallbackDownloadSofaScoresTennisMatches;
        #endregion

        #region Timers
        private Timer _timerScrapATPWorldTourTennisMatchData;
        private Timer _timerScrapTennisH2HData;
        private Timer _timerAverageAcePerMatch;
        private Timer _timerServiceGamesWon;
        private Timer _timerReturnGamesWon;
        private Timer _timerWTARankingData;
        private Timer _timerScrapATPRankingDataRaceToLondon;
        private Timer _timerScrapATPRankingDataSingle;
        private Timer _timerGetWomenDataForPlayerProfile;
        private Timer _timerGetWomenDataForPlayerImage;
        private Timer _timerGetMenDataForPlayerProfileSingle;
        private Timer _timerGetMenDataForPlayerProfileRaceToLondon;
        private Timer _timerCrawlWTALiveMatches;
        private Timer _timerScrapWTAStats;
        private Timer _timerGetMenDataForPlayerOverview;
        private Timer _timerGetMenDataForPlayerGrandSlamSingleResult;
        #endregion

        #region TimerCallbacks
        private TimerCallback _timerCallbackScrapATPWorldTourTennisMatchData;
        private TimerCallback _timerCallbackScrapTennisH2HData;
        private TimerCallback _timerCallbackAverageAcePerMatch;
        private TimerCallback _timerCallbackServiceGamesWon;
        private TimerCallback _timerCallbackReturnGamesWon;
        private TimerCallback _timerCallbackWTARankingData;
        private TimerCallback _timerCallbackScrapATPRankingDataRaceToLondon;
        private TimerCallback _timerCallbackScrapATPRankingDataSingle;
        private TimerCallback _timerCallbackGetWomenDataForPlayerProfile;
        private TimerCallback _timerCallbackGetWomenDataForPlayerImage;
        private TimerCallback _timerCallbackGetMenDataForPlayerProfileSingle;
        private TimerCallback _timerCallbackGetMenDataForPlayerProfileRaceToLondon;
        private TimerCallback _timerCallbackCrawlWTALiveMatches;
        private TimerCallback _timerCallbackScrapWTAStats;
        private TimerCallback _timerCallbackGetMenDataForPlayerOverview;
        private TimerCallback _timerCallbackGetMenDataForPlayerGrandSlamSingleResult;
        #endregion

        #region Private Members
        private TennisScrappingService _tennisScrappingService;
        private string _filePath = @"c:\TennisApiAndScrapper.txt";
        #endregion

        public TennisService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (StreamWriter _streamWriter = File.AppendText(_filePath))
            {
                _streamWriter.WriteLine("Service started at: " + DateTime.Now.ToString());
            }
#if DEBUG
                                    Debugger.Launch();
#endif
            _tennisScrappingService = new TennisScrappingService();

            _timerCallbackScrapATPWorldTourTennisMatchData = new TimerCallback(_tennisScrappingService.ScrapATPWorldTourTennisMatchData);
            _timerScrapATPWorldTourTennisMatchData = new Timer(_timerCallbackScrapATPWorldTourTennisMatchData, null, 0, 120000);

            _timerCallbackCrawlWTALiveMatches = new TimerCallback(_tennisScrappingService.CrawlWTALiveMatches);
            _timerCrawlWTALiveMatches = new Timer(_timerCallbackCrawlWTALiveMatches, null, 5000, 120000);

            _timerCallbackAverageAcePerMatch = new TimerCallback(_tennisScrappingService.AverageAcePerMatch);
            _timerAverageAcePerMatch = new Timer(_timerCallbackAverageAcePerMatch, null, 10000, 7200000);

            _timerCallbackReturnGamesWon = new TimerCallback(_tennisScrappingService.ReturnGamesWon);
            _timerReturnGamesWon = new Timer(_timerCallbackReturnGamesWon, null, 15000, 7200000);

            _timerCallbackScrapTennisH2HData = new TimerCallback(_tennisScrappingService.ScrapTennisH2HData);
            _timerScrapTennisH2HData = new Timer(_timerCallbackScrapTennisH2HData, null, 20000, 7200000);

            _timerCallbackServiceGamesWon = new TimerCallback(_tennisScrappingService.ServiceGamesWon);
            _timerServiceGamesWon = new Timer(_timerCallbackServiceGamesWon, null, 25000, 7200000);

            _timerCallbackWTARankingData = new TimerCallback(_tennisScrappingService.GetWTARankingData);
            _timerWTARankingData = new Timer(_timerCallbackWTARankingData, null, 30000, 86400000);

            _timerCallbackScrapATPRankingDataRaceToLondon = new TimerCallback(_tennisScrappingService.ScrapATPRankingDataRaceToLondon);
            _timerScrapATPRankingDataRaceToLondon = new Timer(_timerCallbackScrapATPRankingDataRaceToLondon, null, 35000, 86400000);

            _timerCallbackScrapATPRankingDataSingle = new TimerCallback(_tennisScrappingService.ScrapATPRankingDataSingle);
            _timerScrapATPRankingDataSingle = new Timer(_timerCallbackScrapATPRankingDataSingle, null, 40000, 86400000);

            _timerCallbackGetWomenDataForPlayerProfile = new TimerCallback(_tennisScrappingService.GetWomenDataForPlayerProfile);
            _timerGetWomenDataForPlayerProfile = new Timer(_timerCallbackGetWomenDataForPlayerProfile, null, 50000, 86400000);

            _timerCallbackScrapWTAStats = new TimerCallback(_tennisScrappingService.ScrapWTAStats);
            _timerScrapWTAStats = new Timer(_timerCallbackScrapWTAStats, null, 55000, 86400000);

            _timerCallbackGetWomenDataForPlayerImage = new TimerCallback(_tennisScrappingService.GetWomenDataForPlayerImage);
            _timerGetWomenDataForPlayerImage = new Timer(_timerCallbackGetWomenDataForPlayerImage, null, 60000, 86400000);

            _timerCallbackGetMenDataForPlayerProfileSingle = new TimerCallback(_tennisScrappingService.GetMenDataForPlayerProfileSingle);
            _timerGetMenDataForPlayerProfileSingle = new Timer(_timerCallbackGetMenDataForPlayerProfileSingle, null, 65000, 86400000);

            _timerCallbackGetMenDataForPlayerProfileRaceToLondon = new TimerCallback(_tennisScrappingService.GetMenDataForPlayerProfileRaceToLondon);
            _timerGetMenDataForPlayerProfileRaceToLondon = new Timer(_timerCallbackGetMenDataForPlayerProfileRaceToLondon, null, 70000, 86400000);

            _timerCallbackGetMenDataForPlayerOverview = new TimerCallback(_tennisScrappingService.GetMenDataForPlayerOverview);
            _timerGetMenDataForPlayerOverview = new Timer(_timerCallbackGetMenDataForPlayerOverview, null, 75000, 86400000);

            _timerCallbackGetMenDataForPlayerGrandSlamSingleResult = new TimerCallback(_tennisScrappingService.GetMenDataForPlayerOverview);
            _timerGetMenDataForPlayerGrandSlamSingleResult = new Timer(_timerCallbackGetMenDataForPlayerGrandSlamSingleResult, null, 85000, 86400000);

            #region Commented Timers
            //_timerCallbackCrawlSportingbetTennisMatches = new TimerCallback(_tennisScrappingService.CrawlSportingbetTennisMatches);
            //_timerCrawlSportingbetTennisMatches = new Timer(_timerCallbackCrawlSportingbetTennisMatches, null, 5000, 86400000);

            //_timerCallbackDownloadSofaScoresTennisMatches = new TimerCallback(_tennisScrappingService.DownloadSofaScoresTennisMatches);
            //_timerDownloadSofaScoresTennisMatches = new Timer(_timerCallbackDownloadSofaScoresTennisMatches, null, 5000, 86400000); 

            //_timerCallbackSinglesRanking = new TimerCallback(_tennisScrappingService.SinglesRanking);
            //_timerSinglesRanking = new Timer(_timerCallbackSinglesRanking, null, 18000, 86400000); 
            #endregion
        }

        protected override void OnStop()
        {
            #region Commented Timers
            //_timerSinglesRanking.Dispose(); 
            //_timerCrawlSportingbetTennisMatches.Dispose();
            //_timerDownloadSofaScoresTennisMatches.Dispose();
            #endregion

            _timerScrapATPWorldTourTennisMatchData.Dispose();
            _timerScrapTennisH2HData.Dispose();
            _timerAverageAcePerMatch.Dispose();
            _timerServiceGamesWon.Dispose();
            _timerReturnGamesWon.Dispose();
            _timerWTARankingData.Dispose();
            _timerScrapATPRankingDataRaceToLondon.Dispose();
            _timerScrapATPRankingDataSingle.Dispose();
            _timerGetWomenDataForPlayerProfile.Dispose();
            _timerGetWomenDataForPlayerImage.Dispose();
            _timerGetMenDataForPlayerProfileSingle.Dispose();
            _timerGetMenDataForPlayerProfileRaceToLondon.Dispose();
            _timerGetMenDataForPlayerOverview.Dispose();
            _timerGetMenDataForPlayerGrandSlamSingleResult.Dispose();
            _timerCrawlWTALiveMatches.Dispose();
            _timerScrapWTAStats.Dispose();
            using (StreamWriter _streamWriter = File.AppendText(_filePath))
            {
                _streamWriter.WriteLine("Service Stopped at: " + DateTime.Now.ToString());
            }
        }
    }
}