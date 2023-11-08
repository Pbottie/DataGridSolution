namespace DataGridProject.Interfaces;

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
