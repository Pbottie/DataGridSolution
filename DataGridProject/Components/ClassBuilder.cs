namespace DataGridProject.Components;

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
