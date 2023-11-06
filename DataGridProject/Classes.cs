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


public abstract class UIComponentBase : ComponentBase
{
    [Inject] public IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Gets or sets the unique id of the element.
    /// </summary>
    /// <remarks>
    /// Note that this ID is not defined for the component but instead for the underlined element that it represents.
    /// eg: for the TextEdit the ID will be set on the input element.
    /// </remarks>
    [Parameter] public string ElementId { get; set; }

    /// <summary>
    /// Toggle component load vs unload
    /// </summary>
    [Parameter] public bool NoRender { get; set; }

    protected readonly IClassBuilder classBuilder;

    public UIComponentBase()
    {
        classBuilder = new ClassBuilder();
    }

    protected override void OnInitialized()
    {
        if (ElementId == null)
            ElementId = GetRandomString();

        base.OnInitialized();
    }

    public async Task JSInvokeVoidAsync(string identifier) => await JSRuntime.InvokeVoidAsync(identifier);
    public async Task JSInvokeVoidAsync(string identifier, params object[] args) => await JSRuntime.InvokeVoidAsync(identifier, args);

    /// <summary>
    /// Gets the built class-names based on all the rules set by the component parameters.
    /// </summary>
    public string ClassNames => classBuilder?
        .AddClassFromAttributes(AdditionalAttributes)
        .GetClass();

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>label</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

    private string GetRandomString()
    {
        string path = System.IO.Path.GetRandomFileName();
        path = path.Replace(".", "");
        return path;
    }
}

public interface IClassBuilder
{
    ClassBuilder Begin(string value);
    ClassBuilder Begin(string value, bool condition);
    ClassBuilder Begin(string prefix, string value);
    ClassBuilder Begin(string prefix, string value, bool condition);
    ClassBuilder AddClass(string value);
    ClassBuilder AddClass(string value, bool condition);
    ClassBuilder AddClass(string prefix, string value);
    ClassBuilder AddClass(string prefix, string value, bool condition);
    ClassBuilder AddClass(IEnumerable<string> values);
    ClassBuilder AddClassFromAttributes(IReadOnlyDictionary<string, object> additionalAttributes);
    string GetClass();
}

public class ClassBuilder : IClassBuilder
{
    private const char DELIMITER = ' ';
    private StringBuilder _builder;

    public ClassBuilder Begin(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        _builder = new StringBuilder(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder Begin(string value, bool condition)
    {
        if (!condition)
        {
            _builder = new StringBuilder();
            return this;
        }

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        _builder = new StringBuilder(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder Begin(string prefix, string value)
    {
        if (prefix == null || value == null)
            throw new ArgumentNullException("prefix || value");

        _builder = new StringBuilder(prefix).Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder Begin(string prefix, string value, bool condition)
    {
        if (!condition)
        {
            _builder = new StringBuilder();
            return this;
        }

        if (prefix == null || value == null)
            throw new ArgumentNullException("prefix || value");

        _builder = new StringBuilder(prefix).Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder AddClass(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _builder.Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder AddClass(string value, bool condition)
    {
        if (condition && !string.IsNullOrEmpty(value))
            _builder.Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder AddClass(string prefix, string value)
    {
        if (!string.IsNullOrEmpty(value))
            _builder.Append(prefix).Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder AddClass(string prefix, string value, bool condition)
    {
        if (condition && !string.IsNullOrEmpty(value))
            _builder.Append(prefix).Append(value).Append(DELIMITER);

        return this;
    }

    public ClassBuilder AddClass(IEnumerable<string> values)
    {
        if (values.Any())
            _builder.Append(string.Join(DELIMITER.ToString(), values)).Append(DELIMITER);

        return this;
    }

    public ClassBuilder RemoveClass(bool condition, params string[] values)
    {
        if (condition && !values.IsNullOrEmpty())
        {
            foreach (var value in values)
            {
                _builder.Replace(value, string.Empty);
            }
        }

        return this;
    }

    public ClassBuilder AddClassFromAttributes(IReadOnlyDictionary<string, object> additionalAttributes)
    {
        if (_builder is null)
            _builder = new StringBuilder();

        // https://stackoverflow.com/questions/70885840/blazor-attribute-splatting-issues-with-cssbuilder-package

        var classAttributes = additionalAttributes?.GetValueOrDefault("class", null);

        if (classAttributes != null && !_builder.ToString().Contains(classAttributes.ToString()))
            _builder.Append($" {classAttributes}");

        return this;
    }

    public string GetClass() => _builder?.ToString().TrimEnd();
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
