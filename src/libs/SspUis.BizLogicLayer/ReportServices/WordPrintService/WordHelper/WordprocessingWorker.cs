using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class WordprocessingWorker : IDisposable
    {

        #region Fields

        private bool _isPropertiesNotReplaced = false;
        private bool _mustSaveSourceFile = false;
        private string _sourceFileName = null;
        private WordprocessingDocument _document = null;
        private Stream _stream = null;
        private DocumentPropertyMapper _propertyMapper = null;

        #endregion

        #region Ctor
        private WordprocessingWorker()
        {
            _propertyMapper = new DocumentPropertyMapper();
        }

        private WordprocessingWorker(Stream stream, byte[] sourceBytes)
            : this()
        {
            if (stream == null)
                throw new ArgumentNullException("wordprocessingStream");

            if (sourceBytes != null)
                stream.Write(sourceBytes, 0, sourceBytes.Length);

            _stream = stream;

            var processSettings = new MarkupCompatibilityProcessSettings
                (
                MarkupCompatibilityProcessMode.ProcessAllParts,
                FileFormatVersions.Office2007
                );
            var openSettings = new OpenSettings()
            {
                MarkupCompatibilityProcessSettings = processSettings,
                AutoSave = true
            };

            _document = WordprocessingDocument.Open(_stream, true, openSettings);
            PrepareDocument();
        }

        public WordprocessingWorker(Stream wordprocessingStream)
            : this(wordprocessingStream, null)
        { }

        public WordprocessingWorker(byte[] sourceBytes)
            : this(new MemoryStream(), sourceBytes)
        { }

        public WordprocessingWorker(string sourceFileName)
            : this(File.ReadAllBytes(sourceFileName))
        {
            _sourceFileName = sourceFileName;
            if (_mustSaveSourceFile)
                SaveSourceFile();
        }

        #endregion

        #region Properties

        public WordprocessingDocument Document
        {
            get
            {
                return _document;
            }
        }

        private Type _typeOfStream = null;
        public Type TypeOfStream
        {
            get
            {
                return _typeOfStream ?? (_typeOfStream = _stream.GetType());
            }
        }

        #endregion

        #region Methods

        public void AddPropertyKeyValue(string key, object value)
        {
            _isPropertiesNotReplaced = true;
            _propertyMapper.AddKeyValue(key, value);
        }

        public void AddPropertiesKeyValue<T>(T model)
        {
            AddPropertiesKeyValue(model, null);
        }

        public void AddPropertiesKeyValue<T>(T model, string prefix)
        {
            _isPropertiesNotReplaced = true;
            var props = Utils.GetProperties(typeof(T));

            if (string.IsNullOrEmpty(prefix))
                foreach (var pr in props)
                    _propertyMapper.AddKeyValue(pr.Name, pr.GetValue(model, null));
            else
                foreach (var pr in props)
                    _propertyMapper.AddKeyValue(string.Concat(prefix, pr.Name), pr.GetValue(model, null));
        }

        public void ReplaceProperties()
        {
            if (_isPropertiesNotReplaced)
            {
                _propertyMapper.Replace(_document, true, false);
                _isPropertiesNotReplaced = false;
            }
        }

        public void ClearPropertyMapper()
        {
            _propertyMapper.Clear();
            _isPropertiesNotReplaced = false;
        }

        public Stream GetStream(bool autoSave = true)
        {
            if (autoSave)
                Save();

            _stream.Seek(0, SeekOrigin.Begin);

            return _stream;
        }

        public void FillTable(IEnumerable source, string bookmarkStartName)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            BookmarkStart bookmarkStart = null;
            Table table = null;

            if ((bookmarkStart = _document.MainDocumentPart.RootElement.Descendants<BookmarkStart>().FirstOrDefault(a => a.Name == bookmarkStartName)) != null &&
                (table = bookmarkStart.Ancestors<Table>().FirstOrDefault()) != null)
            {
                var templateRow = bookmarkStart.Ancestors<TableRow>().FirstOrDefault();
                object firstElement = null;

                foreach (var item in source)
                {
                    firstElement = item;
                    break;
                }

                if (firstElement != null)
                {
                    var templateRowClone = (TableRow)templateRow.Clone();

                    foreach (var bookMarkStartInClone in templateRowClone.Descendants<BookmarkStart>())
                        bookMarkStartInClone.Remove();

                    foreach (var paragraph in templateRowClone.Descendants<Paragraph>())
                        CollectTextsToFirstTextAndReturn(paragraph);

                    Type type = firstElement.GetType();
                    int index = 0;
                    IDictionary<int, string> mergeableCells = new Dictionary<int, string>();

                    foreach (var item in source)
                    {
                        var row = (TableRow)templateRowClone.Clone();
                        int cellIndex = 0;

                        foreach (var cell in row.Elements<TableCell>())
                        {
                            foreach (var paragraph in cell.Descendants<Paragraph>())
                            {
                                Text text = paragraph.Descendants<Text>().FirstOrDefault();

                                if (text != null)
                                {
                                    if (Regex.IsMatch(text.Text, WordConstants.INDEX_PATTERN))
                                        text.Text = Regex.Replace(text.Text, WordConstants.INDEX_PATTERN, (index + 1).ToString());

                                    SetTextFromType(text, type, item);

                                    if (Regex.IsMatch(text.Text, WordConstants.MERGEABLE_PATTERN) && !mergeableCells.ContainsKey(cellIndex))
                                    {
                                        text.Text = Regex.Replace(text.Text, WordConstants.MERGEABLE_PATTERN, string.Empty);
                                        mergeableCells.Add(cellIndex, text.Text);
                                        cell.TableCellProperties.AppendChild(new VerticalMerge { Val = MergedCellValues.Restart });

                                        var templateCell = templateRowClone.Elements<TableCell>().ElementAt(cellIndex);
                                        foreach (var cellParagraph in templateCell.Descendants<Paragraph>())
                                            cellParagraph.RemoveAllChildren();
                                        templateCell.TableCellProperties.AppendChild(new VerticalMerge());
                                    }
                                }
                            }
                            cellIndex++;
                        }

                        table.InsertBefore(row, templateRow);
                        index++;
                    }

                    mergeableCells.Clear();
                    mergeableCells = null;
                }

                table.RemoveChild<TableRow>(templateRow);
            }

        }

        public void BindTable<TRow>(object tableObject, string bookmarkStartName, bool mergeRowDictionaryProperties = true)
        {
            Type type = tableObject.GetType();
            Type rowType = typeof(TRow);
            IDictionary<string, TRow> dictionary = new Dictionary<string, TRow>();

            if (mergeRowDictionaryProperties)
            {
                Type rowDicType = typeof(IDictionary<string, TRow>);
                Type stringType = typeof(string);
                Type[] genericArgs = null;

                foreach (var prop in Utils.GetProperties(type))
                {
                    Type propType = prop.PropertyType;

                    if (propType == rowType)
                    {
                        dictionary.Add(prop.Name, (TRow)prop.GetValue(tableObject, null));
                    }
                    else if ((propType.Name == rowDicType.Name || propType.GetInterface(rowDicType.Name) != null) && (genericArgs = propType.GetGenericArguments()).Length == 2 &&
                            genericArgs[0] == stringType && genericArgs[1] == rowType)
                    {
                        foreach (var item in (IDictionary<string, TRow>)prop.GetValue(tableObject, null))
                            dictionary.Add(item.Key, item.Value);
                    }
                }
            }
            else
            {
                foreach (var prop in Utils.GetProperties(type).Where(a => a.PropertyType == rowType))
                    dictionary.Add(prop.Name, (TRow)prop.GetValue(tableObject, null));
            }

            BindTable(dictionary, bookmarkStartName);
        }

        public void BindTable<TRow>(IDictionary<string, TRow> rowDictionary, string bookmarkStartName)
        {
            if (rowDictionary == null)
                throw new ArgumentNullException("rowDictionary");
            if (rowDictionary.Count == 0)
                return;

            Type type = typeof(TRow);
            BookmarkStart bookmarkStart = null;
            Table table = null;

            if ((bookmarkStart = _document.MainDocumentPart.RootElement.Descendants<BookmarkStart>().FirstOrDefault(a => a.Name == bookmarkStartName)) != null &&
                (table = bookmarkStart.Ancestors<Table>().FirstOrDefault()) != null)
            {
                var templateRow = bookmarkStart.Ancestors<TableRow>().FirstOrDefault();
                var columnPatterns = new Dictionary<int, string>();
                int index = 0;

                foreach (var cell in templateRow.Elements<TableCell>())
                {
                    var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
                    var text = CollectTextsToFirstTextAndReturn(paragraph);

                    if (!(text == null || string.IsNullOrEmpty(text.Text)))
                        columnPatterns.Add(index, text.Text);

                    index++;
                }
                index = 0;

                var nextRow = templateRow.NextSibling<TableRow>();
                table.RemoveChild<TableRow>(templateRow);

                while (nextRow != null)
                {
                    var firstCell = nextRow.Elements<TableCell>().FirstOrDefault();
                    var firstCellParagraph = firstCell.Elements<Paragraph>().FirstOrDefault();
                    Text firstCellText = CollectTextsToFirstTextAndReturn(firstCellParagraph);
                    MatchCollection matches = null;

                    if (firstCellText != null && (matches = Regex.Matches(firstCellText.Text, @"(\$F)?\[[\w]+\]")).Count > 0)
                    {
                        string rowPropertyPattern = matches[0].Value;
                        bool isFormalRow = rowPropertyPattern.StartsWith(@"$F");
                        int rowKeyIndex = rowPropertyPattern.IndexOf('[');
                        string rowKey = rowPropertyPattern.Substring(rowKeyIndex + 1, rowPropertyPattern.Length - rowKeyIndex - 2);

                        if (isFormalRow)
                            firstCellText.Text = firstCellText.Text.Replace(rowPropertyPattern, WordConstants.DEFAULT_FORMAT);

                        if (rowDictionary.ContainsKey(rowKey))
                        {
                            object rowObject = rowDictionary[rowKey];

                            foreach (var cell in nextRow.Elements<TableCell>())
                            {
                                if (columnPatterns.ContainsKey(index))
                                {
                                    string columnPattern = columnPatterns[index];
                                    Text cellText = null;

                                    if (isFormalRow)
                                    {
                                        cell.RemoveAllChildren<Paragraph>();
                                        cell.Append(firstCellParagraph.Clone() as Paragraph);
                                        cellText = cell.Elements<Paragraph>()
                                                       .FirstOrDefault()
                                                       .Descendants<Text>()
                                                       .FirstOrDefault();
                                        cellText.Text = string.Format(firstCellText.Text, columnPattern);
                                    }
                                    else
                                    {
                                        foreach (var item in cell.Descendants<Text>())
                                            item.Text = string.Empty;
                                        var paragraph = cell.Elements<Paragraph>().FirstOrDefault();
                                        cellText = paragraph.Descendants<Text>().FirstOrDefault();

                                        if (cellText == null)
                                        {
                                            if (paragraph.ParagraphProperties == null)
                                                paragraph.ParagraphProperties = new ParagraphProperties();
                                            var run = paragraph.AppendChild<Run>(new Run());
                                            run.RunProperties = new RunProperties(paragraph.ParagraphProperties.Elements().Select(a => a.Clone() as OpenXmlElement));
                                            cellText = run.AppendChild<Text>(new Text());
                                        }

                                        cellText.Text = columnPattern;
                                    }

                                    SetTextFromType(cellText, type, rowObject);
                                }

                                index++;
                            }
                            index = 0;
                        }
                    }

                    nextRow = nextRow.NextSibling<TableRow>();
                }

                #region Remove first column
                var gridColumn = table.Elements<TableGrid>().FirstOrDefault().Elements<GridColumn>().FirstOrDefault();
                var tableWidth = table.Elements<TableProperties>().FirstOrDefault().Elements<TableWidth>().FirstOrDefault();

                tableWidth.Width = (int.Parse(tableWidth.Width) - int.Parse(gridColumn.Width)).ToString();
                gridColumn.Remove();
                foreach (var row in table.Elements<TableRow>())
                    row.Elements<TableCell>().FirstOrDefault().Remove();
                #endregion

            }
        }

        private static bool IsEmptyParagraph(Paragraph paragraph)
        {
            if (paragraph == null)
                throw new ArgumentNullException("paragraph");

            return paragraph.Elements<Run>().Any();
        }

        private static Text CollectTextsToFirstTextAndReturn(OpenXmlElement parentElement)
        {
            var texts = parentElement.Descendants<Text>();
            Text firstText = texts.FirstOrDefault();

            if (firstText != null)
            {
                firstText.Text = string.Concat(texts.Select(a =>
                {
                    string str = a.Text;
                    a.Text = string.Empty;
                    return str;
                }));
            }

            return firstText;
        }

        public void Save()
        {
            ReplaceProperties();
            _document.MainDocumentPart.Document.Save();
        }

        public void SaveAs(string outputFileName)
        {
            if (outputFileName == null)
                throw new ArgumentNullException("outputFileName");

            using (var fs = File.Create(outputFileName))
            {
                GetStream().CopyTo(fs);
            }
        }

        public void SaveSourceFile()
        {
            SaveAs(_sourceFileName);
            _mustSaveSourceFile = false;
        }

        public void Dispose()
        {
            if (_document != null)
            {
                _document.Dispose();
                _document = null;
            }
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
            if (_propertyMapper != null)
            {
                _propertyMapper.Clear();
                _propertyMapper = null;
            }
        }

        private void PrepareDocument()
        {
            if (_document.ExtendedFilePropertiesPart.Properties.Application.Text != typeof(WordprocessingWorker).Name)
            {
                // Prepare document to work ...

                DocumentPropertyMapper.PrepareTemplate(_document);
                GetExtendedFilePropertiesPart().Save(_document.ExtendedFilePropertiesPart);
                _mustSaveSourceFile = true;
            }
        }
        
        private static void SetTextFromType(Text text, Type type, object obj)
        {
            text.Text = Regex.Replace(text.Text, WordConstants.FIELD_PATTERN, match =>
            {
                string propertyName = match.Value.Substring(1, match.Value.Length - 2);

                return Utils.GetProperyValue(type, obj, propertyName);
            });
        }

        private static DocumentFormat.OpenXml.ExtendedProperties.Properties GetExtendedFilePropertiesPart()
        {
            var properties = new DocumentFormat.OpenXml.ExtendedProperties.Properties();
            properties.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
            Application application = new Application();
            application.Text = typeof(WordprocessingWorker).Name;

            properties.Append(application);

            return properties;
        }

        #endregion

    }
}