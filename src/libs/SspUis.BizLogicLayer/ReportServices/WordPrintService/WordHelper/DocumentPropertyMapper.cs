using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class DocumentPropertyMapper
    {

        #region Fields and ctor

        private IDictionary<string, Tuple<string, object>> _dic = null;

        public DocumentPropertyMapper()
        {
            _dic = new Dictionary<string, Tuple<string, object>>();
        }

        #endregion

        #region Methods

        public void AddKeyValue(string key, object value)
        {
            int index = key.IndexOf(':');

            if (index == -1)
                _dic.Add(key, new Tuple<string, object>(null, value));
            else
                _dic.Add(key.Substring(0, index), new Tuple<string, object>(string.Format(WordConstants.CUSTOM_FORMAT, key.Substring(index, key.Length - index)), value));
        }

        #region WordprocessingDocument

        public static void PrepareTemplate(WordprocessingDocument wordDocument, bool autoSave = false)
        {
            Document document = wordDocument.MainDocumentPart.Document;
            var sdtRuns = document.Descendants<SdtRun>();
            var sdtBlocks = document.Descendants<SdtBlock>();
            var sdtCells = document.Descendants<SdtCell>();
            var sdtRows = document.Descendants<SdtRow>();

            foreach (SdtRun sdtRun in sdtRuns)
            {
                var sdtContent = sdtRun.Elements<SdtContentRun>().FirstOrDefault();

                if (sdtContent != null)
                {
                    var contentTexts = sdtContent.Descendants<Text>();
                    var value = GetFormatAndText(contentTexts);
                    var sdtProperties = sdtRun.SdtProperties;
                    var sdtAlias = sdtProperties.GetFirstChild<SdtAlias>();
                    var sdtTag = sdtProperties.GetFirstChild<Tag>();

                    if (sdtAlias != null)
                        sdtAlias.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new SdtAlias() { Val = value.Item2 });

                    if (sdtTag != null)
                        sdtTag.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new Tag() { Val = value.Item2 });

                    foreach (var contentText in contentTexts)
                        contentText.Text = string.Empty;

                    contentTexts.FirstOrDefault().Text = string.Concat(value.Item2, value.Item1);
                }
            }

            foreach (SdtBlock sdtBlock in sdtBlocks)
            {
                var sdtContent = sdtBlock.Elements<SdtContentBlock>().FirstOrDefault();

                if (sdtContent != null)
                {
                    var contentTexts = sdtContent.Descendants<Text>();
                    var value = GetFormatAndText(contentTexts);
                    var sdtProperties = sdtBlock.SdtProperties;
                    var sdtAlias = sdtProperties.GetFirstChild<SdtAlias>();
                    var sdtTag = sdtProperties.GetFirstChild<Tag>();

                    if (sdtAlias != null)
                        sdtAlias.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new SdtAlias() { Val = value.Item2 });

                    if (sdtTag != null)
                        sdtTag.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new Tag() { Val = value.Item2 });

                    foreach (var contentText in contentTexts)
                        contentText.Text = string.Empty;

                    contentTexts.FirstOrDefault().Text = string.Concat(value.Item2, value.Item1);
                }
            }

            foreach (SdtCell sdtCell in sdtCells)
            {
                var sdtContent = sdtCell.Elements<SdtContentCell>().FirstOrDefault();

                if (sdtContent != null)
                {
                    var contentTexts = sdtContent.Descendants<Text>();
                    var value = GetFormatAndText(contentTexts);
                    var sdtProperties = sdtCell.SdtProperties;
                    var sdtAlias = sdtProperties.GetFirstChild<SdtAlias>();
                    var sdtTag = sdtProperties.GetFirstChild<Tag>();

                    if (sdtAlias != null)
                        sdtAlias.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new SdtAlias() { Val = value.Item2 });

                    if (sdtTag != null)
                        sdtTag.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new Tag() { Val = value.Item2 });

                    foreach (var contentText in contentTexts)
                        contentText.Text = string.Empty;

                    contentTexts.FirstOrDefault().Text = string.Concat(value.Item2, value.Item1);
                }
            }

            foreach (SdtRow sdtRow in sdtRows)
            {
                var sdtContent = sdtRow.Elements<SdtContentRow>().FirstOrDefault();

                if (sdtContent != null)
                {
                    var contentTexts = sdtContent.Descendants<Text>();
                    var value = GetFormatAndText(contentTexts);
                    var sdtProperties = sdtRow.SdtProperties;
                    var sdtAlias = sdtProperties.GetFirstChild<SdtAlias>();
                    var sdtTag = sdtProperties.GetFirstChild<Tag>();

                    if (sdtAlias != null)
                        sdtAlias.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new SdtAlias() { Val = value.Item2 });

                    if (sdtTag != null)
                        sdtTag.Val = StringValue.FromString(value.Item2);
                    else
                        sdtProperties.Append(new Tag() { Val = value.Item2 });

                    foreach (var contentText in contentTexts)
                        contentText.Text = string.Empty;

                    contentTexts.FirstOrDefault().Text = string.Concat(value.Item2, value.Item1);
                }
            }

            if (autoSave)
                SaveDocument(wordDocument);
        }

        public void Replace(WordprocessingDocument wordDocument, bool autoClear = true, bool autoSave = true)
        {
            Document document = wordDocument.MainDocumentPart.Document;
            var sdtRuns = document.Descendants<SdtRun>();
            var sdtBlocks = document.Descendants<SdtBlock>();
            var sdtCells = document.Descendants<SdtCell>();
            var sdtRows = document.Descendants<SdtRow>();

            #region SdtRun

            foreach (SdtRun sdtRun in sdtRuns)
            {
                var stdAlias = sdtRun.SdtProperties.GetFirstChild<SdtAlias>();
                string title = null;
                SdtContentRun sdtContent = null;

                if (stdAlias != null && stdAlias.Val.HasValue && !string.IsNullOrEmpty((title = stdAlias.Val.Value))
                    && (sdtContent = sdtRun.GetFirstChild<SdtContentRun>()) != null
                    && _dic.ContainsKey(title))
                {
                    SetValueToContent(sdtContent, _dic[title]);
                }
            }

            #endregion

            #region SdtBlock

            foreach (SdtBlock sdtBlock in sdtBlocks)
            {
                var stdAlias = sdtBlock.SdtProperties.GetFirstChild<SdtAlias>();
                string title = null;
                SdtContentBlock sdtContent = null;

                if (stdAlias != null && stdAlias.Val.HasValue && !string.IsNullOrEmpty((title = stdAlias.Val.Value))
                    && (sdtContent = sdtBlock.GetFirstChild<SdtContentBlock>()) != null
                    && _dic.ContainsKey(title))
                {
                    SetValueToContent(sdtContent, _dic[title]);
                }
            }

            #endregion

            #region SdtCell

            foreach (SdtCell sdtCell in sdtCells)
            {
                var stdAlias = sdtCell.SdtProperties.GetFirstChild<SdtAlias>();
                string title = null;
                SdtContentCell sdtContent = null;

                if (stdAlias != null && stdAlias.Val.HasValue && !string.IsNullOrEmpty((title = stdAlias.Val.Value))
                    && (sdtContent = sdtCell.GetFirstChild<SdtContentCell>()) != null
                    && _dic.ContainsKey(title))
                {
                    SetValueToContent(sdtContent, _dic[title]);
                }
            }

            #endregion

            #region SdtRow

            foreach (SdtRow sdtRow in sdtRows)
            {
                var stdAlias = sdtRow.SdtProperties.GetFirstChild<SdtAlias>();
                string title = null;
                SdtContentRow sdtContent = null;

                if (stdAlias != null && stdAlias.Val.HasValue && !string.IsNullOrEmpty((title = stdAlias.Val.Value))
                    && (sdtContent = sdtRow.GetFirstChild<SdtContentRow>()) != null
                    && _dic.ContainsKey(title))
                {
                    SetValueToContent(sdtContent, _dic[title]);
                }
            }

            #endregion

            if (autoSave)
                SaveDocument(wordDocument);

            if (autoClear)
                Clear();
        }

        private static void SaveDocument(WordprocessingDocument wordDocument)
        {
            wordDocument.MainDocumentPart.Document.Save();
        }

        private static Tuple<string, string> GetFormatAndText(IEnumerable<Text> sdtContentTexts)
        {
            string fullText = string.Join(string.Empty, sdtContentTexts.Select(a => a.Text));
            int index = fullText.IndexOf(':');
            string format = null;
            string text = null;

            if (index == -1)
            {
                text = fullText.Replace(WordConstants.WHITE_SPACE, string.Empty);
            }
            else
            {
                format = fullText.Substring(index, fullText.Length - index);
                text = fullText.Substring(0, index).Replace(WordConstants.WHITE_SPACE, string.Empty);
            }

            return new Tuple<string, string>(format, text);
        }

        private static void SetValueToContent(OpenXmlCompositeElement content, Tuple<string, object> value)
        {
            if (value.Item2 is Paragraph)
            {
                Paragraph valueAsParagraph = (Paragraph)value.Item2;
                OpenXmlElement parent = content.Descendants<Run>().FirstOrDefault().Parent;

                OpenXmlElement runProperties = content.Descendants<ParagraphMarkRunProperties>().Select(a => (OpenXmlElement)a.Clone()).FirstOrDefault()
                                                    ?? content.Descendants<RunProperties>().Select(a => (OpenXmlElement)a.Clone()).FirstOrDefault()
                                                    ?? (OpenXmlElement)new RunProperties();

                parent.RemoveAllChildren<Run>();
                parent.RemoveAllChildren<ProofError>();

                AppendRunsOfParagraphToParent(parent, valueAsParagraph, runProperties);

                runProperties.RemoveAllChildren();
                runProperties = null;
            }
            else if (value.Item2 is IEnumerable<Paragraph>)
            {
                var valueAsParagraphCollection = (IEnumerable<Paragraph>)value.Item2;
                var paragraphProperties = (OpenXmlElement)content.Elements<Paragraph>().FirstOrDefault().ParagraphProperties.Clone();
                OpenXmlElement runProperties = content.Descendants<RunProperties>().Select(a => (OpenXmlElement)a.Clone()).FirstOrDefault() ?? (OpenXmlElement)new RunProperties();
                content.RemoveAllChildren();

                foreach (var paragraph in valueAsParagraphCollection)
                {
                    var p = new Paragraph((OpenXmlElement)paragraphProperties.Clone());
                    AppendRunsOfParagraphToParent(p, paragraph, runProperties);
                    content.AppendChild(p);
                }

                runProperties.RemoveAllChildren();
                runProperties = null;
            }
            else
            {
                Text text = content.Descendants<Text>().FirstOrDefault();

                if (value.Item2 == null || string.IsNullOrEmpty(value.Item2.ToString()))
                {
                    text.Text = WordConstants.WHITE_SPACE;
                    text.Space = SpaceProcessingModeValues.Preserve;
                }
                else
                {
                    int index = text.Text.IndexOf(':');
                    string format = null;

                    if (index == -1)
                        format = value.Item1 ?? WordConstants.DEFAULT_FORMAT;
                    else
                        format = string.Format(WordConstants.CUSTOM_FORMAT, text.Text.Substring(index, text.Text.Length - index));

                    text.Text = string.Format(format, value.Item2);
                }
            }

        }

        private static void AppendRunsOfParagraphToParent(OpenXmlElement parent, Paragraph paragraph, OpenXmlElement runProperties)
        {
            foreach (Run clone in paragraph.Elements<Run>().Select(a => (Run)a.Clone()))
            {
                if (clone.RunProperties == null)
                    clone.PrependChild(new RunProperties());
                foreach (var runProp in runProperties)
                    if (!clone.RunProperties.Any(a => a.LocalName == runProp.LocalName))
                        clone.RunProperties.AppendChild((OpenXmlElement)runProp.Clone());
                parent.AppendChild(clone);
            }
        }

        #endregion

        /*#region SpreadsheetDocument

        public void Replace(SpreadsheetDocument spreadSheet, bool autoClear = true)
        {
            SharedStringTablePart shareStringPart = null;
            if ((shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()) != null)
            {
                string sstText = null;
                using (StreamReader sr = new StreamReader(shareStringPart.GetStream()))
                {
                    sstText = sr.ReadToEnd();
                }

                sstText = Regex.Replace(sstText, @"\$\{[^\}]*\}", match =>
                {
                    string value = match.Value.Substring(2, match.Value.Length - 3);

                    if (_dic.ContainsKey(value))
                        return string.Format(Constants.DEFAULT_FORMAT, _dic[value].Item2 ?? string.Empty);
                    return match.Value;
                });

                using (StreamWriter sw = new StreamWriter(shareStringPart.GetStream(FileMode.Create)))
                {
                    sw.Write(sstText);
                }
            }

            if (autoClear)
                Clear();
        }

        #endregion
*/
        public void Clear()
        {
            _dic.Clear();
        }

        #endregion

    }
}