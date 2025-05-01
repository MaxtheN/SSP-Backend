namespace WEBASE.OfficeTools.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PrintableModelAttribute : Attribute
{
    public PrintableModelAttribute(string text, int tableId)
    {
        Text = text;
        TableId = tableId;
        //ModelType = modelType;
        //DocumentPrintingCache.PrintableModels ??= new();
        //if (!DocumentPrintingCache.PrintableModels.Any(x => x.ModelType == modelType))
        //    DocumentPrintingCache.PrintableModels.Add(this);

        //var types = typeof(AccountServices.AccountUserDto).Assembly.GetTypes()
        //    .Where(a => a.GetCustomAttribute(typeof(PrintableModelAttribute)) != null)
        //    .Select(a => new
        //    {
        //        Type = a,
        //        Attr = a.GetCustomAttribute<PrintableModelAttribute>()
        //    })
        //    .ToArray();

        //types[0].Attr.Text
    }

    public string Text { get; set; }
    public int TableId { get; set; }
    public Type ModelType { get; set; }

}


public static class DocumentPrintingCache
{
    public static List<PrintableModelAttribute> PrintableModels { get; set; }
}