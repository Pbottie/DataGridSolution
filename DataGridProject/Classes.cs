using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace DataGridProject;

public class SearchMeasurementListItem
{
    public int Id { get; set; }
    public bool IsReported { get; set; }
    public bool IsNative { get; set; }
    public bool HasBreakdowns { get; set; }
    public Guid MeasurementId { get; set; }
    public string MeasurementName { get; set; }
    public string MeasurementTypeName { get; set; }
    public string MeasurementTypeDisplayName { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPath { get; set; }
    public string BrandName { get; set; }
    public DateTime StartDate { get; set; }
    public string FormattedStartDate => StartDate.ToShortDateString();
    public DateTime EndDate { get; set; }
    public string FormattedEndDate => EndDate.ToShortDateString();
    public MeasurementStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    public StatisticInfo Impressions { get; set; }
    public StatisticInfo Surveys { get; set; }
    public StatisticInfo TrackingStart { get; set; }
    public List<PptReportType> ReportTypes { get; set; }
    public bool HasCustomReport { get; set; }
    public List<PptReportType> CustomReports { get; set; }
    public double TotalEffect { get; set; }
    public string FormattedTotalEffect => MeasurementTypeName?.ToLower() switch
    {
        "adhoc" => "-",
        "adhocdisplay" => "-",
        "adhocnative" => "-",
        _ => !IsReported ? "-" : $"{TotalEffect}%",
    };

    public string Path { get; set; }
    public string MeasurementPath { get; set; }
    public bool IsSimulation { get; set; }
    public double SortSurveys => Surveys.Count;
    public (string, string) FormattedSurveyValues
    {
        get
        {
            var culture = CultureInfo.GetCultureInfo("en-US");
            var format = "N0";
            return (Surveys.Count.ToString(format, culture), Surveys.Recently.ToString(format, culture));
        }
    }
    public double SortImpression => ImpressionValues.Item1;
    public (string, string) FormattedImpressionValues => (ImpressionValues.Item2, ImpressionValues.Item3);
    public (double, string, string) ImpressionValues
    {
        get
        {
            var imp1 = Impressions.Count;
            var imp2 = Impressions.Recently;
            var tra1 = TrackingStart?.Count ?? 0;
            var tra2 = TrackingStart?.Recently ?? 0;
            var culture = CultureInfo.GetCultureInfo("en-US");
            var format = "N0";

            return imp1 > tra1
                ? (imp1, imp1.ToString(format, culture), imp2.ToString(format, culture))
                : (tra1, tra1.ToString(format, culture), tra2.ToString(format, culture));
        }
    }
}

public class StatisticInfo
{
    public string Type { get; set; }
    public double Count { get; set; }
    public double Recently { get; set; }
    public List<StatisticInfo> Breakdowns { get; set; }
    public List<StatisticInfo> FrequencyGroups { get; set; }
}
public class PptReportType
{
    public Guid MeasurementId { get; set; }
    public string TemplateId { get; set; }
    public string TemplateDisplayName { get; set; }
}

public enum MeasurementStatus
{
    Upcoming,
    Active,
    Warning,
    Calculated
}

public enum StatisticsType
{
    Impression,
    StorageComplete,
    TrackingStart,
}

public static class ArrayExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection != null)
        {
            return !collection.Any();
        }

        return true;
    }
}
