using Bogus;
using System.Collections.Concurrent;

namespace DataGridProject;

public static class BogusData
{
    private static List<SearchMeasurementListItem> bogusList;

    static BogusData()
    {
        bogusList = new();

        Randomizer.Seed = new Random(1234567);

        var fakeId = 0;

        var testImpressions = new Faker<StatisticInfo>()
            .RuleFor(s => s.Type, f => StatisticsType.Impression.ToString())
            .RuleFor(s => s.Count, f => f.Random.Double() * 50000)
            .RuleFor(s => s.Recently, (f, s) => f.Random.Double() * s.Count);

        var testTrackingStart = new Faker<StatisticInfo>()
            .RuleFor(s => s.Type, f => StatisticsType.TrackingStart.ToString())
            .RuleFor(s => s.Count, f => f.Random.Double() * 50000)
            .RuleFor(s => s.Recently, (f, s) => f.Random.Double() * s.Count);

        var testSurveys = new Faker<StatisticInfo>()
            .RuleFor(s => s.Type, f => StatisticsType.StorageComplete.ToString())
            .RuleFor(s => s.Count, f => f.Random.Double() * 50000)
            .RuleFor(s => s.Recently, (f, s) => f.Random.Double() * s.Count);

        var testReportTypes = new Faker<PptReportType>()
            .RuleFor(p => p.MeasurementId, f => Guid.NewGuid())
            .RuleFor(p => p.TemplateId, f => f.Random.Uuid().ToString())
            .RuleFor(p => p.TemplateDisplayName, f => f.Random.Word());


        var testData = new Faker<SearchMeasurementListItem>()
            .StrictMode(true)
            .RuleFor(s => s.Id, f => fakeId++)
            .RuleFor(s => s.IsReported, f => f.Random.Bool())
            .RuleFor(s => s.IsNative, f => f.Random.Bool())
            .RuleFor(s => s.HasBreakdowns, f => f.Random.Bool())
            .RuleFor(s => s.MeasurementId, f => Guid.NewGuid())
            .RuleFor(s => s.MeasurementName, f => f.Name.FirstName())
            .RuleFor(s => s.MeasurementTypeName, f => f.Vehicle.Model())
            .RuleFor(s => s.MeasurementTypeDisplayName, (f, s) => f.Internet.UserName(s.MeasurementTypeName))
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid().OrDefault(f, 0.5f, Guid.Empty))
            .RuleFor(s => s.CustomerName, f => f.Name.FullName())
            .RuleFor(s => s.CustomerPath, f => f.Internet.UrlWithPath())
            .RuleFor(s => s.BrandName, f => f.Vehicle.Model())
            .RuleFor(s => s.StartDate, f => f.Date.Past(1))
            .RuleFor(s => s.FormattedStartDate, (f, s) => s.StartDate.ToShortDateString())
            .RuleFor(s => s.EndDate, f => f.Date.Future(1))
            .RuleFor(s => s.FormattedEndDate, (f, s) => s.EndDate.ToShortDateString())
            .RuleFor(s => s.Status, f => f.PickRandom<MeasurementStatus>())
            .RuleFor(s => s.Status2, f => f.PickRandom<MeasurementStatus>())
            .RuleFor(s => s.ErrorMessage, "")
            .RuleFor(s => s.Impressions, testImpressions.Generate(1).First())
            .RuleFor(s => s.Surveys, testSurveys.Generate(1).First())
            .RuleFor(s => s.TrackingStart, testTrackingStart.Generate(1).First())
            .RuleFor(s => s.ReportTypes, f => testReportTypes.Generate(f.Random.Number(1, 5)).ToList())
            .RuleFor(s => s.HasCustomReport, f => f.Random.Bool())
            .RuleFor(s => s.CustomReports, (f, s) => s.HasCustomReport ? testReportTypes.Generate(f.Random.Number(1, 5)).ToList() : null)
            .RuleFor(s => s.TotalEffect, f => f.Random.Number(0, 200))
            .RuleFor(s => s.FormattedTotalEffect, (f, s) => $"{s.TotalEffect}%")
            .RuleFor(s => s.Path, f => f.Internet.UrlRootedPath())
            .RuleFor(s => s.MeasurementPath, f => f.Internet.UrlRootedPath())
            .RuleFor(s => s.IsSimulation, false)
            .RuleFor(s => s.SortSurveys, (f, s) => s.Surveys.Count)
            .RuleFor(s => s.FormattedSurveyValues, (f, s) => (s.SortSurveys.ToString(), s.Surveys.Recently.ToString()))
            .RuleFor(s => s.ImpressionValues, (f, s)
            => (s.Impressions.Count > s.TrackingStart.Count
            ? (s.Impressions.Count, s.Impressions.Count.ToString(), s.Impressions.Recently.ToString())
            : (s.TrackingStart.Count, s.TrackingStart.Count.ToString(), s.TrackingStart.Recently.ToString())))
            .RuleFor(s=>s.TestDate, f=>f.Date.FutureDateOnly().OrNull(f,0.1f));

        bogusList = testData.Generate(1000).ToList();
    }


    public static List<SearchMeasurementListItem> GetData(int amount)
    {

        return bogusList.Take(amount).ToList();
    }


}
